#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOScale)), CanEditMultipleObjects]
    public sealed class DOScaleEditor : DOLookAtBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty relativeProp;
        private SerializedProperty speedBasedProp;
        private SerializedProperty targetScaleProp;

        #endregion

        private DOScale doScale;
        private RelativeFlags relativeFlags;
        private string scaleKey;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doScale = (DOScale)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            scaleKey = "DOScale_" + instanceId;

            relativeProp = serializedObject.FindProperty("relative");
            speedBasedProp = serializedObject.FindProperty("speedBased");
            targetScaleProp = serializedObject.FindProperty("targetScale");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Look At", "Scale", "Values", "Events");

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

                if (BeginFoldout("Look At Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawLookAtSettings();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            if (toggleStates[3])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Scale Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawScaleSettings();
                    SetTargetScale();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            DrawLookAtTransformHelpbox();

            if (toggleStates[4])
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

            if (toggleStates[5])
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
        /// Draws Speed Based and relative Prop
        /// </summary>
        private void DrawScaleSettings()
        {
            DrawProperty(speedBasedProp);
            DrawProperty(relativeProp);            
        }

        /// <summary>
        /// Update Target Scale when switching from relative/absolute modes
        /// </summary>
        private void SetTargetScale()
        {
            if (doScale.relative)
            {
                if (relativeFlags.firstTimeRelative)
                {
                    doScale.targetScale -= doScale.transform.localScale;

                    Undo.RecordObject(relativeFlags, "DOScaleEditor_firstTimeNonRelative");
                    relativeFlags.firstTimeRelative = false;
                }

                relativeFlags.firstTimeNonRelative = true;
            }
            else
            {
                if (relativeFlags.firstTimeNonRelative)
                {
                    doScale.targetScale += doScale.transform.localScale;

                    Undo.RecordObject(relativeFlags, "DOScaleEditor_firstTimeRelative");
                    relativeFlags.firstTimeNonRelative = false;
                }

                relativeFlags.firstTimeRelative = true;
            }
        }

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(scaleKey, doScale.transform.localScale);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doScale.transform.localScale = SessionState.GetVector3(scaleKey, doScale.transform.localScale);
        }

        #endregion


    }
}

#endif
