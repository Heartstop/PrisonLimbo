using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Priority_Queue;
using PrisonLimbo.Scripts.Extensions;

namespace PrisonLimbo.Scripts
{
    public abstract class NpcActor : Actor
    {
        protected readonly Random RandomSource = new Random();

        protected bool Step(Direction direction)
        {
            if (direction == Direction.None)
                return true;
            if(!World.CanMove(this, direction))
                return false;
            AnimateMove(direction.ToAnimationState(), MapPosition + direction.ToVector2I());
            return true;
        }

        protected IEnumerable<Direction>? Path(Vector2I destination)
        {
            var start = MapPosition;
            if(start == destination)
                return ImmutableList<Direction>.Empty;

            // So we don't search the entire map when it is blocked...
            if (!World.CanMove(this, destination))
                return null;
            
            var toExplore = new SimplePriorityQueue<Vector2I, ulong>();
            toExplore.Enqueue(start, default);
            var visited = new Dictionary<Vector2I, int> {[start] = 0};
            var worldForbidden = new HashSet<Vector2I>();

            IEnumerable<Direction> MakePath()
            {
                var totalSteps = visited[destination];
                var steps = new Direction[totalSteps];

                var current = destination;
                for (var i = totalSteps - 1; i >= 0; i--)
                {
                    var (newDir, newPos) = current
                        .AdjacentDirectionsUnbound()
                        //Shuffle to make the walk more random.
                        .Shuffle(RandomSource)
                        .First(v =>
                        {
                            var found = visited.TryGetValue(v.Item2, out var step);
                            return found && step == i;
                        });

                    current = newPos;
                    steps[i] = newDir.Invert();
                }

                return steps;
            }
            
            while (toExplore.Count > 0)
            {
                var explore = toExplore.Dequeue();
                foreach (var neighbour in explore.AdjacentUnbound())
                {
                    if(worldForbidden.Contains(neighbour))
                        continue;
                    
                    var neighbourSteps = visited[explore] + 1;
                    var visitedBefore = visited.TryGetValue(neighbour, out var previousVisitWalk);
                    switch (visitedBefore)
                    {
                        case true when previousVisitWalk < neighbourSteps:
                            continue;
                        case false when !World.CanMove(this, neighbour):
                            worldForbidden.Add(neighbour);
                            continue;
                        default:
                            visited[neighbour] = neighbourSteps;
                            if (neighbour == destination)
                                return MakePath();
                            toExplore.EnqueueWithoutDuplicates(neighbour, neighbour.DistanceSquaredUL(destination));
                            break;
                    }
                }
            }

            return null;
        }

        protected IEnumerable<Direction> GetStroll()
        {
            var toExplore = new Queue<Vector2I>();
            toExplore.Enqueue(MapPosition);
            var visited = new Dictionary<Vector2I, int> {{MapPosition, 0}};
            var worldBlocked = new HashSet<Vector2I>();
            while (toExplore.Count > 0)
            {
                var explore = toExplore.Dequeue();
                foreach (var neighbour in explore.AdjacentUnbound())
                {
                    if(visited.ContainsKey(neighbour) || worldBlocked.Contains(neighbour))
                        continue;
                    
                    if (!World.CanMove(this, neighbour))
                    {
                        worldBlocked.Add(neighbour);
                        continue;
                    }
                    
                    visited.Add(neighbour, visited[explore] + 1);
                    toExplore.Enqueue(neighbour);
                }
            }

            var destination = visited.ToImmutableArray()[RandomSource.Next(0, visited.Count)];
            var path = new Direction[destination.Value];
            var current = destination.Key;

            for (var i = destination.Value - 1; i >= 0; i--)
            {
                var (newDir, newPos) = current
                    .AdjacentDirectionsUnbound()
                    //Shuffle to make the walk more random.
                    .Shuffle(RandomSource)
                    .First(v =>
                    {
                        var found = visited.TryGetValue(v.Item2, out var step);
                        return found && step == i;
                    });

                current = newPos;
                path[i] = newDir.Invert();
            }

            return Enumerable
                .Repeat(Direction.None, RandomSource.Next(2, 5))
                .Concat(path);
        }
        
        protected void AnimateMove(AnimationState animationState, Vector2I newPosition, Action? postAnimation = null)
        {
            var mark = new NpcMoveMark(World, this, newPosition);
            World.AddChild(mark);
            AnimationController.PlayAnimation(animationState, () => {
                MapPosition = newPosition;
                mark.QueueFree();
                postAnimation?.Invoke();
            });
        }

        public override void ApplyDamage(float damage)
        {
            base.ApplyDamage(damage);

            var soundDistance = Health > 0 ? 20 : 10;
            
            foreach (var guard in World
                .GetChildren()
                .OfType<Guard>()
                .Where(g => Math.Abs(g.MapPosition.X - MapPosition.X) + Math.Abs(g.MapPosition.Y - MapPosition.Y) <= soundDistance))
            {
                guard.Alert();
            }
        }
    }
}
