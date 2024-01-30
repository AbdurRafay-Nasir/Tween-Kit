#if UNITY_EDITOR

    using UnityEngine;
    using UnityEditor;

    namespace DOTweenModular.Editor
    {
        [CustomEditor(typeof(DORotate)), CanEditMultipleObjects]
        public class DORotateEditor : DOBaseEditor
        {
            
        }
    }

#endif
