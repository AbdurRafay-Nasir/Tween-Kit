#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPunchScale)), CanEditMultipleObjects]
    public class DOPunchScaleEditor : DOPunchBaseEditor
    {
        private DOPunchScale doPunchScale;
        private string key;

        public override void OnEnable()
        {
            base.OnEnable();

            doPunchScale = (DOPunchScale)target;
            key = "DOPunchScale_" + instanceId;
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

            SessionState.SetVector3(key, doPunchScale.transform.localScale);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doPunchScale.transform.localScale = SessionState.GetVector3(key, doPunchScale.transform.localScale);
        }

        #endregion


    }
}

#endif
