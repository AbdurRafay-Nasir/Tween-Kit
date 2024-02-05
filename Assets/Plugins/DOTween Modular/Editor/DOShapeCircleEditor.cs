#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOShapeCircle)), CanEditMultipleObjects]
    public sealed class DOShapeCircleEditor : DOBaseEditor
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

        private string positionKey;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doShapeCircle = (DOShapeCircle)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            positionKey = "DOShapeCircle_" + instanceId;

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

                    DrawProperty(centerProp);
                    DrawProperty(endDegreeProp);
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

        public override void OnSceneGUI()
        {
            base.OnSceneGUI();

            Vector3 handlePosition = CalculateCenterPosition(doShapeCircle.transform.position);

            doShapeCircle.center += (Vector2)DrawHandle(handlePosition);
            DrawLine(doShapeCircle.transform.position, handlePosition, Color.green);
        }

        #endregion

        /// <summary>
        /// Draws Snapping, Use Local(If relative = false) and Relative(If useLocal = false) properties
        /// </summary>
        private void DrawCircleSettings()
        {
            DrawProperty(snappingProp);

            if (!doShapeCircle.relative)
                DrawProperty(useLocalProp);

            if (!doShapeCircle.useLocal)
                DrawProperty(relativeProp);
        }

        /// <summary>
        /// Update Center when switching from relative/absolute modes
        /// </summary>
        private Vector3 CalculateCenterPosition(Vector3 currentPosition)
        {
            Vector2 handlePosition;

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
                        doShapeCircle.center = doShapeCircle.center - (Vector2)doShapeCircle.transform.position;

                        Undo.RecordObject(relativeFlags, "DOShapeCircleEditor_firstTimeNonRelative");
                        relativeFlags.firstTimeRelative = false;
                    }

                    handlePosition = (Vector2)currentPosition + doShapeCircle.center;

                    relativeFlags.firstTimeNonRelative = true;
                }
                else
                {
                    if (relativeFlags.firstTimeNonRelative)
                    {
                        doShapeCircle.center = doShapeCircle.center + (Vector2)doShapeCircle.transform.position;

                        Undo.RecordObject(relativeFlags, "DOShapeCircleEditor_firstTimeRelative");
                        relativeFlags.firstTimeNonRelative = false;
                    }

                    handlePosition = doShapeCircle.center;

                    relativeFlags.firstTimeRelative = true;
                }

            }

            return handlePosition;
        }

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            SessionState.SetVector3(positionKey, doShapeCircle.transform.position);
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            doShapeCircle.transform.position = SessionState.GetVector3(positionKey, doShapeCircle.transform.position);
        }

        #endregion

    }
}

#endif
