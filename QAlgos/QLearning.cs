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
     * Q-Learning is a basic state-based learning algorithm.  It essentially memorizes the consequences of every state, leading to its drawback of being too memory intensive for most applications.
     * */
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class QLearning:QAlgo
    {
        protected Dictionary<QStateActionPair, decimal> QValues;
        protected Random random;

        protected decimal learn, discount, explore;

        public override void Initialize()
        {
            QValues = new Dictionary<QStateActionPair, decimal>();
            random = new Random();
            RenameLearningTable("#", "State", "Action", "QValue");
        }

        public override QState Run(QState currentState, int trialNum, decimal learn, decimal discount, decimal explore)
        {
            this.learn = learn; this.discount = discount; this.explore = explore;
            decimal score = 0;
            int actionsTaken = 0;
            while (!currentState.IsEnd() && GetOutcomes(currentState).Count > 0 && isRunning)
            {
                actionsTaken++;
                string a;
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
                decimal r = GetReward(currentState, newState);
                score += r;
                QUpdate(actionsTaken, currentState, a, newState, r);
                if(!HideOutput) WriteOutput((CurrentMode == LEARN ? "Trial " + trialNum + ", " : "") + "#" + actionsTaken + " " + (exp ? "Explore" : "Action") + ": '" + a + "' @ " + currentState.ToString() + " | Gain " + Math.Round(r, 4) + ",  Total " + Math.Round(score, 4));
                currentState = newState;
            }
            if (isRunning)
            {
                if (!HideOutput) WriteOutput("Trial " + trialNum + ": " + Math.Round(score, 4) + " in " + actionsTaken + " step" + (actionsTaken == 1 ? "" : "s") + ".", true);
            }
            return currentState;
        }

        public virtual decimal GetReward(QState currentState, QState newState)
        {
            return newState.GetValue() - currentState.GetValue();
        }

        // Return the value of performing an action at given state or 0 if not done before
        protected virtual decimal GetQValue(QStateActionPair p)
        {
            if (!QValues.ContainsKey(p)) return 0;
            else return QValues[p];
        }

        // Return list of all possible states that can result from an action taken from the current state
        protected virtual List<QStateActionPair> GetOutcomes(QState state)
        {
            return state.GetActions().Select(x => new QStateActionPair(state, x)).ToList() ;
        }

        // Return the value of the best action taken at given state, else 0 if not known
        protected virtual decimal GetMaxValue(QState state)
        {
            return GetOutcomes(state).Select(x => GetQValue(x)).Max();
        }

        // Return best action to take from current state (best QValue or random if tied)
        protected virtual string GetBestAction(QState state)
        {
            decimal maxVal = GetMaxValue(state);
            IEnumerable<string> s = GetOutcomes(state).Where(x => GetQValue(x) == maxVal).Select(x => x.action);
            int n = s.Count();
            if (n == 1) return s.First();
            else
            {
                return s.ElementAt(random.Next(n));
            }
        }
        // Return a random action for exploration
        protected virtual string GetRandomAction(QState state)
        {
            IEnumerable<string> s = GetOutcomes(state).Select(x => x.action);
            return s.ElementAt(random.Next(s.Count()));
        }

        protected virtual void QUpdate(int n, QState currentState, string action, QState newState, decimal reward)
        {
            QStateActionPair p = new QStateActionPair(currentState, action);
            decimal maxQ = GetMaxValue(newState);
            QValues[p] = (1 - learn) * GetQValue(p) + learn * (reward + discount * maxQ);
            UpdateLearningTable(n, currentState, action, QValues[p]);
        }

        public override void Open(object o, QState initialState)
        {
            QValues.Clear();
            ClearLearningTable();

            foreach (KeyValuePair<object[], decimal> kv in ((Dictionary<object[], decimal>)o))
            {
                QValues.Add(new QStateActionPair(initialState.Open(kv.Key[0]), (string)kv.Key[1]), kv.Value);
            }
            
            int i=1;
            foreach (KeyValuePair<QStateActionPair, decimal> kv in QValues)
            {
                UpdateLearningTable(i++, kv.Key.state, kv.Key.action, kv.Value);
            }
        }

        public override object Save()
        {
            Dictionary<object[], decimal> obj = new Dictionary<object[], decimal>();
            foreach (KeyValuePair<QStateActionPair, decimal> kv in QValues)
            {
                object state = kv.Key.state.Save();
                if (state == null) return null;
                obj.Add(new object[] { kv.Key.state.Save(), kv.Key.action }, kv.Value);
            }
            return obj;
        }
    }
}
