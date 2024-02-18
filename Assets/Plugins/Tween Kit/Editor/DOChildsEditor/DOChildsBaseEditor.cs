#if UNITY_EDITOR

using UnityEditor;

namespace DOTweenModular.Editor
{
    public class DOChildsBaseEditor : DOBaseEditor
    {
        protected SerializedProperty joinProp;

        public override void OnEnable()
        {
            base.OnEnable();

            joinProp = serializedObject.FindProperty("join");
        }
    }
}

#endif
