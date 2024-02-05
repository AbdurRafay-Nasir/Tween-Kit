#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DORotate)), CanEditMultipleObjects]
    public sealed class DORotateEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty rotateModeProp;
        private SerializedProperty speedBasedProp;
        private SerializedProperty useLocalProp;
        private SerializedProperty targetRotationProp;

        #endregion

        private DORotate doRotate;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doRotate = (DORotate)target;

            rotateModeProp = serializedObject.FindProperty("rotateMode");
            speedBasedProp = serializedObject.FindProperty("speedBased");
            useLocalProp = serializedObject.FindProperty("useLocal");
            targetRotationProp = serializedObject.FindProperty("targetRotation");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Rotate", "Values", "Events");

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

                if (BeginFoldout("Rotate Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawRotateSettings();

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

                    DrawProperty(targetRotationProp);
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
        /// Draws Rotate Mode, Speed Based and use Local properties
        /// </summary>
        private void DrawRotateSettings()
        {
            DrawProperty(rotateModeProp);
            DrawProperty(speedBasedProp);
            DrawProperty(useLocalProp);
        }
    }
}

#endif
