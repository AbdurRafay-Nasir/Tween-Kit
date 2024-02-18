#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace TweenKit.Editor
{
    [CustomEditor(typeof(DOSizeChilds)), CanEditMultipleObjects]
    public class DOSizeChildsEditor : DOChildsBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty relativeProp;
        private SerializedProperty targetSizeProp;

        #endregion

        private Transform transform;
        private SpriteRenderer[] childSRs;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            transform = ((DOSizeChilds)target).transform;
            childSRs = new SpriteRenderer[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                childSRs[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            }

            relativeProp = serializedObject.FindProperty("relative");
            targetSizeProp = serializedObject.FindProperty("targetSize");
        }

        public override void OnInspectorGUI()
        {
            if (transform.childCount == 0)
            {
                DrawHelpbox("There are no Child Game Objects, What are you supposed to do with this Component?", MessageType.Error);

                return;
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (childSRs[i].drawMode != SpriteDrawMode.Simple)
                    {
                        continue;
                    }
                    else
                    {
                        DrawHelpbox(childSRs[i].transform.name + "'s Sprite Renderer Draw Mode must be set to Sliced or Tiled",
                                    MessageType.Warning);
                    }
                }
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

                if (BeginFoldout("Child Size Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawChildsSizeSettings();

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

        private void DrawChildsSizeSettings()
        {
            DrawProperty(relativeProp);
            DrawProperty(joinProp);
        }

    }
}

#endif
