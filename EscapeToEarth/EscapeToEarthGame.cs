using EscapeToEarth.Ecs;
using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Ecs.Systems;
using EscapeToEarth.Entities;
using GoRogue.MapGeneration.Generators;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace EscapeToEarth {
    class EscapeToEarthGame {
        private readonly Color Grey48 = new Color(48, 48, 48);

        // 960x540 pixels on an 8x16 font. Although the font claims to be 8x16, multiplying doesn't work out.
        // If you take a screenshot and measure, you'll see that these are the correct values; you end up with
        // a 961x544px screen.
        private const string FontName = "IBM.font";
        private const int ScreenAndMapWidth = 120;
        private const int ScreenAndMapHeight = 34;

        private SadConsole.Console mainConsole;
        private bool redrawScreen = true;

        private Container container = new Container();

        private Player player = new Player();

        public EscapeToEarthGame()
        {
            //SadConsole.Settings.UnlimitedFPS = true;
            //SadConsole.Settings.UseHardwareFullScreen = true;

            // Setup the engine and creat the main window.
            // 120x50
            
            //SadConsole.Engine.Initialize("IBM.font", 80, 25, (g) => { g.GraphicsDeviceManager.HardwareModeSwitch = false; g.Window.AllowUserResizing = true; });

            SadConsole.Game.Create($"Fonts/{FontName}", ScreenAndMapWidth, ScreenAndMapHeight);
            // Make click-hold a bit snappier; the time between first pressing, and then repeating, should be short.
            SadConsole.Global.KeyboardState.InitialRepeatDelay = 0.4f; // default of 0.8s was too slow

            this.AddCoreGameLoopSystems();
            this.container.AddEntity(this.player);
        }

        public void Init()
        {
            // Any setup
            //SadConsole.Game.Instance.Components.Add(new SadConsole.Game.FPSCounterComponent(SadConsole.Game.Instance));

            // By default SadConsole adds a blank ready-to-go console to the rendering system. 
            // We don't want to use that for the sample project so we'll remove it.

            //Global.MouseState.ProcessMouseWhenOffScreen = true;

            // We'll instead use our demo consoles that show various features of SadConsole.
            Global.CurrentScreen = new SadConsole.Screen();

            mainConsole = new SadConsole.Console(ScreenAndMapWidth, ScreenAndMapHeight);    

            // Initialize the windows
            Global.CurrentScreen.Children.Add(mainConsole);

            var isWalkableMap = new ArrayMap<bool>(ScreenAndMapWidth, ScreenAndMapHeight);
            CellularAutomataGenerator.Generate(isWalkableMap);

            // Randomly positioned on a ground tile! True = walkable
            var playerPosition = isWalkableMap.RandomPosition(true);
            player.Position.X = playerPosition.X;
            player.Position.Y = playerPosition.Y;

            var map = new ArrayMap<MapTile>(ScreenAndMapWidth, ScreenAndMapHeight);
            this.container.GetSystem<MovementSystem>().Map = map;

            foreach (var tile in isWalkableMap.Positions())
            {
                // Convert from boolean (true/false) to tiles
                map[tile.X, tile.Y] = new MapTile() { IsWalkable = isWalkableMap[tile.X, tile.Y] };
            }

            EventBus.Instance.Register("Player moved", () => { this.redrawScreen = true; });

            this.DrawMap();
        }

        public void Update(GameTime time)
        {
            var elapsedSeconds = time.ElapsedGameTime.TotalMilliseconds;
            // Try to isolate our use of global variables and external dependencies to this class (or to DI with interfaces).
            var keysDown = SadConsole.Global.KeyboardState.KeysPressed;
            this.container.Update(elapsedSeconds);
        }

        public void DrawFrame(GameTime time)
        {
            this.DrawMap();
        }

        private void DrawMap()
        {
            // TODO: draw only what changed
            if (this.redrawScreen)
            {
                this.DrawAllWallsAndFloors();
                this.LightenFov();
                this.DrawCharacter(player.Position.X, player.Position.Y, '@', Color.White);

                this.redrawScreen = false;
            }
        }

        private void DrawAllWallsAndFloors()
        {
            var map = this.container.GetSystem<MovementSystem>().Map;

            for (var y = 0; y < ScreenAndMapHeight; y++)
            {
                for (var x = 0; x < ScreenAndMapWidth; x++)
                {
                    // Draw even if black; because FOV just lightens.
                    // If we don't draw black tiles, we need FOV to draw it, and we need to mark all
                    // the FOV tiles as discovered before we draw here.
                    var colour = map[x, y].IsDiscovered ? this.Grey48 : Color.Black;
                    this.DrawCharacter(x, y, map[x, y].IsWalkable == false ? '#' : '.', colour);
                }
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

        private void LightenFov()
        {
            var map = this.container.GetSystem<MovementSystem>().Map;
            // TODO: move player into container, mayhap.

            // Assumption: previously-lit tiles are dark now
            // Assumption: tiles don't use colour to convey information. Redraw all entities (lava, monsters, etc.)
            for (int y = player.Position.Y - Player.FovRadius; y < player.Position.Y + Player.FovRadius; y++)
            {
                for (int x = player.Position.X - Player.FovRadius; x < player.Position.X + Player.FovRadius; x++)
                {
                    if ((x >= 0 && x < ScreenAndMapWidth && y >= 0 && y < ScreenAndMapHeight) &&
                    (Math.Pow(x - player.Position.X, 2) + Math.Pow(y - player.Position.Y, 2) <= 2 * Player.FovRadius))
                    {
                        map[x, y].IsDiscovered = true;
                        mainConsole.SetForeground(x, y, Color.DarkGray);
                    }
                }
            }
        }

        private void AddCoreGameLoopSystems()
        {
            this.container.AddSystem(new MovementSystem());
        }
    }
}