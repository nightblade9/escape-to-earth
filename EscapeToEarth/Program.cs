using Microsoft.Xna.Framework;
using SadConsole;
using System;
using EscapeToEarth;

namespace StarterProject
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var g = new EscapeToEarthGame();

            SadConsole.Game.Instance.Window.Title = ".NET Core 2.0 Test";

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = g.Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.OnUpdate = g.Update;

            // Hook the "after render" even though we're not using it.
            SadConsole.Game.OnDraw = g.DrawFrame;

            // Start the game. Blocking call that terminates when the game ends.
            SadConsole.Game.Instance.Run();

            //
            // Code here will not run until the game has shut down.
            //
            SadConsole.Game.Instance.Dispose();
        }        
    }
}
