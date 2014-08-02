using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;
using QLearner.Resources;

namespace QLearner.QAlgos
{
    /*
     * This is a simple demo of Search can be applied to a scenario.
     * The Search algorithms provided in QLearner are more likely to be used in conjunction with other QAlgos or QStates, as opposed to stand-alone.  For example, the search algorithms can be called from any other class by instantiating QSearch from the QLearner.Resources namespace.
     * */
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class AStar_Search:QAlgo
    {
        public override QState Run(QState currentState, int trialNum, decimal learn, decimal discount, decimal explore)
        {
            QSearch qsearch = new QSearch();
            qsearch.Inherit(this);
            QSearchResult actions = qsearch.AStar(currentState, true);
            if (actions != null)
            {
                foreach (string action in actions.actionsList)
                {
                    if (!currentState.IsEnd() && isRunning && currentState.GetActions().Contains(action))
                    {
                        WriteOutput(currentState + ": " + action);
                        QState newState = currentState.GetNewState(action);
                        newState.Inherit(currentState);
                        newState.Step();
                        currentState = newState;
                    }
                }
                if (currentState.IsEnd()) WriteOutput(currentState + ": End");
                else
                {
                    WriteOutput("Existing solution no longer applicable.  Re-solving...");
                    return Run(currentState, trialNum, learn, discount, explore);
                }
            }
            else WriteOutput("No solution found.", true);
            return currentState;
        }
    }
}
