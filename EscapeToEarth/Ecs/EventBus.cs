using EscapeToEarth.Ecs.Systems;
using System;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs
{
    public class EventBus
    {
        public static EventBus Instance = new EventBus();

        // We want to keep a number of lists of different Action<T>s for different Ts. We can't easily do that. So instead,
        // We keep Action<Object> here, and simply wrap our callbacks in Action<Object> callbacks that invoke them. Simple.
        private Dictionary<string, List<Action<Object>>> eventCallbacks = new Dictionary<string, List<Action<Object>>>();

        // Public for the sake of unit testing; else, private.
        public EventBus() { }

        public void Register<T>(String eventName, Action<T> callback) where T : class
        {
            if (!this.eventCallbacks.ContainsKey(eventName))
            {
                this.eventCallbacks[eventName] = new List<Action<Object>>();
            }
            
            // Can't easily store Action<T> in Action<Object>. But we can wrap.
            Action<Object> convertedCallback = (obj) => callback(obj as T);
            this.eventCallbacks[eventName].Add(convertedCallback);
        }

        public void Broadcast<T>(string eventName, T data)
        {
            if (this.eventCallbacks.ContainsKey(eventName))
            {
                foreach (var callback in this.eventCallbacks[eventName])
                {
                    callback(data);
                }
            }
        }
    }
}