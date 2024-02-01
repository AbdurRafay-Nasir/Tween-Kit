#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPunchRotation)), CanEditMultipleObjects]
    public class DOPunchRotationEditor : DOPunchBaseEditor
    {
        private DOPunchRotation doPunchRotation;
        private string key;

        public override void OnEnable()
        {
            base.OnEnable();

            doPunchRotation = (DOPunchRotation)target;
            key = "DOPunchRotation_" + instanceId;
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

            SessionState.SetVector3(key, doPunchRotation.transform.eulerAngles);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doPunchRotation.transform.eulerAngles = SessionState.GetVector3(key, doPunchRotation.transform.eulerAngles);
        }

        #endregion

    }
}

#endif
