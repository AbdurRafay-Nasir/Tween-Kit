using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/DO Sequence", 1)]
    public class DOSequence : DOBase
    {
        public SequenceTweens[] sequenceTweens;

        public override void CreateTween()
        {
            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < sequenceTweens.Length; i++)
            {
                DOBase currentTween = sequenceTweens[i].tweenObject;
                currentTween.CreateTween();

                if (sequenceTweens[i].join)
                    sequence.Join(currentTween.Tween);
                else
                    sequence.Append(currentTween.Tween);
            }

            if (tweenType == Enums.TweenType.Looped) 
                sequence.SetLoops(loops, loopType);

            tween = sequence;
            InvokeTweenCreated();
        }
    }

    [System.Serializable]
    public struct SequenceTweens
    {
        [Tooltip("If TRUE, this Tween Object will play with previous Tween Object, has no effect if this is first Tween Object")]
        public bool join;

        [Tooltip("Tween to play")]
        public DOBase tweenObject;
    }

}