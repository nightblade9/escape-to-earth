using EscapeToEarth.Ecs;
using EscapeToEarth.Ecs.Components;
using SadConsole.Input;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs.Systems
{
    class MovementSystem : ISystem
    {
        private IList<Entity> entities = new List<Entity>();

        public void Add(Entity e)
        {            
            if (e.Has<MoveToKeyboardComponent>())
            {
                this.entities.Add(e);
            }
        }

        public void Update(float elapsedSeconds)
        {
            var keysDown = SadConsole.Global.KeyboardState.KeysPressed;

            foreach (var controllable in this.entities)
            {
                controllable.Get<MoveToKeyboardComponent>().Update(keysDown);
            }
        }
    }
}