using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearner.QStates
{
    public class Count_To_100:QState
    {
        private int value, downIncrement, upIncrement;
        private const int CountTo = 100; // The ending count

        public override QState Initialize()
        {
            value = 0;
            upIncrement = downIncrement = 1;
            return new Count_To_100() { value = value, downIncrement = downIncrement, upIncrement = upIncrement };
        }
        public override bool Equals(object obj)
        {
            return value==((Count_To_100)obj).value;
        }
        public override int GetHashCode()
        {
            return value;
        }
        public override QState GetNewState(string action)
        {
            if (action == "up") return new Count_To_100() { value = this.value + this.upIncrement, upIncrement = this.upIncrement, downIncrement = this.downIncrement};
            else return new Count_To_100() { value = this.value - this.downIncrement, upIncrement = this.upIncrement, downIncrement = this.downIncrement };
        }
        public override string[] GetActions()
        {
            return new string[] { "up", "down" };
        }
        public override decimal GetValue()
        {
            return -1*Math.Abs(value-CountTo); // The farther you're away, the worse the value
        }
        public override bool IsEnd()
        {
            return value == CountTo;
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
                {"Upward Increment", ""+upIncrement},
                {"Downward Increment", ""+downIncrement}
            };
            Dictionary<string, string> values = NewSettingsForm(settings);
            try
            {
                upIncrement = Convert.ToInt32(values["Upward Increment"]);
                downIncrement = Convert.ToInt32(values["Downward Increment"]);
            }
            catch (Exception)
            {
                WriteOutput("Invalid settings! Please only use numbers for increment values.");
            }
        }

        public override Dictionary<string, decimal> Features(string action)
        {
            int newVal = value;
            if (action == "up") newVal++;
            else newVal--;
            //popup("Distance from " + newVal + ": " + dst);
            return new Dictionary<string, decimal>() {
                //{value.ToString()+"_"+action, 1}, // Identity for sanity check
                {"Distance_Change", newVal-value}
            };
        }

        public override QState Open(object o)
        {
            object[] a = (object[])o;
            return new Count_To_100() { value = (int)a[0], upIncrement = (int)a[1], downIncrement = (int)a[2] };
        }

        public override object Save()
        {
            return new object[] { value, upIncrement, downIncrement };
        }

    }
}
