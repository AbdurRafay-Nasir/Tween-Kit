#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOShakePosition)), CanEditMultipleObjects]
    public sealed class DOShakePositionEditor : DOShakeBaseEditor
    {
        private SerializedProperty snappingProp;

        private DOShakePosition doShakePosition;
        private string positionKey;

        public override void OnEnable()
        {
            base.OnEnable();

            doShakePosition = (DOShakePosition)target;
            positionKey = "DOShakePosition_" + instanceId;
            
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

            SessionState.SetVector3(positionKey, doShakePosition.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doShakePosition.transform.position = SessionState.GetVector3(positionKey, doShakePosition.transform.position);
        }

        #endregion

    }
}

#endif
