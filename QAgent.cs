using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QLearner.QStates;
using QLearner.QAlgos;
using System.Windows.Forms;

namespace QLearner
{
    public class QAgent
    {
        private QState initialState;
        private QAlgo currentAlgo;
        private QProcess currentProcess=null;

        private QLearner master=null;
        private bool mode=QLearner.LEARN;

        private DateTime startTime;
        private int trialNum;
        private decimal learn, discount, explore;
       
        public QAgent(QAlgo algo, QState initialState)
        {
            trialNum = 0;
            this.initialState = initialState;
            initialState.setQAgent(this);

            currentAlgo = algo;
            currentAlgo.setQAgent(this);
            currentAlgo.Initialize();
        }
        public QAgent(QLearner master, QAlgo algo, QState initialState)
            : this(algo, initialState)
        {
            this.master = master;
        }

        public void Learn(int trials, decimal learn, decimal discount, decimal explore)
        {
            try
            {
                mode = QLearner.LEARN;
                for (int i = 1; i <= trials; i++)
                {
                    Run(learn, discount, explore);
                    if (!isRunning) break;
                }
                currentAlgo.End();
            }
            catch (Exception e)
            {
                WriteOutput("Error: " + e, true);
            }
        }
        public void Learn(BackgroundWorker learnProcess, int trials, decimal learn, decimal discount, decimal explore)
        {
            try
            {
                startTime = DateTime.Now;
                currentProcess = new QProcess(learnProcess);
                WriteOutput("Learning now...", true);
                Learn(trials, learn, discount, explore);
                if (master != null) master.UpdateGuiMetrics(true);
                if (isRunning) WriteOutput("I am done learning.", true);
                else WriteOutput("Learning aborted.", true);
                WriteOutput("Runtime: " + (DateTime.Now - startTime), true);
            }
            catch (Exception e)
            {
                WriteOutput("Error: " + e, true);
            }
        }

        public void Awaken(int trials, decimal learn, decimal discount, decimal explore)
        {
            try
            {
                mode = QLearner.AWAKEN;
                for (int i = 1; i <= trials; i++)
                {
                    Run(learn, discount, explore);
                    if (!isRunning) break;
                }
                currentAlgo.End();
            }
            catch (Exception e)
            {
                WriteOutput("Error: " + e, true);
            }
        }
        public void Awaken(BackgroundWorker awakenProcess, int trials, decimal learn, decimal discount, decimal explore)
        {
            try
            {
                startTime = DateTime.Now;
                currentProcess = new QProcess(awakenProcess);
                WriteOutput("Awakening now...", true);
                Awaken(trials, learn, discount, explore);
                if (master != null) master.UpdateGuiMetrics(true);
                if (isRunning)
                    WriteOutput("I am done.", true);
                else
                    WriteOutput("Process aborted.", true);
                WriteOutput("Runtime: " + (DateTime.Now - startTime), true);
            }
            catch (Exception e)
            {
                WriteOutput("Error: " + e, true);
            }
        }

        private void Run(decimal learn, decimal discount, decimal explore)
        {
            trialNum++;
            this.learn = learn;
            this.discount = discount;
            this.explore = explore;
            if (master != null) master.UpdateTrial(trialNum);
            QState state = initialState.Initialize();
            state.Inherit(initialState);

            QState s = currentAlgo.Run(state, trialNum, learn, discount, explore);
            if (master != null)
            {
                master.UpdateScore(s.GetValue());
                master.UpdateGuiMetrics();
            }
            s.End();
        }

        public bool isRunning { get { return currentProcess!=null && currentProcess.isRunning; } }
        public bool CurrentMode { get { return mode; } }

        public void Open(QLearned o)
        {
            if (o.algo != currentAlgo.GetType().Name) master.popup("QLearned file is for the QAlgo '" + o.algo + "', not the '" + currentAlgo.GetType().Name + "' currently selected.");
            else if (o.state != initialState.GetType().Name) master.popup("QLearned file is for the QState '" + o.state + "', not the '" + initialState.GetType().Name + "' currently selected.");
            else
            {
                currentAlgo.Open(o.contents, initialState);
                WriteOutput("Learning data loaded.");
            }
        }

        public QLearned Save()
        {
            return new QLearned(currentAlgo.GetType().Name, initialState.GetType().Name, learn, discount, explore, trialNum, currentAlgo.Save());
        }

        public void WriteOutput(string s = "", bool force=false)
        {
            if(master!=null) master.WriteOutput(s, true, force);
        }
    }
}
