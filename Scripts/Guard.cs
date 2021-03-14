using System;
using System.Collections.Generic;
using Godot;
using PrisonLimbo.Scripts.Extensions;

namespace PrisonLimbo.Scripts
{
    public class Guard : NpcActor
    {
        private BehaviourState _behaviourState = BehaviourState.Strolling;
        private readonly PackedScene _actorAnimationControllerInstancer = GD.Load<PackedScene>("Scenes/Characters/ActorAnimationController.tscn");

        private Queue<Direction> _strollPath;

        public override void TakeTurn()
        {
            if (_behaviourState != BehaviourState.Strolling)
                _strollPath = null;
            
            switch(_behaviourState){
                case BehaviourState.None:
                    break;
                case BehaviourState.Strolling: {
                    if (_strollPath == null || _strollPath.Count == 0)
                        _strollPath = GetStroll().ToQueue();

                    var stepDir = _strollPath.Dequeue();
                    var steppedSuccess = Step(stepDir);
                    if (!steppedSuccess)
                        _strollPath = null;
                    break;
                }
                case BehaviourState.Attack:
                    throw new NotImplementedException();
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
            deathAnimation.FlipH = AnimationController.FlipH;
            deathAnimation.PlayAnimation(AnimationState.Death, () => deathAnimation.QueueFree());
        }
    }
}
