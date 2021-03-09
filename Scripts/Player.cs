using PrisonLimbo.Scripts.Singletons;

namespace PrisonLimbo.Scripts
{
    public sealed class Player : Actor
    {
        private ActorAnimationController _animationController = null!;
        private bool _canMove = true;

        public override void _Ready()
        {
            base._Ready();
            _animationController = GetNode<ActorAnimationController>("Pivot/ActorAnimationController");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            ProcessMove();
        }

        private void ProcessMove()
        {
            var dir = InputSystem.Direction;
            var newPos = MapPosition + dir.ToVector2();
            if (dir == Direction.None || !_canMove)
                return;
            
            _canMove = false;
            _animationController.PlayAnimation(dir.ToAnimationState(), () => {
                MapPosition = newPos;
                _canMove = true;
                });
        }
    }
}