using DG.Tweening;

namespace DOTweenModular
{
    [UnityEngine.AddComponentMenu("DOTween Modular/DO Shake Scale")]
    public sealed class DOShakeScale : DOShakeBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOShakeScale(duration, strength, vibrato, randomness, fadeout, randomnessMode);
        }
    }
}