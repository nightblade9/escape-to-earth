using GoRogue;

namespace EscapeToEarth.Ecs.Components
{
    public class PositionComponent : BaseComponent
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        
        public PositionComponent(Entity parent) : base(parent)
        {

        }
    }
}