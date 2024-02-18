#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOMove)), CanEditMultipleObjects]
    public sealed class DOMoveEditor : DOLookAtBaseEditor
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

        private Vector3 startPosition;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doMove = (DOMove)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            speedBasedProp = serializedObject.FindProperty("speedBased");
            useLocalProp = serializedObject.FindProperty("useLocal");
            relativeProp = serializedObject.FindProperty("relative");
            snappingProp = serializedObject.FindProperty("snapping");
            targetPositionProp = serializedObject.FindProperty("targetPosition");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Look At", "Move", "Values", "Events");

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

            DrawLookAtTransformHelpbox();

            if (toggleStates[3])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Move Settings"))
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

            if (toggleStates[4])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Values"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawProperty(targetPositionProp);
                    DrawValues();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            if (toggleStates[5])
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

            if (!tweenPreviewing)
            {
                startPosition = doMove.transform.position;
            }

            Vector3 handlePosition = CalculateTargetPosition(startPosition);

            doMove.targetPosition += DrawHandle(handlePosition);
            DrawLine(startPosition, handlePosition, Color.green);
        }

        #endregion

        /// <summary>
        /// Draws Speed Based, Use Local(If relative = false), Relative(If useLocal = false) and Snapping Properties
        /// </summary>
        private void DrawMoveSettings()
        {
            DrawProperty(speedBasedProp);

            if (!doMove.relative)
                DrawProperty(useLocalProp);

            if (!doMove.useLocal)
                DrawProperty(relativeProp);

            DrawProperty(snappingProp);
        }

        /// <summary>
        /// Update Target Position when switching from relative/absolute modes
        /// </summary>
        private Vector3 CalculateTargetPosition(Vector3 currentPosition)
        {
            Vector3 handlePosition;

            if (doMove.useLocal && !doMove.relative)
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

                    handlePosition = currentPosition + doMove.targetPosition;

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

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            startPosition = doMove.transform.position;
        }
    }
}

#endif