#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using TweenKit.Enums;

namespace TweenKit.Editor
{
    [CustomEditor(typeof(DOMoveDirectional)), CanEditMultipleObjects]
    public sealed class DOMoveDirectionalEditor : DOBaseEditor
    {
        #region Serialized Properties

        public SerializedProperty directionProp;
        public SerializedProperty moveLocallyProp;
        public SerializedProperty speedBasedProp;
        public SerializedProperty snappingProp;
        public SerializedProperty moveAmountProp;

        #endregion

        private DOMoveDirectional doMoveDirectional;

        private Vector3 startPosition;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doMoveDirectional = (DOMoveDirectional)target;
            startPosition = doMoveDirectional.transform.position;

            directionProp = serializedObject.FindProperty("direction");
            moveLocallyProp = serializedObject.FindProperty("moveLocally");
            speedBasedProp = serializedObject.FindProperty("speedBased");
            snappingProp = serializedObject.FindProperty("snapping");
            moveAmountProp = serializedObject.FindProperty("moveAmount");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Move", "Values", "Events");

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

            if (toggleStates[3])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Values"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawProperty(moveAmountProp);
                    DrawValues();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            if (toggleStates[4])
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
                startPosition = doMoveDirectional.transform.position;

            Vector3 moveDirection = Vector3.zero;

            // Calculate the movement direction based on the specified direction
            switch (doMoveDirectional.direction)
            {
                case Direction.LocalUp:
                    moveDirection = Vector3.up;
                    break;
                case Direction.LocalRight:
                    moveDirection = Vector3.right;
                    break;
                case Direction.LocalForward:
                    moveDirection = Vector3.forward;
                    break;
            }

            // Transform the movement direction from local space to world space using the child's rotation
            moveDirection = doMoveDirectional.transform.rotation * moveDirection;

            // Normalize the movement direction
            moveDirection.Normalize();

            // Calculate the move position based on moveDirection and moveAmount
            Vector3 targetPosition = startPosition + moveDirection * doMoveDirectional.moveAmount;

            DrawLine(startPosition, targetPosition, Color.green);
        }

        #endregion

        /// <summary>
        /// Draws Direction, Move Locally, Speed Based & Snapping Properties
        /// </summary>
        private void DrawMoveSettings()
        {
            DrawProperty(directionProp);
            DrawProperty(moveLocallyProp);
            DrawProperty(speedBasedProp);
            DrawProperty(snappingProp);
        }

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            startPosition = doMoveDirectional.transform.position;
        }
    }
}

#endif
