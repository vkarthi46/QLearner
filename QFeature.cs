using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace QLearner
{
    [Serializable]
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    [System.Reflection.ObfuscationAttribute(Feature = "properties renaming")]
    public abstract class QFeature : ISerializable 
    {
        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);
        public static bool operator ==(QFeature a, QFeature b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(QFeature a, QFeature b)
        {
            return !(a == b);
        }
    }

    [Serializable]
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    [System.Reflection.ObfuscationAttribute(Feature = "properties renaming")]
    public class QFeature_String : QFeature
    {
        private string text = "";
        public QFeature_String(string t)
        {
            text = t;
        }

        public QFeature_String(SerializationInfo info, StreamingContext context)
        {
            text = (string)info.GetValue("text", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("text", text);
        }

        public override bool Equals(object obj)
        {
            return text == ((QFeature_String)obj).text;
        }
        public override int GetHashCode()
        {
            return text.GetHashCode();
        }
        public override string ToString()
        {
            return text;
        }

        public static Dictionary<QFeature, decimal> FromStringDecimalDictionary(Dictionary<string, decimal> d)
        {
            Dictionary<QFeature, decimal> f = new Dictionary<QFeature, decimal>();
            foreach (KeyValuePair<string, decimal> kv in d)
                f[new QFeature_String(kv.Key)] = kv.Value;
            return f;
        }
    }
}
