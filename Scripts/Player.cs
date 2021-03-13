using PrisonLimbo.Scripts.Singletons;
using System.Linq;

namespace PrisonLimbo.Scripts
{
    public class Player : Actor
    {
        private bool _canMove = true;
        private bool _passTurn = false;

        public override void TakeTurn()
        {
            _passTurn = false;
        }

        public override bool TurnProcess()
        {
            ProcessMove();
            return _passTurn;
        }

        private void ProcessMove()
        {
            var dir = InputSystem.Direction;
            var newPos = MapPosition + dir.ToVector2();
            if (dir == Direction.None || !_canMove || _passTurn)
                return;
            
            var npc = (NpcActor?)World.GetEntities(newPos).SingleOrDefault((entity) => entity is NpcActor);
            _canMove = false;

            if(npc != null){
                _animationController.PlayAnimation(dir.ToAnimationState(AnimationAction.Stab), () => {
                    npc.Health -= Damage;
                    PassTurn();
                });
            } else {
                _animationController.PlayAnimation(dir.ToAnimationState(), () => {
                    MapPosition = newPos;
                    PassTurn();
                    });
            }

        }

        private void PassTurn() {
            _canMove = true;
            _passTurn = true;
        }
    }
}
