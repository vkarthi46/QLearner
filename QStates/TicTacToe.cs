using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using QLearner.Resources;

namespace QLearner.QStates
{
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class TicTacToe:QState
    {
        private List<Point> myMoves, yourMoves, openSpaces;

        private List<List<Point>> winConditions = new List<List<Point>>() {
                new List<Point>(){new Point(0, 0), new Point(0, 1), new Point(0, 2)},
                new List<Point>(){new Point(1, 0), new Point(1, 1), new Point(1, 2)},
                new List<Point>(){new Point(2, 0), new Point(2, 1), new Point(2, 2)},
                new List<Point>(){new Point(0, 0), new Point(1, 0), new Point(2, 0)},
                new List<Point>(){new Point(0, 1), new Point(1, 1), new Point(2, 1)},
                new List<Point>(){new Point(0, 2), new Point(1, 2), new Point(2, 2)},
                new List<Point>(){new Point(0, 0), new Point(1, 1), new Point(2, 2)},
                new List<Point>(){new Point(2, 0), new Point(1, 1), new Point(0, 2)}
            };

        private TicTacToeGUI gui=null;
        private string me="X", you="O";
        private Random random;
        private int score=0;

        public override QState Initialize()
        {
            openSpaces = new List<Point>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    openSpaces.Add(new Point(i, j));
                }
            }

            myMoves = new List<Point>();
            yourMoves = new List<Point>();
            random = new Random();

            if (random.Next(2) > 0)
            {
                me = "X";
                you = "O";
            }
            else
            {
                you = "X";
                me = "O";
            }

            WriteOutput("Me: "+me+", " + (CurrentMode == LEARN ? "Opponent" : "You") + ": " + you);

