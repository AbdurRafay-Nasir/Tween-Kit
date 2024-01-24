#if UNITY_EDITOR

using DOTweenModular2D.Enums;
using UnityEngine;
using UnityEditor;

namespace DOTweenModular2D.Editor
{
    public class DOPunchBaseEditor : DOBaseEditor
    {

        #region Serialized Properties

        private SerializedProperty vibratoProp;
        private SerializedProperty elasticityProp;

        #endregion

        private DOPunchBase doPunch;

        private bool[] tabStates = new bool[5];
        private string[] savedTabStates = new string[5];

        #region Foldout Bool

        private bool punchSettingsFoldout = true;
        private string savedpunchSettingsFoldout;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doPunch = (DOPunchBase)target;

            SetupSerializedProperties();
            SetupSavedVariables(doPunch);
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

                // Draw Punch Settings
                punchSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(punchSettingsFoldout, "Punch Settings");
                EditorPrefs.SetBool(savedpunchSettingsFoldout, punchSettingsFoldout);
                if (punchSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawPunchSettings();

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
            if (doPunch.begin == Begin.After ||
                doPunch.begin == Begin.With)
            {
                Handles.color = Color.white;

                if (doPunch.tweenObject != null)
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

            string[] tabNames = new string[] { "Life", "Type", "Punch", "Values", "Events" };

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

        private void DrawPunchSettings()
        {
            EditorGUILayout.PropertyField(vibratoProp);
            EditorGUILayout.PropertyField(elasticityProp);
        }

        #endregion

        #region Setup Functions

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            vibratoProp = serializedObject.FindProperty("vibrato");
            elasticityProp = serializedObject.FindProperty("elasticity");
        }

        protected override void SetupSavedVariables(DOBase doPunchBase)
        {
            base.SetupSavedVariables(doPunchBase);

            int instanceId = doPunchBase.GetInstanceID();

            savedpunchSettingsFoldout = "DOPunchEditor_punchSettingsFoldout_" + instanceId;
            punchSettingsFoldout = EditorPrefs.GetBool(savedpunchSettingsFoldout, true);

            for (int i = 0; i < savedTabStates.Length; i++)
            {
                savedTabStates[i] = "DOPunchEditor_tabStates_" + i + " " + instanceId;
                tabStates[i] = EditorPrefs.GetBool(savedTabStates[i], true);
            }
        }

        protected override void ClearSavedEditorPrefs()
        {
            base.ClearSavedEditorPrefs();

            if (EditorPrefs.HasKey(savedpunchSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedpunchSettingsFoldout);
            }
        }

        #endregion

    }

}

#endif