#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOShakePosition)), CanEditMultipleObjects]
    public class DOShakePositionEditor : DOShakeBaseEditor
    {
        private SerializedProperty snappingProp;

        private DOShakePosition doShakePosition;
        private string key;

        public override void OnEnable()
        {
            base.OnEnable();

            doShakePosition = (DOShakePosition)target;
            key = "DOShakePosition_" + instanceId;
            
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

            SessionState.SetVector3(key, doShakePosition.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doShakePosition.transform.position = SessionState.GetVector3(key, doShakePosition.transform.position);
        }

        #endregion

    }
}

#endif
