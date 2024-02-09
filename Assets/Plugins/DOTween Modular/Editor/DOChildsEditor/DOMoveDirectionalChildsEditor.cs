#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using DOTweenModular.Enums;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOMoveDirectionalChilds)), CanEditMultipleObjects]
    public sealed class DOMoveDirectionalChildsEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty joinProp;
        private SerializedProperty directionProp;
        private SerializedProperty snappingProp;
        private SerializedProperty moveAmountProp;

        #endregion

        private DOMoveDirectionalChilds doMoveDirectionalChilds;

        private Transform[] childs;
        private Vector3[] startPositions;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doMoveDirectionalChilds = (DOMoveDirectionalChilds)target;

            childs = new Transform[doMoveDirectionalChilds.transform.childCount];
            startPositions = new Vector3[doMoveDirectionalChilds.transform.childCount];

            for (int i = 0; i < doMoveDirectionalChilds.transform.childCount; i++)
            {
                Transform child = doMoveDirectionalChilds.transform.GetChild(i);

                childs[i] = child;
                startPositions[i] = child.position;
            }

            joinProp = serializedObject.FindProperty("join");
            directionProp = serializedObject.FindProperty("direction");
            snappingProp = serializedObject.FindProperty("snapping");
            moveAmountProp = serializedObject.FindProperty("moveAmount");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Type", "Child", "Values", "Events");

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

                if (BeginFoldout("Child Move Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawChildMoveSettings();

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

                    DrawProperty(moveAmountProp);
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

            for (int i = 0; i < doMoveDirectionalChilds.transform.childCount; i++)
            {
                if (!tweenPreviewing)
                {
                    startPositions[i] = childs[i].position;
                }

                Vector3 childMoveDirection = Vector3.zero;

                switch (doMoveDirectionalChilds.direction)
                {
                    case Direction.LocalUp:
                        childMoveDirection = childs[i].TransformDirection(Vector3.up);
                        break;
                    case Direction.LocalRight:
                        childMoveDirection = childs[i].TransformDirection(Vector3.right);
                        break;
                    case Direction.LocalForward:
                        childMoveDirection = childs[i].TransformDirection(Vector3.forward);
                        break;
                }

                DrawLine(startPositions[i], startPositions[i] + childMoveDirection * doMoveDirectionalChilds.moveAmount, Color.green);
            }
        }

        #endregion

        private void DrawChildMoveSettings()
        {
            DrawProperty(directionProp);
            DrawProperty(snappingProp);
            DrawProperty(joinProp);
        }

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            for (int i = 0; i < doMoveDirectionalChilds.transform.childCount; i++)
            {
                startPositions[i] = childs[i].position;
            }
        }
    }
}

#endif
