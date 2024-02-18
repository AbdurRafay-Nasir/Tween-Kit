#if UNITY_EDITOR

using UnityEditor;

namespace TweenKit.Editor
{
    [CustomEditor(typeof(DOShakeRotation)), CanEditMultipleObjects]
    public sealed class DOShakeRotationEditor : DOShakeBaseEditor
    {
        protected override void DrawValues()
        {
            DrawProperty(strengthProp);

            base.DrawValues();
        }
    }
}

#endif
