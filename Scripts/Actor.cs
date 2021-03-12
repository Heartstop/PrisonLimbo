namespace PrisonLimbo.Scripts
{
    public abstract class Actor : WorldEntity
    {
        public override bool CanEnter<T>(T entity)
        {
            return !(entity is Actor);
        }

        public override void _Ready()
        {
            base._Ready();
            ZAsRelative = false;
            ZIndex = (int) Position.y;
        }
        
        public abstract void TakeTurn();
        public abstract bool TurnProcess();

    }
}