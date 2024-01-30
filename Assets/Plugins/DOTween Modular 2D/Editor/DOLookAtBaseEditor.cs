#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    public class DOLookAtBaseEditor : DOBaseEditor
    {
        protected SerializedProperty lookAtProp;
        protected SerializedProperty lookAtPositionProp;
        protected SerializedProperty lookAtTargetProp;
        protected SerializedProperty interpolateProp;

        private DOLookAtBase doLookAt;
        private string key;

        public override void OnEnable()
        {
            base.OnEnable();

            doLookAt = (DOLookAtBase)target;
            key = "DOLookAtBase_" + doLookAt.gameObject.GetInstanceID();

            lookAtProp = serializedObject.FindProperty("lookAt");
            lookAtPositionProp = serializedObject.FindProperty("lookAtPosition");
            lookAtTargetProp = serializedObject.FindProperty("lookAtTarget");
            interpolateProp = serializedObject.FindProperty("interpolate");
        }

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(key, doLookAt.transform.localEulerAngles);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doLookAt.transform.localEulerAngles = SessionState.GetVector3(key, doLookAt.transform.localEulerAngles);
        }

        protected override void OnPreviewForceStopped()
        {
            base.OnPreviewForceStopped();

            doLookAt.transform.localEulerAngles = SessionState.GetVector3(key, doLookAt.transform.localEulerAngles);
        }

        protected void DrawLookAtSettings()
        {
            DrawProperty(lookAtProp);

            if ((Enums.LookAtSimple)lookAtProp.enumValueIndex == Enums.LookAtSimple.None)
                return;

            if ((Enums.LookAtSimple) lookAtProp.enumValueIndex == Enums.LookAtSimple.Position)
                DrawProperty(lookAtPositionProp);

            if ((Enums.LookAtSimple) lookAtProp.enumValueIndex == Enums.LookAtSimple.Transform)
                DrawProperty(lookAtTargetProp);

            DrawProperty(interpolateProp);
        }
    }

}

#endif
