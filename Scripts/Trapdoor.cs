using Godot;
using System;

namespace PrisonLimbo.Scripts
{
    public class Trapdoor : WorldEntity
    {
        private AnimationPlayer _animationPlayer;
        public override void _Ready()
        {
            base._Ready();
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }

        public override bool CanEnter<T>(T entity) => !(entity is NpcActor);
    }
}