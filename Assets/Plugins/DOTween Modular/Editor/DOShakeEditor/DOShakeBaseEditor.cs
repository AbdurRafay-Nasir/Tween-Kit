#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    public class DOShakeBaseEditor : DOBaseEditor
    {
        #region Serialized Properties

        protected SerializedProperty fadeoutProp;
        protected SerializedProperty vibratoProp;
        protected SerializedProperty randomnessProp;
        protected SerializedProperty randomnessModeProp;

        #endregion

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            fadeoutProp = serializedObject.FindProperty("fadeout");
            vibratoProp = serializedObject.FindProperty("vibrato");
            randomnessProp = serializedObject.FindProperty("randomness");
            randomnessModeProp = serializedObject.FindProperty("randomnessMode");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Shake", "Values", "Events");

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

                if (BeginFoldout("Shake Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawShakeSettings();

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

        private void DrawShakeSettings()
        {
            DrawProperty(randomnessModeProp);
            DrawProperty(randomnessProp);
            DrawProperty(vibratoProp);
            DrawProperty(fadeoutProp);
        }
    }
}

#endif
