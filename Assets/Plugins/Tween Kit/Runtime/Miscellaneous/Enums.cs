using UnityEngine;

namespace DOTweenModular.Enums
{
    public enum Begin
    { 
        OnSceneStart, OnVisible, OnTrigger, Manual, After, With
    }

    public enum TweenType
    {
        Simple, Looped    
    }

    public enum Direction
    {
        LocalUp, LocalRight, LocalForward
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