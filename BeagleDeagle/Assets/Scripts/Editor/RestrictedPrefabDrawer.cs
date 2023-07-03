using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RestrictedPrefabAttribute))]
public class RestrictedPrefabDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var restrictedPrefabAttribute = attribute as RestrictedPrefabAttribute;

        if (restrictedPrefabAttribute != null)
        {
            var componentType = restrictedPrefabAttribute.ComponentType;
            var objectReferenceValue = property.objectReferenceValue;

            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.ObjectField(position, label, objectReferenceValue, typeof(GameObject), false);

            if (EditorGUI.EndChangeCheck())
            {
                if (newValue == null || ((GameObject)newValue).GetComponent(componentType) != null)
                {
                    property.objectReferenceValue = newValue;
                }
                else
                {
                    Debug.LogError("Invalid prefab selected. The prefab must have the required component: " + componentType);
                }
            }
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}