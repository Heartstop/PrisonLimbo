using System;
using System.Collections.Immutable;
using Godot;

namespace PrisonLimbo.Scripts
{
    public class Spawner
    {
        private readonly Random _random;
        private readonly World _world;

        private readonly ImmutableArray<Vector2I> _searchPath;

        private long _searchStep = 0;

        public Spawner(Random random, World world)
        {
            _random = random;
            _world = world;
        }
        
        public Vector2I? FindSpawn(WorldEntity entity, Vector2I start, Vector2I end)
        {
            if (start.X > end.X)
                throw new ArgumentOutOfRangeException();
            if (start.Y > end.Y)
                throw new ArgumentOutOfRangeException();
            
            for (var tries = 0; tries < 1000; tries++)
            {
                var pos = new Vector2I(_random.Next(start.X, end.X), _random.Next(start.Y, end.Y));
                if (Spawnable(entity, pos))
                    return pos;
            }

            for (var x = start.X; x < end.X; x++)
            for (var y = start.Y; y < end.Y; y++)
            {
                var pos = new Vector2I(x, y);
                if (Spawnable(entity, pos))
                    return pos;
            }

            return default;
        }

        private bool Spawnable(WorldEntity entity, Vector2I position) => _world.CanMove(entity, (Vector2) position);
    }
}