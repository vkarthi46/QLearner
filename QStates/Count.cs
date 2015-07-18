using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearner.QStates
{
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class Count:QState
    {
        private int start=0, end=10, value, downIncrement=1, upIncrement=1;
        private readonly QAction UP = new QAction_String("up"), DOWN = new QAction_String("down");

        public override QState Initialize()
        {
            return new Count() { start = start, end = end, value = start, downIncrement = downIncrement, upIncrement = upIncrement };
        }
        public override bool Equals(object obj)
        {
            return value==((Count)obj).value;
        }
        public override int GetHashCode()
        {
            return value;
        }
        public override QState GetNewState(QAction action)
        {
            return new Count() { start = this.start, end=this.end, value = (action==UP? this.value + this.upIncrement:this.value - this.downIncrement), upIncrement = this.upIncrement, downIncrement = this.downIncrement};
        }
        public override QAction[] GetActions()
        {
            return new QAction[] { UP, DOWN };
        }
        public override decimal GetValue()
        {
            return end - Math.Abs(end-value);
        }
        public override bool IsEnd()
        {
            return value == end;
        }
        public override string ToString()
        {
            return "" + value.ToString("D8");
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
                {"Start", ""+start},
                {"End", ""+end},
                {"Upward Increment", ""+upIncrement},
                {"Downward Increment", ""+downIncrement}
            };
            Dictionary<string, string> values = NewSettingsForm(settings);
            try
            {
                start = Convert.ToInt32(values["Start"]);
                end = Convert.ToInt32(values["End"]);
                upIncrement = Convert.ToInt32(values["Upward Increment"]);
                downIncrement = Convert.ToInt32(values["Downward Increment"]);
            }
            catch (Exception)
            {
                WriteOutput("Invalid settings! Please only use numbers for increment values.");
            }
        }

        public override Dictionary<QFeature, decimal> GetFeatures(QAction action)
        {
            int newVal = value;
            if (action == UP) newVal++;
            else newVal--;
            return QFeature_String.FromStringDecimalDictionary(new Dictionary<string, decimal>() {
                //{value.ToString()+"_"+action, 1}, // Identity for sanity check
                //{"Distance", (decimal)Math.Abs(end-newVal)}
                {"Distance_Change", Math.Abs(end-newVal)-Math.Abs(end-value)}
            });
        }
        public override decimal GetValueHeuristic()
        {
            return -1 * (decimal)Math.Abs(end - value);
        }

        public override QState Open(object o)
        {
            object[] a = (object[])o;
            return new Count() { start = (int)a[0], end = (int)a[1], value = (int)a[2], upIncrement = (int)a[3], downIncrement = (int)a[4] };
        }

        public override object Save()
        {
            return new object[] { start, end, value, upIncrement, downIncrement };
        }
    }
}
