using EscapeToEarth.Ecs.Systems;
using System;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs
{
    class EventBus
    {
        public static EventBus Instance = new EventBus();

        private Dictionary<string, List<Action>> eventCallbacks = new Dictionary<string, List<Action>>();

        private EventBus() { }

        public void Register(String eventName, Action callback)
        {
            if (!this.eventCallbacks.ContainsKey(eventName))
            {
                this.eventCallbacks[eventName] = new List<Action>();
            }
            
            this.eventCallbacks[eventName].Add(callback);
        }

        public void Broadcast(string eventName)
        {
            if (this.eventCallbacks.ContainsKey(eventName))
            {
                foreach (var callback in this.eventCallbacks[eventName])
                {
                    callback();
                }
            }
        }
    }
}