#if UNITY_EDITOR

using DOTweenModular2D.Enums;
using UnityEngine;
using UnityEditor;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOAnchorPos))]
    public class DOAnchorPosEditor : DOBaseEditor
    {

        #region Serialized Properties

        private SerializedProperty relativeProp;
        private SerializedProperty speedBasedProp;
        private SerializedProperty snappingProp;
        private SerializedProperty targetPositionProp;

        #endregion

        private DOAnchorPos doAnchorPos;
        private RectTransform rectTransform;
        private RelativeFlags relativeFlags;

        private bool[] tabStates = new bool[5];
        private string[] savedTabStates = new string[5];

        #region Foldout Settings

        private bool moveSettingsFoldout = true;
        private string savedMoveSettingsFoldout;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doAnchorPos = (DOAnchorPos)target;
            rectTransform = (RectTransform)doAnchorPos.transform;
            relativeFlags = CreateInstance<RelativeFlags>();

            SetupSerializedProperties();
            SetupSavedVariables(doAnchorPos);
        }

        public override void OnInspectorGUI()
        {
            SetupTargetPosition();

            EditorGUILayout.Space();

            DrawTabs();

            EditorGUILayout.Space();

            if (tabStates[0])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Life Time Settings
                lifeTimeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lifeTimeSettingsFoldout, "Life Time Settings");
                EditorPrefs.SetBool(savedLifeTimeSettingsFoldout, lifeTimeSettingsFoldout);
                if (lifeTimeSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawLifeTimeSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            DrawTweenObjectHelpBox();

            if (tabStates[1])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Type Settings
                typeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(typeSettingsFoldout, "Type Settings");
                EditorPrefs.SetBool(savedTypeSettingsFoldout, typeSettingsFoldout);
                if (typeSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawTypeSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (tabStates[2])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Scale Settings
                moveSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(moveSettingsFoldout, "Move Settings");
                EditorPrefs.SetBool(savedMoveSettingsFoldout, moveSettingsFoldout);
                if (moveSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawMoveSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (tabStates[3])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Values
                valuesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(valuesFoldout, "Values");
                EditorPrefs.SetBool(savedValuesFoldout, valuesFoldout);
                if (valuesFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawValues();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }


            if (tabStates[4])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Events
                eventsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(eventsFoldout, "Events");
                EditorPrefs.SetBool(savedEventsFoldout, eventsFoldout);
                if (eventsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.Space();
                    DrawEvents();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            serializedObject.ApplyModifiedProperties();

            if (EditorApplication.isPlaying)
                return;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            DrawPreviewButtons();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void OnSceneGUI()
        {
            if (doAnchorPos.begin == Begin.After ||
                doAnchorPos.begin == Begin.With)
            {
                Handles.color = Color.white;

                if (doAnchorPos.tweenObject != null)
                    DrawTweenObjectInfo();
            }
        }

        #endregion

        #region Inspector Draw Functions

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();

            GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButton);
            toggleStyle.fixedHeight = 30f;

            string[] tabNames = new string[] { "Life", "Type", "Move", "Values", "Events" };

            for (int i = 0; i < tabStates.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
                bool toggleState = GUILayout.Toggle(tabStates[i], tabNames[i], toggleStyle);
                if (EditorGUI.EndChangeCheck())
                {
                    tabStates[i] = toggleState;
                    EditorPrefs.SetBool(savedTabStates[i], toggleState);
                }
            }

            GUILayout.EndHorizontal();
        }

        private void DrawMoveSettings()
        {
            EditorGUILayout.PropertyField(speedBasedProp);
            EditorGUILayout.PropertyField(relativeProp);
            EditorGUILayout.PropertyField(snappingProp);
        }

        protected override void DrawValues()
        {
            EditorGUILayout.PropertyField(targetPositionProp);

            base.DrawValues();
        }

        #endregion

        #region Setup Functions

        private void SetupTargetPosition()
        {
            if (doAnchorPos.relative)
            {
                if (relativeFlags.firstTimeRelative)
                {
                    doAnchorPos.targetPosition = doAnchorPos.targetPosition - rectTransform.anchoredPosition;

                    Undo.RecordObject(relativeFlags, "DOAnchorPosEditor_firstTimeRelative");
                    relativeFlags.firstTimeRelative = false;
                }

                relativeFlags.firstTimeNonRelative = true;
            }
            else
            {
                if (relativeFlags.firstTimeNonRelative)
                {
                    doAnchorPos.targetPosition = doAnchorPos.targetPosition + rectTransform.anchoredPosition;

                    Undo.RecordObject(relativeFlags, "DOAnchorPosEditor_firstTimeNonRelative");
                    relativeFlags.firstTimeNonRelative = false;
                }

                relativeFlags.firstTimeRelative = true;
            }
        }

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            speedBasedProp = serializedObject.FindProperty("speedBased");
            relativeProp = serializedObject.FindProperty("relative");
            snappingProp = serializedObject.FindProperty("snapping");
            targetPositionProp = serializedObject.FindProperty("targetPosition");
        }

        protected override void SetupSavedVariables(DOBase dOScale)
        {
            base.SetupSavedVariables(dOScale);

            int instanceId = dOScale.GetInstanceID();

            savedMoveSettingsFoldout = "DOAnchorPosEditor_scaleSettingsFoldout_" + instanceId;
            moveSettingsFoldout = EditorPrefs.GetBool(savedMoveSettingsFoldout, true);

            for (int i = 0; i < savedTabStates.Length; i++)
            {
                savedTabStates[i] = "DOAnchorPosEditor_tabStates_" + i + " " + instanceId;
                tabStates[i] = EditorPrefs.GetBool(savedTabStates[i], true);
            }
        }

        protected override void ClearSavedEditorPrefs()
        {
            base.ClearSavedEditorPrefs();

            if (EditorPrefs.HasKey(savedMoveSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedMoveSettingsFoldout);
            }
        }

        #endregion

    }

}

#endif