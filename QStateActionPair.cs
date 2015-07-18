using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;

namespace QLearner
{
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    public class QStateActionPair
    {
        public QState state;
        public QAction action;

        public QStateActionPair(QState s, QAction a)
        {
            state = s;
            action = a;
        }
        public override bool Equals(object obj)
        {

            return state.Equals(((QStateActionPair)obj).state) && action == ((QStateActionPair)obj).action;
        }
        public override int GetHashCode()
        {
            return (state.GetHashCode().ToString()+"_"+action).GetHashCode();
        }

        public override string ToString()
        {
            return string.Join("_", new string[]{state.ToString(),  action.ToString()});
        }
    }
}
