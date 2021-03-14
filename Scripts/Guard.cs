using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Godot;
using PrisonLimbo.Scripts.Extensions;

namespace PrisonLimbo.Scripts
{
    public class Guard : NpcActor
    {
        private BehaviourState _behaviourState = BehaviourState.Strolling;
        private readonly PackedScene _actorAnimationControllerInstancer = GD.Load<PackedScene>("Scenes/Characters/ActorAnimationController.tscn");
        private const int AttackSight = 10;

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
                    var player = World.GetNode<Player>("Player");

                    var stabDirection = player.MapPosition
                        .AdjacentDirectionsUnbound()
                        .Where(pos => pos.Item2 == MapPosition)
                        .Select(d => d.Item1.Invert())
                        .FirstOrDefault();

                    if (stabDirection != default)
                    {
                        if(Health > 0) {
                            SoundSystem.PlayStabSound();
                            AnimationController.PlayAnimation(stabDirection.ToAnimationState(AnimationAction.Stab), () => {
                                player.ApplyDamage(Damage);
                            });
                        }
                        break;
                    }

                    var path = player
                        .MapPosition
                        .AdjacentUnbound()
                        .Shuffle(RandomSource)
                        .OrderBy(v => v.DistanceStepsL(MapPosition))
                        .Select(Path)
                        .FirstOrDefault(p => p != null)?
                        .ToImmutableArray();
                    
                    if(!(path is { } chosenPath))
                        break;

                    if (chosenPath.Length > AttackSight)
                    {
                        _behaviourState = BehaviourState.Strolling;
                        break;
                    }
                    
                    Step(chosenPath.First());

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Alert() => _behaviourState = BehaviourState.Attack;

        public override bool TurnProcess()
        {
            return false;
        }

        protected override float DamageModify(float damage)
        {
            return _behaviourState == BehaviourState.Strolling
                ? base.DamageModify(damage * 3)
                : base.DamageModify(damage);
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
