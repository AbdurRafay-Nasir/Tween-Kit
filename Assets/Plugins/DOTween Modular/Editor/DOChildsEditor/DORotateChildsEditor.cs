#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DORotateChilds)), CanEditMultipleObjects]
    public sealed class DORotateChildsEditor : DOChildsBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty rotateModeProp;
        private SerializedProperty useLocalProp;
        private SerializedProperty targetRotationProp;

        #endregion

        private UnityEngine.Transform transform;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            transform = ((DORotateChilds)target).transform;

            rotateModeProp = serializedObject.FindProperty("rotateMode");
            useLocalProp = serializedObject.FindProperty("useLocal");
            targetRotationProp = serializedObject.FindProperty("targetRotation");
        }

        public override void OnInspectorGUI()
        {
            if (transform.childCount == 0)
            {
                DrawHelpbox("There are no Child Game Objects, What are you supposed to do with this Component?", MessageType.Error);

                return;
            }

            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Childs", "Values", "Events");

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

                if (BeginFoldout("Childs Rotate Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawChildsRotateSettings();

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
        /// Draws Rotate Mode, Use Local & Join properties
        /// </summary>
        private void DrawChildsRotateSettings()
        {
            DrawProperty(joinProp);
            DrawProperty(rotateModeProp);
            DrawProperty(useLocalProp);
        }
    }
}

#endif
