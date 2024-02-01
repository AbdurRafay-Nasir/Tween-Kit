#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPunchPosition)), CanEditMultipleObjects]
    public class DOPunchPositionEditor : DOPunchBaseEditor
    {
        private SerializedProperty snappingProp;

        private DOPunchPosition doPunchPosition;
        private string key;

        public override void OnEnable()
        {
            base.OnEnable();

            doPunchPosition = (DOPunchPosition)target;
            key = "DOPunchPosition_" + instanceId;

            snappingProp = serializedObject.FindProperty("snapping");
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

            SessionState.SetVector3(key, doPunchPosition.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doPunchPosition.transform.position = SessionState.GetVector3(key, doPunchPosition.transform.position);
        }

        #endregion

    }
}

#endif
