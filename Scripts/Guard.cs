using Godot;
using System;
namespace PrisonLimbo.Scripts
{
    public class Guard : NpcActor
    {
        private ActorAnimationController _animationController;
        private Random _random = new Random();

        private BehaviourState _behaviourState = BehaviourState.Strolling;
        public override void _Ready()
        {
            base._Ready();
            GD.Print(GetParent().GetType().ToString());
            _animationController = GetNode<ActorAnimationController>("Pivot/ActorAnimationController");
        }

        public override void TakeTurn()
        {   
            switch(_behaviourState){
                case BehaviourState.Strolling: {
                    var randomDirection = (Direction)_random.Next(0,5);
                    if(randomDirection == Direction.None && !World.CanMove(this, randomDirection))
                        return;
                    _animationController.PlayAnimation(randomDirection.ToAnimationState(), () => {
                        MapPosition = MapPosition + randomDirection.ToVector2();
                    });
                    break;   
                }
                default: break;
            }
            
        }

        public override bool TurnProcess()
        {
            return false;
        }


    }
}