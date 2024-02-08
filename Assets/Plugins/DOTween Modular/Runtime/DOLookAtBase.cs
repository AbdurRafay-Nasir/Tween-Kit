using UnityEngine;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular
{
    public abstract class DOLookAtBase : DOBase
    {
        [Tooltip("The type of Look At: " + "\n" + "\n" +
                 "None - Nothing to Look At, what did you expect?" + "\n" + "\n" +
                 "Position - The Position to Look At, useful when lookAt target won't move" + "\n" + "\n" +
                 "Transform - The gameObject to Look At, useful when lookAt target can/will move")]
        public Enums.LookAtSimple lookAt;

        [Tooltip("The Position to Look At")]
        public Vector3 lookAtPosition;

        [Tooltip("The gameObject to Look At")]
        public Transform lookAtTarget;

        [Tooltip("How smoothly gameObject should rotate to Look At Target" + "\n" + 
                 "Ranges from 0 to 1, 1 means no smoothness")]
        [Range(0f, 1f)] public float interpolate = 0.01f;

        // Called every frame while tween plays
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
