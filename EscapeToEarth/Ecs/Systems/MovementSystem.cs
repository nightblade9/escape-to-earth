using EscapeToEarth.Ecs;
using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Entities;
using GoRogue.MapViews;
using Microsoft.Xna.Framework.Input;
using SadConsole.Input;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs.Systems
{
    public class MovementSystem : AbstractSystem
    {
        private ArrayMap<MapTile> map;

        public MovementSystem(Player player) : base(player)
        {
            EventBus.Instance.Register("Map changed", (map) => this.map = map as ArrayMap<MapTile>);
        }

        override public void Add(Entity e)
        {
            if (e.Has<MoveToKeyboardComponent>())
            {
                this.player = e;
            }
        }

        override public void Update(double elapsedSeconds)
        {
            var keysDown = SadConsole.Global.KeyboardState.KeysPressed;

            var oldPlayerX = this.player.Position.X;
            var oldPlayerY = this.player.Position.Y;

            this.player.Get<MoveToKeyboardComponent>().Update(keysDown);

            // Glorious hack. TODO: query the map tile for this + list of blocking objects
            if (!this.map[this.player.Position.X, this.player.Position.Y].IsWalkable)
            {
                this.player.Position.X = oldPlayerX;
                this.player.Position.Y = oldPlayerY;
            }

            if (oldPlayerX != this.player.Position.X || oldPlayerY != this.player.Position.Y)
            {
                EventBus.Instance.Broadcast("Player moved", player);
            }

            if (keysDown.Contains(AsciiKey.Get(Keys.Escape)))
            {
                System.Environment.Exit(0);
            }
        }
    }
}