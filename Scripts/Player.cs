using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Godot;
using PrisonLimbo.Scripts.Singletons;

namespace PrisonLimbo.Scripts
{
    public sealed class Player : Actor
    {
        private ActorAnimationController _animationController = null!;

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
            if(dir != Direction.None) {
                _animationController.PlayAnimation(Direction.ToAnimationState());

            }
        }
    }
}
