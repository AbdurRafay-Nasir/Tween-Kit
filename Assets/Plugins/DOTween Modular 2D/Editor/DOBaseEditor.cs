#if UNITY_EDITOR

namespace DOTweenModular2D.Editor
{

    using DOTweenModular2D.Enums;
    using DG.DOTweenEditor;
    using DG.Tweening;
    using UnityEngine;
    using UnityEditor;

    public class DOBaseEditor : Editor
    {

        #region Serialized Properties

        protected SerializedProperty beginProp;
        protected SerializedProperty tweenObjectProp;
        protected SerializedProperty killProp;
        protected SerializedProperty destroyComponentProp;
        protected SerializedProperty destroyGameObjectProp;

        protected SerializedProperty delayProp;
        protected SerializedProperty tweenTypeProp;
        protected SerializedProperty loopTypeProp;
        private SerializedProperty easeTypeProp;
        private SerializedProperty curveProp;
        protected SerializedProperty loopsProp;
        protected SerializedProperty durationProp;

        private SerializedProperty onTweenCreatedProp;
        private SerializedProperty onTweenStartedProp;
        private SerializedProperty onTweenCompletedProp;
        private SerializedProperty onTweenKilledProp;

        #endregion

        private DOBase doBase;
        protected bool TweenPreviewing { get; private set; }

        protected Vector3 positionBeforePreview { get; private set; }
        private Quaternion rotationBeforePreview;
        private Vector3 scaleBeforePreview;

        protected Kill killTypeBeforePreview;

        protected const float buttonSize = 40;

        private void OnDisable()
        {
            if (TweenPreviewing)
            {
                DOTweenEditorPreview.Stop(true);
                ClearTweenCallbacks();
                ApplySavedValues();

                doBase.kill = killTypeBeforePreview;
            }

            if (target == null)
            {
                ClearSavedEditorPrefs();               
            }
        }

        #region Handle Properties

        protected int currentHandleIndex = 0;
        protected int currentHandleColorIndex = 0;
        protected int currentLineColorIndex = 0;
        protected float currentHandleRadius = 1f;
        protected float currentLineWidth = 0f;

        private string savedHandleIndex;
        private string savedHandleColorIndex;
        private string savedHandleRadius;
        private string savedLineColorIndex;
        private string savedLineWidth;

        protected Color[] color = new Color[]
        {
            Color.black,
            Color.blue,
            Color.clear,
            Color.cyan,
            Color.gray,
            Color.green,
            Color.magenta,
            Color.red,
            Color.white,
            Color.yellow,
        };

        private string[] colorDropdown = new string[]
        {
            "Black",
            "Blue",
            "Clear",
            "Cyan",
            "Gray",
            "Green",
            "Magenta",
            "Red",
            "White",
            "Yellow"
        };

        private string[] handleDropdown = new string[] { "Position", "Free" };

        private EditorProperties editorProperties;

        #endregion

        #region Inspector Button Properties

        protected bool editPath;
        private string savedEditPath;

        #endregion

        #region Foldout Bools

        protected bool lifeTimeSettingsFoldout = true;
        protected bool typeSettingsFoldout = true;
        protected bool valuesFoldout = true;
        protected bool eventsFoldout = false;
        protected bool editorFoldout = false;

        protected string savedLifeTimeSettingsFoldout;
        protected string savedTypeSettingsFoldout;
        protected string savedValuesFoldout;
        protected string savedEventsFoldout;
        protected string savedEditorFoldout;

        #endregion

