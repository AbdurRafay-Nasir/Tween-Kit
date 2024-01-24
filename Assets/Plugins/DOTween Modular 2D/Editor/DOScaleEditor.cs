#if UNITY_EDITOR

using DOTweenModular2D.Enums;
using UnityEngine;
using UnityEditor;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOScale)), CanEditMultipleObjects]
    public class DOScaleEditor : DOLookAtBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty relativeProp;
        private SerializedProperty speedBasedProp;
        private SerializedProperty targetScaleProp;

        #endregion

        private DOScale doScale;
        private RelativeFlags relativeFlags;

        private bool[] tabStates = new bool[6];
        private string[] savedTabStates = new string[6];

        #region Foldout Settings

        private bool scaleSettingsFoldout = true;
        private string savedScaleSettingsFoldout;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doScale = (DOScale)target;
            relativeFlags = CreateInstance<RelativeFlags>();

            SetupSerializedProperties();
            SetupSavedVariables(doScale);
        }

        public override void OnInspectorGUI()
        {
            SetupTargetScale();

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
                lookAtSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lookAtSettingsFoldout, "Look At Settings");
                EditorPrefs.SetBool(savedLookAtSettingsFoldout, lookAtSettingsFoldout);
                if (lookAtSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawLookAtSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            DrawLookAtHelpBox();

            if (tabStates[3])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Scale Settings
                scaleSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(scaleSettingsFoldout, "Scale Settings");
                EditorPrefs.SetBool(savedScaleSettingsFoldout, scaleSettingsFoldout);
                if (scaleSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawScaleSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
                        
            if (tabStates[4])
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


            if (tabStates[5])
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
            if (doScale.begin == Begin.After ||
                doScale.begin == Begin.With)
            {
                Handles.color = Color.white;

                if (doScale.tweenObject != null)
                    DrawTweenObjectInfo();
            }

            if (doScale.lookAt == LookAtSimple.None)
                return;

            if (doScale.lookAt == LookAtSimple.Position)
            {
                DrawLookAtHandle();
            }

            Handles.color = Color.green;

            DrawRotationClampCircle();
            DrawLookAtLine();
        }

        #endregion

        #region Draw Functions

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();

            GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButton);
            toggleStyle.fixedHeight = 30f;

            string[] tabNames = new string[] { "Life", "Type", "Look At", "Scale", "Values", "Events" };

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

        private void DrawScaleSettings()
        {
            EditorGUILayout.PropertyField(speedBasedProp);
            EditorGUILayout.PropertyField(relativeProp);
        }

        protected override void DrawValues()
        {
            EditorGUILayout.PropertyField(targetScaleProp);

            base.DrawValues();
        }

        #endregion

        #region Setup Functions

        private void SetupTargetScale()
        {
            if (doScale.relative)
            {
                if (relativeFlags.firstTimeRelative)
                {
                    doScale.targetScale = doScale.targetScale - (Vector2)doScale.transform.localScale;

                    Undo.RecordObject(relativeFlags, "DOScaleEditor_firstTimeRelative");
                    relativeFlags.firstTimeRelative = false;
                }

                relativeFlags.firstTimeNonRelative = true;
            }
            else
            {
                if (relativeFlags.firstTimeNonRelative)
                {
                    doScale.targetScale = doScale.targetScale + (Vector2)doScale.transform.localScale;

                    Undo.RecordObject(relativeFlags, "DOScaleEditor_firstTimeNonRelative");
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
            targetScaleProp = serializedObject.FindProperty("targetScale");
        }

        protected override void SetupSavedVariables(DOBase dOScale)
        {
            base.SetupSavedVariables(dOScale);

            int instanceId = dOScale.GetInstanceID();

            savedScaleSettingsFoldout = "DOScaleEditor_scaleSettingsFoldout_" + instanceId;
            scaleSettingsFoldout = EditorPrefs.GetBool(savedScaleSettingsFoldout, true);

            for (int i = 0; i < savedTabStates.Length; i++)
            {
                savedTabStates[i] = "DOScaleEditor_tabStates_" + i + " " + instanceId;
                tabStates[i] = EditorPrefs.GetBool(savedTabStates[i], true);
            }
        }

        protected override void ClearSavedEditorPrefs()
        {
            base.ClearSavedEditorPrefs();

            if (EditorPrefs.HasKey(savedScaleSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedScaleSettingsFoldout);
            }
        }

        #endregion

    }

}

#endif