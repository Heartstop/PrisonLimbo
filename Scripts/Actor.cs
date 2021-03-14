using Godot;
namespace PrisonLimbo.Scripts
{
    public abstract class Actor : WorldEntity
    {
        protected ActorAnimationController AnimationController;
        [Export]
        public float Health { get; private set; } = 100;
        [Export]
        public float Damage { get; set; } = 50;
        public override bool CanEnter<T>(T entity)
        {
            return !(entity is Actor);
        }

        public override void _Ready()
        {
            base._Ready();
            AnimationController = GetNode<ActorAnimationController>("Pivot/ActorAnimationController");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
        }

        public void ApplyDamage(float damage){
            Health -= damage;
            if(Health <= 0)
            {
                Die();
                QueueFree();
            }
        }

        public abstract void Die();
        
        public abstract void TakeTurn();
        public abstract bool TurnProcess();

    }
}
