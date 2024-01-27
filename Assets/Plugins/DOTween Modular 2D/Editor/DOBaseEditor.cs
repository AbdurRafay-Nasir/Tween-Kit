#if UNITY_EDITOR

namespace DOTweenModular.Editor
{
    using DOTweenModular.Enums;
    using DG.Tweening;
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

    public class DOBaseEditor : Editor
    {

        #region Serialized Properties

        protected SerializedProperty beginProp;
        protected SerializedProperty tweenObjectProp;

        protected SerializedProperty delayProp;
        protected SerializedProperty tweenTypeProp;
        protected SerializedProperty loopTypeProp;
        private SerializedProperty easeTypeProp;
        private SerializedProperty curveProp;
        protected SerializedProperty loopsProp;
        protected SerializedProperty durationProp;

        private SerializedProperty onTweenCreatedProp;
        private SerializedProperty onTweenStartedProp;
        private SerializedProperty onTweenUpdatedProp;
        private SerializedProperty onTweenCompletedProp;
        private SerializedProperty onTweenKilledProp;

        #endregion

        private DOBase doBase;
        protected int instanceId;
        private List<string> savedKeys;

        #region Unity Functions

        public virtual void OnEnable()
        {
            doBase = (DOBase)target;
            instanceId = doBase.GetInstanceID();

            beginProp = serializedObject.FindProperty("begin");
            tweenObjectProp = serializedObject.FindProperty("tweenObject");

            delayProp = serializedObject.FindProperty("delay");
            tweenTypeProp = serializedObject.FindProperty("tweenType");
            loopTypeProp = serializedObject.FindProperty("loopType");
            easeTypeProp = serializedObject.FindProperty("easeType");
            curveProp = serializedObject.FindProperty("curve");
            loopsProp = serializedObject.FindProperty("loops");
            durationProp = serializedObject.FindProperty("duration");

            onTweenCreatedProp = serializedObject.FindProperty("onTweenCreated");
            onTweenStartedProp = serializedObject.FindProperty("onTweenStarted");
            onTweenUpdatedProp = serializedObject.FindProperty("onTweenUpdated");
            onTweenCompletedProp = serializedObject.FindProperty("onTweenCompleted");
            onTweenKilledProp = serializedObject.FindProperty("onTweenKilled");
        }

        private void OnDisable()
        { 
            // this means the GameObject/Component was deleted
            // TODO - Confirm it
            if (target == null)
            {
            
            }
        }

        #endregion

        #region GUI Handling

        protected void Space()
        {
            EditorGUILayout.Space();
        }

        protected void DrawHelpbox(string message, MessageType messageType)
        {
            EditorGUILayout.HelpBox(message, messageType);
        }

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
                bool isOn;

                if (!EditorPrefs.HasKey(toggleKeys[i]))
                {
                    isOn = GUILayout.Toggle(true, toggleNames[i], toggleStyle);                    
                    EditorPrefs.SetBool(toggleKeys[i], isOn);
                }
                else
                {
                    isOn = EditorPrefs.GetBool(toggleKeys[i]);

                    EditorGUI.BeginChangeCheck();
                    isOn = GUILayout.Toggle(isOn, toggleNames[i], toggleStyle);

                    if (EditorGUI.EndChangeCheck())
                    {
                        toggleStates[i] = isOn;
                        EditorPrefs.SetBool(toggleKeys[i], isOn);
                    }
                    else
                    {
                        // If no change, use the stored value
                        toggleStates[i] = isOn;
                    }
                }                
            }

            GUILayout.EndHorizontal();

            return toggleStates;
        }

        protected void DrawSeparatorLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        protected void BeginBackgroundBox()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        }

        protected bool BeginFoldout(string foldoutName, bool openByDefault = true)
        {
            string foldoutKey = instanceId + "_" + "Foldout_" + foldoutName;

            if (!EditorPrefs.HasKey(foldoutKey))
            {
                bool open = EditorGUILayout.BeginFoldoutHeaderGroup(openByDefault, foldoutName);
                EditorPrefs.SetBool(foldoutKey, open);
                return open;
            }
            else
            {
                bool open = EditorPrefs.GetBool(foldoutKey);
                open = EditorGUILayout.BeginFoldoutHeaderGroup(open, foldoutName);
                EditorPrefs.SetBool(foldoutKey, open);
                return open;
            }
        }

        protected void EndBackgroundBox()
        {
            EditorGUILayout.EndVertical();
        }

        protected void EndFoldout()
        {
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        #endregion

        #region Scene Draw Functions

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

        protected void DrawLine(Vector3 from, Vector3 to, Color color, float thickness = 2f)
        {
            Color previousColor = Handles.color;
            Handles.color = color;

            Handles.DrawLine(from, to, thickness);

            Handles.color = previousColor;
        }

        protected void DrawDottedLine(Vector3 from, Vector3 to, Color color, float size = 2f)
        {
            Color previousColor = Handles.color;
            Handles.color = color;

            Handles.DrawDottedLine(from, to, size);

            Handles.color = previousColor;
        }

        protected void DrawPoint(Vector3 pointPosition, Color color, float size = 1f)
        {
            Color previousColor = Handles.color;
            Handles.color = color;

            Handles.SphereHandleCap(0, pointPosition, Quaternion.identity, size, EventType.Repaint);
            Handles.color = previousColor;
        }

        #endregion

        #region Inspector Draw Functions

        protected void DrawProperty(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property);
        }

        /// <summary>
        /// Draws begin, tweenObjectProp(if Begin = After or With), kill <br/>
        /// destroy component, destroy gameObject
        /// </summary>
        protected void DrawLifeTimeSettings()
        {
            DrawProperty(beginProp);

            if (doBase.begin == Begin.With ||
                doBase.begin == Begin.After)
            {
                DrawProperty(tweenObjectProp);
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

                    if (GUILayout.Button(trashButton, GUILayout.Height(40), GUILayout.Width(40 * 2f)))
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
        /// Draws loops(if loopType = Looped), delay, duration Property
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
        /// Draws onTweenCreated, onTweenStartedProp, onTweenCompleted, onTweenKilledProp events
        /// </summary>
        protected void DrawEvents()
        {
            DrawProperty(onTweenCreatedProp);
            DrawProperty(onTweenStartedProp);
            DrawProperty(onTweenUpdatedProp);
            DrawProperty(onTweenCompletedProp);
            DrawProperty(onTweenKilledProp);
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

        #endregion

    }

    public class RelativeFlags : ScriptableObject
    {
        public bool firstTimeRelative;
        public bool firstTimeNonRelative;
    }

}

#endif