#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using DOTweenModular.Miscellaneous;

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
        private SerializedProperty wayPointsProp;

        private SerializedProperty lookAtProp;
        private SerializedProperty lookAtPositionProp;
        private SerializedProperty lookAtTargetProp;
        private SerializedProperty lookAheadProp;
        private SerializedProperty stableZRotationProp;

        #endregion

        private DOPath doPath;
        private RelativeFlags relativeFlags;

        private string positionKey;
        private string rotationkey;

        private Vector3 startPosition;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doPath = (DOPath)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            positionKey = "DOPath_" + instanceId;
            rotationkey = "DOPath_LookAt_" + doPath.gameObject.GetInstanceID();

            startPosition = doPath.transform.position;

            pathTypeProp = serializedObject.FindProperty("pathType");
            pathModeProp = serializedObject.FindProperty("pathMode");
            resolutionProp = serializedObject.FindProperty("resolution");
            closePathProp = serializedObject.FindProperty("closePath");
            speedBasedProp = serializedObject.FindProperty("speedBased");
            relativeProp = serializedObject.FindProperty("relative");
            wayPointsProp = serializedObject.FindProperty("wayPoints");

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

                DrawProperty(wayPointsProp);
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

            if (doPath.wayPoints == null)
                return;

            if (!tweenPreviewing)
                startPosition = doPath.transform.position;

            if (doPath.relative)
            {
                ConvertPointsToRelative(startPosition);

                switch (doPath.pathType)
                {
                    case DG.Tweening.PathType.Linear:

                        DrawRelativeLinearPath(startPosition, doPath.closePath);
                        DrawRelativeSimpleHandles(startPosition);
                        break;

                    case DG.Tweening.PathType.CatmullRom:
                        DrawRelativeCatmullRomPath(startPosition, doPath.wayPoints, doPath.closePath);
                        DrawRelativeSimpleHandles(startPosition);
                        break;

                    case DG.Tweening.PathType.CubicBezier:
                        if (doPath.wayPoints.Length % 3 == 0)
                        {
                            DrawRelativeCubicBezierPath(startPosition);
                            DrawRelativeCubicBezierHandles(startPosition);
                        }
                        break;
                }
                    
            }
            else
            {
                ConvertPointsToAbsolute(doPath.transform.position);

                switch (doPath.pathType)
                {
                    case DG.Tweening.PathType.Linear:
                        DrawAbsoluteLinearPath(startPosition, doPath.closePath);
                        DrawAbsoluteSimpleHandles();
                        break;

                    case DG.Tweening.PathType.CatmullRom:
                        DrawAbsoluteCatmullRomPath(startPosition, doPath.wayPoints, doPath.closePath);
                        DrawAbsoluteSimpleHandles();
                        break;

                    case DG.Tweening.PathType.CubicBezier:
                        if (doPath.wayPoints.Length % 3 == 0)
                        {
                            DrawAbsoluteCubicBezierPath(startPosition);
                            DrawAbsoluteCubicBezierHandles();
                        }
                        break;
                }

            }
        }

        #endregion

        #region Inspector Draw Functions

        private void DrawPathSettings()
        {
            DrawProperty(pathTypeProp);
            DrawProperty(pathModeProp);
            
            if (doPath.pathType != DG.Tweening.PathType.Linear)
                DrawProperty(resolutionProp);

            if (doPath.pathType != DG.Tweening.PathType.CubicBezier)
                DrawProperty(closePathProp);

            DrawSeparatorLine();

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

        #endregion

        #region Scene Draw Functions

        private void DrawAbsoluteLinearPath(Vector3 startPosition, bool closed)
        {
            Vector3 lineStart = startPosition;

            for (int i = 0; i < doPath.wayPoints.Length; i++)
            {
                DrawLine(lineStart, doPath.wayPoints[i], Color.green);
                lineStart = doPath.wayPoints[i];
            }

            if (closed)
                DrawLine(lineStart, startPosition, Color.green);
        }

        private void DrawRelativeLinearPath(Vector3 startPosition, bool closed)
        {
            Vector3 lineStart = startPosition;

            for (int i = 0; i < doPath.wayPoints.Length; i++)
            {
                DrawLine(lineStart, startPosition + doPath.wayPoints[i], Color.green);
                lineStart = startPosition + doPath.wayPoints[i];
            }

            if (closed)
                DrawLine(lineStart, startPosition, Color.green);
        }

        private void DrawAbsoluteCatmullRomPath(Vector3 startPosition, Vector3[] points, bool closed)
        {
            Vector3[] catmullRomPoints = Curve.CatmullRom.GetSpline(startPosition, points, 
                                                                    doPath.resolution, closed);

            Vector3 currentLineStart = catmullRomPoints[0];

            for (int i = 0; i < catmullRomPoints.Length; i++)
            {
                DrawLine(currentLineStart, catmullRomPoints[i], Color.green);
                currentLineStart = catmullRomPoints[i];
            }
        }

        private void DrawRelativeCatmullRomPath(Vector3 startPosition, Vector3[] points, bool closed)
        {
            Vector3[] absoultePoints = (Vector3[])points.Clone();

            for (int i = 0; i < absoultePoints.Length; i++)
            {
                absoultePoints[i] += startPosition;
            }

            DrawAbsoluteCatmullRomPath(startPosition, absoultePoints, closed);
        }

        private void DrawAbsoluteCubicBezierPath(Vector3 startPosition)
        {
            Vector3[] cubicBezierPoints = Curve.CubicBezier.GetSpline(startPosition, doPath.wayPoints,
                                                                      doPath.resolution);

            if (cubicBezierPoints == null)
                return;

            Vector3 currentLineStart = startPosition;

            for (int i = 0; i < cubicBezierPoints.Length; i++)
            {
                DrawLine(currentLineStart, cubicBezierPoints[i], Color.green);
                currentLineStart = cubicBezierPoints[i];
            }

            DrawLine(startPosition, doPath.wayPoints[0], Color.green);
            DrawLine(doPath.wayPoints[1], doPath.wayPoints[2], Color.green);

            for (int i = 2; i < doPath.wayPoints.Length - 1; i += 2)
            {
                DrawLine(doPath.wayPoints[i], doPath.wayPoints[i + 1], Color.green, 0.5f);
            }
        }

        private void DrawRelativeCubicBezierPath(Vector3 startPosition)
        {
            Vector3[] absoultePoints = (Vector3[])doPath.wayPoints.Clone();

            for (int i = 0; i < absoultePoints.Length; i++)
            {
                absoultePoints[i] += startPosition;
            }

            Vector3[] cubicBezierPoints = Curve.CubicBezier.GetSpline(startPosition, absoultePoints,
                                                                      doPath.resolution);

            if (cubicBezierPoints == null)
                return;

            Vector3 currentLineStart = startPosition;

            for (int i = 0; i < cubicBezierPoints.Length; i++)
            {
                DrawLine(currentLineStart, cubicBezierPoints[i], Color.green);
                currentLineStart = cubicBezierPoints[i];
            }

            DrawLine(startPosition, absoultePoints[0], Color.green);
            DrawLine(absoultePoints[1], absoultePoints[2], Color.green);

            for (int i = 2; i < doPath.wayPoints.Length - 1; i += 2)
            {
                DrawLine(absoultePoints[i], absoultePoints[i + 1], Color.green, 0.5f);
            }
        }

        private void DrawAbsoluteSimpleHandles()
        {
            for (int i = 0; i < doPath.wayPoints.Length; i++)
            {
                doPath.wayPoints[i] += DrawHandle(doPath.wayPoints[i]);
            }
        }

        private void DrawRelativeSimpleHandles(Vector3 startPosition)
        {
            for (int i = 0; i < doPath.wayPoints.Length; i++)
            {
                doPath.wayPoints[i] += DrawHandle(startPosition + doPath.wayPoints[i]);
            }
        }

        private void DrawAbsoluteCubicBezierHandles()
        {
            for (int i = 0; i < doPath.wayPoints.Length; i += 3)
            {
                doPath.wayPoints[i] += DrawSphereHandle(doPath.wayPoints[i], 0.5f);
                doPath.wayPoints[i + 1] += DrawSphereHandle(doPath.wayPoints[i + 1], 0.5f);

                doPath.wayPoints[i + 2] += DrawHandle(doPath.wayPoints[i + 2]);
            }
        }

        private void DrawRelativeCubicBezierHandles(Vector3 startPosition)
        {
            for (int i = 0; i < doPath.wayPoints.Length; i += 3)
            {
                doPath.wayPoints[i] += DrawSphereHandle(startPosition + doPath.wayPoints[i], 0.5f);
                doPath.wayPoints[i + 1] += DrawSphereHandle(startPosition + doPath.wayPoints[i + 1], 0.5f);

                doPath.wayPoints[i + 2] += DrawHandle(startPosition + doPath.wayPoints[i + 2]);
            }
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

        #endregion

        #region Path points ConvertTo Absolute/Relative Function

        private void ConvertPointsToRelative(Vector3 relativeTo)
        {
            if (relativeFlags.firstTimeRelative)
            {

                for (int i = 0; i < doPath.wayPoints.Length; i++)
                {
                    doPath.wayPoints[i] -= relativeTo;
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

                for (int i = 0; i < doPath.wayPoints.Length; i++)
                {
                    doPath.wayPoints[i] += absoluteTo;
                }

                Undo.RecordObject(relativeFlags, "DOPathEditor_firstTimeRelative");
                relativeFlags.firstTimeNonRelative = false;

            }

            relativeFlags.firstTimeRelative = true;
        }

        #endregion

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            startPosition = doPath.transform.position;

            SessionState.SetVector3(rotationkey, doPath.transform.localEulerAngles);
            SessionState.SetVector3(positionKey, doPath.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doPath.transform.localEulerAngles = SessionState.GetVector3(rotationkey, doPath.transform.localEulerAngles);
            doPath.transform.position = SessionState.GetVector3(positionKey, doPath.transform.position);
        }

        protected override void OnPreviewForceStopped()
        {
            base.OnPreviewForceStopped();

            doPath.transform.localEulerAngles = SessionState.GetVector3(rotationkey, doPath.transform.localEulerAngles);
        }

        #endregion

    }
}

#endif
