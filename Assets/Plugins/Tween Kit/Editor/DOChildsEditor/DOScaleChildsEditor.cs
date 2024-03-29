#if UNITY_EDITOR

using UnityEditor;

namespace TweenKit.Editor
{
    [CustomEditor(typeof(DOScaleChilds)), CanEditMultipleObjects]
    public sealed class DOScaleChildsEditor : DOChildsBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty relativeProp;
        private SerializedProperty targetScaleProp;

        #endregion

        private UnityEngine.Transform transform;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            transform = ((DOScaleChilds)target).transform;

            relativeProp = serializedObject.FindProperty("relative");
            targetScaleProp = serializedObject.FindProperty("targetScale");
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

                if (BeginFoldout("Childs Scale Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawChildsSettings();

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

                    DrawProperty(targetScaleProp);
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

        private void DrawChildsSettings()
        {
            DrawProperty(joinProp);
            DrawProperty(relativeProp);
        }
    }
}

#endif
