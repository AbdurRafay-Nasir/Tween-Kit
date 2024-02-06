#if UNITY_EDITOR

namespace DOTweenModular.Editor
{
    using UnityEngine;
    using UnityEditor;
    using DG.Tweening;
    using DG.DOTweenEditor;
    using DOTweenModular.Enums;

    /// <summary>
    /// Base class for creating DOComponents Editor
    /// </summary>
    public class DOBaseEditor : Editor
    {
        #region Serialized Properties

        protected SerializedProperty beginProp;
        protected SerializedProperty tweenObjectProp;
        protected SerializedProperty layerMaskProp;

        protected SerializedProperty delayProp;
        protected SerializedProperty tweenTypeProp;
        protected SerializedProperty loopTypeProp;
        private SerializedProperty easeTypeProp;
        private SerializedProperty curveProp;
        protected SerializedProperty loopsProp;
        protected SerializedProperty durationProp;

        private SerializedProperty onTweenCreatedProp;
        private SerializedProperty onTweenPlayedProp;
        private SerializedProperty onTweenUpdatedProp;
        private SerializedProperty onTweenCompletedProp;
        private SerializedProperty onTweenKilledProp;

        #endregion

        private DOBase doBase;

        /// <summary>
        /// The ID of this component
        /// </summary>
        protected int instanceId;

        // Used as a key to store state of tween (previewing or stoped)
        private string previewKey;

        /// <summary>
        /// True when tween is previewing in editor
        /// </summary>
        protected bool tweenPreviewing;

        #region Unity Functions

        public virtual void OnEnable()
        {
            doBase = (DOBase)target;

            instanceId = doBase.GetInstanceID();

            previewKey = doBase.gameObject.GetInstanceID() + "Tween Preview";

            beginProp = serializedObject.FindProperty("begin");
            tweenObjectProp = serializedObject.FindProperty("tweenObject");
            layerMaskProp = serializedObject.FindProperty("layerMask");

            delayProp = serializedObject.FindProperty("delay");
            tweenTypeProp = serializedObject.FindProperty("tweenType");
            loopTypeProp = serializedObject.FindProperty("loopType");
            easeTypeProp = serializedObject.FindProperty("easeType");
            curveProp = serializedObject.FindProperty("curve");
            loopsProp = serializedObject.FindProperty("loops");
            durationProp = serializedObject.FindProperty("duration");

            onTweenCreatedProp = serializedObject.FindProperty("onTweenCreated");
            onTweenPlayedProp = serializedObject.FindProperty("onTweenPlayed");
            onTweenUpdatedProp = serializedObject.FindProperty("onTweenUpdated");
            onTweenCompletedProp = serializedObject.FindProperty("onTweenCompleted");
            onTweenKilledProp = serializedObject.FindProperty("onTweenKilled");
        }

        public virtual void OnSceneGUI()
        {
            if (doBase.begin == Begin.After ||
                doBase.begin == Begin.With)
            {
                if (doBase.tweenObject != null)
                    DrawTweenObjectInfo();
            }
        }

        private void OnDestroy()
        {
            if (tweenPreviewing)
            {
                OnPreviewForceStopped();
            }
        }

        #endregion

        // Helpful functions for drawing customized items on Inspector
        #region GUI Handling

        /// <summary>
        /// Adds a space
        /// </summary>
        protected void Space()
        {
            EditorGUILayout.Space();
        }

        /// <summary>
        /// Creates a box with a message
        /// </summary>
        /// <param name="message">The text to show in helpbox</param>
        /// <param name="messageType">type of message (None, Info, Warning, Error)</param>
        protected void DrawHelpbox(string message, MessageType messageType)
        {
            EditorGUILayout.HelpBox(message, messageType);
        }

        /// <summary>
        /// Draw box Toggles
        /// </summary>
        /// <param name="toggleNames">Names of toggles</param>
        /// <returns>State of each toggle, true or false</returns>
        protected bool[] DrawToggles(params string[] toggleNames)
        {
            GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButton);
            toggleStyle.fixedHeight = 30f;