        protected virtual void ClearSavedEditorPrefs()
        {
            if (EditorPrefs.HasKey(savedLifeTimeSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedLifeTimeSettingsFoldout);
            }

            if (EditorPrefs.HasKey(savedTypeSettingsFoldout))
            {
                EditorPrefs.DeleteKey(savedTypeSettingsFoldout);
            }

            if (EditorPrefs.HasKey(savedValuesFoldout))
            {
                EditorPrefs.DeleteKey(savedValuesFoldout);
            }

            if (EditorPrefs.HasKey(savedEventsFoldout))
            {
                EditorPrefs.DeleteKey(savedEventsFoldout);
            }

            if (EditorPrefs.HasKey(savedEditorFoldout))
            {
                EditorPrefs.DeleteKey(savedEditorFoldout);
            }

            if (EditorPrefs.HasKey(savedEditorFoldout))
            {
                EditorPrefs.DeleteKey(savedEditorFoldout);
            }

            if (EditorPrefs.HasKey(savedEditPath))
            {
                EditorPrefs.DeleteKey(savedEditPath);
            }

            if (EditorPrefs.HasKey(savedHandleIndex))
            {
                EditorPrefs.DeleteKey(savedHandleIndex);
            }

            if (EditorPrefs.HasKey(savedHandleColorIndex))
            {
                EditorPrefs.DeleteKey(savedHandleColorIndex);
            }

            if (EditorPrefs.HasKey(savedHandleRadius))
            {
                EditorPrefs.DeleteKey(savedHandleRadius);
            }

            if (EditorPrefs.HasKey(savedLineColorIndex))
            {
                EditorPrefs.DeleteKey(savedLineColorIndex);
            }

            if (EditorPrefs.HasKey(savedLineWidth))
            {
                EditorPrefs.DeleteKey(savedLineWidth);
            }
        }

        #region Setup Functions

        /// <summary>
        /// Must call this method in OnEnable to initialize Common Serialized Properties
        /// </summary>
        protected virtual void SetupSerializedProperties()
        {
            beginProp = serializedObject.FindProperty("begin");
            tweenObjectProp = serializedObject.FindProperty("tweenObject");
            killProp = serializedObject.FindProperty("kill");
            destroyComponentProp = serializedObject.FindProperty("destroyComponent");
            destroyGameObjectProp = serializedObject.FindProperty("destroyGameObject");

            delayProp = serializedObject.FindProperty("delay");
            tweenTypeProp = serializedObject.FindProperty("tweenType");
            loopTypeProp = serializedObject.FindProperty("loopType");
            easeTypeProp = serializedObject.FindProperty("easeType");
            curveProp = serializedObject.FindProperty("curve");
            loopsProp = serializedObject.FindProperty("loops");
            durationProp = serializedObject.FindProperty("duration");

            onTweenCreatedProp = serializedObject.FindProperty("onTweenCreated");
            onTweenStartedProp = serializedObject.FindProperty("onTweenStarted");
            onTweenCompletedProp = serializedObject.FindProperty("onTweenCompleted");
            onTweenKilledProp = serializedObject.FindProperty("onTweenKilled");
        }

        /// <summary>
        /// Must call this method in OnEnable to load saved state of Foldout bools and edit path bool
        /// </summary>
        protected virtual void SetupSavedVariables(DOBase doBase)
        {
            this.doBase = doBase;
            editorProperties = CreateInstance<EditorProperties>();

            SetupSavedVariablesPath(doBase);

            ApplySavedValuesToVariables();
        }

        private void SetupSavedVariablesPath(DOBase doBase)
        {
            int instanceId = doBase.GetInstanceID();

            // Saved Foldout Bool Properties Path
            savedLifeTimeSettingsFoldout = "DOBaseEditor_lifeTimeSettings_" + instanceId;
            savedTypeSettingsFoldout = "DOBaseEditor_typeSettings_" + instanceId;
            savedValuesFoldout = "DOBaseEditor_values_" + instanceId;
            savedEventsFoldout = "DOBaseEditor_events_" + instanceId;
            savedEditorFoldout = "DOBaseEditor_editor_" + instanceId;

            // Saved handles properties Path
            savedHandleIndex = "DOBaseEditor_handleIndex_" + instanceId;
            savedHandleColorIndex = "DOBaseEditor_handleColorIndex_" + instanceId;
            savedLineColorIndex = "DOBaseEditor_lineColorIndex_" + instanceId;
            savedHandleRadius = "DOBaseEditor_handleRadius_" + instanceId;
            savedLineWidth = "DOBaseEditor_lineWidth_" + instanceId;

            // Saved Inspector button properties Path
            savedEditPath = "DOBaseEditor_editPath_" + instanceId;
        }

