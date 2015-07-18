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
     * An enhanced version of Q-Learning that learns from both its own actions as well as observations of what it could have done and what its opponents are doing.  This is an iteration towards the more comprehensive AI I want to build that I outlined on my site:
     * http://www.pftq.com/blabberbox/?blogcat=writings;page=Creating_Sentient_Artificial_Intelligence
     * -pftq
     * */
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class QLearning_Observant:QLearning
    {
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
                decimal r = GetReward(currentState, newState);
                score += r;
                QUpdate(actionsTaken, currentState, a, newState, r);
                if(!HideOutput) WriteOutput((CurrentMode == LEARN ? "Trial " + trialNum + ", " : "") + "#" + actionsTaken + " " + (exp ? "Explore" : "Action") + ": '" + a + "' @ " + currentState.ToString() + " | Gain " + Math.Round(r, 4) + ",  Total " + Math.Round(score, 4));
                
                foreach (KeyValuePair<QStateActionPair, QState> kv in newState.GetObservedStates(currentState, a))
                {
                    QState observedPriorState = kv.Key.state;
                    QAction observedAction = kv.Key.action;
                    QState observedState = kv.Value;
                    decimal observedR = GetReward(observedPriorState, observedState);
                    QUpdate(actionsTaken, observedPriorState, observedAction, observedState, observedR);
                    if (!HideOutput) WriteOutput((CurrentMode == LEARN ? "Trial " + trialNum + ", " : "") + "#" + actionsTaken + " Observed: '" + observedAction + "' @ " + observedPriorState.ToString() + " | Gain " + Math.Round(observedR, 4));
                }

                currentState = newState;
            }
            if (isRunning)
            {
                if (!HideOutput) WriteOutput("Trial " + trialNum + ": " + Math.Round(score, 4) + " in " + actionsTaken + " step" + (actionsTaken == 1 ? "" : "s") + ".", true);
            }
            return currentState;
        }
    }
}
