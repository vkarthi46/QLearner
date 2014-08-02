using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLearner.QStates
{
    public partial class MazeGUI : Form
    {
        private int currentSelfX = -1, currentSelfY = -1, currentGoalX = -1, currentGoalY = -1;
        private List<Point> opponent;

        public MazeGUI(int x, int y, int selfx, int selfy, int goalx, int goaly, Point[] blocked, List<Point> opponent)
        {
            InitializeComponent();
            CenterToParent();
            //BringToFront();
            Width = 60+x*25;
            Height = 60+y*25;

            this.opponent = opponent.ToList();

            for (int i = 0; i < x; i++)
            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "" + i;
                c.HeaderText = "" + i;
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = Math.Max(1,100 / x);
                grid.Columns.Add(c);
            }
            for (int i = 0; i < y; i++)
            {
                grid.Rows.Add("");
                grid.Rows[i].HeaderCell.Value = "" + i;
            }

            foreach (Point p in blocked)
            {
                if(p.Y<=y && p.X <= x && p.X>=0 && p.Y>=0)
                    grid.Rows[p.Y].Cells[p.X].Style.BackColor = Color.Black;
            }

            SetSelf(selfx, selfy);
            SetGoal(goalx, goaly);

            for(int i=0; i<opponent.Count; i++)
                SetOpponent(i, opponent[i].X, opponent[i].Y);

        }

        private delegate void QuitD();
        public void Quit()
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new QuitD(Quit));
            }
            else
            {
                Close();
            }
        }

        private delegate void SetSelfD(int x, int y);
        public void SetSelf(int x, int y)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new SetSelfD(SetSelf),x, y);
            }
            else if (currentSelfX != x || currentSelfY != y)
            {
                if (currentSelfY>=0 && grid.Rows[currentSelfY].Cells[currentSelfX].Value.ToString() == "O") 
                    grid.Rows[currentSelfY].Cells[currentSelfX].Value = "";
                grid.Rows[y].Cells[x].Value = "O";
                currentSelfX = x; currentSelfY = y;
                if (x == currentGoalX && y == currentGoalY)
                    grid.Rows[y].Cells[x].Style.BackColor = Color.Green;
            }   
        }

        private delegate void SetGoalD(int x, int y);
        public void SetGoal(int x, int y)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new SetGoalD(SetGoal),x, y);
            }
            else if (currentGoalX != x || currentGoalY != y)
            {
                if (currentGoalY>=0 && grid.Rows[currentGoalY].Cells[currentGoalX].Value.ToString() == "G") 
                    grid.Rows[currentGoalY].Cells[currentGoalX].Value = "";
                grid.Rows[y].Cells[x].Value = "G";
                currentGoalX = x; currentGoalY = y;
            }
        }

        private delegate void SetOpponentD(int i, int x, int y);
        public void SetOpponent(int i, int x, int y)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new SetOpponentD(SetOpponent), i, x, y);
            }
            else if (opponent[i].X != x || opponent[i].Y != y)
            {
                if (opponent[i].Y>=0 && grid.Rows[opponent[i].Y].Cells[opponent[i].X].Value.ToString() == "X")
                    grid.Rows[opponent[i].Y].Cells[opponent[i].X].Value = "";
                if (!(x == currentGoalX && y == currentGoalY)) grid.Rows[y].Cells[x].Value = "X";
                if (currentSelfX == x && currentSelfY == y) grid.Rows[y].Cells[x].Style.BackColor = Color.Red;
                opponent[i] = new Point(x, y);
            }
        }

    }
}
