using System;
using Godot;

namespace PrisonLimbo.Scripts
{
    public class Key : Node2D
    {
        public World World { get; set; }
        public Lazy<Node2D> Track { get; set; }
        private WorldEntity parent;
        private PackedScene trapdoorInstancer;

        public override void _ExitTree()
        {
            base._ExitTree();
            var trapdoor = (WorldEntity)trapdoorInstancer.Instance();
            const double targetDistancePercent = 0.25d;

            var targetDistance = new Vector2I(World.MapWidth, World.MapHeight).DistanceStepsL(Vector2I.Zero) * targetDistancePercent;
            var currentPos = parent.MapPosition;
            long? bestMissDistance = null;
            Vector2I? bestPoint = null;
            for (var x = 0; x < World.MapWidth; x++)
            for(var y = 0; y < World.MapHeight; y++)
            {
                var point = new Vector2I(x, y);
                var distance = point.DistanceStepsL(currentPos);
                var targetMiss = Math.Abs(targetDistance - distance);
                if(targetMiss > bestMissDistance)
                    continue;

                if(!World.CanMove(trapdoor, point))
                    continue;
                
                bestMissDistance = distance;
                bestPoint = point;
            }

            if (bestPoint is {} spawnPoint)
                trapdoor.MapPosition = spawnPoint;
            else
                throw new Exception("How can't we spawn the trapdoor anywhere???");
            
            World.AddChild(trapdoor);
        }

        public override void _Ready()
        {
            base._Ready();
            parent = GetParent<WorldEntity>();
            trapdoorInstancer = GD.Load<PackedScene>("res://Scenes/Trapdoor.tscn");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            Position = Track.Value.Position;
        }
    }
}