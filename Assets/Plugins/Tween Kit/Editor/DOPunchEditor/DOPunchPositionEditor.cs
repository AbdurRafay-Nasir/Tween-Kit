#if UNITY_EDITOR

using UnityEditor;

namespace TweenKit.Editor
{
    [CustomEditor(typeof(DOPunchPosition)), CanEditMultipleObjects]
    public sealed class DOPunchPositionEditor : DOPunchBaseEditor
    {
        private SerializedProperty snappingProp;

        public override void OnEnable()
        {
            base.OnEnable();

            snappingProp = serializedObject.FindProperty("snapping");
        }

        protected override void DrawValues()
        {
            DrawProperty(punchProp);
            DrawProperty(snappingProp);

            base.DrawValues();
        }
    }
}

#endif
