using EscapeToEarth.Ecs;
using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Ecs.Systems;
using EscapeToEarth.Entities;
using EscapeToEarth.Entities.MapTiles;
using GoRogue.MapGeneration.Generators;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace EscapeToEarth
{
    class EscapeToEarthGame
    {
        private const int ScreenAndMapWidth = 120;
        private const int ScreenAndMapHeight = 34;
        // 960x540 pixels on an 8x16 font. Although the font claims to be 8x16, multiplying doesn't work out.
        // If you take a screenshot and measure, you'll see that these are the correct values; you end up with
        // a 961x544px screen.
        private const string FontName = "IBM.font";
        
        private SadConsole.Console mainConsole;

        private Container container = new Container();

        private ArrayMap<AbstractMapTile> map;
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

            this.AddCoreGameLoopSystems();
            this.container.AddEntity(this.player);

            var mapData = this.GenerateMap();
            var isWalkableMap = mapData.Map;

            map = new ArrayMap<AbstractMapTile>(ScreenAndMapWidth, ScreenAndMapHeight);
            EventBus.Instance.Broadcast("Map changed", map);

            // Convert ArrayMap<Bool> to ArrayMap<MapTile>
            foreach (var tile in isWalkableMap.Positions())
            {
                AbstractMapTile mapTile;

                // Convert from boolean (true/false) to tiles
                if (isWalkableMap[tile.X, tile.Y])
                {
                    mapTile = new FloorTile();
                }
                else
                {
                    mapTile = new WallTile();
                }

                map[tile.X, tile.Y] = mapTile;
            }

            player.Position.X = mapData.PlayerPosition.X;
            player.Position.Y = mapData.PlayerPosition.Y;

            map[mapData.StairsDownPosition.X, mapData.StairsDownPosition.Y] = new StairsDownTile();
            
            this.container.DrawFrame(0); // Initial draw without player moving
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
            this.container.DrawFrame(time.ElapsedGameTime.TotalSeconds);
        }

        private void AddCoreGameLoopSystems()
        {
            this.container.AddSystem(new MovementSystem(this.player));
            this.container.AddSystem(new DrawingSystem(this.player, this.mainConsole));
        }

        // TODO: move to a map generator class
        private MapData GenerateMap()
        {
            const int MinimumPlayerStairsDistance = 5;

            var isWalkableMap = new ArrayMap<bool>(ScreenAndMapWidth, ScreenAndMapHeight);
            CellularAutomataGenerator.Generate(isWalkableMap);

            // Randomly positioned on a ground tile! True = walkable
            var playerPosition = isWalkableMap.RandomPosition(true);

            var stairsDownPosition = playerPosition;
            while (Math.Abs(playerPosition.X - stairsDownPosition.X) + Math.Abs(playerPosition.Y - stairsDownPosition.Y) <= MinimumPlayerStairsDistance)
            {
                stairsDownPosition = isWalkableMap.RandomPosition(true);
            }

            return new MapData() { Map = isWalkableMap, PlayerPosition = playerPosition, StairsDownPosition = stairsDownPosition };
        }

        private class MapData
        {
            public ArrayMap<bool> Map { get; set; }

            // Player position AND stairs-down position
            public GoRogue.Coord PlayerPosition { get; set; }
            public GoRogue.Coord StairsDownPosition { get; set; }
        }
    }
}