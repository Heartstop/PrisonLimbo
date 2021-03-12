using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using PrisonLimbo.Scripts.Extensions;

namespace PrisonLimbo.Scripts.WorldGenerator
{
    /// <summary>
    ///     Makes simple room structures where all rooms are accessible.
    /// </summary>
    public class RoomMaker
    {
        private readonly int _minimalRoomDimension;
        private readonly Func<Random, int, int, bool> _subdivide;

        private readonly Random _random;

        public RoomMaker(Random random, int minimalRoomDimension, Func<Random, int, int, bool> subdivide)
        {
            _random = random;
            if (minimalRoomDimension < 1)
                throw new ArgumentOutOfRangeException(nameof(minimalRoomDimension), minimalRoomDimension, null);
            _minimalRoomDimension = minimalRoomDimension;
            _subdivide = subdivide;
        }

        public RoomCellAbstract[,] GenerateRooms(int width, int height)
        {
            if (width < 1)
                throw new ArgumentOutOfRangeException(nameof(width), width, null);
            if (height < 1)
                throw new ArgumentOutOfRangeException(nameof(height), height, null);

            var outerWalls = OuterWalls(width, height);
            var innerWalls = GenerateWalls(width - 2, height - 2, 1, 1);
            var walls = outerWalls.Concat(innerWalls);
            var area = new RoomCellAbstract[width, height];
            foreach (var wall in walls)
            {
                int end;
                switch (wall.Axis)
                {
                    case Axis.Horizontal:
                        end = wall.XPos + wall.Length;
                        for (var x = wall.XPos; x < end; x++)
                            area[x, wall.YPos] = RoomCellAbstract.Wall;
                        break;
                    case Axis.Vertical:
                        end = wall.YPos + wall.Length;
                        for(var y = wall.YPos; y < end; y++)
                            area[wall.XPos, y] = RoomCellAbstract.Wall;
                        break;
                    case Axis.None:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var rooms = GetRooms(area).ToImmutableArray();
            var doors = GetDoors(rooms);
            foreach (var door in doors)
                area[door.X, door.Y] = RoomCellAbstract.Door;

            return area;
        }

        private IEnumerable<Vector2I> GetDoors(IImmutableList<Room> rooms)
        {
            var possible = rooms.ToImmutableDictionary(
                r => r,
                r => 
                rooms
                    .Select(ir => new KeyValuePair<Room,IImmutableSet<Vector2I>>(ir, r.Walls.Intersect(ir.Walls)))
                    .Where(t => t.Value.Count > 0)
                    .ToImmutableDictionary()
            );
            // This route could be used for actual gameplay as well.
            var route = rooms.Shuffle(_random).ToImmutableArray();
            var resultingPaths = new List<ImmutableList<ImmutableList<Room>>>();
            for (var i = 0; i < route.Length - 1; i++)
            {
                var start = route[i];
                var destination = route[i + 1];
                int? destinationFound = null;
                var bestValue = rooms.ToDictionary(r => r, _ => ImmutableList<ImmutableList<Room>>.Empty);
                var search = new Queue<ImmutableList<Room>>();
                search.Enqueue(ImmutableList.Create(start));
                while (search.Count > 0)
                {
                    var current = search.Dequeue();
                    var tail = current.Last();

                    if (destinationFound.HasValue && destinationFound.Value < current.Count)
                        continue;

                    if (tail == destination)
                        destinationFound = current.Count;

                    var previousValue = bestValue[tail];
                    var explored = previousValue.Count > 0;
                    if (explored && previousValue.First().Count < current.Count)
                        continue;

                    if (!explored || previousValue.First().Count >= current.Count)
                        bestValue[tail] = bestValue[tail]
                            .Where(r => r.Count == current.Count)
                            .Append(current)
                            .ToImmutableList();

                    foreach (var next in possible[tail].Keys.Select(r => current.Append(r).ToImmutableList()))
                        search.Enqueue(next);
                }


                resultingPaths.Add(bestValue[destination]);
            }
            
            // TODO Write algorithm to actually make the smallest amount of doors.
            var paths = resultingPaths
                .Where(p => p.Count > 0)
                .Select(a => a.Shuffle(_random).First());
            var openings = rooms.ToDictionary(room => room, room => new HashSet<Room>(possible[room].Count));
            var doors = new HashSet<Vector2I>();
            foreach (var path in paths)
            {
                for (var i = 0; i < path.Count - 1; i++)
                {
                    var start = path[i];
                    var next = path[i + 1];
                    if(openings[start].Contains(next))
                        continue;

                    openings[start].Add(next);
                    openings[next].Add(start);

                    var possibleDoors = possible[start][next];
                    var newDoor = possibleDoors.ToImmutableArray()[_random.Next(0, possibleDoors.Count)];
                    doors.Add(newDoor);
                }
            }

            return doors;
        }

        [Pure]
        private static IEnumerable<Room> GetRooms(RoomCellAbstract[,] cells)
        {
            int width = cells.GetLength(0),
                height = cells.GetLength(1);
            
            var floorRoom = new int[width, height];
            var rIndex = 0;
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                if (cells[x, y] != RoomCellAbstract.Empty || floorRoom[x, y] != default)
                    continue;

                var currentRoom = ++rIndex;

                var toExplore = new Stack<Vector2I>();
                toExplore.Push(new Vector2I(x, y));

                while (toExplore.Count > 0)
                {
                    var explore = toExplore.Pop();
                    if (floorRoom[explore.X, explore.Y] != default || cells[explore.X, explore.Y] != RoomCellAbstract.Empty)
                        continue;
                    floorRoom[explore.X, explore.Y] = currentRoom;

                    foreach (var item in explore.Adjacent(width, height))
                        toExplore.Push(item);
                }
            }

            var roomFloors = Enumerable.Range(1, rIndex).ToDictionary(i => i, _ => new HashSet<Vector2I>());
            
            for(var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                var room = floorRoom[x, y];
                if(room != default)
                    roomFloors[room].Add(new Vector2I(x, y));
            }


            var wallRooms = new Dictionary<Vector2I, ImmutableHashSet<int>>();

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                if (cells[x, y] != RoomCellAbstract.Wall)
                    continue;

                var pos = new Vector2I(x, y);
                foreach (var neighbour in Vector2I.Adjacent(width, height, x, y))
                {
                    var roomIndex = floorRoom[neighbour.X, neighbour.Y];
                    if (roomIndex == default)
                        continue;
                    if (wallRooms.ContainsKey(pos))
                        wallRooms[pos] = wallRooms[pos].Add(roomIndex);
                    else
                        wallRooms.Add(pos, ImmutableHashSet.Create(roomIndex));
                }
            }

            var roomWalls = wallRooms
                .SelectMany(kvp => kvp.Value.Select(r => (roomIndex: r, position: kvp.Key)))
                .ToLookup(t => t.roomIndex, t => t.position);

            return Enumerable.Range(1, rIndex).Select(i =>
                new Room(
                    roomFloors[i].ToImmutableHashSet(), 
                    roomWalls[i].ToImmutableHashSet()
                    )
            );
        }
        

