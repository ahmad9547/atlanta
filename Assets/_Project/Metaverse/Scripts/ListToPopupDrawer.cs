using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Metaverse
{
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ListToPopupAttribute))]
    public sealed class ListToPopupDrawer : PropertyDrawer
    {
        private const int DefaultValue = 0;
        public override void OnGUI(Rect rect, SerializedProperty serializedProperty, GUIContent guiContent)
        {
            ListToPopupAttribute listToPopupAttribute = (ListToPopupAttribute) attribute;
            List<string> possibleTeleportPoints = null;

            if (listToPopupAttribute.Type.GetField(listToPopupAttribute.PropertyName) != null)
            {
                possibleTeleportPoints = listToPopupAttribute.Type.GetField(listToPopupAttribute.PropertyName).GetValue(listToPopupAttribute.Type) as List<string>;
            }

            if (possibleTeleportPoints != null && possibleTeleportPoints.Count != DefaultValue)
            {
                int selectedIndex = Mathf.Max(possibleTeleportPoints.IndexOf(serializedProperty.stringValue), DefaultValue);

                selectedIndex = EditorGUI.Popup(rect, serializedProperty.name, selectedIndex, possibleTeleportPoints.ToArray());

                serializedProperty.stringValue = possibleTeleportPoints[selectedIndex];
            }
            else
            {
                EditorGUI.PropertyField(rect, serializedProperty, guiContent);
            }
        }
    }
    #endif
}

