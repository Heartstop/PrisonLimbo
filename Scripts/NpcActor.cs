using System;
using Godot;

namespace PrisonLimbo.Scripts
{
    public abstract class NpcActor : Actor
    {
        protected readonly Random _random = new Random();
        
        protected void AnimateMove(AnimationState animationState, Vector2 newPosition, Action? postAnimation = null)
        {
            var mark = new NpcMoveMark(World, this, newPosition);
            World.AddChild(mark);
            _animationController.PlayAnimation(animationState, () => {
                MapPosition = newPosition;
                mark.QueueFree();
                postAnimation?.Invoke();
            });
        }
    }
}
