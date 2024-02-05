#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOText)), CanEditMultipleObjects]
    public sealed class DOTextEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty targetTextProp;
        private SerializedProperty useRichTextProp;
        private SerializedProperty scrambleModeProp;
        private SerializedProperty scrambleCharsProp;

        #endregion

        private DOText doText;
        private UnityEngine.UI.Text textComponent;

        private string textKey;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doText = (DOText)target;
            textKey = "DOText_" + instanceId;

            textComponent = doText.GetComponent<UnityEngine.UI.Text>();

            targetTextProp = serializedObject.FindProperty("targetText");
            useRichTextProp = serializedObject.FindProperty("useRichText");
            scrambleModeProp = serializedObject.FindProperty("scrambleMode");
            scrambleCharsProp = serializedObject.FindProperty("scrambleChars");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Text", "Values", "Events");

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

                if (BeginFoldout("Text Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawTextSettings();

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

                    DrawProperty(targetTextProp);
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
        /// Draws Scramble Mode, Scramble Chars(If scrambleMode = Custom) and Use Rich Text Properties
        /// </summary>
        private void DrawTextSettings()
        {
            DrawProperty(scrambleModeProp);

            if (doText.scrambleMode == DG.Tweening.ScrambleMode.Custom)
                DrawProperty(scrambleCharsProp);

            DrawProperty(useRichTextProp);
        }

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetString(textKey, textComponent.text);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            textComponent.text = SessionState.GetString(textKey, textComponent.text);
        }

        protected override void OnPreviewForceStopped()
        {
            base.OnPreviewForceStopped();

            textComponent.text = SessionState.GetString(textKey, textComponent.text);
        }

        #endregion

    }
}

#endif
