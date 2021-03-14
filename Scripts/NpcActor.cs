using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Godot;
using Priority_Queue;

namespace PrisonLimbo.Scripts
{
    public abstract class NpcActor : Actor
    {
        protected readonly Random Random = new Random();

        protected bool Step(Direction direction)
        {
            if(direction == Direction.None || !World.CanMove(this, direction))
                return false;
            AnimateMove(direction.ToAnimationState(), MapPosition + direction.ToVector2());
            return true;
        }

        protected IEnumerable<Direction>? Path(Vector2I start, Vector2I end)
        {
            if(start == end)
                return ImmutableList<Direction>.Empty;
            
            var toExplore = new SimplePriorityQueue<Vector2I, ulong>();
            toExplore.Enqueue(start, default);
            var visited = new Dictionary<Vector2I, int> {[start] = 0};
            var worldForbidden = new HashSet<Vector2I>();

            IEnumerable<Direction> MakePath()
            {
                var totalSteps = visited[end];
                var steps = new Direction[totalSteps];

                var current = end;
                for (var i = totalSteps - 1; i >= 0; i--)
                {
                    var (newDir, newPos) = current
                        .AdjacentDirectionsUnbound()
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
                            if (neighbour == end)
                                return MakePath();
                            toExplore.EnqueueWithoutDuplicates(neighbour, neighbour.DistanceSquaredUL(end));
                            break;
                    }
                }
            }

            return null;
        }
        
        public IEnumerable<Direction> GetStroll()
        {
            return Enumerable
                .Repeat(Direction.None, Random.Next(2, 5));
        }
        
        protected void AnimateMove(AnimationState animationState, Vector2 newPosition, Action? postAnimation = null)
        {
            var mark = new NpcMoveMark(World, this, newPosition);
            World.AddChild(mark);
            AnimationController.PlayAnimation(animationState, () => {
                MapPosition = newPosition;
                mark.QueueFree();
                postAnimation?.Invoke();
            });
        }
    }
}
