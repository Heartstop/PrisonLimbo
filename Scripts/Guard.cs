using System;
using Godot;

namespace PrisonLimbo.Scripts
{
    public class Guard : NpcActor
    {
        private BehaviourState _behaviourState = BehaviourState.Strolling;

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


    }
}