            if (!HideOutput || CurrentMode==AWAKEN)
            {
                ManualResetEvent wait = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(CreateGUI), new object[] { this, wait });
                wait.WaitOne();
            }

            if (random.Next(2) > 0)
            {
                WriteOutput("I go first.");
            }
            else
            {
                WriteOutput("You go first.");
                Step();
            }
            return new TicTacToe() { me = me, you = you, gui = gui, myMoves = myMoves.ToList(), yourMoves = yourMoves.ToList(), openSpaces = openSpaces.ToList(), random = random, score = score, winConditions = winConditions };
        }

        private void CreateGUI(object o) {
            TicTacToe master = (TicTacToe)((object[])o)[0];
            ManualResetEvent w = (ManualResetEvent)((object[])o)[1];
            master.gui = new TicTacToeGUI();
            w.Set();
            Application.Run(master.gui);
        }

        public override bool Equals(object obj)
        {
            Point[] myMovesA = myMoves.OrderBy(x => x.X).ThenBy(x=>x.Y).ToArray();
            Point[] myMovesB = ((TicTacToe)obj).myMoves.OrderBy(x => x.X).ThenBy(x => x.Y).ToArray();
            if (myMovesA.Length != myMovesB.Length) return false;
            for (int i = 0; i < myMovesA.Length; i++)
                if (myMovesA[i] != myMovesB[i]) return false;

            List<Point> yourMovesA = yourMoves.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            List<Point> yourMovesB = ((TicTacToe)obj).yourMoves.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            if (yourMovesA.Count != yourMovesB.Count) return false;
            for (int i = 0; i < yourMovesA.Count; i++)
                if (yourMovesA[i] != yourMovesB[i]) return false;

            return true;
        }
        public override int GetHashCode()
        {
            return myMoves.Select(x=>(x.X+1)*10+(x.Y+1)).Sum()+yourMoves.Select(x=>(x.X+1)*1000+(x.Y+1)*100).Sum();
        }
        public override QState GetNewState(QAction action = null)
        {
            if (action.ToString() != "")
            {
                string[] point = action.ToString().Split(',');
                Point newPos = new Point(Convert.ToInt32(point[0]), Convert.ToInt32(point[1]));

                List<Point> newSpaces = openSpaces.ToList();
                newSpaces.Remove(newPos);

                List<Point> newMoves = myMoves.ToList();
                newMoves.Add(newPos);

                TicTacToe newState = new TicTacToe() { myMoves = newMoves, yourMoves = yourMoves.ToList(), me = me, you = you, random = random, gui = gui, openSpaces = newSpaces, score = score };

                // Pass all variables to new state
                return newState;
            }
            else return this;
        }
        public override QAction[] GetActions()
        {
            return openSpaces.Any() ? openSpaces.Select(x => new QAction_String(x.X + "," + x.Y)).ToArray() : new QAction[] { new QAction_String("") };
        }
        public override Dictionary<QStateActionPair, QState> GetObservedStates(QState prevState, QAction action)
        {
            TicTacToe stateAfterMyMove = (TicTacToe)prevState.GetNewState(action);
            TicTacToe stateFromOpponentsView = new TicTacToe() { myMoves = stateAfterMyMove.yourMoves.ToList(), yourMoves = stateAfterMyMove.myMoves.ToList(), me = you, you = me, random = random, gui = gui, openSpaces = stateAfterMyMove.openSpaces.ToList(), score = stateAfterMyMove.score };
            TicTacToe stateNowFromOpponentsView = new TicTacToe() { myMoves = yourMoves.ToList(), yourMoves = myMoves.ToList(), me = you, you = me, random = random, gui = gui, openSpaces = openSpaces.ToList(), score = stateAfterMyMove.score };
            
            foreach (Point x in stateNowFromOpponentsView.myMoves)
                if (!stateFromOpponentsView.myMoves.Contains(x)) 
                    return new Dictionary<QStateActionPair, QState>() {
                        {new QStateActionPair(stateFromOpponentsView, new QAction_String(x.X + "," + x.Y)), stateNowFromOpponentsView}
                    };

            return new Dictionary<QStateActionPair, QState>();
        }
        public override decimal GetValue()
        {
            score = 0;
            if (winConditions.Where(x => x.Where(y => myMoves.Contains(y)).Count() == 3).Any())
                score = 100;
            else if (winConditions.Where(x => x.Where(y => yourMoves.Contains(y)).Count() == 3).Any())
                score = -100;
            else if (openSpaces.Count == 0) score = -1;
            return score;
        }
        public override bool IsEnd()
        {
            return GetValue()!=0;
        }
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(myMoves.Contains(new Point(i, j))) s.Append(me);
                    else if(yourMoves.Contains(new Point(i, j))) s.Append(you);
                    else s.Append("_");
                }
            }
            return s.ToString();
        }

        public override void Step()
        {
            if (gui != null && myMoves.Any())
            {
                gui.Set(me, myMoves.Last().X, myMoves.Last().Y);
                if (CurrentMode != LEARN) Thread.Sleep(100);
            }
            if (!IsEnd() && isRunning)
            {
                if (CurrentMode == LEARN)
                {
                    Point p = openSpaces.OrderBy(x => random.Next()).First();
                    openSpaces.Remove(p);
                    yourMoves.Add(p);

                    WriteOutput("Random move: " + p+" => "+ToString());

                    if (gui != null) gui.Set(you, p.X, p.Y);
                }
                else
                {
                    WriteOutput("Waiting for your move...");
                    ManualResetEvent wait = new ManualResetEvent(false);
                    gui.Prompt(wait);
                    wait.WaitOne();


                    Point p = gui.selected;
                    if (p.X >= 0)
                    {
                        WriteOutput("Your move received: " + p);

                        openSpaces.Remove(p);
                        yourMoves.Add(p);

                        gui.Set(you, p.X, p.Y);
                        Thread.Sleep(100);
                    }
                    else
                    {
                        gui = null;
                        Abort();
                    }
                }
            }
        }

        public override void End()
        {
            if (CurrentMode == AWAKEN)
            {
                if (score > 0) popup("I win.");
                else if (score < -1) popup("You win.");
                else if (score == -1) popup("We suck.");
            }
            if (gui != null) gui.Quit();
        }
        /*
        public override Dictionary<QFeature, decimal> GetFeatures(QAction action)
        {
            Dictionary<string, decimal> slots = new Dictionary<string, decimal>() { };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (myMoves.Contains(new Point(i, j)) || i+","+j==action.ToString()) slots[i + "," + j] = 1;
                    else if (yourMoves.Contains(new Point(i, j))) slots[i + "," + j] = -1;
                    else slots[i + "," + j] = 0;
                }
            }
            return QFeature_String.FromStringDecimalDictionary(slots);
        }
        */
        public override object Save()
        {
            return new object[] { myMoves, yourMoves, openSpaces};
        }

        public override QState Open(object o)
        {
            object[] oo = (object[])o;
            return new TicTacToe() { myMoves = (List<Point>)oo[0], yourMoves = (List<Point>)oo[1], openSpaces = (List<Point>)oo[2] };
        }
    }
}
