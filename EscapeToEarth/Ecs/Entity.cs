using System.Collections.Generic;
using System.Linq;
using System;

namespace EscapeToEarth.Ecs
{
    public class Entity
    {
        private Dictionary<Type, Object> components = new Dictionary<Type, Object>();

        /// <summary>
        /// Given a type T, return the first component of that type. If you want multiple components, use GetAll<T>.
        /// </summary>
        public T Get<T>() where T : class
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

        public void Set<T>(Object value) where T : class
        {
            var type = typeof(T);
            this.components[type] = value;
        }
    }
}