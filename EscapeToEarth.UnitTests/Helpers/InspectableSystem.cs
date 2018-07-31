using EscapeToEarth.Ecs;
using EscapeToEarth.Ecs.Systems;
using System.Collections.Generic;

namespace EscapeToEarth.UnitTest.Helpers
{
    class InspectableSystem : ISystem
    {
        public List<Entity> Entities = new List<Entity>();
        public double TotalUpdates { get; private set; } = 0;

        public void Add(Entity e)
        {
            this.Entities.Add(e);
        }

        public void Update(double elapsedSeconds)
        {
            this.TotalUpdates += elapsedSeconds;
        }
    }
}