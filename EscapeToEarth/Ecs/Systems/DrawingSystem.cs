using EscapeToEarth.Ecs;
using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Entities;
using EscapeToEarth.Entities.MapTiles;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EscapeToEarth.Ecs.Systems
{
    public class DrawingSystem : AbstractSystem
    {        
        private readonly Color Grey48 = new Color(48, 48, 48);
        private readonly SadConsole.Console mainConsole;
        private List<Tuple<int, int>> previousFovTiles = new List<Tuple<int, int>>();

        private ArrayMap<AbstractMapTile> map;
        private bool redrawScreen = true;

        public DrawingSystem(Entity player, SadConsole.Console mainConsole) : base(player)
        {
            this.mainConsole = mainConsole;

            EventBus.Instance.Register<ArrayMap<AbstractMapTile>>("Map changed", (map) => this.map = map);
            EventBus.Instance.Register<Player>("Player moved", (data) => this.redrawScreen = true );
            EventBus.Instance.Register<Player>("Player used stairs", (p) => {
                this.mainConsole.Clear();
                this.previousFovTiles.Clear();
                this.redrawScreen = true;
                this.DrawAll();
            });
        }

        override public void Add(Entity e)
        {
            if (e.Has<DisplayComponent>())
            {
                this.entities.Add(e);
            }
        }

        override public void Update(double elapsedSeconds)
        {
        }

        public void Draw(double elapsedSeconds)
        {
            this.DrawAll();
        }

        private void DrawAll()
        {
            // TODO: draw only what changed
            if (this.redrawScreen)
            {
                this.DarkenFov(this.previousFovTiles);

                var fovTiles = this.CalculateFieldOfView();

                this.DrawAllWallsAndFloors(fovTiles);
                this.LightenFov(fovTiles);

                foreach (var entity in this.entities)
                {
                    var display = entity.Get<DisplayComponent>();
                    if (fovTiles.Any(t => t.Item1 == entity.Position.X && t.Item2 == entity.Position.Y))
                    {
                        this.DrawCharacter(entity.Position.X, entity.Position.Y, display.Character, display.Colour);
                    }
                }

                var playerDisplay = this.player.Get<DisplayComponent>();
                this.DrawCharacter(player.Position.X, player.Position.Y, playerDisplay.Character, playerDisplay.Colour);

                this.redrawScreen = false;

                this.previousFovTiles = fovTiles;
            }
        }

        private void DrawAllWallsAndFloors(List<Tuple<int, int>> fovTiles)
        {
            foreach (var tile in fovTiles)
            {
                var x = tile.Item1;
                var y = tile.Item2;

                // Draw even if black; because FOV just lightens.
                // If we don't draw black tiles, we need FOV to draw it, and we need to mark all
                // the FOV tiles as discovered before we draw here.
                var colour = map[x, y].IsDiscovered ? this.Grey48 : Color.Black;
                this.DrawCharacter(x, y, map[x, y].FloorCharacter);
            }
        }

        private void DrawCharacter(int x, int y, int character, Color? colour = null)
        {
            if (colour.HasValue)
            {
                mainConsole.SetForeground(x, y, colour.Value);
            }

            mainConsole.SetGlyph(x, y, character);
        }

        private List<Tuple<int, int>> CalculateFieldOfView()
        {
            var toReturn = new List<Tuple<int, int>>();

            for (int y = player.Position.Y - Player.FovRadius; y < player.Position.Y + Player.FovRadius; y++)
            {
                for (int x = player.Position.X - Player.FovRadius; x < player.Position.X + Player.FovRadius; x++)
                {
                    if ((x >= 0 && x < this.mainConsole.Width && y >= 0 && y < this.mainConsole.Height) &&
                    (Math.Pow(x - player.Position.X, 2) + Math.Pow(y - player.Position.Y, 2) <= 2 * Player.FovRadius))
                    {
                        toReturn.Add(new Tuple<int, int>(x, y));
                    }
                }
            }

            return toReturn;
        }

        private void LightenFov(List<Tuple<int, int>> fovTiles)
        {
            // Assumption: tiles don't use colour to convey information. Redraw all entities (lava, monsters, etc.)
            foreach (var tile in fovTiles)
            {
                var x = tile.Item1;
                var y = tile.Item2;
                map[x, y].IsDiscovered = true;
                mainConsole.SetForeground(x, y, Color.DarkGray);
            }
        }

        private void DarkenFov(List<Tuple<int, int>> fovTiles)
        {
            // TODO: we can make this more efficient by passing in the previous FOV, subtracting the current FOV
            // If this becomes a bottleneck with large FOVs, that might be a good performance optimization.

            // Assumption: tiles don't use colour to convey information. Redraw all entities (lava, monsters, etc.)
            foreach (var tile in fovTiles)
            {
                var x = tile.Item1;
                var y = tile.Item2;
                mainConsole.SetForeground(x, y, Grey48);
            }
        }
    }
}