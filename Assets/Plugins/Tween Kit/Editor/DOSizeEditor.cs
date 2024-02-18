#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace TweenKit.Editor
{
    [CustomEditor(typeof(DOSize)), CanEditMultipleObjects]
    public class DOSizeEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty speedbasedProp;
        private SerializedProperty relativeProp;
        private SerializedProperty targetSizeProp;

        #endregion

        private SpriteRenderer sr;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            DOSize doSize = (DOSize)target;
            sr = doSize.GetComponent<SpriteRenderer>();

            speedbasedProp = serializedObject.FindProperty("speedBased");
            relativeProp = serializedObject.FindProperty("relative");
            targetSizeProp = serializedObject.FindProperty("targetSize");
        }

        public override void OnInspectorGUI()
        {
            if (sr.drawMode == SpriteDrawMode.Simple)
            {
                DrawHelpbox("Sprite Renderer Draw Mode is set to Simple" + "\n" +
                            "Draw Mode must either be Sliced or Tiled", MessageType.Error);

                return;
            }

            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Size", "Values", "Events");

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

                if (BeginFoldout("Size Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawSizeSettings();

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

                    DrawProperty(targetSizeProp);
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

        private void DrawSizeSettings()
        {
            DrawProperty(speedbasedProp);
            DrawProperty(relativeProp);
        }
    }
}

#endif
