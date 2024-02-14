#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOMoveChilds)), CanEditMultipleObjects]
    public sealed class DOMoveChildsEditor : DOChildsBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty relativeProp;
        private SerializedProperty snappingProp;
        private SerializedProperty targetPositionProp;

        #endregion

        private DOMoveChilds doMoveChilds;
        private RelativeFlags relativeFlags;

        private Transform[] childs;
        private Vector3[] startPositions;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doMoveChilds = (DOMoveChilds)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            childs = new Transform[doMoveChilds.transform.childCount];
            startPositions = new Vector3[doMoveChilds.transform.childCount];

            for (int i = 0; i < doMoveChilds.transform.childCount; i++)
            {
                Transform child = doMoveChilds.transform.GetChild(i);

                childs[i] = child;
                startPositions[i] = child.position;
            }

            relativeProp = serializedObject.FindProperty("relative");
            snappingProp = serializedObject.FindProperty("snapping");
            targetPositionProp = serializedObject.FindProperty("targetPosition");
        }

        public override void OnInspectorGUI()
        {
            if (doMoveChilds.transform.childCount == 0)
            {
                DrawHelpbox("There are no Child Game Objects, What are you supposed to do with this Component?", MessageType.Error);

                return;
            }

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

                    DrawProperty(targetPositionProp);
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

            Vector3 handlePosition = CalculateTargetPosition(doMoveChilds.transform.position);

            doMoveChilds.targetPosition += DrawHandle(handlePosition);

            if (doMoveChilds.relative)
            {
                for (int i = 0; i < doMoveChilds.transform.childCount; i++)
                {
                    if (!tweenPreviewing)
                    {
                        startPositions[i] = childs[i].position;
                    }

                    DrawLine(startPositions[i], startPositions[i] + doMoveChilds.targetPosition, Color.green);
                }
            }
            else 
            {
                for (int i = 0; i < doMoveChilds.transform.childCount; i++)
                {
                    if (!tweenPreviewing)
                    {
                        startPositions[i] = childs[i].position;
                    }

                    DrawLine(startPositions[i], doMoveChilds.targetPosition, Color.green);
                }
            }
        }

        #endregion

        /// <summary>
        /// Draws Relative, Snapping & Join Properties
        /// </summary>
        private void DrawChildMoveSettings()
        {
            DrawProperty(relativeProp);
            DrawProperty(snappingProp);
            DrawProperty(joinProp);
        }

        /// <summary>
        /// Update Target Position when switching from relative/absolute modes
        /// </summary>
        private Vector3 CalculateTargetPosition(Vector3 currentPosition)
        {
            Vector3 handlePosition;

            if (doMoveChilds.relative)
            {
                if (relativeFlags.firstTimeRelative)
                {
                    doMoveChilds.targetPosition -= doMoveChilds.transform.position;

                    Undo.RecordObject(relativeFlags, "DOMoveChildsEditor_firstTimeNonRelative");
                    relativeFlags.firstTimeRelative = false;
                }

                handlePosition = currentPosition + doMoveChilds.targetPosition;

                relativeFlags.firstTimeNonRelative = true;
            }
            else
            {
                if (relativeFlags.firstTimeNonRelative)
                {
                    doMoveChilds.targetPosition += doMoveChilds.transform.position;

                    Undo.RecordObject(relativeFlags, "DOMoveChilds_firstTimeRelative");
                    relativeFlags.firstTimeNonRelative = false;
                }

                handlePosition = doMoveChilds.targetPosition;

                relativeFlags.firstTimeRelative = true;
            }

            return handlePosition;
        }

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            for (int i = 0; i < doMoveChilds.transform.childCount; i++)
            {
                startPositions[i] = childs[i].position;
            }
        }
    }
}

#endif
