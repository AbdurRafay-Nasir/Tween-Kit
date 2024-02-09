#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOScaleChilds)), CanEditMultipleObjects]
    public sealed class DOScaleChildsEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty joinProp;
        private SerializedProperty relativeProp;
        private SerializedProperty targetScaleProp;

        #endregion

        private DOScaleChilds doScaleChilds;
        private RelativeFlags relativeFlags;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doScaleChilds = (DOScaleChilds)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            joinProp = serializedObject.FindProperty("join");
            relativeProp = serializedObject.FindProperty("relative");
            targetScaleProp = serializedObject.FindProperty("targetScale");
        }

        public override void OnInspectorGUI()
        {
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

                    DrawChildScaleSettings();
                    SetTargetScale();

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

        /// <summary>
        /// Draws Relative & Join Properties
        /// </summary>
        private void DrawChildScaleSettings()
        {
            DrawProperty(relativeProp);
            DrawProperty(joinProp);
        }

        /// <summary>
        /// Update Target Scale when switching from relative/absolute modes
        /// </summary>
        private void SetTargetScale()
        {
            if (doScaleChilds.relative)
            {
                if (relativeFlags.firstTimeRelative)
                {
                    doScaleChilds.targetScale -= doScaleChilds.transform.localScale;

                    Undo.RecordObject(relativeFlags, "DOScaleChildsEditor_firstTimeNonRelative");
                    relativeFlags.firstTimeRelative = false;
                }

                relativeFlags.firstTimeNonRelative = true;
            }
            else
            {
                if (relativeFlags.firstTimeNonRelative)
                {
                    doScaleChilds.targetScale += doScaleChilds.transform.localScale;

                    Undo.RecordObject(relativeFlags, "DOScaleChildsChildsEditor_firstTimeRelative");
                    relativeFlags.firstTimeNonRelative = false;
                }

                relativeFlags.firstTimeRelative = true;
            }
        }
    }
}

#endif
