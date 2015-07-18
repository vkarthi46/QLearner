using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;
using System.ComponentModel;
using QLearner.Resources.PriorityQueue;

namespace QLearner.Resources
{
    /*
     * The QSearch class contains many common search algorithms for ease of use.  Each of these take a starting state and return a QSearchResult object containing the action sequence and list of QStates.  The QSearchResult object is null when no path is found.
     * */
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    public class QSearch:QAPI
    {
        public QSearch() { }

        public QSearch(QAPI current)
        {
            Inherit(current);
        }

        public QSearchResult AStar(QState startState, bool output = false, int maxQueue = 1000)
        {
            HashSet<QState> explored = new HashSet<QState>();
            Dictionary<QState, decimal> bestSoFar = new Dictionary<QState, decimal>() { { startState, 0 } };
            HeapPriorityQueue<QStateContainer> toDo = new HeapPriorityQueue<QStateContainer>(maxQueue);
            toDo.Enqueue(new QStateContainer(startState), 0);
            Dictionary<QState, QSearchResult> pathTo = new Dictionary<QState, QSearchResult>() { { startState, new QSearchResult() } };
            if (output) WriteOutput("Searching for shortest path via A-Star Search...");
            int steps = 0;
            while (toDo.Count > 0 && isRunning)
            {
                steps++;
                QState current = toDo.Dequeue().qstate;
                if (current.IsEnd())
                {
                    if (output) WriteOutput("Shortest path of " + pathTo[current].Count + " step(s) found after " + steps + " iteration(s).");
                    return pathTo[current];
                }
                else
                {
                    explored.Add(current);
                    foreach (QAction action in current.GetChoices())
                    {
                        QState newState = current.GetNewState(action);
                        if (!explored.Contains(newState))
                        {
                            decimal actualCost = bestSoFar[current] - current.GetValue();
                            if (!bestSoFar.ContainsKey(newState) || actualCost < bestSoFar[newState])
                            {
                                pathTo[newState] = new QSearchResult(pathTo[current]);
                                pathTo[newState].actionsList.Add(action);
                                pathTo[newState].QStatesList.Add(newState);
                                bestSoFar[newState] = actualCost;
                                toDo.Enqueue(new QStateContainer(newState), bestSoFar[newState] - 1 * newState.GetValueHeuristic());
                            }
                        }
                    }
                }
            }
            if (output) WriteOutput("No path found after " + steps + " iteration(s).");
            return null;
        }

        public QSearchResult Breadth_First(QState startState, bool output = false)
        {
            HashSet<QState> explored = new HashSet<QState>();
            Dictionary<QState, decimal> bestSoFar = new Dictionary<QState,decimal>(){{startState, 0}};
            Queue<QState> toDo = new Queue<QState>();
            toDo.Enqueue(startState);
            Dictionary<QState, QSearchResult> pathTo = new Dictionary<QState, QSearchResult>() { { startState, new QSearchResult() } };
            if(output) WriteOutput("Searching for shortest path via Breadth-First Search...");
            int steps = 0;
            while (toDo.Any() && isRunning)
            {
                steps++;
                QState current = toDo.Dequeue();
                if (current.IsEnd())
                {
                    if(output) WriteOutput("Shortest path of " + pathTo[current].Count + " step(s) found after " + steps + " iteration(s).");
                    return pathTo[current];
                }
                else
                {
                    explored.Add(current);
                    foreach (QAction action in current.GetChoices())
                    {
                        QState newState = current.GetNewState(action);
                        if (!explored.Contains(newState))
                        {
                            decimal actualCost = bestSoFar[current] - current.GetValue();
                            if (!bestSoFar.ContainsKey(newState) || actualCost < bestSoFar[newState])
                            {
                                pathTo[newState] = new QSearchResult(pathTo[current]);
                                pathTo[newState].actionsList.Add(action);
                                pathTo[newState].QStatesList.Add(newState);
                                bestSoFar[newState] = actualCost;
                                toDo.Enqueue(newState);
                            }
                        }
                    }
                }
            }
            if(output) WriteOutput("No path found after " + steps + " iteration(s).");
            return null;
        }

        public QSearchResult Depth_First(QState startState, bool output = false)
        {
            HashSet<QState> explored = new HashSet<QState>();
            Dictionary<QState, decimal> bestSoFar = new Dictionary<QState, decimal>() { { startState, 0 } };
            Stack<QState> toDo = new Stack<QState>();
            toDo.Push(startState);
            Dictionary<QState, QSearchResult> pathTo = new Dictionary<QState, QSearchResult>() { { startState, new QSearchResult() } };
            if (output) WriteOutput("Searching for any path via Depth-First Search...");
            int steps = 0;
            while (toDo.Any() && isRunning)
            {
                steps++;
                QState current = toDo.Pop();
                if (current.IsEnd())
                {
                    if (output) WriteOutput("Arbitrary path of " + pathTo[current].Count + " step(s) found after " + steps + " iteration(s).");
                    return pathTo[current];
                }
                else
                {
                    explored.Add(current);
                    foreach (QAction action in current.GetChoices())
                    {
                        QState newState = current.GetNewState(action);
                        if (!explored.Contains(newState))
                        {
                            decimal actualCost = bestSoFar[current] - current.GetValue();
                            if (!bestSoFar.ContainsKey(newState) || actualCost < bestSoFar[newState])
                            {
                                pathTo[newState] = new QSearchResult(pathTo[current]);
                                pathTo[newState].actionsList.Add(action);
                                pathTo[newState].QStatesList.Add(newState);
                                bestSoFar[newState] = actualCost;
                                toDo.Push(newState);
                            }
                        }
                    }
                }
            }
            if (output) WriteOutput("No path found after " + steps + " iteration(s).");
            return null;
        }

        private class QStateContainer : PriorityQueueNode
        {
            public QState qstate;
            public QStateContainer(QState q)
            {
                qstate = q;
            }
            public override bool Equals(object obj)
            {
                return qstate == (QState)obj;
            }
            public override int GetHashCode()
            {
                return qstate.GetHashCode();
            }
        }

    }
    public class QSearchResult
    {
        public List<QState> QStatesList;
        public List<QAction> actionsList;
        public QSearchResult()
        {
            QStatesList = new List<QState>();
            actionsList = new List<QAction>();
        }

        public QSearchResult(QSearchResult q)
        {
            QStatesList = q.QStatesList.ToList();
            actionsList = q.actionsList.ToList();
        }

        public bool Any() { return QStatesList.Any(); }
        public int Count { get { return QStatesList.Count; } }
    }
}
