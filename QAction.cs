using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace QLearner
{
    [Serializable]
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    [System.Reflection.ObfuscationAttribute(Feature = "properties renaming")]
    public abstract class QAction : ISerializable 
    {
        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);
        public static bool operator ==(QAction a, QAction b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(QAction a, QAction b)
        {
            return !(a == b);
        }
    }

    [Serializable]
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    [System.Reflection.ObfuscationAttribute(Feature = "properties renaming")]
    public class QAction_String : QAction
    {
        private string text = "";
        public QAction_String(string t)
        {
            text = t;
        }

        public QAction_String(SerializationInfo info, StreamingContext context)
        {
            text = (string)info.GetValue("text", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("text", text);
        }

        public override bool Equals(object obj)
        {
            return text == ((QAction_String)obj).text;
        }
        public override int GetHashCode()
        {
            return text.GetHashCode();
        }
        public override string ToString()
        {
            return text;
        }

    }
}
