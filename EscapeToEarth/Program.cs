﻿using System;
using Microsoft.Xna.Framework;
using SadConsole;

namespace StarterProject
{
    public static class Program
    {
        private static SadConsole.Console thisConsole;


        [STAThread]
        static void Main()
        {
            //SadConsole.Settings.UnlimitedFPS = true;
            //SadConsole.Settings.UseHardwareFullScreen = true;

            // Setup the engine and creat the main window.
            // 120x50
            SadConsole.Game.Create("Fonts/IBM.font", 120, 50);
            
            //SadConsole.Engine.Initialize("IBM.font", 80, 25, (g) => { g.GraphicsDeviceManager.HardwareModeSwitch = false; g.Window.AllowUserResizing = true; });

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.OnUpdate = Update;

            // Hook the "after render" even though we're not using it.
            //SadConsole.Game.OnDraw = DrawFrame;

            // Start the game.
            SadConsole.Game.Instance.Run();

            //
            // Code here will not run until the game has shut down.
            //

            SadConsole.Game.Instance.Dispose();
        }
        
        private static void Init()
        {
            // Any setup
            SadConsole.Game.Instance.Components.Add(new SadConsole.Game.FPSCounterComponent(SadConsole.Game.Instance));

            SadConsole.Game.Instance.Window.Title = "DemoProject OpenGL";

            // By default SadConsole adds a blank ready-to-go console to the rendering system. 
            // We don't want to use that for the sample project so we'll remove it.

            //Global.MouseState.ProcessMouseWhenOffScreen = true;

            // We'll instead use our demo consoles that show various features of SadConsole.
            Global.CurrentScreen = new SadConsole.Screen();

            thisConsole = new SadConsole.Console(120, 50);    

            // Initialize the windows
            Global.CurrentScreen.Children.Add(thisConsole);

        }

        private static void Update(GameTime time)
        {
            thisConsole.FillWithRandomGarbage();
        }
    }
}
