using System;
using System.Text;
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
            PrintMap(structure);
            throw new NotImplementedException();
        }

        public static void PrintMap(RoomCellAbstract[,] structure)
        {
            var text = new StringBuilder();
            for (var x = 0; x < structure.GetLength(0); x++)
            {
                for (var y = 0; y < structure.GetLength(1); y++)
                {
                    var letter = structure[x, y] switch
                    {
                        RoomCellAbstract.Empty => 'E',
                        RoomCellAbstract.Wall => 'W',
                        RoomCellAbstract.Door => 'D',
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    text.Append(letter);
                }
                GD.Print(text.ToString());
                text.Clear();
            }
        }
    }
}