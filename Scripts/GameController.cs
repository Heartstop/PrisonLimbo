using Godot;
using System;
using System.Threading;
using PrisonLimbo.Scripts.WorldGenerator;
using PrisonLimbo.Scripts.Extensions;

namespace PrisonLimbo.Scripts {
public class GameController : Node
{
        private World _world;
        private Spawner _spawner;
        private ActorTurnController _actorTurnController;
        private SceneTransition _sceneTransition;
        private AudioStreamPlayer _winSoundPlayer;
        private Label _levelLabel;
        private Label _tutorialLabel;
        private readonly Random _random = new Random();
        private readonly PackedScene _guardInstancer = GD.Load<PackedScene>("res://Scenes/Characters/Guard.tscn");
        private readonly PackedScene _playerInstancer = GD.Load<PackedScene>("res://Scenes/Characters/Player.tscn");
        private readonly PackedScene _keyInstancer = GD.Load<PackedScene>("res://Scenes/Key.tscn");
        private int _roomLevel = 1;
        public override void _Ready()
        {
            _levelLabel = GetNode<Label>("GUILayer/TopContainer/HBoxContainer/Level");
            _tutorialLabel = GetNode<Label>("GUILayer/TutorialLabel");
            _actorTurnController = GetNode<ActorTurnController>("ActorTurnController");
            _sceneTransition = GetNode<SceneTransition>("GUILayer/SceneTransition");

            _winSoundPlayer = GetNode<AudioStreamPlayer>("WinSoundPlayer");

            GenerateWorld();
        }

        private void GenerateWorld(){
            _levelLabel.Text = _roomLevel.ToString(); 

            var size = GenerateRoomSize();
            var roomCells = new RoomMaker(_random, 3, (rand, roomW, roomH) => roomW > 10 || roomH > 10 || _roomLevel <= 3 || _random.NextBool(0.75)).GenerateRooms(size.X, size.Y);
            _world = new World(_random, roomCells);
            _spawner = new Spawner(_random, _world);
            AddChild(_world);
            _actorTurnController.World = _world;


            var player = (Player)_playerInstancer.Instance();

            var tutorialText = TutorialText();
            if (tutorialText != null)
            {
                _tutorialLabel.Text = tutorialText;
            } else if(_roomLevel > 1){
                _tutorialLabel.Visible = false;
            }
            
            player.OnEnterTrapdoor = OnPlayerEnterTrapdoor;
            player.OnDeath = OnPlayerDeath;
            var playerSpawn = _spawner.FindSpawn(player, Vector2I.Zero, size);
            if(playerSpawn is Vector2I ps){
                _world.AddChild(player);
                player.MapPosition = ps;
                _actorTurnController.Player = player;
            }
        
            for(var i = 0; i < GenerateAmountOfGuards();i++){
                var guard = (Guard)_guardInstancer.Instance();
                if (i == 0)
                {
                    var key = (Key) _keyInstancer.Instance();
                    key.World = _world;
                    key.Track = new Lazy<Node2D>(() => guard.AnimationController, LazyThreadSafetyMode.None);
                    guard.AddChild(key);
                }
                var guardSpawn = _spawner.FindSpawn(guard, Vector2I.Zero, size);
                if(guardSpawn is Vector2I gs){
                    _world.AddChild(guard);
                    guard.MapPosition = gs;
                } else if (i == 0)
                {
                    throw new InvalidOperationException("We can't even fit a single guard...");
                }
            }

            _sceneTransition.FadeOut();
        }

        private string? TutorialText()
        {
            return _roomLevel switch
            {
                1 => null,
                2 => "Stabbing an unsuspecting victim kills them instantly, others need to be stabbed twice.",
                3 => "The timer at the bottom repressents how much time you have to make a decicion before your turn passes. You can pass the turn yourself by pressing space.",
                4 => "Guards who witness a murder will attack, and guards who survive attacks will yell for help.",
                _ => null
            };
        }

        private void OnPlayerEnterTrapdoor() {
            _winSoundPlayer.Play();
            _sceneTransition.FadeIn(() => {
                _roomLevel += 1;
                _world.QueueFree();
                GenerateWorld();
            });
        }

        private void OnPlayerDeath() {
            SoundSystem.PlayDieSound();
            GetTree().ChangeScene("Scenes/Menu.tscn");
        }

        private Vector2I GenerateRoomSize() => new Vector2I((int)Math.Round(8+(_roomLevel * 0.8)),(int)Math.Round(8+(_roomLevel * 0.8)));
        

        private int GenerateAmountOfGuards() => (int)Math.Round(_roomLevel*_roomLevel*0.09)+1;
    }
}