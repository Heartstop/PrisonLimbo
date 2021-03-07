using System;
using PrisonLimbo.Scripts;

public static class DirectionExtensions {
    public static AnimationState ToAnimationState(this Direction direction){
        return direction switch
        {
            Direction.Up => AnimationState.Up,
            Direction.Right => AnimationState.Right,
            Direction.Down => AnimationState.Down,
            Direction.Left => AnimationState.Left,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}