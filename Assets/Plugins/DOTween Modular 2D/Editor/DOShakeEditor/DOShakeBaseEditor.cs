#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular2D.Editor
{
    public class DOShakeBaseEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty fadeOutProp;
        private SerializedProperty vibratoProp;
        private SerializedProperty randomnessProp;
        private SerializedProperty randomnessModeProp;

        #endregion

        private DOShakeBase doShake;

        private bool[] tabStates = new bool[5];
        private string[] savedTabStates = new string[5];

        #region Foldout Bool

        private bool shakeSettingsFoldout = true;
        private string savedShakeSettingsFoldout;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doShake = (DOShakeBase)target;

            SetupSerializedProperties();
            SetupSavedVariables(doShake);
        }

        public override void OnInspectorGUI()
        {
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

                // Draw Rotate Settings
                shakeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(shakeSettingsFoldout, "Shake Settings");
                EditorPrefs.SetBool(savedShakeSettingsFoldout, shakeSettingsFoldout);
                if (shakeSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawShakeSettings();

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

        protected void OnSceneGUI()
        {
            if (doShake.begin == Enums.Begin.After ||
                doShake.begin == Enums.Begin.With)
            {
                Handles.color = Color.white;

                if (doShake.tweenObject != null)
                    DrawTweenObjectInfo();
            }
        }

        #endregion

        #region Draw Functions

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();

            GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButton);
            toggleStyle.fixedHeight = 30f;

            string[] tabNames = new string[] { "Life", "Type", "Shake", "Values", "Events" };

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

        private void DrawShakeSettings()
        {
            EditorGUILayout.PropertyField(fadeOutProp);
            EditorGUILayout.PropertyField(vibratoProp);
            EditorGUILayout.PropertyField(randomnessProp);
            EditorGUILayout.PropertyField(randomnessModeProp);
        }

        #endregion

        #region Setup Functions

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            fadeOutProp = serializedObject.FindProperty("fadeOut");
            vibratoProp = serializedObject.FindProperty("vibrato");
            randomnessProp = serializedObject.FindProperty("randomness");
            randomnessModeProp = serializedObject.FindProperty("randomnessMode");
        }

        protected override void SetupSavedVariables(DOBase dOShake)
        {
            base.SetupSavedVariables(dOShake);

            int instanceId = dOShake.GetInstanceID();

            savedShakeSettingsFoldout = "DOShakeEditor_shakeSettingsFoldout_" + instanceId;
            shakeSettingsFoldout = EditorPrefs.GetBool(savedShakeSettingsFoldout, true);

            for (int i = 0; i < savedTabStates.Length; i++)
            {
                savedTabStates[i] = "DOShakeEditor_tabStates_" + i + " " + instanceId;
                tabStates[i] = EditorPrefs.GetBool(savedTabStates[i], true);
            }
        }

        protected override void ClearSavedEditorPrefs()
        {
            base.ClearSavedEditorPrefs();

            if (EditorPrefs.HasKey(savedShakeSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedShakeSettingsFoldout);
            }
        }

        #endregion

    }

}

#endif