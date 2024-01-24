#if UNITY_EDITOR

using DOTweenModular.Enums;
using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOMove)), CanEditMultipleObjects]
    public class DOMoveEditor : DOLookAtBaseEditor
    {

        #region Serialized Properties

        private SerializedProperty speedBasedProp;
        private SerializedProperty useLocalProp;
        private SerializedProperty relativeProp;
        private SerializedProperty snappingProp;
        private SerializedProperty targetPositionProp;

        #endregion

        private DOMove doMove;
        private RelativeFlags relativeFlags;
        private Vector3 beginPosition;

        private bool[] tabStates = new bool[7];
        private string[] savedTabStates = new string[7];

        #region Foldout bool Properties

        private bool moveSettingsFoldout = true;
        private string savedMoveSettingsFoldout;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doMove = (DOMove)target;
            relativeFlags = CreateInstance<RelativeFlags>();
            beginPosition = doMove.transform.position;

            SetupSerializedProperties();
            SetupSavedVariables(doMove);
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

                // Draw Move Settings
                moveSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(moveSettingsFoldout, "Move Settings");
                EditorPrefs.SetBool(savedMoveSettingsFoldout, moveSettingsFoldout);
                if (moveSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawMoveSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }


            if (tabStates[3])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw LookAt Settings
                lookAtSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lookAtSettingsFoldout, "LookAt Settings");
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

            serializedObject.ApplyModifiedProperties();

            if (tabStates[6])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Editor
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

            if (EditorApplication.isPlaying)
                return;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            DrawEditButton();

            if (!EditorApplication.isPlaying)
                DrawPreviewButtons();

            DrawResetEditorPropertiesButton();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void OnSceneGUI()
        {
            if (doMove.begin == Begin.After ||
                doMove.begin == Begin.With)
            {
                Handles.color = Color.white;

                if (doMove.tweenObject != null)
                    DrawTweenObjectInfo();
            }

            Color handleColor = color[currentHandleColorIndex];
            Color lineColor = color[currentLineColorIndex];

            Vector3 startPosition;

            if (EditorApplication.isPlaying)
                startPosition = beginPosition;
            else if (TweenPreviewing)
                startPosition = positionBeforePreview;
            else
                startPosition = doMove.transform.position;

            Vector3 handlePosition = CalculateTargetPosition(startPosition);
            DrawTargetLineAndSphere(startPosition, handlePosition, handleColor, lineColor);

            if (doMove.lookAt != LookAtSimple.None)
            {
                DrawLookAtLine();
                DrawRotationClampCircle();
            }

            if (!EditorApplication.isPlaying && editPath)
            {
                DrawTargetHandle(handlePosition, handleColor);

                if (doMove.lookAt == LookAtSimple.Position)
                    DrawLookAtHandle();
            }

        }

        #endregion

        private Vector3 CalculateTargetPosition(Vector2 startPosition)
        {
            Vector3 handlePosition;

            if (doMove.useLocal)
            {
                if (doMove.transform.parent != null)
                {
                    handlePosition = doMove.transform.parent.TransformPoint(doMove.targetPosition);
                }
                else
                {
                    handlePosition = doMove.targetPosition;
                }
            }

            else
            {

                if (doMove.relative)
                {
                    if (relativeFlags.firstTimeRelative)
                    {
                        doMove.targetPosition = doMove.targetPosition - (Vector2)doMove.transform.position;

                        Undo.RecordObject(relativeFlags, "DOMoveEditor_firstTimeNonRelative");
                        relativeFlags.firstTimeRelative = false;
                    }

                    handlePosition = startPosition + doMove.targetPosition;

                    relativeFlags.firstTimeNonRelative = true;
                }
                else
                {
                    if (relativeFlags.firstTimeNonRelative)
                    {
                        doMove.targetPosition = doMove.targetPosition + (Vector2)doMove.transform.position;

                        Undo.RecordObject(relativeFlags, "DOMoveEditor_firstTimeRelative");
                        relativeFlags.firstTimeNonRelative = false;
                    }

                    handlePosition = doMove.targetPosition;

                    relativeFlags.firstTimeRelative = true;
                }

            }

            return handlePosition;
        }

        #region Scene Draw Functions

        private void DrawTargetLineAndSphere(Vector3 startPosition, Vector3 endPosition, Color handleColor, Color lineColor)
        {
            Handles.color = handleColor;
            Handles.SphereHandleCap(2, endPosition, Quaternion.identity, currentHandleRadius, EventType.Repaint);

            Handles.color = lineColor;
            Handles.DrawLine(startPosition, endPosition, currentLineWidth);
        }

        private void DrawTargetHandle(Vector3 handlePosition, Color handleColor)
        {
            Vector3 newHandlePosition;

            if (currentHandleIndex == 0)
                newHandlePosition = Handles.PositionHandle(handlePosition, Quaternion.identity);

            else
            {
                Handles.color = handleColor;

                var fmh_365_76_638417289717838669 = Quaternion.identity; newHandlePosition = Handles.FreeMoveHandle(handlePosition,
                                            currentHandleRadius, Vector3.zero, Handles.SphereHandleCap);
            }

            if (newHandlePosition != handlePosition)
            {
                // Register the current object for undo
                Undo.RecordObject(doMove, "Move Handle");

                // Perform the handle move and update the serialized data
                Vector2 delta = newHandlePosition - handlePosition;
                doMove.targetPosition += delta;
            }
        }

        #endregion

        #region Inspector Draw Functions

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();

            GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButton);
            toggleStyle.fixedHeight = 30f;

            string[] tabNames = new string[] { "Life", "Type", "Move", "Look At", "Values", "Events", "Editor" };

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

        private void DrawMoveSettings()
        {
            EditorGUILayout.PropertyField(speedBasedProp);
            EditorGUILayout.PropertyField(useLocalProp);
            EditorGUILayout.PropertyField(relativeProp);
            EditorGUILayout.PropertyField(snappingProp);
        }

        protected override void DrawValues()
        {
            EditorGUILayout.PropertyField(targetPositionProp);
            base.DrawValues();
        }

        #endregion

        #region Setup Functions

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();
            speedBasedProp = serializedObject.FindProperty("speedBased");
            useLocalProp = serializedObject.FindProperty("useLocal");
            relativeProp = serializedObject.FindProperty("relative");
            snappingProp = serializedObject.FindProperty("snapping");
            targetPositionProp = serializedObject.FindProperty("targetPosition");
        }

        protected override void SetupSavedVariables(DOBase doMove)
        {
            base.SetupSavedVariables(doMove);

            int instanceId = doMove.GetInstanceID();

            savedMoveSettingsFoldout = "DOMoveEditor_moveSettingsFoldout_" + instanceId;
            moveSettingsFoldout = EditorPrefs.GetBool(savedMoveSettingsFoldout, true);

            for (int i = 0; i < savedTabStates.Length; i++)
            {
                savedTabStates[i] = "DOMoveEditor_tabStates_" + i + " " + instanceId;
                tabStates[i] = EditorPrefs.GetBool(savedTabStates[i], true);
            }
        }

        protected override void ClearSavedEditorPrefs()
        {
            base.ClearSavedEditorPrefs();

            if (EditorPrefs.HasKey(savedMoveSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedMoveSettingsFoldout);
            }
        }

        #endregion

    }

}
#endif