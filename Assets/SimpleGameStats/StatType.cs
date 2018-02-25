using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaleranGames.UI;


namespace DaleranGames.SimpleGameStats
{
    [System.Serializable]
    public class StatType : Enumeration
    {
        #region StatTypes

        public static readonly StatType NullStat = new StatType(-1, "Null");


        //Unit Stats
        public static readonly StatType Strength = new StatType(0, "Strength", "Icon_16px_CombatPower".ToSprite(),"Strength is the relative combat power of a unit.");
        public static readonly StatType MaxActionPoints = new StatType(2, "Max Action Points", ("Icon_16px_Good_AP".ToSprite() + "Modifier_16px_Max".ToSprite()).OverlapCharacters());
        public static readonly StatType Happiness = new StatType(3, "Happiness", "Icon_16px_Happiness".ToSprite());
        public static readonly StatType AttackCost = new StatType(4, "Attack Cost", ("Icon_16px_CombatPower".ToSprite() + "Modifier_16px_Minus".ToSprite()).OverlapCharacters());
        #endregion

        #region StatType Object

        [SerializeField]
        protected string _statIcon;
        public string Icon { get { return _statIcon; } }

        [SerializeField]
        protected string _description;
        public string Description { get { return _description; } }

        public StatType () { }

        protected StatType(int value, string displayName) : base(value, displayName)
        {
            _statIcon = "Icon_16_Gen_Question".ToSprite();
            _description = "";
        }

        protected StatType(int value, string displayName, string statIcon) : base(value, displayName)
        {
            _statIcon = statIcon;
            _description = "";
        }

        protected StatType (int value, string displayName, string statIcon, string description) : base (value,displayName)
        {
            _statIcon = statIcon;
            _description = description;
        }

        public string ToStringAll()
        {
            return Value + " " + Name + " - " + Description + "\n" + Icon;
        }

# endregion

    }
}

