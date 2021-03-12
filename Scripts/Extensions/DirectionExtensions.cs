using System;
using System.Diagnostics.Contracts;
using Godot;
using PrisonLimbo.Scripts;

public static class DirectionExtensions
{
    [Pure]
    public static AnimationState ToAnimationState(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => AnimationState.WalkUp,
            Direction.Right => AnimationState.WalkRight,
            Direction.Down => AnimationState.WalkDown,
            Direction.Left => AnimationState.WalkLeft,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [Pure]
    public static Vector2 ToVector2(this Direction direction)
    {
        return direction switch
        {
            Direction.None => Vector2.Zero,
            Direction.Up => Vector2.Up,
            Direction.Right => Vector2.Right,
            Direction.Down => Vector2.Down,
            Direction.Left => Vector2.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}