        private void ApplySavedValuesToVariables()
        {
            // Apply saved values to Foldout Bool Properties
            lifeTimeSettingsFoldout = EditorPrefs.GetBool(savedLifeTimeSettingsFoldout, true);
            typeSettingsFoldout = EditorPrefs.GetBool(savedTypeSettingsFoldout, true);
            valuesFoldout = EditorPrefs.GetBool(savedValuesFoldout, true);
            eventsFoldout = EditorPrefs.GetBool(savedEventsFoldout, false);
            editorFoldout = EditorPrefs.GetBool(savedEditorFoldout, true);

            // Apply saved values to Editor Properties
            editorProperties.handleIndex = EditorPrefs.GetInt(savedHandleIndex, 0);
            editorProperties.handleColorIndex = EditorPrefs.GetInt(savedHandleColorIndex, 5);
            editorProperties.handleRadius = EditorPrefs.GetFloat(savedHandleRadius, 0.5f);
            editorProperties.lineColorIndex = EditorPrefs.GetInt(savedLineColorIndex, 5);
            editorProperties.lineWidth = EditorPrefs.GetFloat(savedLineWidth, 1f);

            // Apply saved values to Inspector Button Properties
            editPath = EditorPrefs.GetBool(savedEditPath, true);
        }

        #endregion

        #region Inspector Draw Functions

        /// <summary>
        /// Draws begin, tweenObjectProp(if Begin = After or With), kill <br/>
        /// destroy component, destroy gameObject
        /// </summary>
        protected void DrawLifeTimeSettings()
        {
            EditorGUILayout.PropertyField(beginProp);

            if ((Begin)beginProp.enumValueIndex == Begin.With ||
                (Begin)beginProp.enumValueIndex == Begin.After)
            {
                EditorGUILayout.PropertyField(tweenObjectProp);
            }

            EditorGUILayout.PropertyField(killProp);

            if (doBase.kill != Kill.Manual)
            {
                EditorGUILayout.PropertyField(destroyComponentProp);
                EditorGUILayout.PropertyField(destroyGameObjectProp);
            }
        }

