#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPunchAnchorPos)), CanEditMultipleObjects]
    public sealed class DOPunchAnchorPosEditor : DOPunchBaseEditor
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
