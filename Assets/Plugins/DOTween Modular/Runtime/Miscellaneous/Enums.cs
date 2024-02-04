using UnityEngine;

namespace DOTweenModular.Enums
{
    public enum Begin
    {
        [Tooltip("Start as soon game runs")]
        OnSceneStart,

        [Tooltip("Start only When visible for first time")]
        OnVisible,

        [Tooltip("Start when something enters the trigger")]
        OnTrigger,

        [Tooltip("Will not start on its own, you have to either manually call CreateTween().PlayTween()" + "\n" +
                 "or use this componenet in a sequence component")]
        Manual,
    
        [Tooltip("Start AFTER Tween Object's tween is completed")]
        After,

        [Tooltip("Start when Tween Object's tween is created")]
        With
    }

    public enum TweenType
    {
        Simple, Looped    
    }

    public enum LookAtSimple
    {
        None, Position, Transform
    }

    public enum LookAtAdvanced
    {
        None, Position, Transform, Percentage
    }

}