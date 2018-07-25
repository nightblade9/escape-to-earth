using GoRogue.MapGeneration.Generators;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
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


        private ArrayMap<bool> map;

        // TODO: into player class
        GoRogue.Coord playerPosition;
        const int PlayerFovRadius = 5;

        public EscapeToEarthGame()
        {
            //SadConsole.Settings.UnlimitedFPS = true;
            //SadConsole.Settings.UseHardwareFullScreen = true;

            // Setup the engine and creat the main window.
            // 120x50
            
            //SadConsole.Engine.Initialize("IBM.font", 80, 25, (g) => { g.GraphicsDeviceManager.HardwareModeSwitch = false; g.Window.AllowUserResizing = true; });

            SadConsole.Game.Create($"Fonts/{FontName}", ScreenAndMapWidth, ScreenAndMapHeight);

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
            this.playerPosition = map.RandomPosition(true);

            this.DrawMap();
        }

        public void Update(GameTime time)
        {

        }

        public void DrawFrame(GameTime time)
        {

        }

        private void DrawMap()
        {
            // TODO: draw only what changed
            this.DrawAllWallsAndFloors();
            this.DrawCharacter(playerPosition.X, playerPosition.Y, '@', Color.White);
            this.LightenFov();
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
            for (int y = playerPosition.Y - PlayerFovRadius; y < playerPosition.Y + PlayerFovRadius; y++)
            {
                for (int x = playerPosition.X - PlayerFovRadius; x < playerPosition.X + PlayerFovRadius; x++)
                {
                    if ((x >= 0 && x < ScreenAndMapWidth && y >= 0 && y < ScreenAndMapHeight) &&
                    (Math.Pow(x - playerPosition.X, 2) + Math.Pow(y - playerPosition.Y, 2) <= 2 * PlayerFovRadius))
                    {
                        mainConsole.SetForeground(x, y, Color.DarkGray);
                    }
                }
            }
        }
    }
}