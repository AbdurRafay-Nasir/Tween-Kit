#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOShapeCircle)), CanEditMultipleObjects]
    public class DOShapeCircleEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty useLocalProp;
        private SerializedProperty relativeProp;
        private SerializedProperty snappingProp;
        private SerializedProperty centerProp;
        private SerializedProperty endDegreeProp;

        #endregion

        private DOShapeCircle doShapeCircle;
        private RelativeFlags relativeFlags;

        private string key;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doShapeCircle = (DOShapeCircle)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            key = "DOShapeCircle_" + instanceId;

            useLocalProp = serializedObject.FindProperty("useLocal");
            relativeProp = serializedObject.FindProperty("relative");
            snappingProp = serializedObject.FindProperty("snapping");
            centerProp = serializedObject.FindProperty("center");
            endDegreeProp = serializedObject.FindProperty("endDegree");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Circle", "Values", "Events");

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

                if (BeginFoldout("Circle Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawCircleSettings();

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

        public override void OnSceneGUI()
        {
            base.OnSceneGUI();

            Vector3 handlePosition = CalculateTargetPosition(doShapeCircle.transform.position);

            doShapeCircle.center += DrawHandle(handlePosition);
            DrawLine(doShapeCircle.transform.position, handlePosition, Color.green);
        }

        #endregion

        #region Inspector Draw Functions

        private void DrawCircleSettings()
        {
            DrawProperty(useLocalProp);
            DrawProperty(relativeProp);
            DrawProperty(snappingProp);
        }

        protected override void DrawValues()
        {
            DrawProperty(centerProp);
            DrawProperty(endDegreeProp);

            base.DrawValues();
        }

        #endregion

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(key, doShapeCircle.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doShapeCircle.transform.position = SessionState.GetVector3(key, doShapeCircle.transform.position);
        }

        #endregion

        private Vector3 CalculateTargetPosition(Vector3 currentPosition)
        {
            Vector3 handlePosition;

            if (doShapeCircle.useLocal && !doShapeCircle.relative)
            {
                if (doShapeCircle.transform.parent != null)
                {
                    handlePosition = doShapeCircle.transform.parent.TransformPoint(doShapeCircle.center);
                }
                else
                {
                    handlePosition = doShapeCircle.center;
                }
            }

            else
            {

                if (doShapeCircle.relative)
                {
                    if (relativeFlags.firstTimeRelative)
                    {
                        doShapeCircle.center = doShapeCircle.center - doShapeCircle.transform.position;

                        Undo.RecordObject(relativeFlags, "DOShapeCircleEditor_firstTimeNonRelative");
                        relativeFlags.firstTimeRelative = false;
                    }

                    handlePosition = currentPosition + doShapeCircle.center;

                    relativeFlags.firstTimeNonRelative = true;
                }
                else
                {
                    if (relativeFlags.firstTimeNonRelative)
                    {
                        doShapeCircle.center = doShapeCircle.center + doShapeCircle.transform.position;

                        Undo.RecordObject(relativeFlags, "DOShapeCircleEditor_firstTimeRelative");
                        relativeFlags.firstTimeNonRelative = false;
                    }

                    handlePosition = doShapeCircle.center;

                    relativeFlags.firstTimeRelative = true;
                }

            }

            return handlePosition;
        }

    }
}

#endif
