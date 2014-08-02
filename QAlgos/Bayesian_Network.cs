using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;
using System.ComponentModel;

namespace QLearner.QAlgos
{
    /*
     * Bayesian networks are still being coded and not usable yet in version 1.2
     * If anyone would like to help code bayes nets, please let me know. - pftq
     * */
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class Bayesian_Network:QAlgo
    {
        protected List<Node> nodes;
        protected Dictionary<QState, List<QStateActionPair>> outcomesCache;
        protected Random random;

        protected decimal learn, discount, explore;

        public override void Initialize()
        {
            nodes = new List<Node>();
            random = new Random();
            RenameLearningTable("#", "Event", "Condition", "Probability");
        }

        public override QState Run(QState currentState, int trialNum, decimal learn, decimal discount, decimal explore)
        {
            this.learn = learn; this.discount = discount; this.explore = explore;
            decimal score = 0;
            int actionsTaken = 0;
            while (!currentState.IsEnd() && getOutcomes(currentState).Count > 0 && isRunning)
            {
                actionsTaken++;
                string a;
                bool exp;
                a = GetRandomAction(currentState);
                exp = true;
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
        protected virtual List<QStateActionPair> getOutcomes(QState state)
        {
            if (outcomesCache != null && outcomesCache.First().Key == state)
                return outcomesCache.First().Value;
            else
            {
                outcomesCache = new Dictionary<QState, List<QStateActionPair>>();
                List<QStateActionPair> possibilities = new List<QStateActionPair>();
                foreach (string a in state.GetActions())
                {
                    possibilities.Add(new QStateActionPair(state, a));
                }
                outcomesCache[state] = possibilities;
                return possibilities;
            }
        }

        // Return a random action for exploration
        protected virtual string GetRandomAction(QState state)
        {
            IEnumerable<string> s = getOutcomes(state).Select(x => x.action);
            return s.ElementAt(random.Next(s.Count()));
        }

        protected virtual void Update(int n, QState currentState, string action, QState newState, decimal reward)
        {
        }

        public class Node
        {
            public List<Node> parents, children;

            public Node()
            {
                parents = new List<Node>();
                children = new List<Node>();
            }
        }
    }
}
