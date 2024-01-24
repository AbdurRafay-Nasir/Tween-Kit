#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular2D.Editor
{

    [CustomEditor(typeof(DOPunchPosition))]
    [CanEditMultipleObjects]
    public class DOPunchPositionEditor : DOPunchBaseEditor
    {
        private SerializedProperty punchAmountProp;
        private SerializedProperty snappingProp;

        protected override void DrawValues()
        {
            EditorGUILayout.PropertyField(punchAmountProp);
            EditorGUILayout.PropertyField(snappingProp);

            base.DrawValues();
        }

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            punchAmountProp = serializedObject.FindProperty("punchAmount");
            snappingProp = serializedObject.FindProperty("snapping");
        }
    }
}

#endif