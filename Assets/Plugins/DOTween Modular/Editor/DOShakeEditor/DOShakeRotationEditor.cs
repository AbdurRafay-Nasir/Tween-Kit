#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOShakeRotation)), CanEditMultipleObjects]
    public class DOShakeRotationEditor : DOShakeBaseEditor
    {
        private DOShakeRotation doShakeRotation;
        private string key;

        public override void OnEnable()
        {
            base.OnEnable();

            doShakeRotation = (DOShakeRotation)target;
            key = "DOShakeRotation_" + instanceId;
        }

        protected override void DrawValues()
        {
            DrawProperty(strengthProp);

            base.DrawValues();
        }

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(key, doShakeRotation.transform.eulerAngles);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doShakeRotation.transform.eulerAngles = SessionState.GetVector3(key, doShakeRotation.transform.eulerAngles);
        }

        #endregion

    }
}

#endif
