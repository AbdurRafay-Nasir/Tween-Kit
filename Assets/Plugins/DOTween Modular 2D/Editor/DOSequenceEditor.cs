#if UNITY_EDITOR

using DOTweenModular2D.Enums;
using DG.DOTweenEditor;
using DG.Tweening;
using UnityEngine;
using UnityEditor;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOSequence)), CanEditMultipleObjects]
    public class DOSequenceEditor : DOBaseEditor
    {
        private SerializedProperty sequenceTweensProp;

        private DOSequence doSequence;
        private bool tweenPreviewing;

        private SavedTransforms[] savedTransforms;

        private bool[] tabStates = new bool[4];
        private string[] savedTabStates = new string[4];

        #region Foldout Bool

        private bool sequenceSettingsFoldout = true;
        private string savedSequenceSettingsFoldout;

        #endregion

        #region Join Bool

        private bool join = false;
        private string savedJoin;

        #endregion

        #region Preview Buttons

        private GUIContent joinButton;
        private GUIContent stopButton;
        private GUIContent playButton;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doSequence = (DOSequence)target;

            SetupSerializedProperties();
            SetupSavedVariables(doSequence);

            joinButton = EditorGUIUtility.IconContent("d_Linked");
            joinButton.tooltip = "Toggle join";

            stopButton = EditorGUIUtility.IconContent("d_winbtn_win_close_h@2x");
            stopButton.tooltip = "Stop Previewing tween";

            playButton = EditorGUIUtility.IconContent("PlayButton On@2x");
            playButton.tooltip = "Start Previewing tween";
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

                // Draw Sequence Settings
                sequenceSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(sequenceSettingsFoldout, "Sequence Settings");
                EditorPrefs.SetBool(savedSequenceSettingsFoldout, sequenceSettingsFoldout);
                if (sequenceSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawSequenceSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (tabStates[2])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                EditorGUILayout.PropertyField(sequenceTweensProp);
                if (doSequence.sequenceTweens != null)
                {
                    for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
                    {
                        DOBase currentTween = doSequence.sequenceTweens[i].tweenObject;

                        if (currentTween != null)
                            currentTween.begin = Begin.Manual;
                        else
                            EditorGUILayout.HelpBox("Element: " + i + " Tween Object is not assigned", MessageType.Error);
                    }
                }
            }

            if (tabStates[3])
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

            if (doSequence.sequenceTweens != null
                && GUILayout.Button(joinButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
            {
                join = !join;
                EditorPrefs.SetBool(savedJoin, join);

                for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
                {
                    doSequence.sequenceTweens[i].join = join;
                }
            }

            DrawPreviewButtons();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void OnSceneGUI()
        {
            if (doSequence.begin == Enums.Begin.After ||
                doSequence.begin == Enums.Begin.With)
            {
                Handles.color = Color.white;

                if (doSequence.tweenObject != null)
                    DrawTweenObjectInfo();
            }

            if (doSequence.sequenceTweens == null)
                return;

            Handles.color = Color.yellow;

            DrawLinesAndLabelToSequenceTweens();
        }

        #endregion

        private void DrawLinesAndLabelToSequenceTweens()
        {
            int k = 1;

            Vector2 startPosition = doSequence.transform.position;

            for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
            {
                DOBase sequenceTweenObj = doSequence.sequenceTweens[i].tweenObject;
                if (sequenceTweenObj == null)
                    continue;

                Vector2 sequenceTweenObjPos = sequenceTweenObj.transform.position;

                Handles.DrawLine(startPosition, sequenceTweenObjPos);

                Vector2 midPoint = (startPosition + sequenceTweenObjPos) * 0.5f;

                Handles.Label(midPoint, k.ToString());

                if (i + 1 < doSequence.sequenceTweens.Length &&
                    doSequence.sequenceTweens[i + 1].tweenObject != null &&
                    !doSequence.sequenceTweens[i + 1].join)
                {
                    k++;
                }
            }
        }

        #region Preview Functions

        private new void DrawPreviewButtons()
        {
            // if tween is previewing enable stopButton
            GUI.enabled = tweenPreviewing;
            if (GUILayout.Button(stopButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
            {
                DOTweenEditorPreview.Stop(true);
                ClearTweenCallbacks();
                ApplySavedValues();

                doSequence.kill = killTypeBeforePreview;
            }

            // if tween is not previewing enable playButton
            GUI.enabled = !tweenPreviewing;
            if (GUILayout.Button(playButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
            {
                SetupSavedTransforms();

                tweenPreviewing = true;

                killTypeBeforePreview = doSequence.kill;
                doSequence.kill = Kill.Manual;

                doSequence.CreateTween();
                doSequence.Tween.onKill += ApplySavedValues;
                doSequence.Tween.onKill += ClearTweenCallbacks;
                DOTweenEditorPreview.PrepareTweenForPreview(doSequence.Tween, false, false);
                DOTweenEditorPreview.Start();
            }
        }

        private void ClearTweenCallbacks()
        {
            doSequence.Tween.OnPause(null);
            doSequence.Tween.OnPlay(null);
            doSequence.Tween.OnRewind(null);
            doSequence.Tween.OnStart(null);
            doSequence.Tween.OnStepComplete(null);
            doSequence.Tween.OnUpdate(null);
            doSequence.Tween.OnWaypointChange(null);
            doSequence.Tween.OnComplete(null);
            doSequence.Tween.OnKill(null);
        }

        private void SetupSavedTransforms()
        {
            savedTransforms = new SavedTransforms[doSequence.sequenceTweens.Length];

            for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
            {
                savedTransforms[i].position = doSequence.sequenceTweens[i].tweenObject.transform.position;
                savedTransforms[i].rotation = doSequence.sequenceTweens[i].tweenObject.transform.rotation;
                savedTransforms[i].scale = doSequence.sequenceTweens[i].tweenObject.transform.localScale;
            }
        }

        private void ApplySavedValues()
        {
            for (int i = 0; i < savedTransforms.Length; i++)
            {
                doSequence.sequenceTweens[i].tweenObject.transform.position = savedTransforms[i].position;
                doSequence.sequenceTweens[i].tweenObject.transform.rotation = savedTransforms[i].rotation;
                doSequence.sequenceTweens[i].tweenObject.transform.localScale = savedTransforms[i].scale;
            }

            tweenPreviewing = false;

            doSequence.kill = killTypeBeforePreview;
        }

        #endregion

        #region Draw Functions

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();

            GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButton);
            toggleStyle.fixedHeight = 30f;

            string[] tabNames = new string[] { "Life", "Sequence", "Tweens", "Events" };

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

        private void DrawSequenceSettings()
        {
            EditorGUILayout.PropertyField(tweenTypeProp);

            if ((Enums.TweenType)tweenTypeProp.enumValueIndex == Enums.TweenType.Looped)
            {
                EditorGUILayout.PropertyField(loopTypeProp);
                EditorGUILayout.PropertyField(loopsProp);
            }
        }

        #endregion

        #region Setup Functions

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            sequenceTweensProp = serializedObject.FindProperty("sequenceTweens");
        }

        protected override void SetupSavedVariables(DOBase doSequence)
        {
            base.SetupSavedVariables(doSequence);

            int instanceId = doSequence.GetInstanceID();

            savedSequenceSettingsFoldout = "DOSequenceEditor_sequenceSettingsFoldout_" + instanceId;
            sequenceSettingsFoldout = EditorPrefs.GetBool(savedSequenceSettingsFoldout, true);

            savedJoin = "DOSequenceEditor_join_" + instanceId;
            join = EditorPrefs.GetBool(savedJoin, false);

            for (int i = 0; i < savedTabStates.Length; i++)
            {
                savedTabStates[i] = "DOSequenceEditor_tabStates_" + i + " " + instanceId;
                tabStates[i] = EditorPrefs.GetBool(savedTabStates[i], true);
            }
        }

        protected override void ClearSavedEditorPrefs()
        {
            base.ClearSavedEditorPrefs();

            if (EditorPrefs.HasKey(savedSequenceSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedSequenceSettingsFoldout);
            }
        }

        #endregion

    }

    struct SavedTransforms
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

}

#endif