using EscapeToEarth.Ecs;
using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Entities;
using GoRogue.MapViews;
using Microsoft.Xna.Framework.Input;
using SadConsole.Input;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs.Systems
{
    class MovementSystem : ISystem
    {
        internal ArrayMap<MapTile> Map { get; set; }
        
        private Entity player;

        public void Add(Entity e)
        {
            if (e.Has<MoveToKeyboardComponent>())
            {
                this.player = e;
            }
        }

        public void Update(double elapsedSeconds)
        {
            // TODO: DI would be nice, but KeyboardState (Keyboard instance) isn't behind an interface
            var keysDown = SadConsole.Global.KeyboardState.KeysPressed;

            var oldPlayerX = this.player.Position.X;
            var oldPlayerY = this.player.Position.Y;

            this.player.Get<MoveToKeyboardComponent>().Update(keysDown);

            // Glorious hack. TODO: query the map tile for this + list of blocking objects
            if (!this.Map[this.player.Position.X, this.player.Position.Y].IsWalkable)
            {
                this.player.Position.X = oldPlayerX;
                this.player.Position.Y = oldPlayerY;
            }

            if (oldPlayerX != this.player.Position.X || oldPlayerY != this.player.Position.Y)
            {
                EventBus.Instance.Broadcast("Player moved");
            }

            if (keysDown.Contains(AsciiKey.Get(Keys.Escape)))
            {
                System.Environment.Exit(0);
            }
        }
    }
}