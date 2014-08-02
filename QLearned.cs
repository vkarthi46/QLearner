using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;
using QLearner.QAlgos;
using System.Runtime.Serialization;

namespace QLearner
{
    [Serializable]
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    public class QLearned:ISerializable
    {
        public string algo;
        public string state;
        public decimal learn, discount, explore;
        public int trials;
        public object contents;

        public QLearned(string algo, string state, decimal learn, decimal discount, decimal explore, int trials, object contents)
        {
            this.algo = algo;
            this.state = state;
            this.learn = learn;
            this.discount = discount;
            this.explore = explore;
            this.trials = trials;
            this.contents = contents;
        }
        public override bool Equals(object obj)
        {

            return algo == ((QLearned)obj).algo
                && state == ((QLearned)obj).state
                && learn == ((QLearned)obj).learn
                && discount == ((QLearned)obj).discount
                && explore == ((QLearned)obj).explore
                && trials == ((QLearned)obj).trials
                && contents == ((QLearned)obj).contents;
        }
        public override int GetHashCode()
        {
            return (algo+"_"+state+"_"+contents.GetHashCode().ToString()).GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("algo", algo);
            info.AddValue("state", state);
            info.AddValue("learn", learn);
            info.AddValue("discount", discount);
            info.AddValue("explore", explore);
            info.AddValue("trials", trials);
            info.AddValue("contents", contents);
        }
        public QLearned(SerializationInfo info, StreamingContext context)
        {
            algo = info.GetString("algo");
            state = info.GetString("state");
            learn = info.GetDecimal("learn");
            discount = info.GetDecimal("discount");
            explore = info.GetDecimal("explore");
            trials = info.GetInt32("trials");
            contents = info.GetValue("contents", typeof(object));
        }
    }
}

