using Godot;

namespace PrisonLimbo.Scripts
{
    public class Actor : WorldEntity
    {
        public Direction Direction = Direction.Down;
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
    }
}