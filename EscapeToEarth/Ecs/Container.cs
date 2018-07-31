using EscapeToEarth.Ecs.Systems;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs
{
    /// <summary>
    /// A container of systems, we operate on their entities.
    /// </summary>
    public class Container
    {
        public List<ISystem> systems = new List<ISystem>();

        public void AddSystem(ISystem system)
        {
            this.systems.Add(system);
        }

        public T GetSystem<T>() where T : ISystem
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

        public void AddEntity(Entity e)
        {
            foreach (var system in this.systems)
            {
                system.Add(e);
            }
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