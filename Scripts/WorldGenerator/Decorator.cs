using System;
using Godot;

namespace PrisonLimbo.Scripts.WorldGenerator
{
    /// <summary>
    ///     Decorates rooms with objects
    /// </summary>
    public class Decorator
    {
        private readonly Random _random;

        public Decorator(Random random)
        {
            _random = random;
        }

        public TileMap Decorate(RoomCellAbstract[,] structure)
        {
            throw new NotImplementedException();
        }
    }
}