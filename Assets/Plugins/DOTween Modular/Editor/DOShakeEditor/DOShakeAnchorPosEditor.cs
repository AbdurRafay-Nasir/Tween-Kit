#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOShakeAnchorPos)), CanEditMultipleObjects]
    public sealed class DOShakeAnchorPosEditor : DOShakeBaseEditor
    {
        private SerializedProperty snappingProp;

        private DOShakeAnchorPos doShakeAnchorPos;
        private RectTransform rectTransform;

        private string anchoredPositionKey;

        public override void OnEnable()
        {
            base.OnEnable();

            doShakeAnchorPos = (DOShakeAnchorPos)target;
            rectTransform = (RectTransform)doShakeAnchorPos.transform;

            anchoredPositionKey = "DOShakeAnchorPos_" + instanceId;

            snappingProp = serializedObject.FindProperty("snapping");
        }

        protected override void DrawValues()
        {
            DrawProperty(strengthProp);
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
