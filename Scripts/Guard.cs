using System;
using System.Collections.Generic;
using PrisonLimbo.Scripts.Extensions;

namespace PrisonLimbo.Scripts
{
    public class Guard : NpcActor
    {
        private BehaviourState _behaviourState = BehaviourState.Strolling;

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


    }
}
