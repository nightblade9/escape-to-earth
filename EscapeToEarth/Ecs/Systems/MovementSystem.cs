using EscapeToEarth.Ecs;
using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Entities.MapTiles;
using GoRogue.MapViews;
using Microsoft.Xna.Framework.Input;
using SadConsole.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs.Systems
{
    public class MovementSystem : AbstractSystem
    {
        private ArrayMap<AbstractMapTile> map;

        public MovementSystem(Entity player) : base(player)
        {
            EventBus.Instance.Register<ArrayMap<AbstractMapTile>>("Map changed", (map) => this.map = map);
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
            var keysPressed = SadConsole.Global.KeyboardState.KeysPressed;
            var keysDown = SadConsole.Global.KeyboardState.KeysDown;

            var oldPlayerX = this.player.Position.X;
            var oldPlayerY = this.player.Position.Y;

            this.player.Get<MoveToKeyboardComponent>().Update(keysPressed);

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

            if (IsShiftKeyDown(keysDown) && keysDown.Any(k => k.Key == Keys.OemPeriod) &&
            this.map[this.player.Position.X, this.player.Position.Y] is StairsDownTile)
            {
                System.Console.WriteLine("DSECEND!!!!");
            }

            if (keysPressed.Contains(AsciiKey.Get(Keys.Escape)))
            {
                System.Environment.Exit(0);
            }
        }

        private bool IsShiftKeyDown(List<AsciiKey> keysDown)
        {
            return keysDown.Contains(AsciiKey.Get(Keys.LeftShift)) || keysDown.Contains(AsciiKey.Get(Keys.RightShift));
        }
    }
}