using Godot;
using System;
using PrisonLimbo.Scripts.WorldGenerator;
namespace PrisonLimbo.Scripts {
public class GameController : Node
{
        private World _world;
        private Spawner _spawner;
        private ActorTurnController _actorTurnController;
        private Random _random = new Random();
        private PackedScene _guardInstancer = GD.Load<PackedScene>("res://Scenes/Characters/Guard.tscn");
        private PackedScene _playerInstancer = GD.Load<PackedScene>("res://Scenes/Characters/Player.tscn");
        private int _roomLevel = 2;
        public override void _Ready()
        {
            _actorTurnController = GetNode<ActorTurnController>("ActorTurnController");
            GenerateWorld();
        }

        private void GenerateWorld(){
            var size = GenerateRoomSize();
            var roomCells = new RoomMaker(_random, 4, (rand, roomW, roomH) => true).GenerateRooms(size.X, size.Y);
            _world = new World(_random, roomCells);
            _spawner = new Spawner(_random, _world);
            AddChild(_world);
            _actorTurnController.World = _world;


            var player = (Player)_playerInstancer.Instance();
            var playerSpawn = _spawner.FindSpawn(player, Vector2I.Zero, size);
            if(playerSpawn is Vector2I){
                _world.AddChild(player);
                player.MapPosition = (Vector2)playerSpawn;
                _actorTurnController.Player = player;
            }
        
            for(var i = 0; i < GenerateAmountOfGuards();i++){
                var guard = (Guard)_guardInstancer.Instance();
                var guardSpawn = _spawner.FindSpawn(guard, Vector2I.Zero, size);
                if(guardSpawn is Vector2I){
                    _world.AddChild(guard);
                    guard.MapPosition = (Vector2)guardSpawn;
                }
            };
        }
        private Vector2I GenerateRoomSize() => new Vector2I((int)Math.Round(6+(_roomLevel * 2.6)),(int)Math.Round(6+(_roomLevel * 2.6)));
        

        private int GenerateAmountOfGuards() => (int)Math.Ceiling((_roomLevel * 0.6) + 0.8);
    }
}