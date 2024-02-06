using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using DOTweenModular.Enums;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular
{
    /// <summary>
    /// Base class for creating DOComponents
    /// </summary>
    public abstract class DOBase : MonoBehaviour
    {
        // DO NOT USE the Tween created using these components
        // Instead use Functions provided by DOTween Modular

        #region Properties

        [Tooltip("When this tween should start")]
        public Begin begin;

        [Tooltip("The DO component After/With which this tween will start")]
        public DOBase tweenObject;

        [Tooltip("Layers that will start this tween")]
        public LayerMask layerMask;

        public Enums.TweenType tweenType;

        [Tooltip("Restart - Start again from start Position/Rotation/Scale " + "\n" +
                 "Yoyo - Start from Target Position/Rotation/Scale")]
        public LoopType loopType;

        [Tooltip("Ease to apply, for custom ease select INTERNAL_Custom. Do not assign INTERNAL_Zero")]
        public Ease easeType;

        [Tooltip("Curve that defines Custom ease")]
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

        /// <summary>
        /// Called when this tween is created
        /// </summary>
        public UnityEvent onTweenCreated;

        /// <summary>
        /// Called the first time this tween starts
        /// </summary>
        public UnityEvent onTweenPlayed;

        /// <summary>
        /// Called every frame while this tween runs
        /// </summary>
        public UnityEvent onTweenUpdated;

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
        /// Assign this to your custom 'Tween'.
        /// </summary>
        protected Tween Tween;

        private bool tweenKilled;

        #region Unity Functions

        private void Awake()
        {
            if (begin == Begin.With)
                tweenObject.onTweenPlayed.AddListener(TweenObjectTween);

            if (begin == Begin.After)
                tweenObject.onTweenCompleted.AddListener(TweenObjectTween);
        }

        private void Start()
        {
            if (begin == Begin.OnSceneStart)
            {
                CreateTween();
                PlayTween();
            }
        }

        private void OnBecameVisible()
        {
            if (begin == Begin.OnVisible)
            {
                CreateTween();
                PlayTween();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (begin != Begin.OnTrigger) return;
            
            if (layerMask.HasLayer(other.gameObject.layer))
            {
                CreateTween();
                PlayTween();
            }
        }

        private void OnDestroy()
        {            
            onTweenCreated.RemoveAllListeners();
            onTweenPlayed.RemoveAllListeners();
            onTweenUpdated.RemoveAllListeners();
            onTweenCompleted.RemoveAllListeners();
            onTweenKilled.RemoveAllListeners();

            if (!tweenKilled)
            {
                Tween.Kill();

                Tween.OnPlay(null);
                Tween.OnUpdate(null);
                Tween.OnComplete(null);
                Tween.OnKill(null);

                tweenKilled = true;
            }

            curve = null;
            tweenObject = null;
            Tween = null;
        }

        #endregion

        /// <summary>
        /// Implement this method for your custom 'Tweens'
        /// </summary>
        /// <returns></returns>
        public abstract Tween CreateTween();

        /// <summary>
        /// This must be called exactly before returning the 'Tween'
        /// </summary>
        protected void TweenCreated()
        {
            OnTweenCreated();
        }

        /// <summary>
        /// Plays the Tween
        /// </summary>
        public void PlayTween()
        {
            Tween.Play();
        }

        /// <summary>
        /// Invoked by OnTweenCreated/OnTweenPlayed events of tweenObject
        /// </summary>
        private void TweenObjectTween()
        {
            CreateTween();
            PlayTween();
        }

        #region Tween Callbacks

        /// <summary>
        /// Called when Tween in Created
        /// </summary>
        protected virtual void OnTweenCreated() 
        {
            onTweenCreated?.Invoke();

            Tween.onPlay += OnTweenPlayed;
            Tween.onUpdate += OnTweenUpdate;
            Tween.onComplete += OnTweenCompleted;
            Tween.onKill += OnTweenKilled;

            print("CREATED");
        }

        /// <summary>
        /// Called when Tween starts to play
        /// </summary>
        protected virtual void OnTweenPlayed()
        {
            onTweenPlayed?.Invoke();
            print("PLAYED");
        }

        /// <summary>
        /// Called every frame while Tween plays
        /// </summary>
        protected virtual void OnTweenUpdate() 
        {
            onTweenUpdated?.Invoke();
            print("UPDATE");
        }

        /// <summary>
        /// Called when the tween completes
        /// </summary>
        protected virtual void OnTweenCompleted()
        {
            onTweenCompleted?.Invoke();
            Tween.Kill();
            print("COMPLETE");
        }

        /// <summary>
        /// Called when the tween is killed
        /// </summary>
        protected virtual void OnTweenKilled()
        {
            tweenKilled = true;

            onTweenKilled?.Invoke();

            Tween.onPlay -= OnTweenPlayed;
            Tween.onUpdate -= OnTweenUpdate;
            Tween.onComplete -= OnTweenCompleted;
            Tween.onKill -= OnTweenKilled;

            // Just to be extra sure
            Tween.OnPlay(null);
            Tween.OnUpdate(null);
            Tween.OnComplete(null);
            Tween.OnKill(null);

            print("KILLED");
        }

        #endregion

    }
}