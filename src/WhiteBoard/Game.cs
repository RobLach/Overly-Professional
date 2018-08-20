#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace WhiteBoard
{
    
    /// <summary>
    /// Sample showing how to manage the different game states involved in
    /// implementing a networked game, with menus for creating, searching,
    /// and joining sessions, a lobby screen, and the game itself. This main
    /// game class is extremely simple: all the interesting stuff happens
    /// in the ScreenManager component.
    /// </summary>
    public class WhiteBoardGame : Microsoft.Xna.Framework.Game
    {
        #region Fields

        public GraphicsDeviceManager graphics;
        public ScreenManager screenManager;

        #endregion


        #region Initialization


        /// <summary>
        /// The main game constructor.
        /// </summary>
        public WhiteBoardGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

            //SET RESOLUTION HERE
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;


            graphics.SynchronizeWithVerticalRetrace = true;

            graphics.PreferMultiSampling = true;

            //graphics.ToggleFullScreen();
            // Create components.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);
            Components.Add(new MessageDisplayComponent(this));
            Components.Add(new GamerServicesComponent(this));


            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
        }


        #endregion

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.ApplyChanges();
            graphics.GraphicsDevice.Clear(Color.Black);
            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }


        #endregion
    }


    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            using (WhiteBoardGame game = new WhiteBoardGame())
            {
                game.IsFixedTimeStep = true;
                game.Run();
                
            }
        }
    }

    #endregion
}
