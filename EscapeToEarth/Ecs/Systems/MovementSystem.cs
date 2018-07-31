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
        internal Entity Player { get; private set; }

        public void Add(Entity e)
        {
            if (e.Has<MoveToKeyboardComponent>())
            {
                this.Player = e;
            }
        }

        public void Update(double elapsedSeconds)
        {
            // TODO: DI would be nice, but KeyboardState (Keyboard instance) isn't behind an interface
            var keysDown = SadConsole.Global.KeyboardState.KeysPressed;

            var oldPlayerX = this.Player.Position.X;
            var oldPlayerY = this.Player.Position.Y;

            this.Player.Get<MoveToKeyboardComponent>().Update(keysDown);

            // Glorious hack. TODO: query the map tile for this + list of blocking objects
            if (!this.Map[this.Player.Position.X, this.Player.Position.Y].IsWalkable)
            {
                this.Player.Position.X = oldPlayerX;
                this.Player.Position.Y = oldPlayerY;
            }

            if (oldPlayerX != this.Player.Position.X || oldPlayerY != this.Player.Position.Y)
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