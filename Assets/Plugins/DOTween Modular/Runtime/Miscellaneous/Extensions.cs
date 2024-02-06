using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Options;

namespace DOTweenModular.Miscellaneous
{
    public static class Extensions
    {
        #region Tween Functions

        /// <summary>Tweens a Transform so that it moves in a circle around the given center.
        /// Also stores the Transform as the tween's target so it can be used for filtered operations.<para/>
        /// IMPORTANT: SetFrom(value) requires a <see cref="Vector2"/> instead of a float, where the X property represents the "from degrees value"</summary>
        /// <param name="center">Circle-center/pivot around which to rotate</param>
        /// <param name="endValueDegrees">The end value degrees to reach (to rotate counter-clockwise pass a negative value)</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="relativeCenter">If TRUE the <see cref="center"/> coordinates will be considered as relative to the target's current Position</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        /// <remarks>Only works in 2D (XY-Plane)</remarks>
        public static TweenerCore<Vector2, Vector2, CircleOptions> DOShapeCircle(
            this Transform target, Vector2 center, float endValueDegrees, float duration,
            bool relativeCenter = false, bool snapping = false
        )

        {
            TweenerCore<Vector2, Vector2, CircleOptions> t = DOTween.To(
                CirclePlugin.Get(), () => target.position, x => target.position = x, center, duration
            );
            t.SetOptions(endValueDegrees, relativeCenter, snapping).SetTarget(target);
            return t;
        }

        /// <summary>Tweens a Transform's local Position so that it moves in a circle around the given center.
        /// Also stores the Transform as the tween's target so it can be used for filtered operations.<para/>
        /// IMPORTANT: SetFrom(value) requires a <see cref="Vector2"/> instead of a float, where the X property represents the "from degrees value"</summary>
        /// <param name="center">Circle-center/pivot around which to rotate</param>
        /// <param name="endValueDegrees">The end value degrees to reach (to rotate counter-clockwise pass a negative value)</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        /// <remarks>Only works in 2D (XY-Plane)</remarks>
        public static TweenerCore<Vector2, Vector2, CircleOptions> DOLocalShapeCircle(
            this Transform target, Vector2 center, float endValueDegrees,
            float duration, bool snapping = false
        )

        {
            TweenerCore<Vector2, Vector2, CircleOptions> t = DOTween.To(
                CirclePlugin.Get(), () => target.localPosition, x => target.localPosition = x, center, duration
            );
            t.SetOptions(endValueDegrees, false, snapping).SetTarget(target);
            return t;
        }

        /// <summary>
        /// Tweens Sprite Renderer Size to given targetSize
        /// </summary>
        /// <param name="targetSize">The Size to Reach</param>
        /// <param name="duration">The duration of the tween</param>
        /// <remarks>Does not take into account the SpriteRenderer Draw Mode. <br/>
        /// SpriteRenderer Draw Mode should be set to Sliced or Tiled
        /// </remarks>
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOSize(
            this SpriteRenderer target, Vector2 targetSize, float duration
        )
        {
            TweenerCore<Vector2, Vector2, VectorOptions> tween = DOTween.To(
                () => target.size, x => target.size = x, targetSize, duration);

            tween.SetTarget(target);
            return tween;
        }

        /// <summary>
        /// Tweens Sprite Renderer Width to given targetWidth
        /// </summary>
        /// <param name="targetWidth">The Width to Reach</param>
        /// <param name="duration">The duration of the tween</param>
        /// <remarks>Does not take into account the SpriteRenderer Draw Mode. <br/>
        /// SpriteRenderer Draw Mode should be set to Sliced or Tiled
        /// </remarks>
        public static TweenerCore<float, float, FloatOptions> DOWidth(
            this SpriteRenderer target, float targetWidth, float duration
        )
        {
            TweenerCore<float, float, FloatOptions> tween = DOTween.To(
                () => target.size.x, x => target.size = new Vector2(x, target.size.y), targetWidth, duration);

            tween.SetTarget(target);
            return tween;
        }

        /// <summary>
        /// Tweens Sprite Renderer Height to given targetheight
        /// </summary>
        /// <param name="targetheight">The Height to Reach</param>
        /// <param name="duration">The duration of the tween</param>
        /// <remarks>Does not take into account the SpriteRenderer Draw Mode. <br/>
        /// SpriteRenderer Draw Mode should be set to Sliced or Tiled
        /// </remarks>
        public static TweenerCore<float, float, FloatOptions> DOHeight(
            this SpriteRenderer target, float targetheight, float duration
        )
        {
            TweenerCore<float, float, FloatOptions> tween = DOTween.To(
                () => target.size.y, y => target.size = new Vector2(target.size.x, y), targetheight, duration);

            tween.SetTarget(target);
            return tween;
        }

        #endregion

        /// <summary>
        /// Rotate on Z-Axis to Look At target
        /// </summary>
        /// <param name="target">The target to look at</param>
        /// <param name="offset">Offset to add in Z Rotation, -90 makes the game Object Look directly At the target</param>
        public static void LookAt2D(this Transform transform, Vector2 target, float offset)
        {
            Vector2 direction = (target - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        }

        /// <summary>
        /// Rotate on Z-Axis to Look At target
        /// </summary>
        /// <param name="target">The target to look at</param>
        /// <param name="offset">Offset to add in Z Rotation, -90 makes the game Object Look directly At the target</param>
        /// <param name="interpolate">How smoothly the Game Object will rotate to Look At target, 0 to 1, 1 means no smoothness</param>
        public static void LookAt2DSmooth(this Transform transform, Vector2 target, float offset, float interpolate)
        {
            Vector2 direction = (target - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotGoal = Quaternion.Euler(0f, 0f, angle + offset);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, interpolate);
        }

        /// <summary>
        /// Smoothly rotate to LookAt target
        /// </summary>
        /// <param name="target">The Point to look at</param>
        /// <param name="interpolate">How smoothly the Game Object will rotate to Look At target,0 to 1, 1 means no smoothness</param>
        public static void LookAtSmooth(this Transform transform, Vector3 target, float interpolate)
        {
            Vector3 lookDirection = target - transform.position;
            lookDirection.Normalize();

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), interpolate);
        }

        /// <summary>
        /// Check if given layer is present in Layer mask
        /// </summary>
        /// <param name="layer">The layer to check</param>
        /// <returns>True if 'layer' is found in Layer Mask</returns>
        public static bool HasLayer(this LayerMask layerMask, int layer)
        {
            return (layerMask.value & (1 << layer)) != 0;
        }
    }
}