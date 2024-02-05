#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPunchAnchorPos)), CanEditMultipleObjects]
    public sealed class DOPunchAnchorPosEditor : DOPunchBaseEditor
    {
        private SerializedProperty snappingProp;

        private DOPunchAnchorPos doPunchAnchorPos;
        private RectTransform rectTransform;
        private string anchoredPositionKey;

        public override void OnEnable()
        {
            base.OnEnable();

            snappingProp = serializedObject.FindProperty("snapping");

            doPunchAnchorPos = (DOPunchAnchorPos)target;
            rectTransform = (RectTransform)doPunchAnchorPos.transform;
            anchoredPositionKey = "DOPunchAnchorPos_" + instanceId;
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

            SessionState.SetVector3(anchoredPositionKey, rectTransform.anchoredPosition);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            rectTransform.anchoredPosition = SessionState.GetVector3(anchoredPositionKey, rectTransform.anchoredPosition);
        }

        #endregion
    }
}

#endif
