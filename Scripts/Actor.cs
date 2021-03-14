using Godot;
namespace PrisonLimbo.Scripts
{
    public abstract class Actor : WorldEntity
    {
        protected ActorAnimationController _animationController;
        [Export]
        public float Health { get; set; } = 100;
        [Export]
        public float Damage { get; set; } = 50;
        public override bool CanEnter<T>(T entity)
        {
            return !(entity is Actor);
        }

        public override void _Ready()
        {
            base._Ready();
            _animationController = GetNode<ActorAnimationController>("Pivot/ActorAnimationController");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            if(Health <= 0){
                QueueFree();
            }
        }

        
        public abstract void TakeTurn();
        public abstract bool TurnProcess();

    }
}
