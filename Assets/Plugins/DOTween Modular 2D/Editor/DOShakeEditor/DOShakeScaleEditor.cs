#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOShakeScale))]
    [CanEditMultipleObjects]
    public class DOShakeScaleEditor : DOShakeBaseEditor
    {
        private SerializedProperty strengthProp;

        protected override void DrawValues()
        {
            EditorGUILayout.PropertyField(strengthProp);

            base.DrawValues();
        }

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            strengthProp = serializedObject.FindProperty("strength");
        }
    }
}

#endif