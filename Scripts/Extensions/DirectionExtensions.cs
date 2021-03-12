using System;
using Godot;
using PrisonLimbo.Scripts;

public static class DirectionExtensions
{
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