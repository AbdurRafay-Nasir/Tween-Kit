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

            relativeFlags = CreateInstance<RelativeFlags>();
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

                if (BeginFoldout("Events"))
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
                if (doMove.tweenObject != null)
                    DrawTweenObjectInfo();
            }

            // TargetPosition
            Vector3 handlePosition = CalculateTargetPosition(doMove.transform.position);

            doMove.targetPosition += DrawHandle(handlePosition);
            DrawLine(doMove.transform.position, handlePosition, Color.green);


            // LookAt
            if (doMove.lookAt == LookAtSimple.None) return;

            Vector3 lookTarget = Vector3.zero;

            if (doMove.lookAt == LookAtSimple.Position)
            {
                doMove.lookAtPosition += DrawHandle(doMove.lookAtPosition);

                lookTarget = doMove.lookAtPosition;
            }

            else if (doMove.lookAt == LookAtSimple.Transform)
            {
                if (doMove.lookAtTarget != null)
                    lookTarget = doMove.lookAtTarget.position;
            }

            DrawDottedLine(doMove.transform.position, lookTarget, Color.green, 10f);
        }

        #endregion

        #region Inspector Draw Functions

        private void DrawMoveSettings()
        {
            DrawProperty(speedBasedProp);

            if (!doMove.relative)
                DrawProperty(useLocalProp);

            if (!doMove.useLocal)
                DrawProperty(relativeProp);

            DrawProperty(snappingProp);
        }

        protected override void DrawValues()
        {
            DrawProperty(targetPositionProp);
            base.DrawValues();
        }

        #endregion

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

    }
}

#endif