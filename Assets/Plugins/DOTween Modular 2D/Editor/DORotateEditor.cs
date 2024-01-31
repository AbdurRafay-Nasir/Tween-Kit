#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DORotate)), CanEditMultipleObjects]
    public class DORotateEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty rotateModeProp;
        private SerializedProperty speedBasedProp;
        private SerializedProperty useLocalProp;
        private SerializedProperty relativeProp;
        private SerializedProperty targetRotationProp;

        #endregion

        private DORotate doRotate;
        private RelativeFlags relativeFlags;

        private string key;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doRotate = (DORotate)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            key = "DORotate_" + instanceId;

            rotateModeProp = serializedObject.FindProperty("rotateMode");
            speedBasedProp = serializedObject.FindProperty("speedBased");
            useLocalProp = serializedObject.FindProperty("useLocal");
            relativeProp = serializedObject.FindProperty("relative");
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
                    SetTargetRotation();

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

        #region Inspector Draw Functions

        private void DrawRotateSettings()
        {
            DrawProperty(rotateModeProp);
            DrawProperty(speedBasedProp);

            if (!doRotate.relative)
                DrawProperty(useLocalProp);

            if (!doRotate.useLocal)
                DrawProperty(relativeProp);
        }

        protected override void DrawValues()
        {
            DrawProperty(targetRotationProp);

            base.DrawValues();
        }

        #endregion

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(key, doRotate.transform.localEulerAngles);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doRotate.transform.localEulerAngles = SessionState.GetVector3(key, doRotate.transform.localEulerAngles);
        }

        #endregion

        private void SetTargetRotation()
        {
            if (doRotate.useLocal && !doRotate.relative)
            {
                if (doRotate.transform.parent != null)
                {
                    doRotate.targetRotation = doRotate.transform.parent.TransformPoint(doRotate.targetRotation);
                }
            }

            else
            {

                if (doRotate.relative)
                {
                    if (relativeFlags.firstTimeRelative)
                    {
                        doRotate.targetRotation = doRotate.targetRotation - doRotate.transform.eulerAngles;

                        Undo.RecordObject(relativeFlags, "DORotateEditor_firstTimeNonRelative");
                        relativeFlags.firstTimeRelative = false;
                    }

                    relativeFlags.firstTimeNonRelative = true;
                }
                else
                {
                    if (relativeFlags.firstTimeNonRelative)
                    {
                        doRotate.targetRotation = doRotate.targetRotation + doRotate.transform.eulerAngles;

                        Undo.RecordObject(relativeFlags, "DORotateEditor_firstTimeRelative");
                        relativeFlags.firstTimeNonRelative = false;
                    }

                    relativeFlags.firstTimeRelative = true;
                }

            }
        }
    }
}

#endif
