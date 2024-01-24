using DG.Tweening;
using DOTweenModular2D.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace DOTweenModular2D
{
    public abstract class DOBase : MonoBehaviour
    {
        #region Properties

        [Tooltip("When this tween should start")]
        public Begin begin;

        [Tooltip("The DO component After/With this tween will start")]
        public DOBase tweenObject;

        [Tooltip("When To kill this tween")]
        public Kill kill;

        [Tooltip("If TRUE, destroys the component when tween is killed")]
        public bool destroyComponent;

        [Tooltip("If TRUE, destroys the Game Object when tween is killed")]
        public bool destroyGameObject;

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

        #region Unity Events

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

        /// <summary>
        /// Must assign this to custom tween that you create 
        /// </summary>
        protected Tween tween;
        public Tween Tween { get { return tween; } }

        private bool visible;
        private bool enteredTrigger;

        private void Awake()
        {
            if (begin == Begin.Manual) return;

            if (begin == Begin.After)
                tweenObject.onTweenCompleted.AddListener(OnStartAfterTweenCompleted);

            else if (begin == Begin.With)
                tweenObject.onTweenStarted.AddListener(OnStartWithTweenStarted);
        }

        private void Start()
        {
            if (begin == Begin.Manual) return;
            if (begin == Begin.After) return;
            if (begin == Begin.With) return;

            CreateTween();

            if (begin != Begin.OnSceneStart) return;

            tween.Play();
            onTweenStarted?.Invoke();
        }

        private void OnBecameVisible()
        {
            if (begin != Begin.OnVisible) return;
            if (visible) return;

            visible = true;
            tween.Play();
            onTweenStarted?.Invoke();
        }

        private void OnBecameInvisible()
        {
            if (kill != Kill.OnInvisible) return;
            if (tween == null) return;
            if (!tween.playedOnce) return;

            onTweenKilled?.Invoke();
            ClearTweenCallbacks();
            tween.Kill();
            tween = null;

            if (destroyComponent) Destroy(this);
            if (destroyGameObject) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (begin != Begin.OnTrigger) return;
            if (enteredTrigger) return;

            enteredTrigger = true;
            tween.Play();
            onTweenStarted?.Invoke();
        }

        protected void OnDestroy()
        {
            if (tween != null)
            {
                onTweenKilled?.Invoke();
                ClearTweenCallbacks();
                tween.Kill();
                tween = null;
            }

            tweenObject = null;

            onTweenCreated?.RemoveAllListeners();
            onTweenStarted?.RemoveAllListeners();
            onTweenCompleted?.RemoveAllListeners();
        }

        /// <summary>
        /// Implement this method for your custom tween <b/>
        /// Must Call InvokeTweenCreated() once custom tween is created
        /// </summary>
        public abstract void CreateTween();

        /// <summary>
        /// Must call this after creating custom tween
        /// </summary>
        protected void InvokeTweenCreated()
        {
            onTweenCreated.AddListener(OnTweenCreated);
            onTweenCreated?.Invoke();
        }

        private void OnTweenCreated()
        {
            tween.onComplete += OnTweenCompleted;
            onTweenCreated.RemoveListener(OnTweenCreated);
        }

        private void OnTweenCompleted()
        {
            onTweenCompleted?.Invoke();
            tween.onComplete -= OnTweenCompleted;

            if (kill == Kill.OnTweenComplete)
            {
                onTweenKilled?.Invoke();

                ClearTweenCallbacks();
                tween.Kill();
                tween = null;

                if (destroyComponent) Destroy(this);
                if (destroyGameObject) Destroy(gameObject);
            }
        }

        private void OnStartWithTweenStarted()
        {
            CreateTween();
            tween.Play();
            onTweenStarted?.Invoke();

            tweenObject.onTweenStarted.RemoveListener(OnStartWithTweenStarted);
        }

        private void OnStartAfterTweenCompleted()
        {
            CreateTween();
            tween.Play();
            onTweenStarted?.Invoke();

            tweenObject.onTweenCompleted.RemoveListener(OnStartAfterTweenCompleted);
        }

        private void ClearTweenCallbacks()
        {
            tween.OnPause(null);
            tween.OnPlay(null);
            tween.OnRewind(null);
            tween.OnStart(null);
            tween.OnStepComplete(null);
            tween.OnUpdate(null);
            tween.OnWaypointChange(null);
            tween.OnComplete(null);
            tween.OnKill(null);
        }
    }
}