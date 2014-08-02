using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace QLearner.QStates
{
    public partial class TicTacToeGUI : Form
    {
        private DataGridViewCellEventHandler gridClicker;
        private ManualResetEvent wait;
        public Point selected;

        public TicTacToeGUI()
        {
            InitializeComponent();
            CenterToParent();
            //BringToFront();
            Width = 150;
            Height = 150;

            for (int i = 0; i < 3; i++)
            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "" + i;
                c.HeaderText = "" + i;
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 33;
                grid.Columns.Add(c);
            }
            for (int i = 0; i < 3; i++)
            {
                grid.Rows.Add("");
                grid.Rows[i].HeaderCell.Value = "" + i;
            }

            gridClicker = new DataGridViewCellEventHandler(grid_Click);

        }

        public delegate void PromptD(ManualResetEvent m);
        public void Prompt(ManualResetEvent m)
        {
            if (InvokeRequired) Invoke(new PromptD(Prompt), m);
            else
            {
                wait = m;
                grid.CellClick += gridClicker;
            }
        }

        private void grid_Click(object sender, DataGridViewCellEventArgs e)
        {
            grid.CellClick -= gridClicker;
            if (grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()=="")
            {
                selected = new Point(e.ColumnIndex, e.RowIndex);
                wait.Set();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (wait != null)
            {
                selected = new Point(-1, -1);
                wait.Set();
            }
            base.OnFormClosing(e);
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

        private delegate void SetD(string type, int x, int y);
        public void Set(string type, int x, int y)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new SetD(Set), type, x, y);
            }
            else
            {
                grid.Rows[y].Cells[x].Value = type;
            }
        }

    }
}
