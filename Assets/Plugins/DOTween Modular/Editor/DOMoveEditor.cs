#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using DOTweenModular.Enums;

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

        private string previewKey;
        private string saveKey;

        private Vector3 startPosition;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doMove = (DOMove)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            previewKey = "DOMove_preview" + instanceId;
            saveKey = "DOMove_" + instanceId;

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

            DrawSeparatorLine();

            DrawPlayButton();
            DrawStopButton();

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnSceneGUI()
        {
            if (doMove.begin == Begin.After ||
                doMove.begin == Begin.With)
            {
                if (doMove.tweenObject != null)
                    DrawTweenObjectInfo();
            }
                        
            if (!SessionState.GetBool(previewKey, false))
            {
                startPosition = doMove.transform.position;
            }

            // TargetPosition
            Vector3 handlePosition = CalculateTargetPosition(startPosition);

            doMove.targetPosition += DrawHandle(handlePosition);
            DrawLine(startPosition, handlePosition, Color.green);
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

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            startPosition = doMove.transform.position;

            SessionState.SetBool(previewKey, true);
            SessionState.SetVector3(saveKey, doMove.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            SessionState.SetBool(previewKey, false);
            doMove.transform.position = SessionState.GetVector3(saveKey, doMove.transform.position);
        }

        #endregion

    }
}

#endif