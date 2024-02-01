#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOShakeScale)), CanEditMultipleObjects]
    public class DOShakeScaleEditor : DOShakeBaseEditor
    {
        private DOShakeScale doShakeScale;
        private string key;

        public override void OnEnable()
        {
            base.OnEnable();

            doShakeScale = (DOShakeScale)target;
            key = "DOShakeScale_" + instanceId;
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

            SessionState.SetVector3(key, doShakeScale.transform.localScale);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doShakeScale.transform.localScale = SessionState.GetVector3(key, doShakeScale.transform.localScale);
        }

        #endregion

    }
}

#endif
