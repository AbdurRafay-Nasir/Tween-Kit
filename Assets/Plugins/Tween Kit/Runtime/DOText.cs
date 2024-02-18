using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("Tween Kit/DO Text")]
    [RequireComponent(typeof(Text))]
    public sealed class DOText : DOBase
    {
        #region Properties

        [Tooltip("The text to write")]
        [TextArea]
        public string targetText = "DOTween Modular";

        public bool useRichText;

        [Tooltip("Type of scramble to apply to string")]
        public ScrambleMode scrambleMode;

        [Tooltip("Custom Scamble Characters")]
        public string scrambleChars = "klawdfuiawjwn";

        #endregion

        public override Tween CreateTween()
        {
            Text text = GetComponent<Text>();
            Tween = text.DOText(targetText, duration, useRichText, scrambleMode, scrambleChars);

            if (easeType == Ease.INTERNAL_Custom)
                Tween.SetEase(curve);
            else
                Tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                Tween.SetLoops(loops, loopType);

            Tween.SetDelay(delay);

            TweenCreated();

            return Tween;
        }
    }
}