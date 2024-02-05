#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPunchPosition)), CanEditMultipleObjects]
    public sealed class DOPunchPositionEditor : DOPunchBaseEditor
    {
        private SerializedProperty snappingProp;

        private DOPunchPosition doPunchPosition;
        private string positionKey;

        public override void OnEnable()
        {
            base.OnEnable();

            doPunchPosition = (DOPunchPosition)target;
            positionKey = "DOPunchPosition_" + instanceId;

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

            SessionState.SetVector3(positionKey, doPunchPosition.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doPunchPosition.transform.position = SessionState.GetVector3(positionKey, doPunchPosition.transform.position);
        }

        #endregion
    }
}

#endif
