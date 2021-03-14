using System;
using Godot;

namespace PrisonLimbo.Scripts
{
    public abstract class WorldEntity : Node2D
    {   
        [Export]
        private Vector2 _initialMapPosistion = default;
        private Vector2 _mapPosition;
        protected World World { get; set; }

        public Vector2 MapPosition
        {
            get => _mapPosition;
            set
            {
                _mapPosition = value;
                ZIndex = (int) _mapPosition.y;
                Position = World.MapToWorld(value);
            }
        }

        public abstract bool CanEnter<T>(T entity) where T : WorldEntity;

        public override void _Ready()
        {
            base._Ready();
            ZAsRelative = false;
            World = GetParent<World>();

            if(_initialMapPosistion != default){
                MapPosition = _initialMapPosistion;
            }
        }
    }
}