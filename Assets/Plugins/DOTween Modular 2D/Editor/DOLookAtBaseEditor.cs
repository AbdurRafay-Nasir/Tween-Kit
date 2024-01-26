#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular.Editor
{
    [CustomEditor(typeof(DOLookAtBase)), CanEditMultipleObjects]
    public class DOLookAtBaseEditor : DOBaseEditor
    {
        protected SerializedProperty lookAtProp;
        protected SerializedProperty lookAtPositionProp;
        protected SerializedProperty lookAtTargetProp;


    }

}

#endif
