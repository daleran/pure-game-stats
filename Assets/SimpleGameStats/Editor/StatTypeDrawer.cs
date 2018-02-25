using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DaleranGames.SimpleGameStats
{
    [CustomPropertyDrawer(typeof(StatType), false)]
    public class StatTypeDrawer : PropertyDrawer
    {
        static List<StatType> statTypes = Enumeration.GetAll<StatType>().ToList();
        string[] statNames = GetNames(statTypes);



        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string name = property.displayName;

            SerializedProperty statName = property.FindPropertyRelative("_name");
            SerializedProperty statValue = property.FindPropertyRelative("_value");
            SerializedProperty statIcon = property.FindPropertyRelative("_statIcon");
            SerializedProperty statDescription = property.FindPropertyRelative("_description");

            int index = 0;
            for (int i = 0; i < statNames.Length; i++)
            {
                if (statNames[i] == statName.stringValue)
                    index = i;
            }

            // Begin/end property & change check make each field
            // behave correctly when multi-object editing.
            EditorGUI.BeginProperty(position, label, property);
            {
                EditorGUI.BeginChangeCheck();
                if (label == GUIContent.none)
                    index = EditorGUI.Popup(position, index, statNames);
                else
                    index = EditorGUI.Popup(position, name, index, statNames);

                if (EditorGUI.EndChangeCheck())
                {
                    StatType selected = Enumeration.FromName<StatType>(statNames[index]);

                    statName.stringValue = selected.Name;
                    statValue.intValue = selected.Value;
                    statIcon.stringValue = selected.Icon;
                    statDescription.stringValue = selected.Description;
                }               
            }
            EditorGUI.EndProperty();
        }

        static string[] GetNames(List<StatType> types)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < types.Count; i++)
            {
                names.Add(types[i].Name);
            }
            return names.ToArray();
        }
    }
}
