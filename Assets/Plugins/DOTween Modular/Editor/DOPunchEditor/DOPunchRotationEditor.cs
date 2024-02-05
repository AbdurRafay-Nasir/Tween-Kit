#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPunchRotation)), CanEditMultipleObjects]
    public sealed class DOPunchRotationEditor : DOPunchBaseEditor
    {
        private DOPunchRotation doPunchRotation;
        private string rotationKey;

        public override void OnEnable()
        {
            base.OnEnable();

            doPunchRotation = (DOPunchRotation)target;
            rotationKey = "DOPunchRotation_" + instanceId;
        }

        protected override void DrawValues()
        {
            DrawProperty(punchProp);

            base.DrawValues();
        }

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(rotationKey, doPunchRotation.transform.eulerAngles);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doPunchRotation.transform.eulerAngles = SessionState.GetVector3(rotationKey, doPunchRotation.transform.eulerAngles);
        }

        #endregion
    }
}

#endif
