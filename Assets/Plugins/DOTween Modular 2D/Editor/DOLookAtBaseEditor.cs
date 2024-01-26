#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    public class DOLookAtBaseEditor : DOBaseEditor
    {
        protected SerializedProperty lookAtProp;
        protected SerializedProperty lookAtPositionProp;
        protected SerializedProperty lookAtTargetProp;

        public override void OnEnable()
        {
            base.OnEnable();

            lookAtProp = serializedObject.FindProperty("lookAt");
            lookAtPositionProp = serializedObject.FindProperty("lookAtPosition");
            lookAtTargetProp = serializedObject.FindProperty("lookAtTarget");
        }

        protected void DrawLookAtSettings()
        {
            DrawProperty(lookAtProp);

            if ((Enums.LookAtSimple) lookAtProp.enumValueIndex == Enums.LookAtSimple.Position)
                DrawProperty(lookAtPositionProp);

            if ((Enums.LookAtSimple) lookAtProp.enumValueIndex == Enums.LookAtSimple.Transform)
                DrawProperty(lookAtTargetProp);
        }
    }

}

#endif
