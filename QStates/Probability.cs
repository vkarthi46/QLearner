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
    public class Probability:QState
    {
        private readonly QAction _ = new QAction_String(""), A = new QAction_String("A"), B = new QAction_String("B"), C = new QAction_String("C");
        private QAction state = new QAction_String("");
        public override QState Initialize()
        {
            return new Probability() { };
        }


        public override bool Equals(object obj)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return 0;
        }
        public override QState GetNewState(QAction action)
        {
            return new Probability() { state = action };
        }
        public override QAction[] GetChoices()
        {
            return new QAction[] { A, B, C };
        }
        public override decimal GetValue()
        {
            return new Dictionary<QAction, decimal>() {
                {_, 0},
                {A, 1},
                {B, 2},
                {C, 1}
            }[state];
        }
        public override Dictionary<QStateActionPair, QState> GetObservedStates(QState prevState, QAction action)
        {
            return new Dictionary<QStateActionPair, QState>() {
                {new QStateActionPair(prevState, _), new Probability() { state = _ }},
                {new QStateActionPair(prevState, A), new Probability() { state = A }},
                {new QStateActionPair(prevState, B), new Probability() { state = B }},
                {new QStateActionPair(prevState, C), new Probability() { state = C }}
            };
        }
        public override bool IsEnd()
        {
            return state.ToString()!="";
        }
        public override string ToString()
        {
            return state.ToString();
        }

        public override void Step()
        {
            
        }

        public override void End()
        {

        }


    }
}
