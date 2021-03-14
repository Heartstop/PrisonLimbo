using System;
using Godot;

namespace PrisonLimbo.Scripts
{
    public class Guard : NpcActor
    {
        private BehaviourState _behaviourState = BehaviourState.Strolling;
        private PackedScene _actorAnimationControllerInstancer = GD.Load<PackedScene>("Scenes/Characters/ActorAnimationController.tscn");

        public override void TakeTurn()
        {   
            switch(_behaviourState){
                case BehaviourState.Strolling: {
                    var randomDirection = (Direction)_random.Next(0,5);
                    if(randomDirection == Direction.None || !World.CanMove(this, randomDirection))
                        return;
                    AnimateMove(randomDirection.ToAnimationState(),MapPosition + randomDirection.ToVector2());
                    break;
                }
                case BehaviourState.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public override bool TurnProcess()
        {
            return false;
        }

        public override void Die()
        {
            var deathAnimation = (ActorAnimationController)_actorAnimationControllerInstancer.Instance();
            var pivot = new Position2D();

            pivot.AddChild(deathAnimation);
            World.AddChild(pivot);

            pivot.GlobalPosition = GlobalPosition + new Vector2(8,16);
            deathAnimation.Animation = "guard";
            deathAnimation.FlipH = _animationController.FlipH;
            deathAnimation.PlayAnimation(AnimationState.Death, () => deathAnimation.QueueFree());
        }
    }
}
