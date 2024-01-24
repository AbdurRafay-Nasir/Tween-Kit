#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOShakeAnchorPos))]
    [CanEditMultipleObjects]
    public class DOShakeAnchorPosEditor : DOShakePositionEditor
    {
        // Empty because unity does not allow more than one 
        // CustomEditor attribute on one editor script
        // and this editor script was exactly same as DOPunchPositionEditor
    }
}

#endif