#if UNITY_EDITOR

using UnityEditor;

namespace TweenKit.Editor
{
    [CustomEditor(typeof(DOPunchScale)), CanEditMultipleObjects]
    public sealed class DOPunchScaleEditor : DOPunchBaseEditor
    {
        protected override void DrawValues()
        {
            DrawProperty(punchProp);

            base.DrawValues();
        }
    }
}

#endif
