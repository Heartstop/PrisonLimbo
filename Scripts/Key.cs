using System;
using System.Collections.Generic;
using Godot;

namespace PrisonLimbo.Scripts
{
    public class Key : Node2D
    {
        public World World { get; set; }
        public Lazy<Node2D> Track { get; set; }
        private WorldEntity parent;
        private PackedScene trapdoorInstancer;
        protected readonly Random RandomSource = new Random();

        public override void _ExitTree()
        {
            base._ExitTree();
            var trapdoor = (Trapdoor)trapdoorInstancer.Instance();
            const double targetDistancePercent = 0.25d;

            var targetDistance = (long) (new Vector2I(World.MapWidth, World.MapHeight).DistanceStepsL(Vector2I.Zero) * targetDistancePercent);
            var currentPos = parent.MapPosition;
            long? smallestMiss = null;
            var bestPoints = new List<Vector2I>();
            for (var x = 0; x < World.MapWidth; x++)
            for (var y = 0; y < World.MapHeight; y++)
            {
                var point = new Vector2I(x, y);
                var distance = point.DistanceStepsL(currentPos);
                var miss = Math.Abs(targetDistance - distance);
                if(miss > smallestMiss)
                    continue;

                if(!World.CanMove(trapdoor, point))
                    continue;

                if(miss < smallestMiss)
                    bestPoints.Clear();
                
                smallestMiss = miss;
                bestPoints.Add(point);
            }
            
            if(bestPoints.Count == 0)
                throw new Exception("How can't we spawn the trapdoor anywhere???");
            
            World.AddChild(trapdoor);
            var posIndex = RandomSource.Next(0, bestPoints.Count);
            trapdoor.MapPosition = bestPoints[posIndex];
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