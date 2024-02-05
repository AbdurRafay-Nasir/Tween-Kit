#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOAnchorPos)), CanEditMultipleObjects]
    public sealed class DOAnchorPosEditor : DOBaseEditor
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

        private string anchoredPositionKey;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doAnchorPos = (DOAnchorPos)target;
            rectTransform = (RectTransform)doAnchorPos.transform;

            relativeFlags = CreateInstance<RelativeFlags>();

            anchoredPositionKey = "DOAnchorPos_" + instanceId;

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

                    DrawProperty(targetPositionProp);
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

        #endregion

        /// <summary>
        /// Draws speedBased, relative, snapping properties
        /// </summary>
        private void DrawMoveSettings()
        {
            DrawProperty(speedBasedProp);
            DrawProperty(relativeProp);
            DrawProperty(snappingProp);
        }

        /// <summary>
        /// Update Target Position when switching from relative/absolute modes
        /// </summary>
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

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(anchoredPositionKey, rectTransform.anchoredPosition);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            rectTransform.anchoredPosition = SessionState.GetVector3(anchoredPositionKey, rectTransform.anchoredPosition);
        }

        #endregion


    }
}

#endif
