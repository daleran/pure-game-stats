using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DaleranGames.SimpleGameStats
{
    public class StatText : MonoBehaviour
    {
        [SerializeField]
        protected StatBehaviour stats;

        [SerializeField]
        protected StatType trackedType;

        protected TextMeshProUGUI label;

        // Use this for initialization
        protected virtual void Awake()
        {
            label = GetComponent<TextMeshProUGUI>();
            stats.StatModified += OnStatChange;
            label.text = trackedType.Icon + stats[trackedType];
        }

        protected virtual void OnDestroy()
        {
            stats.StatModified -= OnStatChange;
        }

        protected virtual void OnStatChange(IStatCollection<StatType> col, StatType type)
        {
            if (type == trackedType)
            {
                label.text = type.Icon + stats[type];
            }
        }
    }
}

