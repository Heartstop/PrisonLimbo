using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Godot;

namespace PrisonLimbo.Scripts.Singletons
{
    public class InputSystem : Node
    {
        private static readonly ImmutableDictionary<string, Direction> DirectionKeys =
            new Dictionary<string, Direction>
            {
                {"ui_up", Direction.Up},
                {"ui_right", Direction.Right},
                {"ui_down", Direction.Down},
                {"ui_left", Direction.Left}
            }.ToImmutableDictionary();

        public static Direction Direction { get; private set; }
        public static bool Act { get; private set; }

        public override void _Ready()
        {
            base._Ready();
            PauseMode = PauseModeEnum.Process;
            ProcessPriority = int.MaxValue;
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            Direction = GetDirection();
            Act = GetAct();
        }

        private static Direction GetDirection()
        {
            var active = DirectionKeys
                .Where(kvp => Input.IsActionPressed(kvp.Key))
                .ToImmutableArray();
            return active.Length != 1
                ? Direction.None
                : active.Single().Value;
        }

        private static bool GetAct()
        {
            return Input.IsActionPressed("ui_accept");
        }
    }
}