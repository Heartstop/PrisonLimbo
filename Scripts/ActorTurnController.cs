using Godot;
using System;
using System.Linq;

namespace PrisonLimbo.Scripts
{
    public class ActorTurnController : Node
    {
        public Player Player { get; set; }

        public World World { get; set; }
        private Boolean _isPlayerTurn = true;
        private Timer _turnTimer;
        private Timer _turnDelay;
        private ProgressBar _timeBar;
        public override void _Ready()
        {
            _turnTimer = GetNode<Timer>("TurnTimer");
            _turnDelay = GetNode<Timer>("TurnDelay");
            _timeBar = GetNode<ProgressBar>("../GUILayer/VBox/Timebar");


            _timeBar.MaxValue = _turnTimer.WaitTime;
            _turnTimer.Start();
        }

        public override void _Process(float delta){
            if(_isPlayerTurn) {
                _timeBar.Value = _turnTimer.TimeLeft;
                if(Player.TurnProcess()){
                    _turnTimer.Stop();
                    _timeBar.Value = 0;
                    OnTurnTimerTimeout();
                };
            }
        }

        private void RunNpcTurnProcess() {
            var npcActors = World.GetChildren().OfType<NpcActor>().ToList();
            npcActors.ForEach((actor) => actor.TakeTurn());
        }

        private void OnTurnTimerTimeout() {
            _isPlayerTurn = false;
            _turnDelay.Start();
            RunNpcTurnProcess();
        }

        private void OnTurnDelayTimeout() {
            _isPlayerTurn = true;
            Player.TakeTurn();
            _turnTimer.Start();
        }

    }

}