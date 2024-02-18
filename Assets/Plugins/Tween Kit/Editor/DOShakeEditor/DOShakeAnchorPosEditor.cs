#if UNITY_EDITOR

using UnityEditor;

namespace TweenKit.Editor
{
    [CustomEditor(typeof(DOShakeAnchorPos)), CanEditMultipleObjects]
    public sealed class DOShakeAnchorPosEditor : DOShakeBaseEditor
    {
        private SerializedProperty snappingProp;

        public override void OnEnable()
        {
            base.OnEnable();

            snappingProp = serializedObject.FindProperty("snapping");
        }

        protected override void DrawValues()
        {
            DrawProperty(strengthProp);
            DrawProperty(snappingProp);

            base.DrawValues();
        }
    }
}

#endif
