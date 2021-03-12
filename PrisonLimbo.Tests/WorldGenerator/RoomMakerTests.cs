using System;
using System.Text;
using PrisonLimbo.Scripts.Extensions;
using PrisonLimbo.Scripts.WorldGenerator;
using Xunit;
using Xunit.Abstractions;

namespace PrisonLimbo.Tests.WorldGenerator
{
    public sealed class RoomMakerTests
    {
        private readonly ITestOutputHelper _output;

        public RoomMakerTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Theory]
        [InlineData(10, 10, 3)]
        [InlineData(10, 10, 1)]
        [InlineData(3, 3, 1)]
        [InlineData(25, 25, 1)]
        [InlineData(100, 100, 5)]
        [InlineData(10, 10, 9)]
        [InlineData(100, 100, 50)]
        [InlineData(500, 500, 5)]
        public void GenerateRooms_Anything_Anything(int width, int height, int minRoom)
        {
            // Given
            var random = new Random(1);
            var roomMaker = new RoomMaker(random, minRoom, Subdivide);

            var rooms = roomMaker.GenerateRooms(width, height);
            PrintMap(rooms);
        }

        private bool Subdivide(Random random, int width, int height) => (long)width * height > 100_000 || random.NextBool(0.75d);

        private void PrintMap(RoomCellAbstract[,] structure)
        {
            var text = new StringBuilder();
            for (var y = structure.GetLength(1) - 1; y >= 0; y--)
            {
                for (var x = 0; x < structure.GetLength(0); x++)
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
                
                _output.WriteLine(text.ToString());
                text.Clear();
            }
        }
    }
}