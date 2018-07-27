using GoRogue;

namespace EscapeToEarth.Ecs.Components
{
    public class PositionComponent
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public void UpdateTo(Coord coordinates)
        {
            this.X = coordinates.X;
            this.Y = coordinates.Y;
        }
    }
}