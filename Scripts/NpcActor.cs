namespace PrisonLimbo.Scripts
{
    public class NpcActor : Actor
    {
        public override void _Ready()
        {
            base._Ready();
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
        }

        public override void TakeTurn() {
        }

        public override bool TurnProcess()
        {
            return false;
        }

    }
}