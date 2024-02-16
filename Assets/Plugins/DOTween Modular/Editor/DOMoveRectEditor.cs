#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOMoveRect)), CanEditMultipleObjects]
    public class DOMoveRectEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty startFromProp;
        private SerializedProperty centerProp;
        private SerializedProperty cornerRadiusProp;
        private SerializedProperty resolutionProp;

        #endregion

        private DOMoveRect doMoveRect;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doMoveRect = (DOMoveRect)target;

            startFromProp = serializedObject.FindProperty("startFrom");
            centerProp = serializedObject.FindProperty("center");
            cornerRadiusProp = serializedObject.FindProperty("cornerRadius");
            resolutionProp = serializedObject.FindProperty("resolution");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Rect", "Values", "Events");

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

                if (BeginFoldout("Rect Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawRectSettings();
                    float rectWidth = Mathf.Abs(doMoveRect.transform.position.x - doMoveRect.center.x);
                    float rectHeight = Mathf.Abs(doMoveRect.transform.position.y - doMoveRect.center.y);
                    float minRadius = Mathf.Min(rectWidth, rectHeight);
                    doMoveRect.cornerRadius = Mathf.Clamp(doMoveRect.cornerRadius, 0f, minRadius);

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

            doMoveRect.center += (Vector2)DrawHandle(doMoveRect.center);

            float rectWidth = Mathf.Abs(doMoveRect.transform.position.x - doMoveRect.center.x);
            float rectHeight = Mathf.Abs(doMoveRect.transform.position.y - doMoveRect.center.y);

            Vector2[] points = Curve.Arc.GetRect(doMoveRect.center, rectWidth, rectHeight,
                                                 doMoveRect.cornerRadius, doMoveRect.resolution);

            DrawPolyLine(points);
        }

        #endregion

        private void DrawRectSettings()
        {
            DrawProperty(startFromProp);
            DrawProperty(cornerRadiusProp);
            DrawProperty(resolutionProp);
        }

        private void DrawPolyLine(params Vector2[] points)
        {
            Vector2 currentPosition = points[0];

            for (int i = 1; i < points.Length; i++)
            {
                Handles.DrawLine(currentPosition, points[i]);
                currentPosition = points[i];
            }
        }
    }
}

#endif
