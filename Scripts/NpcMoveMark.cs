using Godot;

namespace PrisonLimbo.Scripts
{
    public class NpcMoveMark : WorldEntity
    {
        public NpcActor Creator { get; }

        public NpcMoveMark(World world, NpcActor creator, Vector2 position)
        {
            World = world;
            Creator = creator;
            MapPosition = position;
        }
        public override bool CanEnter<T>(T entity) => entity == Creator;
    }
}