#if UNITY_EDITOR

using DOTweenModular.Enums;
using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOMove)), CanEditMultipleObjects]
    public class DOMoveEditor : DOBaseEditor
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

        private bool[] tabStates = new bool[5];
        private string[] savedTabStates = new string[5];

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            speedBasedProp = serializedObject.FindProperty("speedBased");
            useLocalProp = serializedObject.FindProperty("useLocal");
            relativeProp = serializedObject.FindProperty("relative");
            snappingProp = serializedObject.FindProperty("snapping");
            targetPositionProp = serializedObject.FindProperty("targetPosition");

            doMove = (DOMove)target;
            beginPosition = doMove.transform.position;

            relativeFlags = CreateInstance<RelativeFlags>();

            SetupSavedVariables();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            DrawTabs();

            EditorGUILayout.Space();            

            if (tabStates[0])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Life Time Settings", true))
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

            if (tabStates[1])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Type Settings", true))
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

            if (tabStates[2])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Move Settings", true))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawMoveSettings();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            if (tabStates[3])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Values", true))
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

            if (tabStates[4])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Events", true))
                {
                    EditorGUI.indentLevel++;

                    Space();

                    DrawEvents();

                    EditorGUI.indentLevel--;
                }
                EndFoldout();
            }

            serializedObject.ApplyModifiedProperties();
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

            Vector3 handlePosition = CalculateTargetPosition(Vector2.zero);
            DrawTargetLineAndSphere(Vector2.zero, handlePosition, Color.green, Color.green);
        }

        #endregion

        #region Draw Properties Funtions

        private void DrawSpeedBasedProperty()
        {
            EditorGUILayout.PropertyField(speedBasedProp);
        }
        private void DrawUseLocalProperty()
        {
            EditorGUILayout.PropertyField(useLocalProp);
        }
        private void DrawRelativeProperty()
        {
            EditorGUILayout.PropertyField(relativeProp);
        }
        private void DrawSnappingProperty()
        {
            EditorGUILayout.PropertyField(snappingProp);
        }
        private void DrawTargetPositionProperty()
        {
            EditorGUILayout.PropertyField(targetPositionProp);
        }

        #endregion

        private Vector3 CalculateTargetPosition(Vector3 startPosition)
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
                        doMove.targetPosition = doMove.targetPosition - doMove.transform.position;

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
                        doMove.targetPosition = doMove.targetPosition + doMove.transform.position;

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

        private void DrawTargetLine(Vector3 startPosition, Vector3 endPosition, Color lineColor)
        {
            Handles.color = lineColor;
            Handles.DrawLine(startPosition, endPosition, 2f);
        }

        private void DrawTargetPoint(Vector3 startPosition, Vector3 endPosition, Color handleColor, float radius)
        {
            Handles.color = handleColor;
            Handles.SphereHandleCap(2, endPosition, Quaternion.identity, 2f, EventType.Repaint);
        }

        private void DrawTargetLineAndSphere(Vector3 startPosition, Vector3 endPosition, Color handleColor, Color lineColor)
        {
            Handles.color = handleColor;
            Handles.SphereHandleCap(2, endPosition, Quaternion.identity, 2f, EventType.Repaint);
        }

        private void DrawTargetHandle(Vector3 handlePosition, Color handleColor)
        {
            Vector3 newHandlePosition;

            newHandlePosition = Handles.PositionHandle(handlePosition, Quaternion.identity);

            Handles.color = handleColor;

            if (newHandlePosition != handlePosition)
            {
                // Register the current object for undo
                Undo.RecordObject(doMove, "Move Handle");

                // Perform the handle move and update the serialized data
                Vector3 delta = newHandlePosition - handlePosition;
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

            string[] tabNames = new string[] { "Life", "Type", "Move", "Values", "Events" };

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
            DrawSpeedBasedProperty();
            DrawUseLocalProperty();
            DrawRelativeProperty();
            DrawSnappingProperty();
        }

        protected override void DrawValues()
        {
            DrawTargetPositionProperty();
            base.DrawValues();
        }

        #endregion

        protected void SetupSavedVariables()
        {
            for (int i = 0; i < savedTabStates.Length; i++)
            {
                savedTabStates[i] = "DOMoveEditor_tabStates_" + i + " " + instanceId;
                tabStates[i] = EditorPrefs.GetBool(savedTabStates[i], true);
            }
        }

    }

}
#endif