        /// <summary>
        /// Draws Helpbox for Inspector messages regarding tweenObject and Begin property
        /// </summary>
        protected void DrawTweenObjectHelpBox()
        {
            if ((Begin)beginProp.enumValueIndex == Begin.After ||
                (Begin)beginProp.enumValueIndex == Begin.With)
            {
                if (tweenObjectProp.objectReferenceValue == null)
                    EditorGUILayout.HelpBox("Tween Object is not assigned", MessageType.Error);
            }
            else
            {
                if (tweenObjectProp.objectReferenceValue != null)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.HelpBox("Tween Object is assigned, it should be removed", MessageType.Warning);

                    GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
                    trashButton.tooltip = "Remove Tween Object";

                    if (GUILayout.Button(trashButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize * 2f)))
                    {
                        tweenObjectProp.objectReferenceValue = null;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        /// <summary>
        /// Draws tweenType loopType (if tweenType = Looped), <br/> 
        /// easeType, curve(if easeType = INTERNAL_Custom)
        /// </summary>
        protected virtual void DrawTypeSettings()
        {
            EditorGUILayout.PropertyField(tweenTypeProp);

            if ((Enums.TweenType)tweenTypeProp.enumValueIndex == Enums.TweenType.Looped)
            {
                EditorGUILayout.PropertyField(loopTypeProp);
            }

            EditorGUILayout.PropertyField(easeTypeProp);

            if ((Ease)easeTypeProp.enumValueIndex == Ease.INTERNAL_Custom)
            {
                EditorGUILayout.PropertyField(curveProp);
            }
        }

        /// <summary>
        /// Draws loops(if loopType = Looped), delay, duration Property
        /// </summary>    
        protected virtual void DrawValues()
        {
            if ((Enums.TweenType)tweenTypeProp.enumValueIndex == Enums.TweenType.Looped)
            {
                EditorGUILayout.PropertyField(loopsProp);
            }

            EditorGUILayout.PropertyField(delayProp);
            EditorGUILayout.PropertyField(durationProp);
        }

        /// <summary>
        /// Draws onTweenCreated, onTweenStartedProp, onTweenCompleted, onTweenKilledProp events
        /// </summary>
        protected void DrawEvents()
        {
            EditorGUILayout.PropertyField(onTweenCreatedProp);
            EditorGUILayout.PropertyField(onTweenStartedProp);
            EditorGUILayout.PropertyField(onTweenCompletedProp);
            EditorGUILayout.PropertyField(onTweenKilledProp);
        }

        /// <summary>
        /// Draws Handle, Handle Color, Handle Radius, Line Color, Line Width Editor Inspector Properties
        /// </summary>
        protected void DrawEditorProperties()
        {
            currentHandleIndex = EditorGUILayout.Popup("Handle", editorProperties.handleIndex, handleDropdown);
            if (currentHandleIndex != editorProperties.handleIndex)
            {
                Undo.RecordObject(editorProperties, "handleIndex");
                editorProperties.handleIndex = currentHandleIndex;

                EditorPrefs.SetInt(savedHandleIndex, currentHandleIndex);
            }

            currentHandleColorIndex = EditorGUILayout.Popup("Handle Color", editorProperties.handleColorIndex, colorDropdown);
            if (currentHandleColorIndex != editorProperties.handleColorIndex)
            {
                Undo.RecordObject(editorProperties, "handleColorIndex");
                editorProperties.handleColorIndex = currentHandleColorIndex;
                
                EditorPrefs.SetInt(savedHandleColorIndex, currentHandleColorIndex);
            }

            currentHandleRadius = EditorGUILayout.Slider("Handle Radius", editorProperties.handleRadius, 0.5f, 3f);
            if (currentHandleRadius != editorProperties.handleRadius)
            {
                Undo.RecordObject(editorProperties, "handleRadius");
                editorProperties.handleRadius = currentHandleRadius;

                EditorPrefs.SetFloat(savedHandleRadius, currentHandleRadius);
                
                SceneView.RepaintAll();
            }

            currentLineColorIndex = EditorGUILayout.Popup("Line Color", editorProperties.lineColorIndex, colorDropdown);
            if (currentLineColorIndex != editorProperties.lineColorIndex)
            {
                Undo.RecordObject(editorProperties, "lineColor");
                editorProperties.lineColorIndex = currentLineColorIndex;
                
                EditorPrefs.SetInt(savedLineColorIndex, currentLineColorIndex);
            }

            currentLineWidth = EditorGUILayout.Slider("Line Width", editorProperties.lineWidth, 1f, 20f);
            if (currentLineWidth != editorProperties.lineColorIndex)
            {
                Undo.RecordObject(editorProperties, "lineWidth");
                editorProperties.lineWidth = currentLineWidth;

                EditorPrefs.SetFloat(savedLineWidth, currentLineWidth);
                
                SceneView.RepaintAll();
            }
        }

        /// <summary>
        /// Draws Edit Button
        /// </summary>
        protected void DrawEditButton()
        {
            GUIContent editButton = EditorGUIUtility.IconContent("EditCollider");
            editButton.tooltip = "Toggle Path Editing";

            if (GUILayout.Button(editButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
            {
                editPath = !editPath;

                SceneView.RepaintAll();

                EditorPrefs.SetBool(savedEditPath, editPath);
            }
        }

        /// <summary>
        /// Draws Reset Editor Properties Button
        /// </summary>
        protected void DrawResetEditorPropertiesButton()
        {
            GUIContent resetButton = EditorGUIUtility.IconContent("Refresh@2x");
            resetButton.tooltip = "Reset Editor Properties";

            if (GUILayout.Button(resetButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
            {
                editorProperties.handleIndex = 0;
                EditorPrefs.SetInt(savedHandleIndex, editorProperties.handleIndex);

                editorProperties.handleColorIndex = 5;
                EditorPrefs.SetInt(savedHandleColorIndex, editorProperties.handleColorIndex);

                editorProperties.handleRadius = 0.5f;
                EditorPrefs.SetFloat(savedHandleRadius, editorProperties.handleRadius);

                editorProperties.lineColorIndex = 5;
                EditorPrefs.SetInt(savedLineColorIndex, editorProperties.lineColorIndex);

                editorProperties.lineWidth = 1f;
                EditorPrefs.SetFloat(savedLineWidth, editorProperties.lineWidth);

                SceneView.RepaintAll();
            }
        }

        #endregion

        #region Preview Functions

        /// <summary>
        /// Draws complete lines to backward Tween Objects, also displays arrow head and Begin Property
        /// </summary>
        protected void DrawTweenObjectInfo()
        {
            Handles.color = Color.cyan;

            DOBase tweenObj = (DOBase)tweenObjectProp.objectReferenceValue;

            Vector2 lineStart = doBase.transform.position;
            Vector2 lineEnd = tweenObj.transform.position;

            Handles.DrawLine(lineStart, lineEnd);

            Vector2 midPoint = (lineStart + lineEnd) * 0.5f;
            string text = doBase.begin.ToString();
            Handles.Label(midPoint, text);

            Vector2 arrowPosition = Vector2.Lerp(lineStart, lineEnd, 0.1f);

            Vector2 arrowDirection = lineStart - midPoint;

            Handles.ConeHandleCap(10, arrowPosition, Quaternion.LookRotation(arrowDirection), 0.5f, EventType.Repaint);

            while ((tweenObj.begin == Begin.After || tweenObj.begin == Begin.With)
                     && tweenObj.tweenObject != null)
            {
                text = tweenObj.begin.ToString();

                lineStart = tweenObj.transform.position;
                tweenObj = tweenObj.tweenObject;

                lineEnd = tweenObj.transform.position;

                Handles.DrawLine(lineStart, lineEnd);

                midPoint = (lineStart + lineEnd) * 0.5f;                
                Handles.Label(midPoint, text);

                arrowPosition = Vector2.Lerp(lineStart, lineEnd, 0.1f);
                arrowDirection = lineStart - midPoint;
                Handles.ConeHandleCap(10, arrowPosition, Quaternion.LookRotation(arrowDirection), 0.5f, EventType.Repaint);
            }
        }

        /// <summary>
        /// Draws Tween Preview buttons (Play and Stop)
        /// </summary>
        protected void DrawPreviewButtons()
        {
            GUIContent stopButton = EditorGUIUtility.IconContent("d_winbtn_win_close_h@2x");
            stopButton.tooltip = "Stop Previewing tween";

            GUIContent playButton = EditorGUIUtility.IconContent("PlayButton On@2x");
            playButton.tooltip = "Start Previewing tween";

            // if tween is previewing enable stopButton
            GUI.enabled = TweenPreviewing;
            if (GUILayout.Button(stopButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
            {
                DOTweenEditorPreview.Stop(true);
                ClearTweenCallbacks();
                ApplySavedValues();

                doBase.kill = killTypeBeforePreview;
            }

            // if tween is not previewing enable playButton
            GUI.enabled = !TweenPreviewing;
            if (GUILayout.Button(playButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
            {
                SaveDefaultTransform();
                TweenPreviewing = true;

                killTypeBeforePreview = doBase.kill;
                doBase.kill = Kill.Manual;

                doBase.CreateTween();
                doBase.Tween.onKill += ApplySavedValues;
                doBase.Tween.onKill += ClearTweenCallbacks;
                DOTweenEditorPreview.PrepareTweenForPreview(doBase.Tween, false, false);
                DOTweenEditorPreview.Start();
            }
        }

        private void ClearTweenCallbacks()
        {
            doBase.Tween.OnKill(null);
            doBase.Tween.OnPause(null);
            doBase.Tween.OnPlay(null);
            doBase.Tween.OnRewind(null);
            doBase.Tween.OnStart(null);
            doBase.Tween.OnStepComplete(null);
            doBase.Tween.OnUpdate(null);
            doBase.Tween.OnWaypointChange(null);
            doBase.Tween.OnComplete(null);
        }

        private void ApplySavedValues()
        {
            doBase.transform.position = positionBeforePreview;
            doBase.transform.rotation = rotationBeforePreview;
            doBase.transform.localScale = scaleBeforePreview;

            TweenPreviewing = false;

            doBase.kill = killTypeBeforePreview;
        }

        private void SaveDefaultTransform()
        {
            positionBeforePreview = doBase.transform.position;
            rotationBeforePreview = doBase.transform.rotation;
            scaleBeforePreview = doBase.transform.lossyScale;
        }

        #endregion

    }

    public class EditorProperties : ScriptableObject
    {
        public int handleIndex;
        public int handleColorIndex;
        public float handleRadius;

        public int lineColorIndex;
        public float lineWidth;
    }

    public class RelativeFlags : ScriptableObject
    {
        public bool firstTimeRelative;
        public bool firstTimeNonRelative;
    }

}

#endif