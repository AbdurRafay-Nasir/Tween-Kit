using UnityEngine;
using DG.Tweening;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular
{
    public sealed class DOMoveRect : DOBase
    {
        #region Properties

        public StartFrom startFrom;
        public Vector2 center;

        [Min(0f)]
        public float cornerRadius;

        [Min(1f)]
        public int resolution = 1;

        #endregion

        public override Tween CreateTween()
        {
            float rectWidth = Mathf.Abs(transform.position.x - center.x);
            float rectHeight = Mathf.Abs(transform.position.y - center.y);

            Vector2[] roundedRectPoints = Curve.Arc.GetRect(center, rectWidth, rectHeight, cornerRadius, resolution);

            roundedRectPoints.Print();

            //Vector2 startPosition = Vector2.zero;

            //switch (startFrom)
            //{
            //    case StartFrom.TopLeft1:
            //        startPosition = roundedRectPoints[0];
            //        break;
            //    case StartFrom.TopLeft2:
            //        startPosition = roundedRectPoints[resolution];
            //        break;
            //    case StartFrom.TopRight1:
            //        startPosition = roundedRectPoints[^2];
            //        break;
            //    case StartFrom.TopRight2:
            //        startPosition = roundedRectPoints[roundedRectPoints.Length - 2 - resolution];
            //        break;
            //    //case StartFrom.BottomLeft1:
            //    //    startPosition = roundedRectPoints[1 + resolution];
            //    //    break;
            //    //case StartFrom.BottomLeft2:
            //    //    startPosition = roundedRectPoints[1 + resolution + resolution];
            //    //    break;
            //    //case StartFrom.BottomRight1:
            //    //    startPosition = roundedRectPoints[2 + resolution + resolution];
            //    //    break;
            //    //case StartFrom.BottomRight2:
            //    //    startPosition = roundedRectPoints[roundedRectPoints.Length - 2 - resolution];
            //    //    break;
            //}

            transform.position = roundedRectPoints[0];

            Tween = transform.DOPath(roundedRectPoints.ToVector3Array(), duration);

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

    public enum StartFrom
    {
        TopLeft1, TopLeft2,
        TopRight1, TopRight2,
        BottomLeft1, BottomLeft2,
        BottomRight1, BottomRight2
    }
}