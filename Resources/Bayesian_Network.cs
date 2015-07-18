using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;
using System.ComponentModel;

namespace QLearner.Resources
{
    /*
     * Bayesian networks are a data structure for storing conditional probabilities and querying information from them.
     * This is still under construction as of 1.3 and is not functional yet.  If anyone would like to help with this and other algos, please let me know. -pftq
     * */
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class Bayesian_Network
    {
        public Dictionary<QFeature, Node> nodes;

        public Bayesian_Network()
        {
            nodes = new Dictionary<QFeature, Node>();
        }

        private void AddNode(QFeature feature)
        {
            nodes[feature] = new Node(feature);
        }

        public bool HasFeature(QFeature f)
        {
            return nodes.ContainsKey(f);
        }

        public bool HasFeature(QFeature f, decimal v)
        {
            return nodes.ContainsKey(f) && nodes[f].conditionalProbabilityTable.ContainsKey(v);
        }

        public QFeature[] GetFeatures()
        {
            return nodes.Keys.ToArray();
        }

        public decimal[] GetValues(QFeature feature)
        {
            return nodes.ContainsKey(feature) ? nodes[feature].conditionalProbabilityTable.Keys.ToArray() : new decimal[] { };
        }

        public Clause[] GetConditions(QFeature feature, decimal value)
        {
            return nodes.ContainsKey(feature) && nodes[feature].conditionalProbabilityTable.ContainsKey(value)? nodes[feature].conditionalProbabilityTable[value].Keys.ToArray() : new Clause[] { };
        }

        public void SetProbability(QFeature feature, decimal value, Clause condition, decimal probability)
        {
            if (!nodes.ContainsKey(feature)) AddNode(feature);
            nodes[feature].AddProbability(value, condition, probability);
        }

        public decimal GetProbability(QFeature feature, decimal value)
        {
            return nodes.ContainsKey(feature)? nodes[feature].GetProbability(value):0;
        }

        public decimal GetProbability(QFeature feature, decimal value, Clause condition)
        {
            return nodes.ContainsKey(feature) ? nodes[feature].GetProbability(value, condition) : 0;
        }

        public decimal GetProbability(Clause clause)
        {
            if (clause.variables.Any())
            {
                decimal p = 1;
                foreach (KeyValuePair<QFeature, decimal> kv in clause.variables)
                {
                    if (nodes.ContainsKey(kv.Key))
                    {
                        Clause c = new Clause();
                        foreach (Node n in nodes[kv.Key].parents.Where(parent => clause.variables.ContainsKey(parent.feature)))
                            c.AddVariable(n.feature, clause.variables[n.feature]);
                        p *= nodes[kv.Key].GetProbability(kv.Value, c);
                    }
                    else return 0;
                }
                return p;
            }
            else return 0;
        }

        public class Node
        {
            public QFeature feature;
            public HashSet<Node> parents;

            // feature value, parent condition, probability
            public Dictionary<decimal, Dictionary<Clause, decimal>> conditionalProbabilityTable;

            public Node(QFeature feature)
            {
                this.feature = feature;
                parents = new HashSet<Node>();
                conditionalProbabilityTable = new Dictionary<decimal, Dictionary<Clause, decimal>>(); // value, condition, probability
            }

            public void AddParent(Node n)
            {
                if(!parents.Contains(n)) parents.Add(n);
            }

            public void AddProbability(decimal value, Clause condition, decimal probability)
            {
                if (!conditionalProbabilityTable.ContainsKey(value))
                {
                    conditionalProbabilityTable[value] = new Dictionary<Clause, decimal>();
                }
                conditionalProbabilityTable[value][condition] = probability;
            }

            public decimal GetProbability(decimal value)
            {
                return conditionalProbabilityTable.ContainsKey(value) ? conditionalProbabilityTable[value].Sum(x => x.Value) : 0;
            }

            public decimal GetProbability(decimal value, Clause condition)
            {
                return conditionalProbabilityTable.ContainsKey(value) && conditionalProbabilityTable[value].ContainsKey(condition)? conditionalProbabilityTable[value][condition]:0;
            }

            public string ToString(decimal value, Clause condition)
            {
                return feature + "=" + value + (condition.Any() ? "|" + string.Join(",", condition.variables.Select(x => x.Key + "=" + x.Value)) : "");
            }

            public override bool Equals(object obj)
            {
                return feature == ((Node)obj).feature;
            }

            public override string ToString()
            {
                return feature.ToString();
            }
            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }
        }

        public class Clause
        {
            public Dictionary<QFeature, decimal> variables;
            
            public Clause()
            {
                variables = new Dictionary<QFeature, decimal>();
            }
            public bool Any()
            {
                return variables.Any();
            }
            public void AddVariable(QFeature feature, decimal value)
            {
                variables[feature] = value;
            }
            public override bool Equals(object obj)
            {
                Dictionary<QFeature, decimal> other = ((Clause)obj).variables;
                if (variables.Count != other.Count) return false;
                foreach (KeyValuePair<QFeature, decimal> kv in variables)
                    if (!other.ContainsKey(kv.Key) || other[kv.Key]!=kv.Value) return false;
                return true;
            }
            public override string ToString()
            {
                return (variables.Any()? string.Join(",", variables.Select(x=>x.Key+"="+x.Value)):"");
            }
            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }
        }
    }
}
