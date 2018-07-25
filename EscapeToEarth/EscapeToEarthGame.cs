using GoRogue.MapGeneration.Generators;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;

namespace EscapeToEarth {
    class EscapeToEarthGame {
        // 960x540 pixels on an 8x16 font. Although the font claims to be 8x16, multiplying doesn't work out.
        // If you take a screenshot and measure, you'll see that these are the correct values; you end up with
        // a 961x544px screen.
        private const string FontName = "IBM.font";
        private const int ScreenAndMapWidth = 120;
        private const int ScreenAndMapHeight = 34;

        private SadConsole.Console mainConsole;


        private ArrayMap<bool> map;
        // TODO: into player class?
        GoRogue.Coord playerPosition;

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
            for (var y = 0; y < ScreenAndMapHeight; y++)
            {
                for (var x = 0; x < ScreenAndMapWidth; x++)
                {
                    mainConsole.SetForeground(x, y, Color.DarkGray);
                    mainConsole.SetGlyph(x, y, map[x, y] == false ? '#' : '.');
                }
            }

            mainConsole.SetForeground(playerPosition.X, playerPosition.Y, Color.White);
            mainConsole.SetGlyph(playerPosition.X, playerPosition.Y, '@');
        }
    }
}