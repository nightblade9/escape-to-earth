using EscapeToEarth.Ecs;
using EscapeToEarth.Entities;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs.Systems
{
    public abstract class AbstractSystem : ISystem
    {
        protected Entity player;
        protected List<Entity> entities = new List<Entity>(); // doesn't include player

        protected AbstractSystem(Entity player)
        {
            this.player = player;
        }

        abstract public void Add(Entity e);
        abstract public void Update(double elapsedSeconds);
    }
}