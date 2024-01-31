#if UNITY_EDITOR

using System.Linq;
using UnityEditor;

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

            if (!doSequence.sequenceTweens.Any(sequenceTween => sequenceTween.tweenObject == null))
            {
                DrawPlayButton();
                DrawStopButton();
            }

            serializedObject.ApplyModifiedProperties();
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
