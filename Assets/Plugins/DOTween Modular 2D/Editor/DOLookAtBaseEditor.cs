#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DOTweenModular2D.Editor
{
    public class DOLookAtBaseEditor : DOBaseEditor
    {
        #region Serialized Properties

        protected SerializedProperty lookAtProp;
        protected SerializedProperty lookAtTargetProp;
        protected SerializedProperty lookAtPositionProp;
        protected SerializedProperty offsetProp;
        protected SerializedProperty minProp;
        protected SerializedProperty maxProp;
        protected SerializedProperty smoothFactorProp;

        #endregion

        #region Foldout Bool

        protected bool lookAtSettingsFoldout = true;
        protected string savedLookAtSettingsFoldout;

        #endregion

        private DOLookAt doLookAt;

        #region Inspector Draw Functions

        protected void DrawLookAtSettings()
        {
            EditorGUILayout.PropertyField(lookAtProp);

            if (doLookAt.lookAt == Enums.LookAtSimple.None)
                return;

            switch (doLookAt.lookAt)
            {
                case Enums.LookAtSimple.Position:
                    EditorGUILayout.PropertyField(lookAtPositionProp);
                    break;

                case Enums.LookAtSimple.Transform:
                    EditorGUILayout.PropertyField(lookAtTargetProp);
                    break;
            }
            EditorGUILayout.PropertyField(offsetProp);
            EditorGUILayout.PropertyField(smoothFactorProp);
            EditorGUILayout.PropertyField(minProp);
            EditorGUILayout.PropertyField(maxProp);
        }

        protected void DrawLookAtHelpBox()
        {
            if (doLookAt.lookAt == Enums.LookAtSimple.Transform && doLookAt.lookAtTarget == null)
            {
                EditorGUILayout.HelpBox("Look At Target not Assigned", MessageType.Error);
            }
            else if (doLookAt.lookAt != Enums.LookAtSimple.Transform && doLookAt.lookAtTarget != null)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.HelpBox("Look At Target is still Assigned, it Should be removed", MessageType.Warning);

                GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
                trashButton.tooltip = "Remove Look At Target";

                if (GUILayout.Button(trashButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize * 2f)))
                {
                    doLookAt.lookAtTarget = null;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        #endregion

        #region Scene View Draw Functions

        protected void DrawLookAtHandle()
        {
            Vector2 newLookAtPosition = Handles.PositionHandle(doLookAt.lookAtPosition, Quaternion.identity);

            if (newLookAtPosition != doLookAt.lookAtPosition)
            {
                Undo.RecordObject(doLookAt, "Change Look At Position_DOLookAt");
                doLookAt.lookAtPosition = newLookAtPosition;
            }
        }

        protected void DrawRotationClampCircle()
        {
            Vector3 position = doLookAt.transform.position;

            // Calculate the endpoints of the arc based on the min and max angles
            float minAngle = (doLookAt.min + 90) * Mathf.Deg2Rad;
            float maxAngle = (doLookAt.max + 90) * Mathf.Deg2Rad;
            Vector3 minDir = new Vector3(Mathf.Cos(minAngle), Mathf.Sin(minAngle), 0);
            Vector3 maxDir = new Vector3(Mathf.Cos(maxAngle), Mathf.Sin(maxAngle), 0);

            // Draw the circle representing the range
            Handles.DrawWireArc(position, Vector3.forward, minDir, doLookAt.max - doLookAt.min, 2f);

            // Draw lines from the center to the min and max angles
            Handles.DrawLine(position, position + minDir * 2f);
            Handles.DrawLine(position, position + maxDir * 2f);
        }

        protected void DrawLookAtLine()
        {
            if (doLookAt.lookAt == Enums.LookAtSimple.Position)
            {
                Handles.DrawDottedLine(doLookAt.transform.position, doLookAt.lookAtPosition, 5f);
            }
            else if (doLookAt.lookAtTarget != null)
            {
                Handles.DrawDottedLine(doLookAt.transform.position, doLookAt.lookAtTarget.position, 5f);
            }
        }

        #endregion

        #region Setup Functions

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            lookAtProp = serializedObject.FindProperty("lookAt");
            lookAtTargetProp = serializedObject.FindProperty("lookAtTarget");
            lookAtPositionProp = serializedObject.FindProperty("lookAtPosition");
            offsetProp = serializedObject.FindProperty("offset");
            smoothFactorProp = serializedObject.FindProperty("smoothFactor");
            minProp = serializedObject.FindProperty("min");
            maxProp = serializedObject.FindProperty("max");
        }

        protected override void SetupSavedVariables(DOBase doLookAt)
        {
            base.SetupSavedVariables(doLookAt);
            this.doLookAt = (DOLookAt)doLookAt;
            int instanceId = doLookAt.GetInstanceID();

            savedLookAtSettingsFoldout = "DOLookAtBaseEditor_lookAtSettingsFoldout_" + instanceId;
            lookAtSettingsFoldout = EditorPrefs.GetBool(savedLookAtSettingsFoldout, true);
        }

        protected override void ClearSavedEditorPrefs()
        {
            base.ClearSavedEditorPrefs();

            if (EditorPrefs.HasKey(savedLookAtSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedLookAtSettingsFoldout);
            }
        }

        #endregion
    }
}

#endif