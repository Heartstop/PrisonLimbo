using PrisonLimbo.Scripts.Singletons;
using System.Linq;
using System;

namespace PrisonLimbo.Scripts
{
    public class Player : Actor
    {
        private bool _canMove = true;
        private bool _passTurn = false;
        public Action OnEnterTrapdoor {get; set;} = null;
        public override void TakeTurn()
        {
            _passTurn = false;
        }

        public override bool TurnProcess()
        {
            ProcessMove();
            return _passTurn;
        }

        public override void Die()
        {
            throw new System.NotImplementedException();
        }

        private void ProcessMove()
        {
            var dir = InputSystem.Direction;
            var newPos = MapPosition + dir.ToVector2I();
            if (dir == Direction.None || !_canMove || _passTurn)
                return;
            
            var entities = World.GetEntities(newPos);
            var npc = (NpcActor?)entities.SingleOrDefault((entity) => entity is NpcActor);
            var trapdoor = (Trapdoor?)entities.SingleOrDefault((entity) => entity is Trapdoor);
            _canMove = false;

            if(npc != null){
                AnimationController.PlayAnimation(dir.ToAnimationState(AnimationAction.Stab), () => {
                    npc.ApplyDamage(Damage);
                    PassTurn();
                });
            } else if(trapdoor != null){
                OnEnterTrapdoor!.Invoke();
            } else if(World.CanMove(this, newPos)) {
                AnimationController.PlayAnimation(dir.ToAnimationState(), () => {
                    MapPosition = newPos;
                    PassTurn();
                    });
            }
            else
            {
                _canMove = true;
            }
        }

        private void PassTurn() {
            _canMove = true;
            _passTurn = true;
        }
    }
}
