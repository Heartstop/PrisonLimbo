using System;
using System.Collections.Generic;
using Godot;

namespace PrisonLimbo.Scripts
{
    public readonly struct Vector2I : IEquatable<Vector2I>
    {
        public static Vector2I Up = new Vector2I(0, 1);
        public static Vector2I Right = new Vector2I(1, 0);
        public static Vector2I Down = new Vector2I(0, -1);
        public static Vector2I Left = new Vector2I(-1, 0);

        public bool Equals(Vector2I other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2I other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(Vector2I left, Vector2I right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2I left, Vector2I right)
        {
            return !left.Equals(right);
        }

        public static Vector2I operator +(Vector2I left, Vector2I right)
        {
            return new Vector2I(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2I operator -(Vector2I left, Vector2I right)
        {
            return new Vector2I(left.X - right.X, left.Y - right.Y);
        }

        public IEnumerable<Vector2I> Adjacent(int width, int height)
        {
            return Adjacent(width, height, X, Y);
        }

        public static IEnumerable<Vector2I> Adjacent(int width, int height, int x, int y)
        {
            if (x < width - 1)
                yield return new Vector2I(x + 1, y);
            if (x > 0)
                yield return new Vector2I(x - 1, y);
            if (y < height - 1)
                yield return new Vector2I(x, y + 1);
            if (y > 0)
                yield return new Vector2I(x, y - 1);
        }

        public static explicit operator Vector2(Vector2I vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public int X { get; }
        public int Y { get; }

        public Vector2I(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}