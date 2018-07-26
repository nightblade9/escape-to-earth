using GoRogue.MapGeneration.Generators;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;

// for keyboard input
using SadConsole.Input;
using Microsoft.Xna.Framework.Input;

using EscapeToEarth.Entities;
using System;

namespace EscapeToEarth {
    class EscapeToEarthGame {
        private readonly Color ReallyDarkGrey = new Color(96, 96, 96);

        // 960x540 pixels on an 8x16 font. Although the font claims to be 8x16, multiplying doesn't work out.
        // If you take a screenshot and measure, you'll see that these are the correct values; you end up with
        // a 961x544px screen.
        private const string FontName = "IBM.font";
        private const int ScreenAndMapWidth = 120;
        private const int ScreenAndMapHeight = 34;

        private SadConsole.Console mainConsole;
        private bool redrawScreen = true;

        private ArrayMap<bool> map;

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

            this.map = new ArrayMap<bool>(ScreenAndMapWidth, ScreenAndMapHeight);
            CellularAutomataGenerator.Generate(map);
            // Randomly positioned on a ground tile!
            player.Position = map.RandomPosition(true);

            this.DrawMap();
        }

        public void Update(GameTime time)
        {
            var keysDown = SadConsole.Global.KeyboardState.KeysPressed;

            var dx = 0;
            var dy = 0;

            if (keysDown.Contains(AsciiKey.Get(Keys.Left)))
            {
                dx = -1;
            }
            else if (keysDown.Contains(AsciiKey.Get(Keys.Right)))
            {
                dx = 1;
            }

            if (keysDown.Contains(AsciiKey.Get(Keys.Up)))
            {
                dy = -1;
            }
            else if (keysDown.Contains(AsciiKey.Get(Keys.Down)))
            {
                dy = 1;
            }

            if (dx != 0 || dy != 0)
            {
                this.player.Position = this.player.Position.Translate(dx, dy);
                this.redrawScreen = true;
            }

            if (keysDown.Contains(AsciiKey.Get(Keys.Escape)))
            {
                System.Environment.Exit(0);
            }
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
                this.DrawCharacter(player.Position.X, player.Position.Y, '@', Color.White);
                this.LightenFov();

                this.redrawScreen = false;
            }
        }

        private void DrawAllWallsAndFloors()
        {
            for (var y = 0; y < ScreenAndMapHeight; y++)
            {
                for (var x = 0; x < ScreenAndMapWidth; x++)
                {
                    this.DrawCharacter(x, y, map[x, y] == false ? '#' : '.', ReallyDarkGrey);
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
            // Assumption: previously-lit tiles are dark now
            // Assumption: tiles don't use colour to convey information. Redraw all entities (lava, monsters, etc.)
            for (int y = player.Position.Y - Player.FovRadius; y < player.Position.Y + Player.FovRadius; y++)
            {
                for (int x = player.Position.X - Player.FovRadius; x < player.Position.X + Player.FovRadius; x++)
                {
                    if ((x >= 0 && x < ScreenAndMapWidth && y >= 0 && y < ScreenAndMapHeight) &&
                    (Math.Pow(x - player.Position.X, 2) + Math.Pow(y - player.Position.Y, 2) <= 2 * Player.FovRadius))
                    {
                        mainConsole.SetForeground(x, y, Color.DarkGray);
                    }
                }
            }
        }
    }
}