#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOAnchorPos)), CanEditMultipleObjects]
    public class DOAnchorPosEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty speedBasedProp;
        private SerializedProperty relativeProp;
        private SerializedProperty snappingProp;
        private SerializedProperty targetPositionProp;

        #endregion

        private DOAnchorPos doAnchorPos;
        private RectTransform rectTransform;

        private RelativeFlags relativeFlags;

        private string saveKey;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doAnchorPos = (DOAnchorPos)target;
            rectTransform = (RectTransform)doAnchorPos.transform;

            relativeFlags = CreateInstance<RelativeFlags>();

            saveKey = "DOAnchorPos_" + instanceId;

            speedBasedProp = serializedObject.FindProperty("speedBased");            
            relativeProp = serializedObject.FindProperty("relative");
            snappingProp = serializedObject.FindProperty("snapping");
            targetPositionProp = serializedObject.FindProperty("targetPosition");
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
                    SetTargetPosition();

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

        #endregion

        #region Inspector Draw Functions

        private void DrawMoveSettings()
        {
            DrawProperty(speedBasedProp);
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

            SessionState.SetVector3(saveKey, doAnchorPos.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doAnchorPos.transform.position = SessionState.GetVector3(saveKey, doAnchorPos.transform.position);
        }

        #endregion

        private void SetTargetPosition()
        {
            if (doAnchorPos.relative)
            {
                if (relativeFlags.firstTimeRelative)
                {
                    doAnchorPos.targetPosition -= rectTransform.anchoredPosition;

                    Undo.RecordObject(relativeFlags, "DOAnchorPosEditor_firstTimeNonRelative");
                    relativeFlags.firstTimeRelative = false;
                }

                relativeFlags.firstTimeNonRelative = true;
            }
            else
            {
                if (relativeFlags.firstTimeNonRelative)
                {
                    doAnchorPos.targetPosition += rectTransform.anchoredPosition;

                    Undo.RecordObject(relativeFlags, "DOAnchorPosEditor_firstTimeRelative");
                    relativeFlags.firstTimeNonRelative = false;
                }

                relativeFlags.firstTimeRelative = true;
            }
        }

    }
}

#endif
