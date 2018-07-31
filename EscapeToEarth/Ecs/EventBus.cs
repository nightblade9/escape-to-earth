using EscapeToEarth.Ecs.Systems;
using System;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs
{
    class EventBus
    {
        public static EventBus Instance = new EventBus();

        private Dictionary<string, List<Action<Object>>> eventCallbacks = new Dictionary<string, List<Action<Object>>>();

        private EventBus() { }

        public void Register(String eventName, Action<Object> callback)
        {
            if (!this.eventCallbacks.ContainsKey(eventName))
            {
                this.eventCallbacks[eventName] = new List<Action<Object>>();
            }
            
            this.eventCallbacks[eventName].Add(callback);
        }

        public void Broadcast(string eventName, object data)
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