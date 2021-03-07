using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Godot;
using PrisonLimbo.Scripts.Singletons;

namespace PrisonLimbo.Scripts
{
    public sealed class Player : Actor
    {
        public override void _Process(float delta)
        {
            base._Process(delta);

        }

        private void ProcessMove()
        {
            var dir = InputSystem.Direction;
        }
    }
}