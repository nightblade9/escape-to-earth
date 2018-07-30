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

        public void Add(Entity e)
        {
            // Not needed, we just access Player.Instance directly. Hack? Probably.
        }

        public void Update(double elapsedSeconds)
        {
            // TODO: DI would be nice, but KeyboardState (Keyboard instance) isn't behind an interface
            var keysDown = SadConsole.Global.KeyboardState.KeysPressed;
            var player = Player.Instance;

            var oldPlayerX = player.Position.X;
            var oldPlayerY = player.Position.Y;

            player.Get<MoveToKeyboardComponent>().Update(keysDown);

            // Glorious hack. TODO: query the map tile for this + list of blocking objects
            if (!this.Map[player.Position.X, player.Position.Y].IsWalkable)
            {
                player.Position.X = oldPlayerX;
                player.Position.Y = oldPlayerY;
            }

            if (oldPlayerX != player.Position.X || oldPlayerY != player.Position.Y)
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