        private IEnumerable<Wall> GenerateWalls(int width, int height, int xOffset, int yOffset)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(width), width, null);
            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height), height, null);

            if (!_subdivide(_random, width, height))
                return ImmutableArray<Wall>.Empty;

            bool vertical = width > _minimalRoomDimension * 2,
                horizontal = height > _minimalRoomDimension * 2;
            
            if (vertical && (!horizontal || _random.NextBool()))
            {
                var divider = _random.Next(_minimalRoomDimension, width - _minimalRoomDimension);
                var wall = new Wall(Axis.Vertical, divider + xOffset, yOffset, height);
                var left = GenerateWalls(divider, height, xOffset, yOffset);
                var right = GenerateWalls(width - divider - 1, height, wall.XPos + 1, wall.YPos);
                return left.Concat(right).Append(wall);
            }

            if (horizontal)
            {
                var divider = _random.Next(_minimalRoomDimension, height - _minimalRoomDimension);
                var wall = new Wall(Axis.Horizontal, xOffset, divider + yOffset, width);
                var bottom = GenerateWalls(width, divider, xOffset, yOffset);
                var top = GenerateWalls(width, height - divider - 1, wall.XPos, wall.YPos + 1);
                return bottom.Concat(top).Append(wall);
            }

            return ImmutableArray<Wall>.Empty;
        }

        private IEnumerable<Wall> OuterWalls(int width, int height)
        {
            yield return new Wall(Axis.Vertical, 0, 0, height);
            yield return new Wall(Axis.Vertical, width - 1, 0, height);
            yield return new Wall(Axis.Horizontal, 0, 0, width);
            yield return new Wall(Axis.Horizontal, 0, height - 1, width);
        }

        private enum Axis
        {
            None,
            Horizontal,
            Vertical
        }

        private readonly struct Wall
        {
            public Axis Axis { get; }
            public int XPos { get; }
            public int YPos { get; }
            public int Length { get; }

            public Wall(Axis axis, int xPos, int yPos, int length)
            {
                Axis = axis;
                XPos = xPos;
                YPos = yPos;
                Length = length;
            }
        }
        
        private sealed class Room : IEquatable<Room>
        {
            [Pure]
            public bool Equals(Room? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Floors.Equals(other.Floors) && Walls.Equals(other.Walls);
            }

            [Pure]
            public override bool Equals(object? obj)
            {
                return ReferenceEquals(this, obj) || obj is Room other && Equals(other);
            }

            [Pure]
            public override int GetHashCode()
            {
                unchecked
                {
                    return (Floors.GetHashCode() * 397) ^ Walls.GetHashCode();
                }
            }

            [Pure]
            public static bool operator ==(Room? left, Room? right)
            {
                return Equals(left, right);
            }

            [Pure]
            public static bool operator !=(Room? left, Room? right)
            {
                return !Equals(left, right);
            }

            public IImmutableSet<Vector2I> Floors { get; }
            public IImmutableSet<Vector2I> Walls { get; }

            public Room(IImmutableSet<Vector2I> floors, IImmutableSet<Vector2I> walls)
            {
                Floors = floors;
                Walls = walls;
            }
        }
    }
}