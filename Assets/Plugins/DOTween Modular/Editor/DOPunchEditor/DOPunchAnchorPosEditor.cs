#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPunchAnchorPos)), CanEditMultipleObjects]
    public class DOPunchAnchorPosEditor : DOPunchBaseEditor
    {
        private SerializedProperty snappingProp;

        private DOPunchAnchorPos doPunchAnchorPos;
        private RectTransform rectTransform;
        private string key;

        public override void OnEnable()
        {
            base.OnEnable();

            snappingProp = serializedObject.FindProperty("snapping");

            doPunchAnchorPos = (DOPunchAnchorPos)target;
            rectTransform = (RectTransform)doPunchAnchorPos.transform;
            key = "DOPunchAnchorPos_" + instanceId;
        }

        protected override void DrawValues()
        {
            DrawProperty(punchProp);
            DrawProperty(snappingProp);

            base.DrawValues();
        }

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(key, rectTransform.anchoredPosition);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            rectTransform.anchoredPosition = SessionState.GetVector3(key, rectTransform.anchoredPosition);
        }

        #endregion
    }
}

#endif
