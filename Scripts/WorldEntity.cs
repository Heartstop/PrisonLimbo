using Godot;

namespace PrisonLimbo.Scripts
{
    public abstract class WorldEntity : Node2D
    {
        protected World World = null!;
        private Vector2 _mapPosition;

        public Vector2 MapPosition
        {
            get => _mapPosition;
            set
            {
                _mapPosition = value;
                Position = World.MapToWorld(value);
            }
        }

        public abstract bool CanEnter<T>(T entity) where T : WorldEntity;

        public override void _Ready()
        {
            base._Ready();
            World = GetParent<World>();
        }
    }
}