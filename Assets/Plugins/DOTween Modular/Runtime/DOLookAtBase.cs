using UnityEngine;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular
{
    public abstract class DOLookAtBase : DOBase
    {
        public Enums.LookAtSimple lookAt;
        public Vector3 lookAtPosition;
        public Transform lookAtTarget;
        public float interpolate = 0.01f;

        protected override void OnTweenUpdate()
        {
            base.OnTweenUpdate();

            if (lookAt == Enums.LookAtSimple.None) return;

            if (lookAt == Enums.LookAtSimple.Position)
                transform.LookAtSmooth(lookAtPosition, interpolate);
            else if (lookAt == Enums.LookAtSimple.Transform)
                transform.LookAtSmooth(lookAtTarget.position, interpolate);
        }
    }
}
