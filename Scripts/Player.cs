using PrisonLimbo.Scripts.Singletons;

namespace PrisonLimbo.Scripts
{
    public class Player : Actor
    {
        private ActorAnimationController _animationController = null!;
        private bool _canMove = true;
        private bool _passTurn = false;

        public override void _Ready()
        {
            base._Ready();
            _animationController = GetNode<ActorAnimationController>("Pivot/ActorAnimationController");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
        }

        public override void TakeTurn()
        {
            _passTurn = false;
        }

        public override bool TurnProcess()
        {
            ProcessMove();
            return _passTurn;
        }

        private void ProcessMove()
        {
            var dir = InputSystem.Direction;
            var newPos = MapPosition + dir.ToVector2();
            if (dir == Direction.None || !_canMove || _passTurn)
                return;
            
            _canMove = false;
            _animationController.PlayAnimation(dir.ToAnimationState(), () => {
                MapPosition = newPos;
                _canMove = true;
                _passTurn = true;
                });
        }
    }
}