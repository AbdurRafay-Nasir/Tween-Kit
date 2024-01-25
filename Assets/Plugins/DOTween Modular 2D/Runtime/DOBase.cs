using DG.Tweening;
using DOTweenModular.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace DOTweenModular
{
    public abstract class DOBase : MonoBehaviour
    {
        #region Properties

        [Tooltip("When this tween should start")]
        public Begin begin;

        [Tooltip("The DO component After/With this tween will start")]
        public DOBase tweenObject;

        [Tooltip("Time after which this tween will play")]
        public float delay;

        public Enums.TweenType tweenType;

        [Tooltip("Restart - Start again from start Position/Rotation/Scale " + "\n" +
                 "Yoyo - Start from Target Position/Rotation/Scale")]
        public LoopType loopType;

        [Tooltip("Ease to apply, for custom ease select INTERNAL_Custom. Do not assign INTERNAL_Zero")]
        public Ease easeType;
        public AnimationCurve curve;

        [Tooltip("Number of loops, -1 for infinite loops " + "\n" +
                 "For Yoyo Loop Type the backward movement will also be counted")]
        [Min(-1)] public int loops = -1;

        [Tooltip("How long this tween will play")]
        [Min(0)] public float duration = 1;

        #endregion

        #region Events

        // Events
        /// <summary>
        /// Called when this tween is created
        /// </summary>
        public UnityEvent onTweenCreated;

        /// <summary>
        /// Called the first time this tween starts
        /// </summary>
        public UnityEvent onTweenStarted;

        /// <summary>
        /// Called when this tween completes, in-case of infinite loops this will not invoke
        /// </summary>
        public UnityEvent onTweenCompleted;

        /// <summary>
        /// Called when this tween is Killed, in-case of infinite loops this will not invoke
        /// </summary>
        public UnityEvent onTweenKilled;

        #endregion

        private Tween tween;

        private void Awake()
        {
            tween = CreateTween();

            OnTweenCreated();
            onTweenCreated?.Invoke();
        }

        private void Start()
        {
            if (begin == Begin.OnSceneStart)
            {
                tween.Play();
            }
        }

        private void OnBecameVisible()
        {
            if (begin == Begin.OnVisible)
            {
                tween.Play();
            }
        }

        private void OnTweenCreated()
        {

        }

        public abstract Tween CreateTween();
    }
}