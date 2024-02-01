#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    public class DOLookAtBaseEditor : DOBaseEditor
    {
        #region Serialized Properties

        protected SerializedProperty lookAtProp;
        protected SerializedProperty lookAtPositionProp;
        protected SerializedProperty lookAtTargetProp;
        protected SerializedProperty interpolateProp;

        #endregion

        private DOLookAtBase doLookAt;
        private string lookAtkey;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doLookAt = (DOLookAtBase)target;
            lookAtkey = "DOLookAtBase_" + doLookAt.gameObject.GetInstanceID();

            lookAtProp = serializedObject.FindProperty("lookAt");
            lookAtPositionProp = serializedObject.FindProperty("lookAtPosition");
            lookAtTargetProp = serializedObject.FindProperty("lookAtTarget");
            interpolateProp = serializedObject.FindProperty("interpolate");
        }

        public override void OnSceneGUI()
        {
            base.OnSceneGUI();

            if (doLookAt.lookAt == Enums.LookAtSimple.None) return;

            if (doLookAt.lookAt == Enums.LookAtSimple.Position)
            {
                doLookAt.lookAtPosition += DrawHandle(doLookAt.lookAtPosition);

                DrawDottedLine(doLookAt.transform.position, doLookAt.lookAtPosition, Color.green, 10f);
            }

            else if (doLookAt.lookAt == Enums.LookAtSimple.Transform)
            {
                if (doLookAt.lookAtTarget != null)
                    DrawDottedLine(doLookAt.transform.position, doLookAt.lookAtPosition, Color.green, 10f);
            }            
        }

        #endregion

        #region Inspector Draw Functions

        protected void DrawLookAtSettings()
        {
            DrawProperty(lookAtProp);

            if (doLookAt.lookAt == Enums.LookAtSimple.None)
                return;

            if (doLookAt.lookAt == Enums.LookAtSimple.Position)
                DrawProperty(lookAtPositionProp);

            if (doLookAt.lookAt == Enums.LookAtSimple.Transform)
                DrawProperty(lookAtTargetProp);

            DrawProperty(interpolateProp);
        }

        protected void DrawLookAtTransformHelpbox()
        {
            if (doLookAt.lookAt == Enums.LookAtSimple.Transform &&
                doLookAt.lookAtTarget == null)
            {
                DrawHelpbox("LookAt Target not assigned", MessageType.Error);
            }

            if (doLookAt.lookAt != Enums.LookAtSimple.Transform &&
                doLookAt.lookAtTarget != null)
            {
                EditorGUILayout.BeginHorizontal();

                DrawHelpbox("LookAt Target is assigned, it should be removed", MessageType.Warning);

                GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
                trashButton.tooltip = "Remove LookAt Target";

                if (GUILayout.Button(trashButton, GUILayout.Height(40), GUILayout.Width(80)))
                {
                    doLookAt.lookAtTarget = null;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        #endregion

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(lookAtkey, doLookAt.transform.localEulerAngles);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doLookAt.transform.localEulerAngles = SessionState.GetVector3(lookAtkey, doLookAt.transform.localEulerAngles);
        }

        protected override void OnPreviewForceStopped()
        {
            base.OnPreviewForceStopped();

            doLookAt.transform.localEulerAngles = SessionState.GetVector3(lookAtkey, doLookAt.transform.localEulerAngles);
        }

        #endregion
    }

}

#endif
