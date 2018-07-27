using GoRogue;

namespace EscapeToEarth.Ecs.Components
{
    public class PositionComponent : BaseComponent
    {
        public PositionComponent(Entity parent) : base(parent)
        {

        }

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public void UpdateTo(Coord coordinates)
        {
            this.X = coordinates.X;
            this.Y = coordinates.Y;
        }
    }
}