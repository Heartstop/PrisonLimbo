using Godot;

namespace PrisonLimbo.Scripts
{
    public abstract class WorldEntity : Node2D
    {
        #pragma warning disable CS0649
        [Export]
        private int _initialMapPositionX;
        [Export]
        private int _initialMapPositionY;
        #pragma warning restore CS0649
        
        private Vector2I _mapPosition;
        protected World World { get; set; }

        public Vector2I MapPosition
        {
            get => _mapPosition;
            set
            {
                _mapPosition = value;
                ZIndex = _mapPosition.Y;
                Position = World.MapToWorld(value);
            }
        }

        public abstract bool CanEnter<T>(T entity) where T : WorldEntity;

        public override void _Ready()
        {
            base._Ready();
            ZAsRelative = false;
            World = GetParent<World>();

            if(_initialMapPositionX != default || _initialMapPositionY != default){
                MapPosition = new Vector2I(_initialMapPositionX, _initialMapPositionY);
            }
        }
    }
}