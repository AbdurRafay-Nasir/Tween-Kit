#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOAnchorPosChilds)), CanEditMultipleObjects]
    public sealed class DOAnchorPosChildsEditor : DOChildsBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty relativeProp;
        private SerializedProperty snappingProp;
        private SerializedProperty targetPositionProp;

        #endregion

        private DOAnchorPosChilds doAnchorPosChilds;
        private RectTransform rectTransform;
        private RelativeFlags relativeFlags;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doAnchorPosChilds = (DOAnchorPosChilds)target;
            rectTransform = (RectTransform)doAnchorPosChilds.transform;
            relativeFlags = CreateInstance<RelativeFlags>();

            relativeProp = serializedObject.FindProperty("relative");
            snappingProp = serializedObject.FindProperty("snapping");
            targetPositionProp = serializedObject.FindProperty("targetPosition");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Child", "Values", "Events");

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

                if (BeginFoldout("Childs Move Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawChildMoveSettings();
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
        /// Draws Relative, Snapping & Join Properties
        /// </summary>
        private void DrawChildMoveSettings()
        {
            DrawProperty(relativeProp);
            DrawProperty(snappingProp);
            DrawProperty(joinProp);
        }

        /// <summary>
        /// Update Target Position when switching from relative/absolute modes
        /// </summary>
        private void SetTargetPosition()
        {
            if (doAnchorPosChilds.relative)
            {
                if (relativeFlags.firstTimeRelative)
                {
                    doAnchorPosChilds.targetPosition -= rectTransform.anchoredPosition;

                    Undo.RecordObject(relativeFlags, "DOAnchorPosChildsEditor_firstTimeNonRelative");
                    relativeFlags.firstTimeRelative = false;
                }

                relativeFlags.firstTimeNonRelative = true;
            }
            else
            {
                if (relativeFlags.firstTimeNonRelative)
                {
                    doAnchorPosChilds.targetPosition += rectTransform.anchoredPosition;

                    Undo.RecordObject(relativeFlags, "DOAnchorPosChildsEditor_firstTimeRelative");
                    relativeFlags.firstTimeNonRelative = false;
                }

                relativeFlags.firstTimeRelative = true;
            }
        }
    }
}

#endif
