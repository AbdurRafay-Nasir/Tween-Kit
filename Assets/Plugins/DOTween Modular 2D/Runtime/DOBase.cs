using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using DOTweenModular.Enums;

namespace DOTweenModular
{
    public abstract class DOBase : MonoBehaviour
    {
        #region Properties

        [Tooltip("When this tween should start")]
        public Begin begin;

        [Tooltip("The DO component After/With this tween will start")]
        public DOBase tweenObject;

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

        [Tooltip("Time after which this tween will play")]
        [Min(0)] public float delay;

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

        #region Unity Functions

        private void Awake()
        {
            if (begin == Begin.Manual) return;

            if (begin == Begin.With)
                tweenObject.onTweenStarted.AddListener(OnTweenObjectTweenStarted);

            if (begin == Begin.After)
                tweenObject.onTweenCompleted.AddListener(OnTweenObjectTweenCompleted);
        }

        private void Start()
        {
            if (begin == Begin.Manual) return;

            if (begin == Begin.OnSceneStart)
            {
                SetupAndPlayTween();
            }
        }

        private void OnBecameVisible()
        {
            if (begin == Begin.Manual) return;

            if (begin == Begin.OnVisible)
            {
                SetupAndPlayTween();
            }
        }

        private void OnDestroy()
        {
            tween.Kill();
            onTweenKilled?.Invoke();

            tween.OnComplete(null);
            tween.OnKill(null);

            onTweenCreated?.RemoveAllListeners();
            onTweenStarted?.RemoveAllListeners();
            onTweenCompleted?.RemoveAllListeners();
            onTweenKilled?.RemoveAllListeners();

            curve = null;
            tweenObject = null;
            tween = null;
        }

        #endregion

        private void OnTweenCompleted()
        {
            onTweenCompleted?.Invoke();

            tween.Kill();
        }

        private void OnTweenKilled()
        {
            onTweenKilled?.Invoke();

            tween.OnComplete(null);
            tween.OnKill(null);
        }

        private void OnTweenObjectTweenStarted()
        {
            SetupAndPlayTween();
        }

        private void OnTweenObjectTweenCompleted()
        {
            SetupAndPlayTween();
        }

        private void SetupAndPlayTween()
        {
            tween = CreateTween();
            onTweenCreated?.Invoke();

            tween.onComplete += OnTweenCompleted;
            tween.onKill += OnTweenKilled;

            tween.Play();
            onTweenStarted?.Invoke();
        }

        public abstract Tween CreateTween();
    }
}