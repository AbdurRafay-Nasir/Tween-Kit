#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPath)), CanEditMultipleObjects]
    public sealed class DOPathEditor : DOBaseEditor
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

        private string rotationkey;

        private Vector3 startPosition;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doPath = (DOPath)target;
            relativeFlags = CreateInstance<RelativeFlags>();

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

            bool[] toggleStates = DrawToggles("Life", "Type", "LookAt", "Path", "Waypoints", "Values", "Events");

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

            DrawWaypointCubicBezierHelpbox();

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

                if (BeginFoldout("Events", false))
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

            DrawLookAtHandleAndLine();
            DrawInstructionsLabel();

            Event e = Event.current;
            Vector3 mousePosition = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;

            if (e.type == EventType.MouseDown && e.button == 1 && e.control)
            {
                CreateNewWaypoint(mousePosition);
            }

            if (e.type == EventType.MouseDown && e.button == 1 && e.alt)
            {
                DeleteWaypoint(mousePosition);
            }

            if (e.type == EventType.MouseDown && e.button == 2 && e.control)
            {
                InsertWaypoint(mousePosition);
            }


            if (doPath.wayPoints.Count < 1)
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
                        if (doPath.wayPoints.Count % 3 == 0)
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
                        if (doPath.wayPoints.Count % 3 == 0)
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

        /// <summary>
        /// Draws Path Type, Path Mode, Resolution(If pathType is not Linear), <br/>
        /// Close Path(If pathType is not Cubic Bezier), Speed Based and Relative Properties
        /// </summary>
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

        /// <summary>
        /// Draws Look At, Look At Position(If Look At = Position), Look At Target(If Look At = Transform), <br/>
        /// Look Ahead(If Look At = Percentage and Stable Z Rotation properties
        /// </summary>
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

        /// <summary>
        /// Draws Helpbox for Inspector messages regarding Look At
        /// </summary>
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

        /// <summary>
        /// Draws Helpbox if path is cubic bezier and waypoints are not multiple of 3 
        /// </summary>
        private void DrawWaypointCubicBezierHelpbox()
        {
            if (doPath.pathType != DG.Tweening.PathType.CubicBezier)
                return;

            if (doPath.wayPoints == null)
                return;

            if (doPath.wayPoints.Count % 3 != 0)
                DrawHelpbox("Cubic Bezier Path requires waypoints multiple of 3" + "\n" + 
                            "can't show Handles/Lines", MessageType.Warning);
        }

        #endregion

        private void CreateNewWaypoint(Vector3 position)
        {
            if (SceneView.currentDrawingSceneView.in2DMode)
                position.z = 0f;

            Undo.RecordObject(doPath, "Added Waypoint");
            doPath.wayPoints.Add(position);
        }

        private void DeleteWaypoint(Vector3 position)
        {
            if (SceneView.currentDrawingSceneView.in2DMode)
                position.z = 0f;

            float minDistanceToPoint = 0.5f;
            int closestWaypointIndex = -1;

            for (int i = 0; i < doPath.wayPoints.Count; i++)
            {
                float distance = Vector3.Distance(position, doPath.wayPoints[i]);

                if (distance < minDistanceToPoint)
                {
                    minDistanceToPoint = distance;
                    closestWaypointIndex = i;
                }
            }

            if (closestWaypointIndex != -1)
            {
                Undo.RecordObject(doPath, "Deleted Waypoint");
                doPath.wayPoints.RemoveAt(closestWaypointIndex);
            }
        }

        private void InsertWaypoint(Vector3 position)
        {
            if (SceneView.currentDrawingSceneView.in2DMode)
                position.z = 0f;

            float minDistance = 0.8f;
            int closestIndex = -1;

            for (int i = 0; i < doPath.wayPoints.Count - 1; i++)
            {
                float distance = HandleUtility.DistancePointLine(position, doPath.wayPoints[i], doPath.wayPoints[i + 1]);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }

            if (closestIndex != -1)
            {
                Undo.RecordObject(doPath, "Inserted Waypoint");
                doPath.wayPoints.Insert(closestIndex + 1, position);
            }
        }

        #region Scene Draw Functions

        private void DrawAbsoluteLinearPath(Vector3 startPosition, bool closed)
        {
            Vector3 lineStart = startPosition;

            for (int i = 0; i < doPath.wayPoints.Count; i++)
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

            for (int i = 0; i < doPath.wayPoints.Count; i++)
            {
                DrawLine(lineStart, startPosition + doPath.wayPoints[i], Color.green);
                lineStart = startPosition + doPath.wayPoints[i];
            }

            if (closed)
                DrawLine(lineStart, startPosition, Color.green);
        }

        private void DrawAbsoluteCatmullRomPath(Vector3 startPosition, List<Vector3> points, bool closed)
        {
            Vector3[] catmullRomPoints = Curve.CatmullRom.GetSpline(startPosition, points.ToArray(), 
                                                                    doPath.resolution, closed);

            Vector3 currentLineStart = catmullRomPoints[0];

            for (int i = 0; i < catmullRomPoints.Length; i++)
            {
                DrawLine(currentLineStart, catmullRomPoints[i], Color.green);
                currentLineStart = catmullRomPoints[i];
            }
        }

        private void DrawRelativeCatmullRomPath(Vector3 startPosition, List<Vector3> points, bool closed)
        {
            List<Vector3> absolutePoints = new (points);

            for (int i = 0; i < absolutePoints.Count; i++)
            {
                absolutePoints[i] += startPosition;
            }

            DrawAbsoluteCatmullRomPath(startPosition, absolutePoints, closed);
        }

        private void DrawAbsoluteCubicBezierPath(Vector3 startPosition)
        {
            Vector3[] cubicBezierPoints = Curve.CubicBezier.GetSpline(startPosition, doPath.wayPoints.ToArray(),
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

            for (int i = 2; i < doPath.wayPoints.Count - 1; i += 2)
            {
                DrawLine(doPath.wayPoints[i], doPath.wayPoints[i + 1], Color.green, 0.5f);
            }
        }

        private void DrawRelativeCubicBezierPath(Vector3 startPosition)
        {
            List<Vector3> absolutePoints = new(doPath.wayPoints);

            for (int i = 0; i < absolutePoints.Count; i++)
            {
                absolutePoints[i] += startPosition;
            }

            Vector3[] cubicBezierPoints = Curve.CubicBezier.GetSpline(startPosition, absolutePoints.ToArray(),
                                                                      doPath.resolution);

            if (cubicBezierPoints == null)
                return;

            Vector3 currentLineStart = startPosition;

            for (int i = 0; i < cubicBezierPoints.Length; i++)
            {
                DrawLine(currentLineStart, cubicBezierPoints[i], Color.green);
                currentLineStart = cubicBezierPoints[i];
            }

            DrawLine(startPosition, absolutePoints[0], Color.green);
            DrawLine(absolutePoints[1], absolutePoints[2], Color.green);

            for (int i = 2; i < doPath.wayPoints.Count - 1; i += 2)
            {
                DrawLine(absolutePoints[i], absolutePoints[i + 1], Color.green, 0.5f);
            }
        }

        private void DrawAbsoluteSimpleHandles()
        {
            for (int i = 0; i < doPath.wayPoints.Count; i++)
            {
                doPath.wayPoints[i] += DrawHandle(doPath.wayPoints[i]);
            }
        }

        private void DrawRelativeSimpleHandles(Vector3 startPosition)
        {
            for (int i = 0; i < doPath.wayPoints.Count; i++)
            {
                doPath.wayPoints[i] += DrawHandle(startPosition + doPath.wayPoints[i]);
            }
        }

        private void DrawAbsoluteCubicBezierHandles()
        {
            for (int i = 0; i < doPath.wayPoints.Count; i += 3)
            {
                doPath.wayPoints[i] += DrawSphereHandle(doPath.wayPoints[i], 0.5f);
                doPath.wayPoints[i + 1] += DrawSphereHandle(doPath.wayPoints[i + 1], 0.5f);

                doPath.wayPoints[i + 2] += DrawHandle(doPath.wayPoints[i + 2]);
            }
        }

        private void DrawRelativeCubicBezierHandles(Vector3 startPosition)
        {
            for (int i = 0; i < doPath.wayPoints.Count; i += 3)
            {
                doPath.wayPoints[i] += DrawSphereHandle(startPosition + doPath.wayPoints[i], 0.5f);
                doPath.wayPoints[i + 1] += DrawSphereHandle(startPosition + doPath.wayPoints[i + 1], 0.5f);

                doPath.wayPoints[i + 2] += DrawHandle(startPosition + doPath.wayPoints[i + 2]);
            }
        }

        private void DrawInstructionsLabel()
        {
            GUIStyle style = new()
            {
                fontSize = 20,

                normal = 
                { 
                    textColor = Color.white 
                }
            };

            Camera sceneViewCamera = SceneView.currentDrawingSceneView.camera;

            Vector2 positionInViewport = new(0.025f, 0.95f);
            Vector3 positionInWorld = sceneViewCamera.ViewportToWorldPoint(new Vector3(positionInViewport.x, positionInViewport.y, 10f));

            Handles.Label(positionInWorld, "CTRL + RMB  ------- Add Segment", style);

            positionInViewport.y -= 0.05f;
            positionInWorld = sceneViewCamera.ViewportToWorldPoint(new Vector3(positionInViewport.x, positionInViewport.y, 10f));

            Handles.Label(positionInWorld, "CTRL + MMB  ------- Insert Segment", style);
            
            positionInViewport.y -= 0.05f;
            positionInWorld = sceneViewCamera.ViewportToWorldPoint(new Vector3(positionInViewport.x, positionInViewport.y, 10f));

            Handles.Label(positionInWorld, "ALT + RMB   ------- Remove Segment", style);
        }

        /// <summary>
        /// Draws a Position Hand for Look At Position and <br/>
        /// Line to Look At Position or Look At Target
        /// </summary>
        private void DrawLookAtHandleAndLine()
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

                for (int i = 0; i < doPath.wayPoints.Count; i++)
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

                for (int i = 0; i < doPath.wayPoints.Count; i++)
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
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doPath.transform.localEulerAngles = SessionState.GetVector3(rotationkey, doPath.transform.localEulerAngles);
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
