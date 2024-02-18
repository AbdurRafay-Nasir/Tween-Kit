#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DOTweenModular.Miscellaneous;
using System.Linq;

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
        private Vector3 currentPosition;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doPath = (DOPath)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            rotationkey = "DOPath_LookAt_" + doPath.gameObject.GetInstanceID();

            startPosition = doPath.transform.position;
            currentPosition = startPosition;

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
                CreateSegment(mousePosition);
            }

            if (e.type == EventType.MouseDown && e.button == 1 && e.alt)
            {
                DeleteSegment(mousePosition);
            }

            if (e.type == EventType.MouseDown && e.button == 2 && e.control)
            {
                InsertWaypoint(mousePosition);
            }


            if (doPath.wayPoints.Count < 1)
                return;

            if (!tweenPreviewing)
                currentPosition = doPath.transform.position;

            ConvertWaypointsToAbsoluteOrRelativeIfNeeded(currentPosition);
            DrawPath(currentPosition);
            DrawHandles(currentPosition);
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

        #region Create Segment Functions

        private void CreateSegment(Vector3 position)
        {
            if (SceneView.currentDrawingSceneView.in2DMode)
                position.z = 0f;

            if (doPath.pathType == DG.Tweening.PathType.CubicBezier)
            {
                CreateCubicBezierSegment(position);
            }
            else
            {
                CreateSimpleSegment(position);
            }
        }

        private void CreateSimpleSegment(Vector3 position)
        {
            Undo.RecordObject(doPath, "Added Segment");
            doPath.wayPoints.Add(position);
        }

        private void CreateCubicBezierSegment(Vector3 position)
        {
            Undo.RecordObject(doPath, "Added Segment");

            if (doPath.wayPoints.Count == 0)
            {
                doPath.wayPoints.Add(currentPosition + Vector3.up * 2f);
                doPath.wayPoints.Add(position + Vector3.up * 2f);
                doPath.wayPoints.Add(position);

                return;
            }

            doPath.wayPoints.Add(doPath.wayPoints[^1] * 2 - doPath.wayPoints[^2]);
            doPath.wayPoints.Add((doPath.wayPoints[^1] + position) * .5f);
            doPath.wayPoints.Add(position);
        }

        #endregion

        #region Delete Segments Functions

        private void DeleteSegment(Vector3 position)
        {
            if (SceneView.currentDrawingSceneView.in2DMode)
                position.z = 0f;

            if (doPath.pathType == DG.Tweening.PathType.CubicBezier)
            {
                DeleteCubicBezierSegment(position);

            }
            else
            {
                DeleteSimpleSegment(position);
            }
        }

        private void DeleteSimpleSegment(Vector3 position)
        {
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
                Undo.RecordObject(doPath, "Deleted Segment");
                doPath.wayPoints.RemoveAt(closestWaypointIndex);
            }
        }

        private void DeleteCubicBezierSegment(Vector3 position)
        {
            float minDistanceToPoint = 0.5f;
            int closestWaypointIndex = -1;

            for (int i = 2; i < doPath.wayPoints.Count; i += 3)
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
                Undo.RecordObject(doPath, "Deleted Segment");

                // if last segment is to be deleted
                if (closestWaypointIndex >= doPath.wayPoints.Count - 1)
                {
                    doPath.wayPoints.RemoveAt(doPath.wayPoints.Count - 1);
                    doPath.wayPoints.RemoveAt(doPath.wayPoints.Count - 1);
                    doPath.wayPoints.RemoveAt(doPath.wayPoints.Count - 1);
                }

                // if any segment other than last is to be deleted
                else
                {
                    doPath.wayPoints.RemoveRange(closestWaypointIndex - 1, 3);
                }
            }
        }

        #endregion

        #region Insert Segments Functions

        private void InsertWaypoint(Vector3 position)
        {
            if (SceneView.currentDrawingSceneView.in2DMode)
                position.z = 0f;

            if (doPath.pathType == DG.Tweening.PathType.CubicBezier)
            {
                InsertCubicBezierSegment(position);
            }
            else
            {
                InsertSimpleSegment(position);
            }
        }

        private void InsertSimpleSegment(Vector3 position)
        {
            float minDistance = 0.8f;
            int closestIndex = -1;

            // For inserting waypoint between current position and 1st way point
            float distance = HandleUtility.DistancePointLine(position, currentPosition, doPath.wayPoints[0]);
            if (distance < minDistance)
            {
                Undo.RecordObject(doPath, "Inserted Waypoint");
                doPath.wayPoints.Insert(0, position);

                return;
            }

            // For inserting waypoint anywhere on path
            for (int i = 0; i < doPath.wayPoints.Count - 1; i++)
            {
                distance = HandleUtility.DistancePointLine(position, doPath.wayPoints[i], doPath.wayPoints[i + 1]);

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

            // For inserting waypoint between current position and last way point (in case of closed loop)
            if (doPath.closePath)
            {
                distance = HandleUtility.DistancePointLine(position, currentPosition, doPath.wayPoints[^1]);
                if (distance < minDistance)
                {
                    Undo.RecordObject(doPath, "Inserted Waypoint");
                    doPath.wayPoints.Add(position);
                }
            }
        }

        private void InsertCubicBezierSegment(Vector3 position)
        {
            float minDistance = 0.8f;
            int closestIndex = -1;

            // For inserting segment between segment formed between gameObject's current Position and waypoint at index 2
            float distance = HandleUtility.DistancePointBezier(position, currentPosition, doPath.wayPoints[2],
                                                               doPath.wayPoints[0], doPath.wayPoints[1]);
            if (distance < minDistance)
            {
                Vector3 directionToPrevAnchor = (position - currentPosition).normalized;
                Vector3 startTangent = position - directionToPrevAnchor * 2f;
                Vector3 endTangent = position + directionToPrevAnchor * 2f;

                Undo.RecordObject(doPath, "Inserted Waypoint");
                doPath.wayPoints.InsertRange(1, new Vector3[] { startTangent, position, endTangent });

                return;
            }



            // For inserting anywhere on Cubic bezier path
            for (int i = 2; i < doPath.wayPoints.Count - 1; i += 3)
            {
                distance = HandleUtility.DistancePointBezier(position, doPath.wayPoints[i], doPath.wayPoints[i + 3],
                                                             doPath.wayPoints[i + 1], doPath.wayPoints[i + 2]);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }

            if (closestIndex != -1)
            {
                Vector3 directionToPrevAnchor = (position - doPath.wayPoints[closestIndex]).normalized;
                Vector3 startTangent = position - directionToPrevAnchor * 2f;
                Vector3 endTangent = position + directionToPrevAnchor * 2f;

                Undo.RecordObject(doPath, "Inserted Waypoint");
                doPath.wayPoints.InsertRange(closestIndex + 2, new Vector3[] { startTangent, position, endTangent });
            }

        }

        #endregion

        #region Scene Draw Functions

        private void ConvertWaypointsToAbsoluteOrRelativeIfNeeded(Vector3 currentPosition)
        {
            if (doPath.relative)
            {
                if (relativeFlags.firstTimeRelative)
                {

                    for (int i = 0; i < doPath.wayPoints.Count; i++)
                    {
                        doPath.wayPoints[i] -= currentPosition;
                    }

                    Undo.RecordObject(relativeFlags, "DOPathEditor_firstTimeNonRelative");
                    relativeFlags.firstTimeRelative = false;

                }

                relativeFlags.firstTimeNonRelative = true;
            }
            else
            {
                if (relativeFlags.firstTimeNonRelative)
                {

                    for (int i = 0; i < doPath.wayPoints.Count; i++)
                    {
                        doPath.wayPoints[i] += currentPosition;
                    }

                    Undo.RecordObject(relativeFlags, "DOPathEditor_firstTimeRelative");
                    relativeFlags.firstTimeNonRelative = false;

                }

                relativeFlags.firstTimeRelative = true;
            }
        }

        private void DrawPath(Vector3 currentPosition)
        {
            List<Vector3> pathPoints = new();

            switch (doPath.pathType)
            {
                case DG.Tweening.PathType.Linear:

                    pathPoints = GetLinearPath(currentPosition, doPath.wayPoints, doPath.closePath, doPath.relative);
                    DrawPolyLine(pathPoints);
                    break;

                case DG.Tweening.PathType.CatmullRom:

                    pathPoints = GetCatmullRomPath(currentPosition, doPath.wayPoints, doPath.resolution,
                                                   doPath.closePath, doPath.relative);
                    DrawPolyLine(pathPoints);
                    break;

                case DG.Tweening.PathType.CubicBezier:

                    pathPoints = GetCubicBezierPath(currentPosition, doPath.wayPoints, doPath.resolution, doPath.relative);
                    if (pathPoints != null && doPath.wayPoints.Count % 3 == 0)
                    {
                        DrawPolyLine(pathPoints);
                        DrawBezierLines(currentPosition, doPath.wayPoints, doPath.relative);
                    }
                    break;
            }
        }

        private void DrawHandles(Vector3 currentPosition)
        {
            if (doPath.pathType != DG.Tweening.PathType.CubicBezier)
            {
                DrawSimpleHandles(currentPosition, doPath.relative);
            }
            else
            {
                if (doPath.wayPoints.Count % 3 == 0)
                    DrawCubicBezierHandles(currentPosition, doPath.relative);
            }
        }

        private void DrawPolyLine(List<Vector3> points)
        {
            Vector3 currentLineStart = points[0];

            for (int i = 0; i < points.Count; i++)
            {
                DrawLine(currentLineStart, points[i], Color.green);
                currentLineStart = points[i];
            }
        }

        private void DrawBezierLines(Vector3 startPosition, List<Vector3> waypoints, bool relative)
        {
            if (relative)
            {
                List<Vector3> absolutePoints = new(waypoints);

                for (int i = 0; i < absolutePoints.Count; i++)
                {
                    absolutePoints[i] += startPosition;
                }

                DrawLine(startPosition, absolutePoints[0], Color.green);

                int lastIndex = absolutePoints.Count - 1;

                for (int i = 1; i < lastIndex; i += 3)
                {
                    DrawLine(absolutePoints[i], absolutePoints[i + 1], Color.green, 0.5f);

                    if (i + 2 < lastIndex)
                        DrawLine(absolutePoints[i + 1], absolutePoints[i + 2], Color.green, 0.5f);
                }
            }
            else
            {
                DrawLine(startPosition, waypoints[0], Color.green);

                int lastIndex = waypoints.Count - 1;

                for (int i = 1; i < lastIndex; i += 3)
                {
                    DrawLine(waypoints[i], waypoints[i + 1], Color.green, 0.5f);

                    if (i + 2 < lastIndex)
                        DrawLine(waypoints[i + 1], waypoints[i + 2], Color.green, 0.5f);
                }
            }
        }

        private void DrawSimpleHandles(Vector3 startPosition, bool relative)
        {
            if (relative)
            {
                for (int i = 0; i < doPath.wayPoints.Count; i++)
                {
                    doPath.wayPoints[i] += DrawHandle(startPosition + doPath.wayPoints[i]);
                }
            }
            else
            {
                for (int i = 0; i < doPath.wayPoints.Count; i++)
                {
                    doPath.wayPoints[i] += DrawHandle(doPath.wayPoints[i]);
                }
            }
        }

        private void DrawCubicBezierHandles(Vector3 currentPositionOfGameObject, bool relative)
        {
            int lastIndex = doPath.wayPoints.Count - 1;

            if (relative)
            {
                doPath.wayPoints[0] += DrawSphereHandle(doPath.wayPoints[0] + currentPositionOfGameObject, 0.5f);

                for (int i = 1; i < lastIndex; i += 3)
                {
                    Vector3 changeInPositionOfControlPoint = Vector3.zero;

                    // Control Point
                    changeInPositionOfControlPoint = DrawHandle(doPath.wayPoints[i + 1] + currentPositionOfGameObject);
                    doPath.wayPoints[i + 1] += changeInPositionOfControlPoint;

                    // Tangent 1
                    doPath.wayPoints[i] += DrawSphereHandle(doPath.wayPoints[i] + currentPositionOfGameObject, 0.5f)
                                         + changeInPositionOfControlPoint;

                    // Tangent 2
                    if (i + 2 < lastIndex)
                        doPath.wayPoints[i + 2] += DrawSphereHandle(doPath.wayPoints[i + 2] + currentPositionOfGameObject, 0.5f)
                                                 + changeInPositionOfControlPoint;
                }
            }
            else
            {
                Vector3 changeincurrentposition = Vector3.zero;

                if (startPosition != currentPosition)
                {
                    changeincurrentposition = currentPosition - startPosition;
                    startPosition = currentPosition;
                }

                doPath.wayPoints[0] += DrawSphereHandle(doPath.wayPoints[0], 0.5f) + changeincurrentposition;

                for (int i = 1; i < lastIndex; i += 3)
                {
                    Vector3 changeInPositionOfControlPoint = Vector3.zero;

                    // Control Point
                    changeInPositionOfControlPoint = DrawHandle(doPath.wayPoints[i + 1]);
                    doPath.wayPoints[i + 1] += changeInPositionOfControlPoint;

                    // Tangent 1
                    doPath.wayPoints[i] += DrawSphereHandle(doPath.wayPoints[i], 0.5f) + changeInPositionOfControlPoint;

                    // Tangent 2
                    if (i + 2 < lastIndex)
                        doPath.wayPoints[i + 2] += DrawSphereHandle(doPath.wayPoints[i + 2], 0.5f) + changeInPositionOfControlPoint;
                }
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

        #region Get Path Functions

        private List<Vector3> GetLinearPath(Vector3 startPosition, List<Vector3> waypoints, bool closed, bool relative)
        {
            List<Vector3> linearPoints = new();

            if (relative)
            {
                List<Vector3> absolutePoints = new(waypoints);

                for (int i = 0; i < absolutePoints.Count; i++)
                {
                    absolutePoints[i] += startPosition;
                }

                linearPoints.Add(startPosition);
                linearPoints.AddRange(absolutePoints);

                if (closed)
                    linearPoints.Add(startPosition);
            }
            else
            {
                linearPoints.Add(startPosition);
                linearPoints.AddRange(waypoints);

                if (closed)
                    linearPoints.Add(startPosition);
            }

            return linearPoints;
        }

        private List<Vector3> GetCatmullRomPath(Vector3 startPosition, List<Vector3> waypoints, int resolution, 
                                                bool closed, bool relative)
        {
            List<Vector3> catmullRomPoints;

            if (relative)
            {
                List<Vector3> absolutePoints = new(waypoints);

                for (int i = 0; i < absolutePoints.Count; i++)
                {
                    absolutePoints[i] += startPosition;
                }

                catmullRomPoints = (Curve.CatmullRom.GetSpline(startPosition, absolutePoints.ToArray(), resolution, closed)).ToList();
            }
            else
            {
                catmullRomPoints = (Curve.CatmullRom.GetSpline(startPosition, waypoints.ToArray(), resolution, closed)).ToList();
            }

            return catmullRomPoints;
        }

        private List<Vector3> GetCubicBezierPath(Vector3 startPosition, List<Vector3> waypoints, int resolution, bool relative)
        {
            List<Vector3> cubicBezierPoints;

            if (relative)
            {
                List<Vector3> absolutePoints = new(waypoints);

                for (int i = 0; i < absolutePoints.Count; i++)
                {
                    absolutePoints[i] += startPosition;
                }

                Vector3[] bezierPoints = Curve.CubicBezier.GetSpline(startPosition, absolutePoints.ToArray(), resolution);

                cubicBezierPoints = bezierPoints?.ToList();
            }
            else
            {
                Vector3[] bezierPoints = Curve.CubicBezier.GetSpline(startPosition, waypoints.ToArray(), resolution);
                
                cubicBezierPoints = bezierPoints?.ToList();
            }

            
            return cubicBezierPoints;
        }

        #endregion

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            currentPosition = doPath.transform.position;

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
