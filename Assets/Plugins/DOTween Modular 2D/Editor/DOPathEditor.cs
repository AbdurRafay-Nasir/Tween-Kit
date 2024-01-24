#if UNITY_EDITOR


using DOTweenModular2D.Enums;
using DOTweenModular2D.Miscellaneous;
using UnityEditor;
using UnityEngine;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOPath)), CanEditMultipleObjects]
    public class DOPathEditor : DOBaseEditor
    {

        #region Serialized Properties

        private SerializedProperty pathTypeProp;
        private SerializedProperty resolutionProp;
        private SerializedProperty connectStartAndEndProp;

        private SerializedProperty lookAtProp;
        private SerializedProperty lookAtPositionProp;
        private SerializedProperty lookAtTargetProp;
        private SerializedProperty offsetProp;
        private SerializedProperty smoothFactorProp;
        private SerializedProperty percentageProp;

        private SerializedProperty speedBasedProp;
        private SerializedProperty relativeProp;
        private SerializedProperty pathPointsProp;

        #endregion

        private DOPath doPath;
        private RelativeFlags relativeFlags;
        private Vector2 beginPosition;

        private bool[] tabStates = new bool[8];
        private string[] savedTabStates = new string[8];

        #region Foldout bools

        private bool pathSettingsFoldout = true;
        private string savedPathSettingsFoldout;

        private bool lookAtSettingsFoldout = true;
        private string savedLookAtSettingsFoldout;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doPath = (DOPath)target;
            relativeFlags = CreateInstance<RelativeFlags>();
            beginPosition = doPath.transform.position;

            SetupSerializedProperties();
            SetupSavedVariables(doPath);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            DrawTabs();

            EditorGUILayout.Space();

            if (tabStates[0])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Life Time Settings
                lifeTimeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lifeTimeSettingsFoldout, "Life Time Settings");
                EditorPrefs.SetBool(savedLifeTimeSettingsFoldout, lifeTimeSettingsFoldout);
                if (lifeTimeSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawLifeTimeSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            DrawTweenObjectHelpBox();

            if (tabStates[1])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Type Settings
                typeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(typeSettingsFoldout, "Type Settings");
                EditorPrefs.SetBool(savedTypeSettingsFoldout, typeSettingsFoldout);
                if (typeSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawTypeSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (tabStates[2])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Path Settings
                pathSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(pathSettingsFoldout, "Path Settings");
                EditorPrefs.SetBool(savedPathSettingsFoldout, pathSettingsFoldout);
                if (pathSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawPathSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (doPath.pathType == DG.Tweening.PathType.CubicBezier)
            {
                DrawCubicBezierHelpBox();
            }

            if (tabStates[3])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Look At Settings
                lookAtSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lookAtSettingsFoldout, "Look At Settings");
                EditorPrefs.SetBool(savedLookAtSettingsFoldout, lookAtSettingsFoldout);
                if (lookAtSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawLookAtSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            DrawLookAtHelpBox();

            if (tabStates[4])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Values
                valuesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(valuesFoldout, "Values");
                EditorPrefs.SetBool(savedValuesFoldout, valuesFoldout);
                if (valuesFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawValues();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            
            if (tabStates[5])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.PropertyField(pathPointsProp);
            }

            if (tabStates[6])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Events
                eventsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(eventsFoldout, "Events");
                EditorPrefs.SetBool(savedEventsFoldout, eventsFoldout);
                if (eventsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.Space();
                    DrawEvents();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (tabStates[7])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Editor Properties
                editorFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(editorFoldout, "Editor");
                EditorPrefs.SetBool(savedEditorFoldout, editorFoldout);
                if (editorFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawEditorProperties();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            serializedObject.ApplyModifiedProperties();

            if (EditorApplication.isPlaying)
                return;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            DrawEditButton();
            DrawPreviewButtons();
            DrawResetEditorPropertiesButton();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void OnSceneGUI()
        {
            if (doPath.begin == Begin.After ||
                doPath.begin == Begin.With)
            {
                Handles.color = Color.white;

                if (doPath.tweenObject != null)
                    DrawTweenObjectInfo();
            }

            SetupPoints(doPath.relative);

            Vector3 startPosition;

            if (EditorApplication.isPlaying)
                startPosition = beginPosition;
            else if (TweenPreviewing)
                startPosition = positionBeforePreview;
            else
                startPosition = doPath.transform.position;

            Handles.color = color[currentHandleColorIndex];

            // do not allow path editing when in play mode
            if (!EditorApplication.isPlaying && editPath && doPath.pathPoints != null)
            {
                switch (doPath.pathType)
                {
                    case DG.Tweening.PathType.Linear:
                    case DG.Tweening.PathType.CatmullRom:
                        DrawSimpleHandle(startPosition, doPath.relative, currentHandleIndex);

                        break;

                    case DG.Tweening.PathType.CubicBezier:
                        if (doPath.pathPoints.Length % 3 == 0)
                            DrawCubicBezierHandle(startPosition, doPath.relative, currentHandleIndex);

                        break;
                }
            }

            if (doPath.pathPoints != null)
            {
                switch (doPath.pathType)
                {
                    case DG.Tweening.PathType.Linear:
                        DrawLinearPath(startPosition, doPath.relative, doPath.connectStartAndEnd);
                        break;

                    case DG.Tweening.PathType.CatmullRom:
                        if (doPath.connectStartAndEnd)
                        {
                            DrawClosedCatmullRomPath(startPosition, doPath.pathPoints, doPath.relative);
                        }
                        else
                        {
                            DrawOpenCatmullRomPath(startPosition, doPath.pathPoints, doPath.relative);
                        }

                        break;

                    case DG.Tweening.PathType.CubicBezier:
                        DrawCubicBezierPath(startPosition, doPath.relative);
                        break;
                }
            }

            if (doPath.lookAt == LookAtPath.None)
                return;

            if (doPath.lookAt == LookAtPath.Position)
            {
                doPath.lookAtPosition = Handles.PositionHandle(doPath.lookAtPosition, Quaternion.identity);
                Handles.DrawDottedLine(doPath.transform.position, doPath.lookAtPosition, 5f);
            }
            else if (doPath.lookAt == LookAtPath.Transform &&
                    doPath.lookAtTarget != null)
            {
                Handles.DrawDottedLine(doPath.transform.position, doPath.lookAtTarget.position, 5f);
            }
        }

        #endregion

        #region Inspector Draw Functions

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();

            GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButton);
            toggleStyle.fixedHeight = 30f;

            string[] tabNames = new string[] { "Life", "Type", "Path", "Look At", "Values", 
                                                "Points", "Events", "Editor" };

            for (int i = 0; i < tabStates.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
                bool toggleState = GUILayout.Toggle(tabStates[i], tabNames[i], toggleStyle);
                if (EditorGUI.EndChangeCheck())
                {
                    tabStates[i] = toggleState;
                    EditorPrefs.SetBool(savedTabStates[i], toggleState);
                }
            }

            GUILayout.EndHorizontal();
        }

        private void DrawPathSettings()
        {
            EditorGUILayout.PropertyField(pathTypeProp);

            if ((DG.Tweening.PathType)pathTypeProp.enumValueIndex != DG.Tweening.PathType.Linear)
            {
                EditorGUILayout.PropertyField(resolutionProp);
            }

            if (doPath.pathType != DG.Tweening.PathType.CubicBezier)
                EditorGUILayout.PropertyField(connectStartAndEndProp);
        }

        private void DrawCubicBezierHelpBox()
        {
            if (doPath.pathPoints == null) return;

            if (doPath.pathPoints.Length % 3 != 0)
            {
                EditorGUILayout.HelpBox("Path Points should be multiple of 3 for Cubic Bezier Curve", 
                                         MessageType.Warning);
            }
        }

        private void DrawLookAtSettings()
        {
            EditorGUILayout.PropertyField(lookAtProp);

            switch ((LookAtPath)lookAtProp.enumValueIndex)
            {
                case LookAtPath.None:
                    return;

                case LookAtPath.Position:
                    EditorGUILayout.PropertyField(lookAtPositionProp);
                    EditorGUILayout.PropertyField(offsetProp);
                    EditorGUILayout.PropertyField(smoothFactorProp);
                    break;

                case LookAtPath.Transform:
                    EditorGUILayout.PropertyField(lookAtTargetProp);
                    EditorGUILayout.PropertyField(offsetProp);
                    EditorGUILayout.PropertyField(smoothFactorProp);
                    break;

                case LookAtPath.Percentage:
                    EditorGUILayout.PropertyField(percentageProp);
                    break;
            }
        }

        private void DrawLookAtHelpBox()
        {
            if (doPath.lookAt == LookAtPath.Transform && doPath.lookAtTarget == null)
            {
                EditorGUILayout.HelpBox("Look At Target not Assigned", MessageType.Error);
            }
            else if (doPath.lookAt != LookAtPath.Transform && doPath.lookAtTarget != null)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.HelpBox("Look At Target is still Assigned, it Should be removed", MessageType.Warning);

                GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
                trashButton.tooltip = "Remove Look At Target";

                if (GUILayout.Button(trashButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize * 2f)))
                {
                    doPath.lookAtTarget = null;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        protected override void DrawValues()
        {
            EditorGUILayout.PropertyField(speedBasedProp);
            EditorGUILayout.PropertyField(relativeProp);

            base.DrawValues();
        }

        #endregion

        #region Draw Handles

        private void DrawSimpleHandle(Vector2 startPosition, bool relative, int handleIndex)
        {
            Handles.color = color[currentHandleColorIndex];

            if (relative)
            {
                for (int i = 0; i < doPath.pathPoints.Length; i++)
                {
                    Vector2 handlePos, newHandlePos;

                    handlePos = startPosition + doPath.pathPoints[i];

                    if (handleIndex == 0)
                        newHandlePos = Handles.PositionHandle(handlePos, Quaternion.identity);
                    else
                        { var fmh_481_74_638417289718576512 = Quaternion.identity; newHandlePos = Handles.FreeMoveHandle(handlePos, currentHandleRadius,
                                                              Vector3.zero, Handles.SphereHandleCap); }

                    if (newHandlePos != handlePos)
                    {
                        Undo.RecordObject(doPath, "Simple Move Handle Relative");

                        Vector2 delta = newHandlePos - handlePos;
                        doPath.pathPoints[i] = doPath.pathPoints[i] + delta;
                    }

                }
            }
            else
            {
                for (int i = 0; i < doPath.pathPoints.Length; i++)
                {
                    Vector2 handlePos = doPath.pathPoints[i];
                    Vector2 newHandlePos;

                    if (handleIndex == 0)
                        newHandlePos = Handles.PositionHandle(handlePos, Quaternion.identity);
                    else
                        { var fmh_504_74_638417289718582228 = Quaternion.identity; newHandlePos = Handles.FreeMoveHandle(handlePos,
                                            currentHandleRadius, Vector3.zero, Handles.SphereHandleCap); }

                    if (newHandlePos != handlePos)
                    {
                        Undo.RecordObject(doPath, "Simple Move Handle Non Relative");
                        doPath.pathPoints[i] = newHandlePos;
                    }
                }
            }
        }

        private void DrawCubicBezierHandle(Vector3 startPosition, bool relative, int handleIndex)
        {
            Handles.color = color[currentHandleColorIndex];

            if (!TweenPreviewing && !relative)
            {
                if (beginPosition != (Vector2)doPath.transform.position)
                {
                    Vector2 delta = (Vector2)doPath.transform.position - beginPosition;

                    doPath.pathPoints[1] += delta;

                    beginPosition = doPath.transform.position;
                }
            }

            for (int i = 0; i < doPath.pathPoints.Length; i += 3)
            {
                if (relative)
                {
                    Vector2 handlePosition, newHandlePosition, delta;

                    // draw handle at point
                    handlePosition = (Vector2)startPosition + doPath.pathPoints[i];

                    if (handleIndex == 0)
                    {
                        newHandlePosition = Handles.PositionHandle(handlePosition, Quaternion.identity);
                    }
                    else
                    {
                        var fmh_547_84_638417289718592099 = Quaternion.identity; newHandlePosition = Handles.FreeMoveHandle(handlePosition,
                                            currentHandleRadius, Vector3.zero, Handles.SphereHandleCap);
                    }

                    if (newHandlePosition != handlePosition)
                    {
                        Undo.RecordObject(doPath, "Cubic Bezier Point Move Handle Relative");

                        delta = newHandlePosition - handlePosition;
                        doPath.pathPoints[i] += delta;

                        doPath.pathPoints[i + 2] += delta;

                        if (i + 4 < doPath.pathPoints.Length)
                            doPath.pathPoints[i + 4] += delta;
                    }

                    // draw handle at 1st contorl point
                    handlePosition = (Vector2)startPosition + doPath.pathPoints[i + 1];

                    var fmh_567_80_638417289718595892 = Quaternion.identity; newHandlePosition = Handles.FreeMoveHandle(handlePosition,
                                        currentHandleRadius, Vector3.zero, Handles.SphereHandleCap);

                    if (newHandlePosition != handlePosition)
                    {
                        Undo.RecordObject(doPath, "Cubic Bezier Point Control 1 Move Handle Relative");

                        delta = newHandlePosition - handlePosition;
                        doPath.pathPoints[i + 1] += delta;
                    }

                    // draw handle at 2nd contorl point
                    handlePosition = (Vector2)startPosition + doPath.pathPoints[i + 2];
                    var fmh_580_80_638417289718599433 = Quaternion.identity; newHandlePosition = Handles.FreeMoveHandle(handlePosition,
                                        currentHandleRadius, Vector3.zero, Handles.SphereHandleCap);

                    if (newHandlePosition != handlePosition)
                    {
                        Undo.RecordObject(doPath, "Cubic Bezier Point Control 2 Move Handle Relative");

                        delta = newHandlePosition - handlePosition;
                        doPath.pathPoints[i + 2] += delta;
                    }
                }

                else
                {
                    Vector2 handlePosition, newHandlePosition;

                    handlePosition = doPath.pathPoints[i];

                    // draw handle at point
                    if (handleIndex == 0)
                        newHandlePosition = Handles.PositionHandle(handlePosition, Quaternion.identity);
                    else
                        { var fmh_602_84_638417289718604827 = Quaternion.identity; newHandlePosition = Handles.FreeMoveHandle(handlePosition,
                                            currentHandleRadius, Vector3.zero, Handles.SphereHandleCap); }

                    if (newHandlePosition != handlePosition)
                    {
                        Undo.RecordObject(doPath, "Cubic Bezier Point Move Handle Non Relative");
                        Vector2 delta = newHandlePosition - handlePosition;

                        doPath.pathPoints[i] = newHandlePosition;
                        doPath.pathPoints[i + 2] += delta;

                        if (i + 4 < doPath.pathPoints.Length)
                            doPath.pathPoints[i + 4] += delta;
                    }


                    // draw control point 1 handle
                    handlePosition = doPath.pathPoints[i + 1];

                    var fmh_621_80_638417289718608588 = Quaternion.identity; newHandlePosition = Handles.FreeMoveHandle(handlePosition,
                                        currentHandleRadius, Vector3.zero, Handles.SphereHandleCap);

                    if (newHandlePosition != handlePosition)
                    {
                        Undo.RecordObject(doPath, "Cubic Bezier Control 1 Move Handle Non Relative");

                        doPath.pathPoints[i + 1] = newHandlePosition;
                    }

                    // draw control point 2 handle
                    handlePosition = doPath.pathPoints[i + 2];
                    var fmh_633_80_638417289718612118 = Quaternion.identity; newHandlePosition = Handles.FreeMoveHandle(handlePosition,
                                        currentHandleRadius, Vector3.zero, Handles.SphereHandleCap);

                    if (newHandlePosition != handlePosition)
                    {
                        Undo.RecordObject(doPath, "Cubic Bezier Control 2 Move Handle Non Relative");

                        doPath.pathPoints[i + 2] = newHandlePosition;
                    }
                }
            }

        }

        #endregion

        #region Draw Path

        private void DrawLinearPath(Vector2 startPosition, bool relative, bool connectStartAndEnd)
        {
            Vector3 previousPosition = startPosition;

            if (relative)
            {
                for (int i = 0; i < doPath.pathPoints.Length; i++)
                {
                    Handles.color = color[currentLineColorIndex];

                    Vector3 nextPosition = startPosition + doPath.pathPoints[i];
                    Handles.DrawLine(previousPosition, nextPosition, currentLineWidth);
                    previousPosition = nextPosition;

                    if (currentHandleIndex == 0)
                    {
                        Handles.color = color[currentHandleColorIndex];
                        Handles.SphereHandleCap(5, previousPosition, Quaternion.identity,
                                                currentHandleRadius, EventType.Repaint);
                    }
                }
            }
            else
            {

                for (int i = 0; i < doPath.pathPoints.Length; i++)
                {
                    Handles.color = color[currentLineColorIndex];

                    Handles.DrawLine(previousPosition, doPath.pathPoints[i], currentLineWidth);
                    previousPosition = doPath.pathPoints[i];

                    if (currentHandleIndex == 0)
                    {
                        Handles.color = color[currentHandleColorIndex];
                        Handles.SphereHandleCap(5, previousPosition, Quaternion.identity,
                                                currentHandleRadius, EventType.Repaint);
                    }
                }
            }

            Handles.color = color[currentLineColorIndex];

            if (connectStartAndEnd)
            {
                Vector3 lastPoint;

                if (doPath.relative)
                {
                    lastPoint = startPosition + doPath.pathPoints[doPath.pathPoints.Length - 1];
                }
                else
                {
                    lastPoint = doPath.pathPoints[doPath.pathPoints.Length - 1];
                }

                Handles.DrawLine(lastPoint, startPosition, currentLineWidth);
            }
        }

        private void DrawOpenCatmullRomPath(Vector3 startPosition, Vector2[] points, bool relative)
        {
            Handles.color = color[currentHandleColorIndex];
            if (currentHandleIndex == 0)
            {
                if (relative)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        Handles.SphereHandleCap(5, startPosition + (Vector3)points[i], Quaternion.identity,
                                            currentHandleRadius, EventType.Repaint);
                    }
                }
                else
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        Handles.SphereHandleCap(5, points[i], Quaternion.identity,
                                            currentHandleRadius, EventType.Repaint);
                    }
                }
            }

            Vector2[] catmullRomPoints = Curve.GetOpenCatmullRomPoints(startPosition, points, doPath.resolution, relative);

            if (catmullRomPoints == null)
                return;

            Handles.color = color[currentLineColorIndex];

            for (int i = 0; i < catmullRomPoints.Length - 1; i++)
            {
                Handles.DrawLine(catmullRomPoints[i], catmullRomPoints[i + 1], currentLineWidth);
            }

        }

        private void DrawClosedCatmullRomPath(Vector3 startPosition, Vector2[] points, bool relative)
        {
            Handles.color = color[currentHandleColorIndex];
            if (currentHandleIndex == 0)
            {
                if (relative)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        Handles.SphereHandleCap(5, startPosition + (Vector3)points[i], Quaternion.identity,
                                            currentHandleRadius, EventType.Repaint);
                    }
                }
                else
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        Handles.SphereHandleCap(5, points[i], Quaternion.identity,
                                            currentHandleRadius, EventType.Repaint);
                    }
                }
            }

            Vector2[] catmullRomPoints = Curve.GetClosedCatmullRomPoints(startPosition, points, doPath.resolution, relative);

            if (catmullRomPoints == null)
                return;

            Handles.color = color[currentLineColorIndex];

            for (int i = 0; i < catmullRomPoints.Length - 1; i++)
            {
                Handles.DrawLine(catmullRomPoints[i], catmullRomPoints[i + 1], currentLineWidth);
            }

        }

        private void DrawCubicBezierPath(Vector3 startPosition, bool relative)
        {
            Vector2[] points = Curve.GetCubicBezierPoints(startPosition, doPath.pathPoints, doPath.resolution, relative);

            if (points == null)
                return;

            Handles.color = color[currentLineColorIndex];

            if (relative)
            {
                Handles.DrawLine(startPosition, startPosition + (Vector3)doPath.pathPoints[1]);

                for (int i = 0; i < doPath.pathPoints.Length; i += 3)
                {
                    Handles.DrawLine(startPosition + (Vector3)doPath.pathPoints[i],
                                     startPosition + (Vector3)doPath.pathPoints[i + 2]);

                    if (i + 4 < doPath.pathPoints.Length)
                        Handles.DrawLine(startPosition + (Vector3)doPath.pathPoints[i],
                                         startPosition + (Vector3)doPath.pathPoints[i + 4]);
                }
            }
            else
            {
                Handles.DrawLine(startPosition, doPath.pathPoints[1]);

                for (int i = 0; i < doPath.pathPoints.Length; i += 3)
                {
                    Handles.DrawLine(doPath.pathPoints[i], doPath.pathPoints[i + 2]);

                    if (i + 4 < doPath.pathPoints.Length)
                        Handles.DrawLine(doPath.pathPoints[i], doPath.pathPoints[i + 4]);
                }
            }

            // draw cubic bezier curve
            for (int i = 0; i < points.Length - 1; i++)
            {
                Handles.DrawLine(points[i], points[i + 1], currentLineWidth);
            }
        }

        #endregion

        #region Setup Functions

        private void SetupPoints(bool relative)
        {
            if (relative)
            {
                if (relativeFlags.firstTimeRelative)
                {
                    doPath.pathPoints = Curve.GetRelativePoints(doPath.transform.position, doPath.pathPoints);

                    Undo.RecordObject(relativeFlags, "DOPath_firstTimeRelative");
                    relativeFlags.firstTimeRelative = false;
                }

                relativeFlags.firstTimeNonRelative = true;
            }
            else
            {
                if (relativeFlags.firstTimeNonRelative)
                {
                    doPath.pathPoints = Curve.GetAbsolutePoints(doPath.transform.position, doPath.pathPoints);

                    Undo.RecordObject(relativeFlags, "DOPath_firstTimeNonRelative");
                    relativeFlags.firstTimeNonRelative = false;
                }

                relativeFlags.firstTimeRelative = true;
            }

        }

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            pathTypeProp = serializedObject.FindProperty("pathType");
            resolutionProp = serializedObject.FindProperty("resolution");

            lookAtProp = serializedObject.FindProperty("lookAt");
            lookAtPositionProp = serializedObject.FindProperty("lookAtPosition");
            lookAtTargetProp = serializedObject.FindProperty("lookAtTarget");
            offsetProp = serializedObject.FindProperty("offset");
            smoothFactorProp = serializedObject.FindProperty("smoothFactor");
            percentageProp = serializedObject.FindProperty("percentage");

            speedBasedProp = serializedObject.FindProperty("speedBased");
            relativeProp = serializedObject.FindProperty("relative");
            connectStartAndEndProp = serializedObject.FindProperty("connectStartAndEnd");
            pathPointsProp = serializedObject.FindProperty("pathPoints");
        }

        protected override void SetupSavedVariables(DOBase doPath)
        {
            base.SetupSavedVariables(doPath);

            int instanceId = doPath.GetInstanceID();

            savedPathSettingsFoldout = "DOPathEditor_pathSettingsFoldout_" + instanceId;
            pathSettingsFoldout = EditorPrefs.GetBool(savedPathSettingsFoldout, true);

            savedLookAtSettingsFoldout = "DOPathEditor_lookAtSettingsFoldout_" + instanceId;
            lookAtSettingsFoldout = EditorPrefs.GetBool(savedLookAtSettingsFoldout, true);

            for (int i = 0; i < savedTabStates.Length; i++)
            {
                savedTabStates[i] = "DOPathEditor_tabStates_" + i + " " + instanceId;
                tabStates[i] = EditorPrefs.GetBool(savedTabStates[i], true);
            }
        }

        protected override void ClearSavedEditorPrefs()
        {
            base.ClearSavedEditorPrefs();

            if (EditorPrefs.HasKey(savedPathSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedPathSettingsFoldout);
            }

            if (EditorPrefs.HasKey(savedLookAtSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedLookAtSettingsFoldout);
            }
        }

        #endregion

    }

}

#endif