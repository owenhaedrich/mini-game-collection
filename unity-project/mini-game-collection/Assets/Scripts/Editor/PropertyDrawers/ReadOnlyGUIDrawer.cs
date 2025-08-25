using UnityEditor;
using UnityEngine;

namespace MiniGameCollection
{
    /// <summary>
    ///     Make editor field read-only.
    /// </summary>
    /// <seealso cref="https://discussions.unity.com/t/how-to-make-a-readonly-property-in-inspector/75448"/>
    [CustomPropertyDrawer(typeof(ReadOnlyGUIAttribute))]
    public class ReadOnlyGUIDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
