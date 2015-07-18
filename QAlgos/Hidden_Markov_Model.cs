using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;
using System.ComponentModel;
using QLearner.Resources;

namespace QLearner.QAlgos
{
    /*
     * HMMs are still being coded and not usable yet in version 1.3
     * If anyone would like to help code HMMs and other algos, please let me know. - pftq
     * */
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class Hidden_Markov_Model:QAlgo
    {
        protected Bayesian_Network bayesNet;
        protected Dictionary<QState, List<QStateActionPair>> outcomesCache;
        protected Random random;

        private Dictionary<QStateActionPair, Dictionary<QFeature, decimal>> featureCache;

        protected decimal learn, discount, explore;

        private QFeature_String outcomeFeature = new QFeature_String("Outcome");

        public override void Initialize()
        {
            bayesNet = new Bayesian_Network();
            random = new Random();
            RenameLearningTable("#", "Condition", "Outcome", "Probability");
            featureCache = new Dictionary<QStateActionPair, Dictionary<QFeature, decimal>>();
        }

        public override QState Run(QState currentState, int trialNum, decimal learn, decimal discount, decimal explore)
        {
            this.learn = learn; this.discount = discount; this.explore = explore;
            decimal score = 0;
            int actionsTaken = 0;
            while (!currentState.IsEnd() && GetOutcomes(currentState).Count > 0 && isRunning)
            {
                actionsTaken++;
                QAction a;
                bool exp;
                if (explore > 0 && (decimal)random.NextDouble() <= explore)
                {
                    a = GetRandomAction(currentState);
                    exp = true;
                }
                else
                {
                    a = GetBestAction(currentState);
                    exp = false;
                }
                QState newState = currentState.GetNewState(a);
                newState.Inherit(currentState);
                newState.Step();
                decimal r = getReward(currentState, newState);
                score += r;
                Update(actionsTaken, currentState, a, newState, r);
                WriteOutput((CurrentMode == LEARN ? "Trial " + trialNum + ", " : "") + "#" + actionsTaken + " " + (exp ? "Explore" : "Action") + ": '" + a + "' @ " + currentState.ToString() + " | Gain " + Math.Round(r, 4) + ",  Total " + Math.Round(score, 4));
                currentState = newState;
            }
            if (isRunning)
            {
                if (CurrentMode == LEARN) WriteOutput("Final Result of Trial " + trialNum + ": " + Math.Round(score, 4) + " in " + actionsTaken + " step" + (actionsTaken == 1 ? "" : "s") + ".", true);
                else WriteOutput("Finished the problem in " + actionsTaken + " step" + (actionsTaken == 1 ? "" : "s") + " with a final total of " + Math.Round(score, 4) + ".", true);
            }
            return currentState;
        }

        public virtual decimal getReward(QState currentState, QState newState)
        {
            return newState.GetValue() - currentState.GetValue();
        }

        // Return list of all possible states that can result from an action taken from the current state
        protected virtual List<QStateActionPair> GetOutcomes(QState state)
        {
            if (outcomesCache != null && outcomesCache.First().Key == state)
                return outcomesCache.First().Value;
            else
            {
                outcomesCache = new Dictionary<QState, List<QStateActionPair>>();
                List<QStateActionPair> possibilities = new List<QStateActionPair>();
                foreach (QAction a in state.GetChoices())
                {
                    possibilities.Add(new QStateActionPair(state, a));
                }
                outcomesCache[state] = possibilities;
                return possibilities;
            }
        }

        // Return the value of performing an action based on the features expected from the action.
        protected virtual decimal GetQValue(QStateActionPair p)
        {
            decimal qv = 0;

            Dictionary<QFeature, decimal> features;
            //features = p.state.GetFeaturesEstimate(p.action);
            if (featureCache.ContainsKey(p)) features = featureCache[p];
            else
            {
                features = p.state.GetFeatures(p.action);
                featureCache[p] = features;
            }
            
            Bayesian_Network.Clause c = new Bayesian_Network.Clause();
            foreach (KeyValuePair<QFeature, decimal> otherFeature in features)
                c.AddVariable(otherFeature.Key, otherFeature.Value);

            foreach (decimal d in bayesNet.GetValues(outcomeFeature))
                qv += bayesNet.GetProbability(outcomeFeature, d, c) * d;

            return qv;
        }

        // Return the value of the best action taken at given state, else 0 if not known
        protected virtual decimal GetMaxValue(QState state)
        {
            return GetOutcomes(state).Select(x => GetQValue(x)).Max();
        }

        // Return best action to take from current state (best QValue or random if tied)
        protected virtual QAction GetBestAction(QState state)
        {
            decimal maxVal = GetMaxValue(state);
            IEnumerable<QAction> s = GetOutcomes(state).Where(x => GetQValue(x) == maxVal).Select(x => x.action);
            int n = s.Count();
            if (n == 1) return s.First();
            else
            {
                return s.ElementAt(random.Next(n));
            }
        }

        // Return a random action for exploration
        protected virtual QAction GetRandomAction(QState state)
        {
            IEnumerable<QAction> s = GetOutcomes(state).Select(x => x.action);
            return s.ElementAt(random.Next(s.Count()));
        }

        protected virtual void Update(int n, QState currentState, QAction action, QState newState, decimal reward)
        {
            Dictionary<QFeature, decimal> features = currentState.GetFeatures(action);
            Bayesian_Network.Clause c = new Bayesian_Network.Clause();
            foreach (KeyValuePair<QFeature, decimal> ff in features)
            {
                c.AddVariable(ff.Key, ff.Value);
            }
            bayesNet.SetProbability(outcomeFeature, reward, c, bayesNet.GetProbability(outcomeFeature, reward, c) + 1);

            int i = 1;
            foreach (QFeature f in bayesNet.GetFeatures())
            {
                foreach (decimal d in bayesNet.GetValues(f))
                {
                    foreach (Bayesian_Network.Clause cc in bayesNet.GetConditions(f, d))
                    {
                        UpdateLearningTable(i++, cc.ToString(), ""+d, bayesNet.GetProbability(f, d, cc));
                    }
                }
            }
        }
    }
}
