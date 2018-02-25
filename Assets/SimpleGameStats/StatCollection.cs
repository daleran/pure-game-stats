using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DaleranGames.UI;

namespace DaleranGames.SimpleGameStats
{
    [System.Serializable]
    public class StatCollection : IStatCollection<StatType>
    {
        protected Dictionary<StatType, List<Modifier>> modifiers;
        protected Dictionary<StatType, float> totals;

        public event Action<IStatCollection<StatType>, StatType> StatModified;

        public int Count { get { return totals.Count; } }

        public static readonly NullStatCollection Null = new NullStatCollection();

        public StatCollection()
        {
            modifiers = new Dictionary<StatType, List<Modifier>>();
            totals = new Dictionary<StatType, float>();
        }

        public virtual float this[StatType statType]
        {
            get
            {
                if (totals.ContainsKey(statType))
                    return totals[statType];
                else
                    return 0f;
            }
        }

        public virtual List<StatType> Types
        {
            get
            {
                return new List<StatType>(modifiers.Keys);
            }
        }

        public virtual List<Modifier> GetAllOfType(StatType statType)
        {
            if (modifiers.ContainsKey(statType))
                return new List<Modifier>(modifiers[statType]);
            else
                return null;
        }

        public virtual List<Modifier> GetAll()
        {
            List<Modifier> mods = new List<Modifier>();
            foreach(KeyValuePair<StatType,List<Modifier>> m in modifiers)
            {
                mods.AddRange(m.Value);
            }
            return mods;
        }

        public virtual bool Contains(StatType statType)
        {
            return modifiers.ContainsKey(statType);
        }

        public virtual void Add (Modifier mod)
        {
            if (mod.Value != 0)
            {
                if (!modifiers.ContainsKey(mod.Type))
                {
                    modifiers.Add(mod.Type, new List<Modifier>());
                    totals.Add(mod.Type, 0);
                }
                modifiers[mod.Type].Add(mod);
                totals[mod.Type] = RecalculateStatType(mod.Type);
                RaiseStatModified(this, mod.Type);
            }
        }
        
        public virtual void Add (IList<Modifier> mods)
        {
            for (int i=0; i < mods.Count; i++)
            {
                Add(mods[i]);
            }
        }

        public virtual void Remove (Modifier mod)
        {
            if (modifiers.ContainsKey(mod.Type))
            {
                if (modifiers[mod.Type].Contains(mod))
                {
                    modifiers[mod.Type].Remove(mod);
                    totals[mod.Type] -= mod.Value;
                    RaiseStatModified(this, mod.Type);
                }
            }
        }

        public virtual void Remove(IList<Modifier> mods)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                Remove(mods[i]);
            }
        }

        public virtual void Clear (StatType statType)
        {
            if (totals.ContainsKey(statType))
            {
                totals[statType] = 0;

                RaiseStatModified(this, statType);
            }
            if (modifiers.ContainsKey(statType))
            {
                modifiers[statType].Clear();

                RaiseStatModified(this, statType);
            }
        }

        public virtual void ClearAll()
        {
            modifiers.Clear();
            totals.Clear();
        }

        protected virtual void RaiseStatModified(IStatCollection<StatType> stats, StatType statType)
        {
            if (StatModified != null)
                StatModified(stats, statType);
        }

        protected virtual float RecalculateStatType(StatType type)
        {
            List<Modifier> mods = modifiers[type];
            float baseValue = 0f;
            float multiplier = 1f;


            for (int i = 0; i < mods.Count; i++)
            {
                if (mods[i].Multiplier)
                    multiplier += mods[i].Value;
                else
                    baseValue += mods[i].Value;
            }

            return baseValue * multiplier;
        }

        public virtual string Info
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                bool first = true;
                foreach (KeyValuePair<StatType, float> kvp in totals)
                {
                    if (first == true)
                    {
                        first = false;
                    }
                    else
                        sb.AppendLine();

                    sb.Append(kvp.Key.Name);
                    sb.Append(kvp.Key.Icon + " ");
                    sb.Append(TextUtilities.ColorBasedOnNumber(kvp.Value.ToString(), kvp.Value, false));
                }
                return sb.ToString();
            }
        }


    }
}
