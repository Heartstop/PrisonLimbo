using System;
using System.Diagnostics.Contracts;
using PrisonLimbo.Scripts;

public static class DirectionExtensions
{
    [Pure]
    public static AnimationState ToAnimationState(this Direction direction, AnimationAction action = AnimationAction.Walk)
    {   
        return action switch {
            AnimationAction.Walk => direction switch
            {
                Direction.Up => AnimationState.WalkUp,
                Direction.Right => AnimationState.WalkRight,
                Direction.Down => AnimationState.WalkDown,
                Direction.Left => AnimationState.WalkLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            },
            AnimationAction.Stab => direction switch
            {
                Direction.Up => AnimationState.StabUp,
                Direction.Right => AnimationState.StabRight,
                Direction.Down => AnimationState.StabDown,
                Direction.Left => AnimationState.StabLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }

    [Pure]
    public static Vector2I ToVector2I(this Direction direction)
    {
        return direction switch
        {
            Direction.None => Vector2I.Zero,
            Direction.Up => Vector2I.Up,
            Direction.Right => Vector2I.Right,
            Direction.Down => Vector2I.Down,
            Direction.Left => Vector2I.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    [Pure]
    public static Direction Invert(this Direction direction)
    {
        return direction switch
        {
            Direction.None => Direction.None,
            Direction.Up => Direction.Down,
            Direction.Right => Direction.Left,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}