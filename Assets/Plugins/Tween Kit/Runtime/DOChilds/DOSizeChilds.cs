using UnityEngine;
using DG.Tweening;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular
{
    [AddComponentMenu("Tween Kit/DO Size Childs")]
    public sealed class DOSizeChilds : DOChildsBase
    {
        #region Properties

        [Tooltip("If TRUE, the targetSize will be calculated as: " + "\n" +
          "targetSize = targetSize + spriteRenderer.size")]
        public bool relative;

        [Tooltip("The size to reach")]
        public Vector2 targetSize = new(3f, 3f);

        #endregion

        protected override Tween InitializeChildTween(Transform currentChild)
        {
            SpriteRenderer sr = currentChild.GetComponent<SpriteRenderer>();

            return sr.DOSize(targetSize, duration).SetRelative(relative);
        }
    }
}