#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOJump)), CanEditMultipleObjects]
    public class DOJumpEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty powerProp;
        private SerializedProperty jumpCountProp;
        private SerializedProperty snappingProp;
        private SerializedProperty useLocalProp;
        private SerializedProperty relativeProp;
        private SerializedProperty targetPositionProp;

        #endregion

        private DOJump doJump;
        private RelativeFlags relativeFlags;

        private string key;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doJump = (DOJump)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            key = "DOJump_" + instanceId;

            powerProp = serializedObject.FindProperty("power");
            jumpCountProp = serializedObject.FindProperty("jumpCount");
            snappingProp = serializedObject.FindProperty("snapping");
            useLocalProp = serializedObject.FindProperty("useLocal");
            relativeProp = serializedObject.FindProperty("relative");
            targetPositionProp = serializedObject.FindProperty("targetPosition");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Jump", "Values", "Events");

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

                if (BeginFoldout("Jump Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawJumpSettings();

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

            Vector3 handlePosition = CalculateTargetPosition(doJump.transform.position);

            doJump.targetPosition += DrawHandle(handlePosition);
            DrawLine(doJump.transform.position, handlePosition, Color.green);
        }

        #endregion

        #region Inspector Draw Functions

        private void DrawJumpSettings()
        {
            DrawProperty(powerProp);
            DrawProperty(jumpCountProp);

            if (!doJump.relative)
                DrawProperty(useLocalProp);

            if (!doJump.useLocal)
                DrawProperty(relativeProp);

            DrawProperty(snappingProp);
        }

        protected override void DrawValues()
        {
            DrawProperty(targetPositionProp);

            base.DrawValues();
        }

        #endregion

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(key, doJump.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doJump.transform.position = SessionState.GetVector3(key, doJump.transform.position);
        }

        #endregion

        private Vector3 CalculateTargetPosition(Vector3 currentPosition)
        {
            Vector3 handlePosition;

            if (doJump.useLocal && !doJump.relative)
            {
                if (doJump.transform.parent != null)
                {
                    handlePosition = doJump.transform.parent.TransformPoint(doJump.targetPosition);
                }
                else
                {
                    handlePosition = doJump.targetPosition;
                }
            }

            else
            {

                if (doJump.relative)
                {
                    if (relativeFlags.firstTimeRelative)
                    {
                        doJump.targetPosition = doJump.targetPosition - doJump.transform.position;

                        Undo.RecordObject(relativeFlags, "DOJumpEditor_firstTimeNonRelative");
                        relativeFlags.firstTimeRelative = false;
                    }

                    handlePosition = currentPosition + doJump.targetPosition;

                    relativeFlags.firstTimeNonRelative = true;
                }
                else
                {
                    if (relativeFlags.firstTimeNonRelative)
                    {
                        doJump.targetPosition = doJump.targetPosition + doJump.transform.position;

                        Undo.RecordObject(relativeFlags, "DOJumpEditor_firstTimeRelative");
                        relativeFlags.firstTimeNonRelative = false;
                    }

                    handlePosition = doJump.targetPosition;

                    relativeFlags.firstTimeRelative = true;
                }

            }

            return handlePosition;
        }

    }
}

#endif
