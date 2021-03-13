using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Immutable;
using Godot;
using PrisonLimbo.Scripts.WorldGenerator;
namespace PrisonLimbo.Scripts
{
    public class World : Node
    {

        private TileMap _tileMap;
        private Random _random;
        private static readonly IImmutableSet<Tiles> tilesWithoutCollision = ImmutableHashSet.Create(Tiles.Floor, Tiles.FloorShadow);

        public World(Random random, RoomCellAbstract[,] roomCells){
            _random = random;
            _tileMap = new Decorator(_random).Decorate(roomCells);
        }
        
        public override void _Ready()
        {
            var tileSet = (TileSet)GD.Load("res://Assets/tileset.tres");
            _tileMap.TileSet = tileSet;
            _tileMap.CellSize = new Vector2(16,16);
            _tileMap.ZIndex = -100;
            _tileMap.UpdateBitmaskRegion();
            AddChild(_tileMap);
        }

        public bool CanMove<T>(T requestingEntity, Direction direction) where T : WorldEntity
        {
            var newPos = requestingEntity.MapPosition + direction.ToVector2();
            return CanMove(requestingEntity, newPos);
        }

        public bool CanMove<T>(T requestingEntity, Vector2 target) where T : WorldEntity
        {
            var cell = _tileMap.GetCellv(target);
            if (!tilesWithoutCollision.Contains((Tiles) cell))
                return false;
            
            return GetEntities(target)
                .All(we => we.CanEnter(requestingEntity));
        }

        public Vector2 MapToWorld(Vector2 mapPosistion) => mapPosistion * 16;            

        public IEnumerable<WorldEntity> GetEntities(Vector2 position)
        {
            return GetChildren()
                .OfType<WorldEntity>()
                .Where(we => we.MapPosition == position);
        }
    }
}
