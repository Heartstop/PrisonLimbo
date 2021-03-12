using Godot;
using System;
using System.Linq;

namespace PrisonLimbo.Scripts
{
    public class ActorTurnController : Node
    {
        private Boolean _isPlayerTurn = true;
        private Player _player;
        private Timer _turnTimer;
        private Timer _turnDelay;
        private ProgressBar _timeBar;
        public override void _Ready()
        {
            _player = GetNode<Player>("../Player");
            _turnTimer = GetNode<Timer>("TurnTimer");
            _turnDelay = GetNode<Timer>("TurnDelay");
            _timeBar = GetNode<ProgressBar>("Timebar");


            _timeBar.MaxValue = _turnTimer.WaitTime;
            _turnTimer.Start();
        }

        public override void _Process(float delta){
            if(_isPlayerTurn) {
                                _timeBar.Value = _turnTimer.TimeLeft;
                _player.TurnProcess();
            }
        }

        private void RunNpcTurnProcess() {
            var npcActors = GetParent().GetChildren().OfType<NpcActor>().ToList();
            npcActors.ForEach((actor) => actor.TurnProcess());
        }

        private void OnTurnTimerTimeout() {
            _isPlayerTurn = false;
            _turnDelay.Start();
            RunNpcTurnProcess();
        }

        private void OnTurnDelayTimeout() {
            _isPlayerTurn = true;
            _turnTimer.Start();
        }

    }

}