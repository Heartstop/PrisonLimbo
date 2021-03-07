using System;
using Godot;

namespace PrisonLimbo.Scripts
{
    public static class Vector2Extensions
    {
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
}