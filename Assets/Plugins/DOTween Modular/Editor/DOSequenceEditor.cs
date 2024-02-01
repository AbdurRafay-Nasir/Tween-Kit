#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOSequence)), CanEditMultipleObjects]
    public class DOSequenceEditor : DOBaseEditor
    {
        private SerializedProperty sequenceTweensProp;

        private DOSequence doSequence;

        #region Unity Functions

        public override void OnEnable()
        {
            base.OnEnable();

            doSequence = (DOSequence)target;

            sequenceTweensProp = serializedObject.FindProperty("sequenceTweens");
        }

        public override void OnInspectorGUI()
        {
            Space();

            bool[] toggleStates = DrawToggles("Life", "Sequence", "Tweens", "Events");

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

                if (BeginFoldout("Sequence Settings"))
                {
                    EditorGUI.indentLevel++;

                    BeginBackgroundBox();
                    Space();

                    DrawSequenceSettings();

                    Space();
                    EndBackgroundBox();

                    EditorGUI.indentLevel--;
                }

                EndFoldout();
            }

            if (toggleStates[2])
            {
                DrawSeparatorLine();

                DrawProperty(sequenceTweensProp);

                DrawSequenceTweensHelpbox();

                EndFoldout();
            }

            if (toggleStates[3])
            {
                DrawSeparatorLine();

                if (BeginFoldout("Events"))
                {
                    EditorGUI.indentLevel++;

                    Space();

                    DrawEvents();

                    EditorGUI.indentLevel--;
                }
                EndFoldout();
            }

            DrawSeparatorLine();

            if (doSequence.sequenceTweens != null && doSequence.sequenceTweens.Length != 0)
            {
                if (!doSequence.sequenceTweens.Any(sequenceTween => sequenceTween.tweenObject == null))
                {
                    DrawPlayButton();
                    DrawStopButton();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            if (doSequence.sequenceTweens == null) return;
            if (doSequence.sequenceTweens.Length == 0) return;

            DrawLinesToSequenceTweens();
            DrawLabelsToSequenceTweens();
        }

        #endregion

        #region Inspector Draw Functions

        private void DrawSequenceSettings()
        {
            EditorGUILayout.PropertyField(tweenTypeProp);

            if ((Enums.TweenType)tweenTypeProp.enumValueIndex == Enums.TweenType.Looped)
            {
                EditorGUILayout.PropertyField(loopTypeProp);
                EditorGUILayout.PropertyField(loopsProp);
            }
        }

        private void DrawSequenceTweensHelpbox()
        {
            if (doSequence.sequenceTweens != null)
            {
                for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
                {
                    DOBase currentTween = doSequence.sequenceTweens[i].tweenObject;

                    if (currentTween == null)
                    {
                        DrawHelpbox("Element: " + i + " Tween Object is not assigned", MessageType.Error);
                    }
                }
            }
        }

        #endregion

        #region Scene Draw Functions

        private void DrawLinesToSequenceTweens()
        {
            Vector3 startPosition = doSequence.transform.position;

            for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
            {
                DOBase sequenceTweenObj = doSequence.sequenceTweens[i].tweenObject;
                if (sequenceTweenObj == null)
                    continue;

                Vector3 sequenceTweenObjPos = sequenceTweenObj.transform.position;

                DrawLine(startPosition, sequenceTweenObjPos, Color.cyan);
            }
        }

        private void DrawLabelsToSequenceTweens()
        {
            int num = 1;

            Vector3 startPosition = doSequence.transform.position;

            for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
            {
                DOBase sequenceTweenObj = doSequence.sequenceTweens[i].tweenObject;
                if (sequenceTweenObj == null)
                    continue;

                Vector3 sequenceTweenObjPos = sequenceTweenObj.transform.position;
                Vector3 midPoint = (startPosition + sequenceTweenObjPos) * 0.5f;

                Handles.Label(midPoint, num.ToString(), new GUIStyle() { fontSize = 20 });

                if (i + 1 < doSequence.sequenceTweens.Length &&
                    doSequence.sequenceTweens[i + 1].tweenObject != null &&
                    !doSequence.sequenceTweens[i + 1].join)
                {
                    num++;
                }
            }
        }

        #endregion

        #region Tween Preview Functions

        protected override void OnPreviewStarted()
        {
            base.OnPreviewStarted();

            for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
            {
                string positionKey = "DOSequence_" + instanceId + "_sequenceTween_" + i + "_Position";
                string rotationKey = "DOSequence_" + instanceId + "_sequenceTween_" + i + "_Rotation";
                string scaleKey = "DOSequence_" + instanceId + "_sequenceTween_" + i + "_Scale";

                DOBase currentTween = doSequence.sequenceTweens[i].tweenObject;

                SessionState.SetVector3(positionKey, currentTween.transform.localPosition);
                SessionState.SetVector3(rotationKey, currentTween.transform.localEulerAngles);
                SessionState.SetVector3(scaleKey, currentTween.transform.localScale);
            }
        }

        protected override void OnPreviewStopped()
        {
            base.OnPreviewStopped();

            for (int i = 0; i < doSequence.sequenceTweens.Length; i++)
            {
                string positionKey = "DOSequence_" + instanceId + "_sequenceTween_" + i + "_Position";
                string rotationKey = "DOSequence_" + instanceId + "_sequenceTween_" + i + "_Rotation";
                string scaleKey = "DOSequence_" + instanceId + "_sequenceTween_" + i + "_Scale";

                DOBase currentTween = doSequence.sequenceTweens[i].tweenObject;

                currentTween.transform.localPosition = SessionState.GetVector3(positionKey, currentTween.transform.localPosition);
                currentTween.transform.localEulerAngles = SessionState.GetVector3(rotationKey, currentTween.transform.localEulerAngles);
                currentTween.transform.localScale = SessionState.GetVector3(scaleKey, currentTween.transform.localScale);
            }
        }

        #endregion

    }
}

#endif
