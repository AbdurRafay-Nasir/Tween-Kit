#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOShakeAnchorPos)), CanEditMultipleObjects]
    public class DOShakeAnchorPosEditor : DOShakeBaseEditor
    {
        private SerializedProperty snappingProp;

        private DOShakeAnchorPos doShakeAnchorPos;
        private RectTransform rectTransform;

        private string key;

        public override void OnEnable()
        {
            base.OnEnable();

            doShakeAnchorPos = (DOShakeAnchorPos)target;
            rectTransform = (RectTransform)doShakeAnchorPos.transform;

            key = "DOShakeAnchorPos_" + instanceId;

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
