namespace EscapeToEarth.Ecs.Components
{
    public abstract class BaseComponent
    {
        public Entity Parent { get; }

        public BaseComponent(Entity parent)
        {
            this.Parent = parent;
        }
    }
}