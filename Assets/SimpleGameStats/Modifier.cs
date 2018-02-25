using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DaleranGames.UI;

namespace DaleranGames.SimpleGameStats
{
    [System.Serializable]
    public struct Modifier : IFormattable, IEquatable<Modifier>, IComparable<Modifier>, IComparable
    {
        [SerializeField]
        StatType type;
        public StatType Type { get { return type; } }

        [SerializeField]
        float value;
        public float Value { get { return value; } }

        [SerializeField]
        bool multiplier;
        public bool Multiplier { get { return multiplier; } }

        [SerializeField]
        sbyte priority;
        public sbyte Priority { get { return priority; } }

        [SerializeField]
        [TextArea(2,5)]
        string description;
        public string Description { get { return description; } }

        public Modifier (StatType type, float value, bool multiplier = false, sbyte priority = 0, string description = "")
        {
            this.type = type;
            this.value = value;
            this.multiplier = multiplier;
            this.priority = priority;
            this.description = description;
        }


        public static Modifier ParseCSV(List<string> csvLine, int startingIndex)
        {
            return new Modifier(Enumeration.FromName<StatType>(csvLine[startingIndex]),float.Parse(csvLine[startingIndex+1]),Boolean.Parse(csvLine[startingIndex + 2]),SByte.Parse(csvLine[startingIndex+3]),csvLine[startingIndex+4]);            
        }

        public static List<Modifier> ParseCSVList (List<string> csvList)
        {
            List<Modifier> items = new List<Modifier>();
            for (int i=0; i < csvList.Count; i+=3)
            {
                items.Add(ParseCSV(csvList, i));
            }
            return items;
        }

        public override string ToString()
        {
            if (multiplier)
                return string.Format("{0} ({1}%) {2}", Type, Value, Description);
            else
                return string.Format("{0} ({1}) {2}", Type, Value, Description);

        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString();
        }

        public string FormatedValue(bool color = false, bool withPlus = false)
        {
            string val = Value.ToString();

            if (multiplier)
               val = (Value*100f).ToString()+ "%";
            else
               val = Value.ToString();

            if (color)
                val = TextUtilities.ColorBasedOnNumber(val, Value, withPlus);

            return val;
        }

        public string FormatedDescription (bool color = false)
        {
            string desc = Description;

            if (color)
                desc = TextUtilities.ColorBasedOnNumber(desc,Value);

            return desc;
        }

        public string ToDisplayString(bool showTypeName = true, bool showIcon=true, bool showDescription = false, bool colorBasedOnValue = false, bool withPlusSign = false)
        {
            string output = FormatedValue(colorBasedOnValue, withPlusSign);

            if (showTypeName)
                output = Type.ToString() + ": " + output;

            if (showIcon)
                output += Type.Icon;

            if (showDescription)
                output += " " + FormatedDescription(colorBasedOnValue);

            return output;
        }

        public string ToSimpleString()
        {
            if (multiplier)
                return (Value * 100f).ToString() + "% " + Description;
            else
                return Value.ToString() +" "+ Description;
        }

        public bool Equals(Modifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Type == Type && other.Value == Value  && other.Description == Description;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(Modifier) && Equals((Modifier)obj);
        }

        public int CompareTo(Modifier other)
        {
            if (other.Type == Type)
            {
                return Value.CompareTo(other.Value);
            }
            else
            {
                if (other.Type.Value > Type.Value)
                    return 1;
                else if (other.Type.Value == Type.Value)
                    return 0;
                else
                    return -1;
            }
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;

            if (obj.GetType() == typeof(Modifier) && Equals((Modifier)obj))
                return CompareTo((Modifier)obj);
            else
                throw new ArgumentException("Object is not a Modifer");
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Description.GetHashCode()) ^ Type.GetHashCode();
            }
        }
        /*
        public static implicit operator int(Modifier m)
        {
            return m.Value;
        }

        public static implicit operator StatType(Modifier m)
        {
            return m.Type;
        }

        public static implicit operator Stat(Modifier m)
        {
            return new Stat(m.type, m.value);
        }
        */
    }
}