            string[] toggleKeys = new string[toggleNames.Length];

            bool[] toggleStates = new bool[toggleNames.Length];

            for (int i = 0; i < toggleKeys.Length; i++)
            {
                toggleKeys[i] = instanceId + "_" + "Toggle_" + toggleNames[i];
            }

            GUILayout.BeginHorizontal();

            for (int i = 0; i < toggleKeys.Length; i++)
            {
                bool isOn = SessionState.GetBool(toggleKeys[i], true);

                EditorGUI.BeginChangeCheck();
                isOn = GUILayout.Toggle(isOn, toggleNames[i], toggleStyle);

                if (EditorGUI.EndChangeCheck())
                {
                    toggleStates[i] = isOn;
                    SessionState.SetBool(toggleKeys[i], isOn);
                }
                else
                {
                    // If no change, use the stored value
                    toggleStates[i] = isOn;
                }               
            }

            GUILayout.EndHorizontal();

            return toggleStates;
        }

        /// <summary>
        /// Draws a simple line, useful for visually separating properties in inspector
        /// </summary>
        protected void DrawSeparatorLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        /// <summary>
        /// Starts Drawing a backgroud
        /// </summary>
        /// <remarks>Use EndBackgroundBox() to Stop the background</remarks>
        protected void BeginBackgroundBox()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        }

        /// <summary>
        /// Ends the Background Box, started by BeginBackgroundBox()
        /// </summary>
        protected void EndBackgroundBox()
        {
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Starts a foldable Header group
        /// </summary>
        /// <param name="foldoutName">Name of Foldout</param>
        /// <param name="openByDefault">If true, Toggle will be expaned by default</param>
        /// <returns>True if foldout is open, False otherwise</returns>
        protected bool BeginFoldout(string foldoutName, bool openByDefault = true)
        {
            string foldoutKey = instanceId + "_" + "Foldout_" + foldoutName;

            bool open = SessionState.GetBool(foldoutKey, openByDefault);
            open = EditorGUILayout.BeginFoldoutHeaderGroup(open, foldoutName);
            SessionState.SetBool(foldoutKey, open);
            return open;

        }

        /// <summary>
        /// Stops a foldout header group started by BeginFoldout()
        /// </summary>
        protected void EndFoldout()
        {
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        /// <summary>
        /// Draws a Play button in inspector to Preview tween withing editor
        /// </summary>
        /// <remarks>Will not draw Play button if in Play Mode <br/>
        /// OnPreviewStarted() is called when this button is pressed</remarks>
        protected void DrawPlayButton()
        {
            if (EditorApplication.isPlaying)
                return;
                        
            GUI.enabled = !SessionState.GetBool(previewKey, false);

            GUIStyle style = new GUIStyle(EditorStyles.miniButton);
            style.fixedHeight = 30f;
            style.fontSize = 20;

            if (GUILayout.Button("Play", style))
            {
                tweenPreviewing = true;
                SessionState.SetBool(previewKey, tweenPreviewing);

                Tween tween = doBase.CreateTween();

                tween.onComplete += OnPreviewStopped;

                DOTweenEditorPreview.PrepareTweenForPreview(tween, false, false);
                DOTweenEditorPreview.Start();

                OnPreviewStarted();
            }

            GUI.enabled = true;
        }

        /// <summary>
        /// Draws a Stop button in inspector to stop tween Previewing withing editor
        /// </summary>
        /// <remarks>Will not draw Stop button if in Play Mode <br/>
        /// OnPreviewForceStopped() is called when this button is pressed</remarks>
        protected void DrawStopButton()
        {
            if (EditorApplication.isPlaying)
                return;

            GUI.enabled = SessionState.GetBool(previewKey, false);

            GUIStyle style = new(EditorStyles.miniButton);
            style.fixedHeight = 30f;
            style.fontSize = 20;

            if (GUILayout.Button("Stop", style))
            {
                OnPreviewForceStopped();
            }

            GUI.enabled = true;
        }

        #endregion

        // Functions for drawing Common properties for DOComponents
        #region Inspector Draw Functions

        /// <summary>
        /// Creates a field for a property
        /// </summary>
        /// <param name="property">Reference to property</param>
        protected void DrawProperty(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property);
        }

        /// <summary>
        /// Draws begin, tweenObjectProp(if Begin = After or With) and LayerMask(If Begin = OnTrigger) properties
        /// </summary>
        protected void DrawLifeTimeSettings()
        {
            DrawProperty(beginProp);

            if (doBase.begin == Begin.With ||
                doBase.begin == Begin.After)
            {
                DrawProperty(tweenObjectProp);
            }

            if (doBase.begin == Begin.OnTrigger)
            {
                DrawProperty(layerMaskProp);
            }
        }

        /// <summary>
        /// Draws Helpbox for Inspector messages regarding tweenObject and Begin property
        /// </summary>
        protected void DrawTweenObjectHelpBox()
        {
            if (doBase.begin == Begin.After ||
                doBase.begin == Begin.With)
            {
                if (doBase.tweenObject == null)
                    EditorGUILayout.HelpBox("Tween Object is not assigned", MessageType.Error);
            }
            else
            {
                if (doBase.tweenObject != null)
                {
                    EditorGUILayout.BeginHorizontal();

                    DrawHelpbox("Tween Object is assigned, it should be removed", MessageType.Warning);

                    GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
                    trashButton.tooltip = "Remove Tween Object";

                    if (GUILayout.Button(trashButton, GUILayout.Height(40), GUILayout.Width(40 * 2f)))
                    {
                        doBase.tweenObject = null;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        /// <summary>
        /// Draws tweenType, loopType (if tweenType = Looped), <br/> 
        /// easeType, curve(if easeType = INTERNAL_Custom) properties
        /// </summary>
        protected void DrawTypeSettings()
        {
            DrawProperty(tweenTypeProp);

            if (doBase.tweenType == Enums.TweenType.Looped)
            {
                DrawProperty(loopTypeProp);
            }

            DrawProperty(easeTypeProp);

            if ((Ease)easeTypeProp.enumValueIndex == Ease.INTERNAL_Custom)
            {
                DrawProperty(curveProp);
            }
        }

        /// <summary>
        /// Draws loops(if loopType = Looped), delay, duration Properties
        /// </summary>    
        protected virtual void DrawValues()
        {
            if (doBase.tweenType == Enums.TweenType.Looped)
            {
                DrawProperty(loopsProp);
            }

            DrawProperty(delayProp);
            DrawProperty(durationProp);
        }

        /// <summary>
        /// Draws onTweenCreated, onTweenPlayed, onTweenUpdated, onTweenCompleted, onTweenKilled events
        /// </summary>
        protected void DrawEvents()
        {
            DrawProperty(onTweenCreatedProp);
            DrawProperty(onTweenPlayedProp);
            DrawProperty(onTweenUpdatedProp);
            DrawProperty(onTweenCompletedProp);
            DrawProperty(onTweenKilledProp);
        }

        #endregion

        // Helpful functions for drawing stuff in scene view
        #region Scene Draw Functions

        /// <summary>
        /// Draws a position handle at given handlePosition
        /// </summary>
        /// <param name="handlePosition">The position to draw handle at</param>
        /// <returns>Change in Position of handle</returns>
        protected Vector3 DrawHandle(Vector3 handlePosition)
        {
            Vector3 newHandlePosition = Handles.PositionHandle(handlePosition, Quaternion.identity);

            Vector3 delta;

            if (newHandlePosition != handlePosition)
            {
                Undo.RecordObject(doBase, "Move Handle");

                // Perform the handle move and update the serialized data
                delta = newHandlePosition - handlePosition;
            }
            else
            {
                delta = Vector3.zero;
            }

            return delta;
        }

        /// <summary>
        /// Draws a Free position handle at given handlePosition
        /// </summary>
        /// <param name="handlePosition"></param>
        /// <param name="size">Size of spehere handle</param>
        /// <returns>Change in Position of handle</returns>
        protected Vector3 DrawSphereHandle(Vector3 handlePosition, float size)
        {
            Vector3 newHandlePosition = Handles.FreeMoveHandle(handlePosition, size, Vector3.zero, Handles.SphereHandleCap);

            Vector3 delta;

            if (newHandlePosition != handlePosition)
            {
                Undo.RecordObject(doBase, "Move Handle");

                // Perform the handle move and update the serialized data
                delta = newHandlePosition - handlePosition;
            }
            else
            {
                delta = Vector3.zero;
            }

            return delta;
        }
        
        /// <summary>
        /// Draws a Line in scene
        /// </summary>
        /// <param name="from">Start of Line</param>
        /// <param name="to">End of Line</param>
        /// <param name="color">Color of line</param>
        /// <param name="thickness">Thickness of line, default is 2</param>
        protected void DrawLine(Vector3 from, Vector3 to, Color color, float thickness = 2f)
        {
            Color previousColor = Handles.color;
            Handles.color = color;

            Handles.DrawLine(from, to, thickness);

            Handles.color = previousColor;
        }

        /// <summary>
        /// Draws a Dotted Line in scene
        /// </summary>
        /// <param name="from">Start of Line</param>
        /// <param name="to">End of Line</param>
        /// <param name="color">Color of line</param>
        /// <param name="size">The size in pixels for the lengths of the line segments and the gaps between them.</param>
        protected void DrawDottedLine(Vector3 from, Vector3 to, Color color, float size = 2f)
        {
            Color previousColor = Handles.color;
            Handles.color = color;

            Handles.DrawDottedLine(from, to, size);

            Handles.color = previousColor;
        }

        /// <summary>
        /// Draws a Point (sphere) on given Position
        /// </summary>
        /// <param name="pointPosition">Position of Point</param>
        /// <param name="color">Color of Point</param>
        /// <param name="radius">Radius of Point</param>
        protected void DrawPoint(Vector3 pointPosition, Color color, float radius = 1f)
        {
            Color previousColor = Handles.color;
            Handles.color = color;

            Handles.SphereHandleCap(0, pointPosition, Quaternion.identity, radius, EventType.Repaint);
            Handles.color = previousColor;
        }

        /// <summary>
        /// Draws a line to Tween Object, and draws a line to Tween Object of that Tween Object and so on <br/>
        /// Also displays arrow head to each Tween Object and Begin Property of those Tween Objects
        /// </summary>
        private void DrawTweenObjectInfo()
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

        #endregion

        #region Tween Preview Callbacks

        /// <summary>
        /// Called when Tween Preview is started, (when Play button in inspector is pressed)
        /// </summary>
        protected virtual void OnPreviewStarted()
        {

        }

        /// <summary>
        /// Called when Tween Completes preview, never called in case of infinite loop
        /// </summary>
        protected virtual void OnPreviewStopped()
        {
            DOTweenEditorPreview.Stop(true);

            tweenPreviewing = false;
            SessionState.SetBool(previewKey, tweenPreviewing);
        }

        /// <summary>
        /// Called when Tween preview is stopped using Stop Button in inspector
        /// </summary>
        protected virtual void OnPreviewForceStopped()
        {
            DOTweenEditorPreview.Stop(true);

            tweenPreviewing = false;
            SessionState.SetBool(previewKey, tweenPreviewing);
        }

        #endregion
    }

    /// <summary>
    /// Utility Class used for correct undo/redo from relative to absolute modes
    /// </summary>
    public class RelativeFlags : ScriptableObject
    {
        public bool firstTimeRelative;
        public bool firstTimeNonRelative;
    }
}

#endif