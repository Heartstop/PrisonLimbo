using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            var tileMap = new TileMap();
            var width = structure.GetLength(0);
            var height = structure.GetLength(1);

            for(var x=0; x<width; x++){
                for(var y=0; y<height; y++){
                    var adjacent = Vector2I.AdjacentDirections(width, height, x, y).ToImmutableDictionary((t) => t.Item1, (t) => structure[t.Item2.X, t.Item2.Y]);
                    if(structure[x,y] == RoomCellAbstract.Empty || structure[x,y] == RoomCellAbstract.Door){
                        tileMap.SetCellv(new Vector2(x,y), (int)MapFloor(adjacent));
                    } else {
                        tileMap.SetCellv(new Vector2(x,y), (int)MapWall(adjacent));
                    }
                }
            }

            return tileMap;
        }

        private Tiles MapFloor(ImmutableDictionary<Direction, RoomCellAbstract> adjacent){
            if(adjacent.TryGetValue(Direction.Up, out var up) && up == RoomCellAbstract.Wall){
                return Tiles.FloorShadow;
            }
            return Tiles.Floor;
        }

        private Tiles MapWall(ImmutableDictionary<Direction, RoomCellAbstract> adjacent) {
            adjacent = adjacent.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value == RoomCellAbstract.Door ? RoomCellAbstract.Empty : kvp.Value);
            RoomCellAbstract
                up=adjacent.ContainsKey(Direction.Up)? adjacent[Direction.Up] : RoomCellAbstract.Empty, 
                right=adjacent.ContainsKey(Direction.Right)? adjacent[Direction.Right] : RoomCellAbstract.Empty, 
                down=adjacent.ContainsKey(Direction.Down)? adjacent[Direction.Down] : RoomCellAbstract.Empty, 
                left=adjacent.ContainsKey(Direction.Left)? adjacent[Direction.Left] : RoomCellAbstract.Empty;
    
            return up switch {
                RoomCellAbstract.Empty => right switch {
                    RoomCellAbstract.Empty => down switch {
                        RoomCellAbstract.Wall => left switch {
                            RoomCellAbstract.Wall => Tiles.SolidSingleLeft,
                            RoomCellAbstract.Empty => Tiles.SolidSingleStop,
                            _ => Tiles.Wall,
                        },
                        RoomCellAbstract.Empty => left switch {
                            RoomCellAbstract.Wall => Tiles.WallRightEnd,
                            _ => Tiles.Wall,
                        },
                        _ => Tiles.Wall,
                    },
                    RoomCellAbstract.Wall => down switch {
                        RoomCellAbstract.Empty => left switch {
                            RoomCellAbstract.Empty => Tiles.WallLeftEnd,
                            RoomCellAbstract.Wall => Tiles.WallContinous,
                            _ => Tiles.Wall,
                        },
                        RoomCellAbstract.Wall => left switch {
                            RoomCellAbstract.Wall => Tiles.SolidSingleTShape,
                            RoomCellAbstract.Empty => Tiles.SolidSingleRight,
                            _ => Tiles.Wall,
                        },
                        _ => Tiles.Wall,
                    },
                    _ => Tiles.Wall,
                },
                RoomCellAbstract.Wall => right switch {
                    RoomCellAbstract.Empty => down switch {
                        RoomCellAbstract.Empty => left switch {
                            RoomCellAbstract.Wall => Tiles.WallSingleLeft,
                            RoomCellAbstract.Empty => Tiles.WallSingleStop,
                            _ => Tiles.Wall,
                        },
                        RoomCellAbstract.Wall => left switch {
                            RoomCellAbstract.Wall => Tiles.SolidSingleContinousLeft,
                            RoomCellAbstract.Empty => Tiles.SolidContinous,
                            _ => Tiles.Wall,
                        },
                        _ => Tiles.Wall,
                    },
                    RoomCellAbstract.Wall => down switch {
                        RoomCellAbstract.Empty => left switch {
                            RoomCellAbstract.Empty => Tiles.WallSingleRight,
                            RoomCellAbstract.Wall => Tiles.WallSingleTShape,
                            _ => Tiles.Wall,
                        },
                        RoomCellAbstract.Wall => left switch {
                            RoomCellAbstract.Empty => Tiles.SolidSingleContinousRight,
                            RoomCellAbstract.Wall => Tiles.SolidSingleContinousTShape,
                            _ => Tiles.Wall,
                        },
                        _ => Tiles.Wall,
                    },
                    _ => Tiles.Wall,
                },
                _ => Tiles.Wall,
            };
        }
    }
}