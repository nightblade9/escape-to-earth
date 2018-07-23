﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SadConsole
{
    /// <summary>
    /// A MonoGame <see cref="Game"/> instance that runs SadConsole. This is used when you don't provide one and call <see cref="Engine.Initialize(string, int, int)"/>.
    /// </summary>
    public partial class Game : Microsoft.Xna.Framework.Game
    {
        #region Static
        public static Microsoft.Xna.Framework.Game Instance { get; set; }

        /// <summary>
        /// Called after each frame of update logic has happened.
        /// </summary>
        public static Action<GameTime> OnUpdate;

        /// <summary>
        /// Called after a frame has been drawn.
        /// </summary>
        public static Action<GameTime> OnDraw;

        /// <summary>
        /// Called when the device is created.
        /// </summary>
        public static Action OnInitialize;

        /// <summary>
        /// Called when the game is ending.
        /// </summary>
        public static Action OnDestroy;

        public static void Create(string font, int consoleWidth, int consoleHeight, Action<Game> ctorCallback = null)
        {
            Instance = new Game(font, consoleWidth, consoleHeight, ctorCallback);
        }
        #endregion

        /// <summary>
        /// Indicates the window is going to resize itself.
        /// </summary>
        public bool ResizeBusy = false;
        private string font;
        private int consoleWidth;
        private int consoleHeight;
        public GraphicsDeviceManager GraphicsDeviceManager;


        /// <summary>
        /// Raised when the window is resized and the render area has been calculated.
        /// </summary>
        public event EventHandler WindowResized;

        protected Game(string font, int consoleWidth, int consoleHeight, Action<Game> ctorCallback)
        {
            if (Instance == null)
                Instance = this;

            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.GraphicsProfile = Settings.GraphicsProfile;
            Content.RootDirectory = "Content";

            this.font = font;
            this.consoleHeight = consoleHeight;
            this.consoleWidth = consoleWidth;
#if MONOGAME
            GraphicsDeviceManager.HardwareModeSwitch = Settings.UseHardwareFullScreen;
#endif


            ctorCallback?.Invoke(this);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (!ResizeBusy)
            {
                if (!Global.GraphicsDeviceManager.IsFullScreen && Settings.WindowMinimumSize != Point.Zero)
                {
                    if (GraphicsDeviceManager.PreferredBackBufferWidth < Settings.WindowMinimumSize.X
                        || GraphicsDeviceManager.PreferredBackBufferHeight < Settings.WindowMinimumSize.Y)
                    {
                        ResizeBusy = true;
                        GraphicsDeviceManager.PreferredBackBufferWidth = Global.WindowWidth = Settings.WindowMinimumSize.X;
                        GraphicsDeviceManager.PreferredBackBufferHeight = Global.WindowHeight = Settings.WindowMinimumSize.Y;
                        GraphicsDeviceManager.ApplyChanges();
                    }
                }
            }
            else
                ResizeBusy = false;

            //if (!resizeBusy && Settings.IsExitingFullscreen)
            //{
            //    GraphicsDeviceManager.PreferredBackBufferWidth = Global.WindowWidth;
            //    GraphicsDeviceManager.PreferredBackBufferHeight = Global.WindowHeight;

            //    resizeBusy = true;
            //    GraphicsDeviceManager.ApplyChanges();
            //    resizeBusy = false;
            //    Settings.IsExitingFullscreen = false;
            //}

            //Global.WindowWidth = GraphicsDeviceManager.PreferredBackBufferWidth;
            //Global.WindowHeight = GraphicsDeviceManager.PreferredBackBufferHeight;
            //Global.WindowWidth = Global.RenderWidth = GraphicsDeviceManager.PreferredBackBufferWidth;
            //Global.WindowHeight = Global.RenderHeight = GraphicsDeviceManager.PreferredBackBufferHeight;
            Global.ResetRendering();

            if (!ResizeBusy)
                WindowResized?.Invoke(this, EventArgs.Empty);
        }

        protected override void UnloadContent()
        {
            OnDestroy?.Invoke();
        }

        protected override void Initialize()
        {
            if (Settings.UnlimitedFPS)
            {
                GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
                IsFixedTimeStep = false;
            }

            // Let the XNA framework show the mouse.
            IsMouseVisible = true;

            // Initialize the SadConsole engine with a font, and a screen size that mirrors MS-DOS.
            Components.Add(new ClearScreenGameComponent(this));
            Components.Add(new SadConsoleGameComponent(this));

            // Call the default initialize of the base class.
            base.Initialize();

            // Hook window change for resolution fixes
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Window.AllowUserResizing = SadConsole.Settings.AllowWindowResize;

            Global.GraphicsDevice = GraphicsDevice;
            Global.GraphicsDeviceManager = GraphicsDeviceManager;
            Global.SpriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);
            Global.FontDefault = Global.LoadFont(font).GetFont(Font.FontSizes.One);
            Global.FontDefault.ResizeGraphicsDeviceManager(GraphicsDeviceManager, consoleWidth, consoleHeight, 0, 0);
            Global.ResetRendering();

            // Tell the main engine we're ready
            OnInitialize?.Invoke();

            // After we've init, clear the graphics device so everything is ready to start
            Global.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
