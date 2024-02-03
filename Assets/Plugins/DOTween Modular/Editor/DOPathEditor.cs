#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPath)), CanEditMultipleObjects]
    public class DOPathEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty pathTypeProp;
        private SerializedProperty pathModeProp;
        private SerializedProperty resolutionProp;
        private SerializedProperty closePathProp;
        private SerializedProperty speedBasedProp;
        private SerializedProperty relativeProp;
        private SerializedProperty pathPointsProp;

        private SerializedProperty lookAtProp;
        private SerializedProperty lookAtPositionProp;
        private SerializedProperty lookAtTargetProp;
        private SerializedProperty lookAheadProp;
        private SerializedProperty stableZRotationProp;

        #endregion

        private DOPath doPath;
        private RelativeFlags relativeFlags;

        private string tweenPreviewKey;
        private string key;

        private Vector3 startPosition;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doPath = (DOPath)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            tweenPreviewKey = "DOPath_preview" + instanceId;
            key = "DOPath_" + instanceId;

            pathTypeProp = serializedObject.FindProperty("pathType");
            pathModeProp = serializedObject.FindProperty("pathMode");
            resolutionProp = serializedObject.FindProperty("resolution");
            closePathProp = serializedObject.FindProperty("closePath");
            speedBasedProp = serializedObject.FindProperty("speedBased");
            relativeProp = serializedObject.FindProperty("relative");
            pathPointsProp = serializedObject.FindProperty("pathPoints");

            lookAtProp = serializedObject.FindProperty("lookAt");
            lookAtPositionProp = serializedObject.FindProperty("lookAtPosition");
            lookAtTargetProp = serializedObject.FindProperty("lookAtTarget");
            lookAheadProp = serializedObject.FindProperty("lookAhead");
            stableZRotationProp = serializedObject.FindProperty("stableZRotation");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "LookAt", "Path", "Points", "Values", "Events");

            Space();

            if (toggleStates[0])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Life Time Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawLifeTimeSettings();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            DrawTweenObjectHelpBox();

            if (toggleStates[1])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Type Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawTypeSettings();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            if (toggleStates[2])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Look At Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawLookAtSettings();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            DrawLookAtHelpbox();

            if (toggleStates[3])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Path Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawPathSettings();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            if (toggleStates[4])
            {
                DrawSeparatorLine();

                DrawProperty(pathPointsProp);
            }


            if (toggleStates[5])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Values"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawValues();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            if (toggleStates[6])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Events"))
                {
                    EditorGUI.indentLevel++;

                    Space();

                    DrawEvents();

                    EditorGUI.indentLevel--;
                }
                EndFoldout();
            }

            DrawSeparatorLine();

            DrawPlayButton();
            DrawStopButton();

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnSceneGUI()
        {
            base.OnSceneGUI();

            DrawLookAtLine();

            if (doPath.pathPoints == null)
                return;

            if (!SessionState.GetBool(tweenPreviewKey, false))
            {
                startPosition = doPath.transform.position;
            }

            if (doPath.relative)
            {
                ConvertPointsToRelative(startPosition);

                DrawRelativeLinearPath(startPosition, doPath.closePath);

                for (int i = 0; i < doPath.pathPoints.Length; i++)
                {
                    doPath.pathPoints[i] += DrawHandle(startPosition + doPath.pathPoints[i]);
                }
            }
            else
            {
                ConvertPointsToAbsolute(startPosition);

                DrawAbsoluteLinearPath(startPosition, doPath.closePath);

                for (int i = 0; i < doPath.pathPoints.Length; i++)
                {
                    doPath.pathPoints[i] += DrawHandle(doPath.pathPoints[i]);
                }

            }
        }

        #endregion

        private void DrawPathSettings()
        {
            DrawProperty(pathTypeProp);
            DrawProperty(pathModeProp);
            DrawProperty(resolutionProp);
            DrawProperty(closePathProp);
            DrawProperty(speedBasedProp);
            DrawProperty(relativeProp);
        }

        private void DrawLookAtSettings()
        {
            DrawProperty(lookAtProp);

            switch (doPath.lookAt)
            {
                case Enums.LookAtAdvanced.Position:
                    DrawProperty(lookAtPositionProp);
                    break;

                case Enums.LookAtAdvanced.Transform:
                    DrawProperty(lookAtTargetProp);
                    break;

                case Enums.LookAtAdvanced.Percentage:
                    DrawProperty(lookAheadProp);
                    break;

                default:
                    return;
            }

            DrawProperty(stableZRotationProp);
        }

        private void DrawLookAtHelpbox()
        {
            if (doPath.lookAt != Enums.LookAtAdvanced.None &&
                doPath.pathMode == DG.Tweening.PathMode.Ignore)
            {
                DrawHelpbox("PathMode is set to Ignore, LookAt will not work", MessageType.Warning);
            }

            if (doPath.lookAt == Enums.LookAtAdvanced.Transform &&
                doPath.lookAtTarget == null)
            {
                DrawHelpbox("LookAt Target not assigned", MessageType.Error);
            }

            if (doPath.lookAt != Enums.LookAtAdvanced.Transform &&
                doPath.lookAtTarget != null)
            {
                EditorGUILayout.BeginHorizontal();

                DrawHelpbox("LookAt Target is assigned, it should be removed", MessageType.Warning);

                GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
                trashButton.tooltip = "Remove LookAt Target";

                if (GUILayout.Button(trashButton, GUILayout.Height(40), GUILayout.Width(80)))
                {
                    doPath.lookAtTarget = null;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawRelativeLinearPath(Vector3 startPosition, bool closed)
        {
            Vector3 lineStart = startPosition;

            for (int i = 0; i < doPath.pathPoints.Length; i++)
            {
                DrawLine(lineStart, startPosition + doPath.pathPoints[i], Color.green);
                lineStart = startPosition + doPath.pathPoints[i];
            }

            if (closed)
                DrawLine(lineStart, startPosition, Color.green);
        }

        private void DrawAbsoluteLinearPath(Vector3 startPosition, bool closed)
        {
            Vector3 lineStart = startPosition;

            for (int i = 0; i < doPath.pathPoints.Length; i++)
            {
                DrawLine(lineStart, doPath.pathPoints[i], Color.green);
                lineStart = doPath.pathPoints[i];
            }

            if (closed)
                DrawLine(lineStart, startPosition, Color.green);
        }

        private void DrawLookAtLine()
        {
            if (doPath.lookAt == Enums.LookAtAdvanced.None || 
                doPath.lookAt == Enums.LookAtAdvanced.Percentage) 
                return;

            if (doPath.lookAt == Enums.LookAtAdvanced.Position)
            {
                doPath.lookAtPosition += DrawHandle(doPath.lookAtPosition);

                DrawDottedLine(doPath.transform.position, doPath.lookAtPosition, Color.green, 10f);
            }

            else if (doPath.lookAt == Enums.LookAtAdvanced.Transform)
            {
                if (doPath.lookAtTarget != null)
                    DrawDottedLine(doPath.transform.position, doPath.lookAtTarget.position, Color.green, 10f);
            }
        }

        private void ConvertPointsToRelative(Vector3 relativeTo)
        {
            if (relativeFlags.firstTimeRelative)
            {

                for (int i = 0; i < doPath.pathPoints.Length; i++)
                {
                    doPath.pathPoints[i] -= relativeTo;
                }

                Undo.RecordObject(relativeFlags, "DOPathEditor_firstTimeNonRelative");
                relativeFlags.firstTimeRelative = false;

            }

            relativeFlags.firstTimeNonRelative = true;
        }

        private void ConvertPointsToAbsolute(Vector3 absoluteTo)
        {
            if (relativeFlags.firstTimeNonRelative)
            {

                for (int i = 0; i < doPath.pathPoints.Length; i++)
                {
                    doPath.pathPoints[i] += absoluteTo;
                }

                Undo.RecordObject(relativeFlags, "DOPathEditor_firstTimeRelative");
                relativeFlags.firstTimeNonRelative = false;

            }

            relativeFlags.firstTimeRelative = true;
        }

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            startPosition = doPath.transform.position;

            SessionState.SetBool(tweenPreviewKey, true);
            SessionState.SetVector3(key, doPath.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            SessionState.SetBool(tweenPreviewKey, false);
            doPath.transform.position = SessionState.GetVector3(key, doPath.transform.position);
        }

        #endregion

    }
}

#endif
