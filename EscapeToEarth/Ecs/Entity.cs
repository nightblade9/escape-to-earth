using EscapeToEarth.Ecs.Components;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EscapeToEarth.Ecs
{
    public class Entity
    {
        public PositionComponent Position { get; }

        private Dictionary<Type, BaseComponent> components = new Dictionary<Type, BaseComponent>();

        public Entity()
        {
            this.Position = new PositionComponent(this);
        }

        public Entity(int x, int y) : this()
        {
            this.Position.X = x;
            this.Position.Y = y;
        }

        public bool Has<T>() where T : BaseComponent
        {
            return this.Get<T>() != default(T);
        }

        /// <summary>
        /// Given a type T, return the first component of that type. If you want multiple components, use GetAll<T>.
        /// </summary>
        public T Get<T>() where T : BaseComponent
        {
            var type = typeof(T);
            if (!this.components.ContainsKey(type))
            {
                return default(T);
            }
            else
            {
                return this.components[type] as T;
            }
        }

        public void Set(BaseComponent component)
        {
            var type = component.GetType();
            this.components[type] = component;
        }
    }
}