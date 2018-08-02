using EscapeToEarth.Ecs;
using EscapeToEarth.Entities;

namespace EscapeToEarth.Ecs.Systems
{
    public abstract class AbstractSystem : ISystem
    {
        protected Entity player;

        protected AbstractSystem(Entity player)
        {
            this.player = player;
        }

        abstract public void Add(Entity e);
        abstract public void Update(double elapsedSeconds);
    }
}