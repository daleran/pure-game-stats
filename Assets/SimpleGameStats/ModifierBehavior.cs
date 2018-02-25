using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaleranGames.SimpleGameStats
{
    public class ModifierBehavior : MonoBehaviour
    {

        public StatBehaviour Stats;
        public List<Modifier> Modifiers;


        protected void OnEnable()
        {
            Stats.Add(Modifiers);
        }

        protected void OnDisable()
        {
            Stats.Remove(Modifiers);
        }
    }
}

