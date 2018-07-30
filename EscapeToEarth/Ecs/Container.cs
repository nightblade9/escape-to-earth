using EscapeToEarth.Ecs.Systems;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs
{
    /// <summary>
    /// A container of systems, we operate on their entities.
    /// </summary>
    class Container
    {
        public List<ISystem> systems = new List<ISystem>();

        public void Add(ISystem system)
        {
            this.systems.Add(system);
        }

        public void Update(float elapsedSeconds)
        {
            foreach (var system in this.systems)
            {
                system.Update(elapsedSeconds);
            }
        }
    }
}