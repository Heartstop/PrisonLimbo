using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Godot;
using PrisonLimbo.Scripts.Extensions;

namespace PrisonLimbo.Scripts
{
    public readonly struct Vector2I : IEquatable<Vector2I>
    {
        public static Vector2I Up = new Vector2I(0, 1);
        public static Vector2I Right = new Vector2I(1, 0);
        public static Vector2I Down = new Vector2I(0, -1);
        public static Vector2I Left = new Vector2I(-1, 0);

        [Pure]
        public bool Equals(Vector2I other)
        {
            return X == other.X && Y == other.Y;
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong DistanceSquaredUL(Vector2I other) => (X - other.X).SquaredUl() + (Y - other.Y).SquaredUl();

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double DistanceReal(Vector2I other) => Math.Sqrt(DistanceSquaredUL(other));

        [Pure]
        public override bool Equals(object? obj)
        {
            return obj is Vector2I other && Equals(other);
        }

        [Pure]
        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        [Pure]
        public static bool operator ==(Vector2I left, Vector2I right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(Vector2I left, Vector2I right)
        {
            return !left.Equals(right);
        }

        [Pure]
        public static Vector2I operator +(Vector2I left, Vector2I right)
        {
            return new Vector2I(left.X + right.X, left.Y + right.Y);
        }

        [Pure]
        public static Vector2I operator -(Vector2I left, Vector2I right)
        {
            return new Vector2I(left.X - right.X, left.Y - right.Y);
        }

        [Pure]
        public IEnumerable<Vector2I> Adjacent(int width, int height)
        {
            return Adjacent(width, height, X, Y);
        }

        [Pure]
         public IEnumerable<(Direction, Vector2I)> AdjacentDirections(int width, int height)
        {
            return AdjacentDirections(width, height, X, Y);
        }

         [Pure]
         public IEnumerable<Vector2I> AdjacentUnbound() => AdjacentUnbound(X, Y);
         
         [Pure]
         public static IEnumerable<Vector2I> AdjacentUnbound(int x, int y)
         {
             yield return new Vector2I(x + 1, y);
             yield return new Vector2I(x - 1, y);
             yield return new Vector2I(x, y + 1);
             yield return new Vector2I(x, y - 1);
         }

         [Pure]
         public IEnumerable<(Direction, Vector2I)> AdjacentDirectionsUnbound() => AdjacentDirectionsUnbound(X, Y);
         
         [Pure]
         public static IEnumerable<(Direction, Vector2I)> AdjacentDirectionsUnbound(int x, int y){
             yield return (Direction.Right, new Vector2I(x + 1, y));
             yield return (Direction.Left, new Vector2I(x - 1, y));
             yield return (Direction.Down, new Vector2I(x, y + 1));
             yield return (Direction.Up, new Vector2I(x, y - 1));
         }

        [Pure]
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

        [Pure]
        public static IEnumerable<(Direction, Vector2I)> AdjacentDirections(int width, int height, int x, int y){
            if (x < width - 1)
                yield return (Direction.Right, new Vector2I(x + 1, y));
            if (x > 0)
                yield return (Direction.Left, new Vector2I(x - 1, y));
            if (y < height - 1)
                yield return (Direction.Down, new Vector2I(x, y + 1));
            if (y > 0)
                yield return (Direction.Up, new Vector2I(x, y - 1));
        }

        [Pure]
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