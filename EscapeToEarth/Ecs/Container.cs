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

        public T Get<T>() where T : ISystem
        {
            foreach (var system in this.systems)
            {
                if (system is T)
                {
                    return (T)system;
                }
            }

            return default(T);
        }

        public void Update(double elapsedSeconds)
        {
            foreach (var system in this.systems)
            {
                system.Update(elapsedSeconds);
            }
        }
    }
}