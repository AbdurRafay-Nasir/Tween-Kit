using UnityEngine;

namespace DOTweenModular
{
    public abstract class DOLookAtBase : DOBase
    {
        public Enums.LookAtSimple lookAt;
        public Vector3 lookAtPosition;
        public Transform lookAtTarget;
    }
}
