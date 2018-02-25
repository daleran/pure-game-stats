using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaleranGames.SimpleGameStats
{
    public class StatBehaviour : MonoBehaviour, IStatCollection<StatType>
    {
        StatCollection stats = new StatCollection();

        [SerializeField]
        List<StatView> statView = new List<StatView>();

        public event Action<IStatCollection<StatType>, StatType> StatModified;
        protected virtual void RaiseEvent(IStatCollection<StatType> col, StatType type)
        {
            StatModified?.Invoke(this, type);
        }

        public virtual int Count { get { return stats.Count; } }

        protected virtual void Awake()
        {
            stats.StatModified += RaiseEvent;
        }

        protected virtual void OnDestroy()
        {
            stats.StatModified -= RaiseEvent;
        }

        public virtual float this[StatType statType] { get { return stats[statType]; } }
        public virtual List<StatType> Types { get { return stats.Types; } }
        public virtual List<Modifier> GetAllOfType(StatType statType)
        {
            return stats.GetAllOfType(statType);
        }

        public virtual List<Modifier> GetAll()
        {
            return stats.GetAll();
        }

        public virtual bool Contains(StatType statType)
        {
            return stats.Contains(statType);
        }

        public virtual void Add(Modifier mod)
        {
            stats.Add(mod);
            bool found = false;

            for (int i = 0; i < statView.Count; i++)
            {
               if (statView[i].Type == mod.Type)
                {
                    found = true;
                    statView[i].Modifiers.Add(new ModifierView(mod));
                    statView[i].Value = stats[mod.Type];
                }
            }

            if (!found)
            {
                StatView newView = new StatView(mod.Type);
                newView.Modifiers.Add(new ModifierView(mod));
                newView.Value = stats[mod.Type];
                statView.Add(newView);
            }
        }

        public virtual void Add(IList<Modifier> mods)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                Add(mods[i]);
            }
        }

        public virtual void Remove(Modifier mod)
        {
            stats.Remove(mod);

            for (int i = 0; i < statView.Count; i++)
            {
                if(statView[i].Type == mod.Type)
                {
                    for (int j = 0; j < statView[i].Modifiers.Count; j++)
                    {
                        if (statView[i].Modifiers[j].TrackedModifier.Equals(mod))
                            statView[i].Modifiers.RemoveAt(j);
                    }
                    statView[i].Value = stats[mod.Type];
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

        public virtual void Clear(StatType statType)
        {
            stats.Clear(statType);

            for (int i = 0; i < statView.Count; i++)
            {
                if (statView[i].Type == statType)
                {
                    statView[i].Modifiers.Clear();
                    statView[i].Value = 0;
                }
            }
        }

        public virtual void ClearAll()
        {
            stats.ClearAll();
            statView.Clear();
        }

        [System.Serializable]
        public class StatView
        {
            
            public StatType Type;
            [ReadOnly]
            public float Value = 0;
            [ReadOnly]
            public List<ModifierView> Modifiers;


            public StatView(StatType type)
            {
                this.Type = type;
                Modifiers = new List<ModifierView>();
            }
        }

        [System.Serializable]
        public class ModifierView
        {
            Modifier trackedMod;
            public Modifier TrackedModifier { get { return trackedMod; } }

            [SerializeField]
            [ReadOnly]
            string displayText;

            public ModifierView(Modifier mod)
            {
                trackedMod = mod;
                displayText = mod.ToSimpleString();
            }
        }

    }
}

