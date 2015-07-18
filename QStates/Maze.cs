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
    public class Maze:QState
    {
        private Point self, start = new Point(0, 0), goal = new Point(9, 9);
        private int width = 10, height = 10;
        private double wallDensity = 0.25, opponentDifficulty=0.25;
        private MazeGUI maze;
        private int score = 0, opponents = 1;
        List<Point> opponent = new List<Point>();
        private Random random = new Random();
        private Dictionary<QState, QSearchResult> bestOpponentPath = new Dictionary<QState, QSearchResult>();

        private readonly QAction UP = new QAction_String("up"), DOWN = new QAction_String("down"), LEFT = new QAction_String("left"), RIGHT = new QAction_String("right");

        private Point[] walls = new Point[] { 
        };

        public override QState Initialize()
        {
            WriteOutput("Dimensions: " + width + "x" + height);
            self = new Point(start.X, start.Y);
            score = 0;

            walls = new Point[] { };

            // Generate Walls Randomly
            if (wallDensity > 0)
            {
                
                HashSet<Point> bestPathSoFar = new HashSet<Point>();
                WriteOutput("Generating walls randomly with density target of " + wallDensity + "...");
                for (int w = 0; w < width; w++)
                {
                    for (int h = 0; h < height; h++)
                    {
                        double r = random.NextDouble();
                        //WriteOutput("Wall Probability for " + w + "x" + h + ": " + r+" vs threshold "+walls);
                        if (r < wallDensity)
                        {
                            //WriteOutput("Wall created at " + w + "x" + h);
                            Point newWall = new Point(w, h);
                            if (start == newWall || goal == newWall) continue;
                            Point[] tempWalls = walls.Concat(new Point[] { newWall }).ToArray();
                            QState tempState = new Maze() { maze = maze, self = self, goal = goal, width = width, height = height, walls = tempWalls};
                            if (!bestPathSoFar.Any() || bestPathSoFar.Contains(newWall))
                            {
                                QSearchResult path = new QSearch().AStar(tempState);
                                if (path != null)
                                {
                                    bestPathSoFar.Clear();
                                    foreach (QState q in path.QStatesList)
                                        bestPathSoFar.Add(((Maze)q).self);
                                    walls = tempWalls;
                                }
                            }
                            else walls = tempWalls;
                        }

                    }
                }
                WriteOutput("Maze generation complete.");
            }

            opponent = new List<Point>();
            for (int i = 0; i < opponents; i++)
                opponent.Add(new Point(goal.X, goal.Y));

            if (!HideOutput)
            {
                ManualResetEvent wait = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(CreateGUI), new object[] { width, height, self.X, self.Y, goal.X, goal.Y, walls, this, wait });
                wait.WaitOne();
            }

            return new Maze() { width = width, height = height, self = self, goal = goal, walls = walls, wallDensity = wallDensity, opponent = opponent, opponentDifficulty = opponentDifficulty, random = random, maze = maze, start = start, opponents = opponents, bestOpponentPath = bestOpponentPath, score = score };
        }

        private void CreateGUI(object o) {
            int width = (int)((object[])o)[0];
            int height = (int)((object[])o)[1];
            int selfx = (int)((object[])o)[2];
            int selfy = (int)((object[])o)[3];
            int goalx = (int)((object[])o)[4];
            int goaly = (int)((object[])o)[5];
            Point[] blocked = (Point[])((object[])o)[6];
            Maze master = (Maze)((object[])o)[7];
            ManualResetEvent w = (ManualResetEvent)((object[])o)[8];
            master.maze = new MazeGUI(width, height, selfx, selfy, goalx, goaly, blocked, opponent);
            w.Set();
            Application.Run(maze);
        }

        public override bool Equals(object obj)
        {
            Point[] wallA = walls.OrderBy(x => x.X).ThenBy(x=>x.Y).ToArray();
            Point[] wallB = ((Maze)obj).walls.OrderBy(x => x.X).ThenBy(x => x.Y).ToArray();
            if (wallA.Length != wallB.Length) return false;
            for (int i = 0; i < wallA.Length; i++)
                if (wallA[i] != wallB[i]) return false;

            List<Point> oppA = opponent.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            List<Point> oppB = ((Maze)obj).opponent.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            if (oppA.Count != oppB.Count) return false;
            for (int i = 0; i < oppA.Count; i++)
                if (oppA[i] != oppB[i]) return false;

            return 
                self==((Maze)obj).self&&
                goal==((Maze)obj).goal&&
                width==((Maze)obj).width&&
                height==((Maze)obj).height;
        }
        public override int GetHashCode()
        {
            return self.X*100000+self.Y*10000+goal.X*1000+goal.Y*100+width*10+height;
        }
        public override QState GetNewState(QAction action)
        {
            
            int newX = self.X, newY = self.Y;

            // Translate decision to new coordinate
            if (action == UP) newY--;
            else if (action == DOWN) newY++;
            else if (action == LEFT) newX--;
            else if (action == RIGHT) newX++;

            Point newPos = new Point(newX, newY);

            // Cannot go there
            if (newY < 0 || newX < 0 || newY >= height || newX >= width || walls.Contains(newPos))
            {
                newX = self.X;
                newY = self.Y;
                newPos = new Point(newX, newY);
            }

            // Pass all variables to new state
            return new Maze() { maze = maze, self = newPos, goal = goal, width=width, height=height, walls=walls, score=score-1, opponent = opponent.ToList(), opponents=opponents, random=random, opponentDifficulty=opponentDifficulty};
        }
        public override QAction[] GetChoices()
        {
            List<QAction> options = new List<QAction>();
            if (self.X > 0 && !walls.Contains(new Point(self.X - 1, self.Y))) options.Add(LEFT);
            if (self.X < width - 1 && !walls.Contains(new Point(self.X + 1, self.Y))) options.Add(RIGHT);
            if (self.Y > 0 && !walls.Contains(new Point(self.X, self.Y - 1))) options.Add(UP);
            if (self.Y < height - 1 && !walls.Contains(new Point(self.X, self.Y + 1))) options.Add(DOWN);
            return options.ToArray();
        }
        public override decimal GetValue()
        {
            return score;
        }
        public override bool IsEnd()
        {
            return self==goal || opponent.Contains(self);
        }
        public override string ToString()
        {
            return "Position: " + self + " | Opponents: (" + string.Join("),(", opponent) + ")";
        }
        public override void Step()
        {

            if (!HideOutput && maze!=null)
            {
                try
                {
                    maze.SetSelf(self.X, self.Y);
                }
                catch (Exception e)
                {
                    WriteOutput("" + e, true);
                }
            }

            if (opponent.Any() && goal!=self)
            {
                for (int i = 0; i < opponent.Count; i++)
                {
                    QState tempState = new Maze() { maze = maze, self = opponent[i], goal = self, width = width, height = height, walls = walls};
                    QSearchResult opponentPath;
                    if (bestOpponentPath.ContainsKey(tempState)) opponentPath = bestOpponentPath[tempState];
                    else
                    {
                        opponentPath = new QSearch().AStar(tempState);
                        bestOpponentPath[tempState] = opponentPath;
                    }
                    if (opponentPath!=null && opponentPath.Any())
                    {
                        int newX = opponent[i].X, newY = opponent[i].Y;

                        QAction action = opponentPath.actionsList.First();

                        if (random.NextDouble() > opponentDifficulty)
                        {
                            QAction[] allActions = tempState.GetChoices();
                            action = allActions.ElementAt(random.Next(allActions.Length));
                        }

                        // Translate decision to new coordinate
                        if (action.ToString() == "up") newY--;
                        else if (action.ToString() == "down") newY++;
                        else if (action.ToString() == "left") newX--;
                        else if (action.ToString() == "right") newX++;

                        //WriteOutput("Moving adversary at " + opponent[i] + " " + action + " to "+newX+", "+newY+"...");
                        opponent[i] = new Point(newX, newY);
                        if (!HideOutput && maze!=null)
                        {
                            maze.SetOpponent(i, newX, newY);
                        }
                    }
                }
            }

            if (goal == self)
            {
                score += 100;
            }
            else if (opponent.Contains(self))
            {
                score -= 100;
            }

            if (!HideOutput && CurrentMode == AWAKEN) Thread.Sleep(100);
        }
        public override void End()
        {
            if (!HideOutput || maze!=null)
            {
                try
                {
                    if(CurrentMode == AWAKEN) Thread.Sleep(500);
                    maze.Quit();
                }
                catch (Exception e)
                {
                    WriteOutput("" + e, true);
                }
            }
        }
        public override bool HasSettings
        {
            get
            {
                return true;
            }
        }
        public override void Settings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>(){
                {"Width", ""+width},
                {"Height", ""+height},
                {"Start-X", ""+start.X},
                {"Start-Y", ""+start.Y},
                {"Goal-X", ""+goal.X},
                {"Goal-Y", ""+goal.Y},
                {"Wall Density", ""+wallDensity},
                {"Opponents", ""+opponents},
                {"Opponent Difficulty", ""+opponentDifficulty}
            };
            Dictionary<string, string> values = NewSettingsForm(settings);
            try
            {
                width = Convert.ToInt32(values["Width"]);
                height = Convert.ToInt32(values["Height"]);
                start = new Point(Convert.ToInt32(values["Start-X"]), Convert.ToInt32(values["Start-Y"]));
                goal = new Point(Convert.ToInt32(values["Goal-X"]), Convert.ToInt32(values["Goal-Y"]));
                wallDensity = Convert.ToDouble(values["Wall Density"]);
                opponents = Convert.ToInt32(values["Opponents"]);
                opponentDifficulty = Convert.ToDouble(values["Opponent Difficulty"]);

                if (width <1) width = 1;
                if (height < 1) height = 1;
                if(width<5 || height<5) walls = new Point[]{};
                if (start.X < 0 || start.X >= width) start.X = 0;
                if (start.Y < 0 || start.Y >= height) start.Y = 0;
                if (goal.X < 0 || goal.X >= width) goal.X = width-1;
                if (goal.Y < 0 || goal.Y >= height) goal.Y = height-1;
                if (wallDensity < 0 || wallDensity > 1) wallDensity = 1;
                if (opponents < 0) opponents = 0;
                if (opponentDifficulty < 0 || opponentDifficulty > 1) opponentDifficulty = 1;
            }
            catch (Exception)
            {
                WriteOutput("Invalid settings! Please only use integer numbers.");
            }
        }

        public override Dictionary<QFeature, decimal> GetFeatures(QAction action)
        {
            Point self = ((Maze)GetNewState(action)).self;
            QSearch qsearch = new QSearch(this);
            Maze simpleMaze = new Maze() { maze = maze, self = self, goal = goal, width = width, height = height, walls = walls.ToArray() };
            QSearchResult bestPath = qsearch.AStar(simpleMaze);

            List<Point> bestOppMoves = new List<Point>();
            foreach (Point o in opponent)
            {
                bestOppMoves.Add(o);
                bestOppMoves.Add(new Point(o.X + 1, o.Y));
                bestOppMoves.Add(new Point(o.X - 1, o.Y));
                bestOppMoves.Add(new Point(o.X, o.Y + 1));
                bestOppMoves.Add(new Point(o.X, o.Y - 1));
            }
            foreach (Point o in bestOppMoves.ToArray())
            {
                bestOppMoves.Add(new Point(o.X + 1, o.Y));
                bestOppMoves.Add(new Point(o.X - 1, o.Y));
                bestOppMoves.Add(new Point(o.X, o.Y + 1));
                bestOppMoves.Add(new Point(o.X, o.Y - 1));
            }
            Maze safeMaze = new Maze() { maze = maze, self = self, goal = goal, width = width, height = height, walls = walls.Concat(bestOppMoves).ToArray() };
            QSearchResult safePath = qsearch.AStar(safeMaze);
            Dictionary<string, decimal> features = new Dictionary<string, decimal>() {
                //{this.GetHashCode().ToString()+"_"+action, 1}, // Identity to convert this back to QLearning
                {"Goal", goal==self? 1:0},
                {"Direct_Distance_To_Goal", bestPath==null? 1:(decimal) bestPath.Count / (decimal)(width * height)},
                {"Safe_Distance_To_Goal", (safePath==null? 1: (decimal)safePath.Count/ (decimal)(width * height))}
            };

            decimal distanceToOpponent = decimal.MaxValue;
            if (opponent.Any())
            {
                features["Opponent"] = goal!=self && opponent.Where(p => (Math.Abs(p.X - self.X) <= 1 && p.Y == self.Y) || (Math.Abs(p.Y - self.Y) <= 1 && p.X == self.X)).Any() ? 1 : 0;

                distanceToOpponent = opponent.Select(o => qsearch.AStar(new Maze() { maze = maze, self = self, goal = o, width = width, height = height, walls = walls })).Select(x=>x==null? width*height:x.Count).Min();
                features["Distance_To_Opponent"] = distanceToOpponent>=5? 1:distanceToOpponent / (decimal)(width * height);

                if (goal != self)
                {
                    Maze deadEnd = new Maze() { maze = maze, self = self, goal = goal, width = width, height = height, walls = walls.Concat(new Point[] { new Point(this.self.X - 1, this.self.Y), new Point(this.self.X + 1, this.self.Y), new Point(this.self.X, this.self.Y - 1), new Point(this.self.X, this.self.Y + 1) }).ToArray() };
                    QSearchResult deadPath = qsearch.Depth_First(deadEnd);
                    if (deadPath == null)
                    {
                        features["Dead_End"] = 1;
                    }
                }
            }

            

            return QFeature_String.FromStringDecimalDictionary(features);
        }

        public override decimal GetValueHeuristic()
        {
            // Manhatten distance
            return -1 * ((decimal)Math.Abs(self.X - goal.X) + Math.Abs(self.Y - goal.Y));
        }

        public override object Save()
        {
            return new object[]{self, start, goal, width, height, opponent, walls, wallDensity, opponentDifficulty};
        }

        public override QState Open(object o)
        {
            object[] oo = (object[]) o;
            return new Maze() { self = (Point)oo[0], start = (Point)oo[1], goal = (Point)oo[2], width = (int)oo[3], height = (int)oo[4], opponent = (List<Point>)oo[5], walls = (Point[])oo[6], wallDensity = (double)oo[7], opponentDifficulty = (double)oo[8] };
        }
    }
}
