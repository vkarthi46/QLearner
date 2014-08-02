using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace QLearner
{
    public class QProcess
    {
        private BackgroundWorker b;
        public QProcess(BackgroundWorker b)
        {
            this.b = b;
        }
        public bool isRunning { get { return !b.CancellationPending; } }
    }
}
