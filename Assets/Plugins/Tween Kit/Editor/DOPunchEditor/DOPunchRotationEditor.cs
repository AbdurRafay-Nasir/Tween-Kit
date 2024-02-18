#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOPunchRotation)), CanEditMultipleObjects]
    public sealed class DOPunchRotationEditor : DOPunchBaseEditor
    {
        protected override void DrawValues()
        {
            DrawProperty(punchProp);

            base.DrawValues();
        }
    }
}

#endif
