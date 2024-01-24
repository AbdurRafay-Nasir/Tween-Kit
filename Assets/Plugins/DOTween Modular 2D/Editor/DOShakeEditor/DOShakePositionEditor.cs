#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOShakePosition))]
    [CanEditMultipleObjects]
    public class DOShakePositionEditor : DOShakeBaseEditor
    {
        private SerializedProperty strengthProp;
        private SerializedProperty snappingProp;

        protected override void DrawValues()
        {
            EditorGUILayout.PropertyField(strengthProp);
            EditorGUILayout.PropertyField(snappingProp);

            base.DrawValues();
        }

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            strengthProp = serializedObject.FindProperty("strength");
            snappingProp = serializedObject.FindProperty("snapping");
        }
    }
}

#endif