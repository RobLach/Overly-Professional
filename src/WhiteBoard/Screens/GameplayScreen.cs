#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using WhiteBoard.Controller;
using WhiteBoard.Objects;
using WhiteBoard.Collisions;
using WhiteBoard;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace WhiteBoard
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        public List<CharacterController> charControllers = new List<CharacterController>();

        Boolean DEBUG = false;
        Vector2? collisionAngle = Vector2.Zero;

        NetworkSession networkSession;
        string curTarget = "BOTH";

        ContentManager content;
        public static SpriteFont gameFont;
        public static SpriteFont diagFont;

        public int centerTile = 2;
        public int lastTile = 2;

        //Test Stuff
        AnimationTexture testAnimTex;
        AnimatedSpriteSheet testSS;
        AnimatedSpriteSheet testProjectile;
        AnimatedSpriteSheet[] projectileSS;
        AnimatedSpriteSheet briefCaseSS;
        AnimatedSpriteSheet hatSS;

        Texture2D[,] backgroundTiles;

        Vector2 baseRes;

        Texture2D border;

        Texture2D bgImage;
        Texture2D projectileTex;
        Texture2D healthTex;

        public Projectile[] projectiles = new Projectile[40];

        Vector2 curCameraOffset;
        Vector2 resolution;

        System.Collections.Generic.List<Rectangle> levelSolidBBs;
        System.Collections.Generic.List<Rectangle> levelTopBBs;

        Matrix resolutionScaler;
        public Utility util;

        HUD testHUD;

        Random rand = new Random();

        int numCollisionSolutions = 0;

        #endregion

        #region Properties


        /// <summary>
        /// The logic for deciding whether the game is paused depends on whether
        /// this is a networked or single player game. If we are in a network session,
        /// we should go on updating the game even when the user tabs away from us or
        /// brings up the pause menu, because even though the local player is not
        /// responding to input, other remote players may not be paused. In single
        /// player modes, however, we want everything to pause if the game loses focus.
        /// </summary>
        new bool IsActive
        {

            get
            {
                if (networkSession == null)
                {
                    // Pause behavior for single player games.
                    return base.IsActive;
                }
                else
                {
                    // Pause behavior for networked games.
                    return !IsExiting;
                }
            }
        }

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(NetworkSession networkSession)
        {
            this.networkSession = networkSession;

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {

            baseRes = new Vector2(1280*1.2f, 720*1.2f);

            resolution = new Vector2(   ScreenManager.GraphicsDevice.PresentationParameters.BackBufferWidth,
                                        ScreenManager.GraphicsDevice.PresentationParameters.BackBufferHeight);

            curCameraOffset = Vector2.Zero;

            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
                util = new Utility(content);
            }

            

            bgImage = content.Load<Texture2D>("backgroundTest");

            backgroundTiles = new Texture2D[70,5];
            for (int i = 1; i < 71; i++)
            {
                backgroundTiles[i-1,0] = content.Load<Texture2D>(@"Background/BG/background_"+(i).ToString("00"));
                backgroundTiles[i-1,1] = content.Load<Texture2D>(@"Background/BG/background_"+(i+70).ToString("00"));
                backgroundTiles[i-1,2] = content.Load<Texture2D>(@"Background/BG/background_"+(i+140).ToString("00"));
                backgroundTiles[i-1,3] = content.Load<Texture2D>(@"Background/BG/background_"+(i+210).ToString("00"));
                backgroundTiles[i-1,4] = content.Load<Texture2D>(@"Background/BG/background_"+(i+280).ToString("00"));

            }
            levelSolidBBs = new System.Collections.Generic.List<Rectangle>(40);
            levelSolidBBs.Add(new Rectangle(0,660,5600,10));
            levelSolidBBs.Add(new Rectangle(0,-34,122,754));
            levelSolidBBs.Add(new Rectangle( 4230, 445 ,230 , 50 ));
            levelSolidBBs.Add(new Rectangle( 4520, 440, 500 , 400 ));
            //levelSolidBBs.Add(new Rectangle(5230, -110, 256, 830));
            levelSolidBBs.Add(new Rectangle( 5070 , 282 , 126 , 438 ));
            //levelSolidBBs.Add(new Rectangle(5620, -88, 774, 808));
            //levelSolidBBs.Add(new Rectangle( 6550, -276, 358, 996 ));
            //levelSolidBBs.Add(new Rectangle(7355, -460, 484, 1182));
            //levelSolidBBs.Add(new Rectangle(7980 ,-542, 245, 1260));
            //levelSolidBBs.Add(new Rectangle(8660, 138, 1474, 582));
            //levelSolidBBs.Add(new Rectangle(10538, 88, 600, 632));
            levelSolidBBs.Add(new Rectangle(11076, 626, 7494 , 220));
            levelSolidBBs.Add(new Rectangle(12240, 426, 356, 294));
            //levelSolidBBs.Add(new Rectangle(12620,258,682,462));
            levelSolidBBs.Add(new Rectangle(13300, 564, 5260, 156));
            levelSolidBBs.Add(new Rectangle(19222, 552, 4932, 20));
            levelSolidBBs.Add(new Rectangle(22265, 398, 76, 322));
            //levelSolidBBs.Add(new Rectangle(22404, 320, 21, 400));
            levelSolidBBs.Add(new Rectangle(22692, 208, 84, 542));
            levelSolidBBs.Add(new Rectangle(24166, -326, 792, 1046));

            levelTopBBs = new System.Collections.Generic.List<Rectangle>(40);
            levelTopBBs.Add(new Rectangle(1620, 542, 140, 180));
            levelTopBBs.Add(new Rectangle(1880, -22, 660, 50));
            levelTopBBs.Add(new Rectangle(3964, 480, 192, 50));
            levelTopBBs.Add(new Rectangle(4320, 360, 130, 50));
            levelTopBBs.Add(new Rectangle(4500, 286, 500, 200));
            levelTopBBs.Add(new Rectangle(5070, 64, 120, 20));
            levelTopBBs.Add(new Rectangle(8395, -560 ,20,154));
            levelTopBBs.Add(new Rectangle(8520, -330 ,20,154));
            levelTopBBs.Add(new Rectangle(18552, 368, 224, 352));
            levelTopBBs.Add(new Rectangle(233701,406,164,314));
            levelTopBBs.Add(new Rectangle(5620, -88, 774, 808));
            levelTopBBs.Add(new Rectangle(6550, -276, 358, 996));
            levelTopBBs.Add(new Rectangle(7355, -460, 484, 1182));
            levelTopBBs.Add(new Rectangle(7980, -542, 245, 1260));
            levelTopBBs.Add(new Rectangle(8660, 138, 1474, 582));
            levelTopBBs.Add(new Rectangle(10360, 88, 600, 632));
            levelTopBBs.Add(new Rectangle(12620, 258, 682, 462));
            levelTopBBs.Add(new Rectangle(22404, 320, 201, 400));
            levelTopBBs.Add(new Rectangle(5230, -110, 256, 830));
            levelTopBBs.Add(new Rectangle(22692, 178, 84, 542));
            levelTopBBs.Add(new Rectangle(18802, 434, 286,428));



            projectileTex = content.Load<Texture2D>("testProj");

            briefCaseSS = new AnimatedSpriteSheet(TimeSpan.Zero, true, content.Load<Texture2D>("briefcase"), new Vector2(48f, 48f), 1);
            

            healthTex = content.Load<Texture2D>("hat");
            hatSS = new AnimatedSpriteSheet(TimeSpan.Zero, true, healthTex, new Vector2(48f, 48f), 1);
            
            gameFont = content.Load<SpriteFont>("gamefont");
            diagFont = content.Load<SpriteFont>("diagFont");

            border = content.Load<Texture2D>(@"Background/border");

            testAnimTex = new AnimationTexture(TimeSpan.Zero, true);

            testProjectile = new AnimatedSpriteSheet(TimeSpan.Zero, true, projectileTex, new Vector2(projectileTex.Width, projectileTex.Height), 1);

            projectileSS = new AnimatedSpriteSheet[8];
            
            for (int i = 1; i < 9; i++)
            {
                projectileSS[i-1] = new AnimatedSpriteSheet(TimeSpan.Zero, true, content.Load<Texture2D>(@"Projectiles/item" + i),new Vector2(64f,64f),1);
            }


            for (int i = 1; i < 10; i++)
            {
                testAnimTex.AddFrame(content.Load<Texture2D>(@"testAnim1\tempAnim000" + i));
            }
            for (int i = 10; i< 28; i++)
            {
                testAnimTex.AddFrame(content.Load<Texture2D>(@"testAnim1\tempAnim00" + i));
            }
            

            for(int i = 0; i<projectiles.Length; i++)
            {
                projectiles[i] = new Projectile();
            }

            /*xml parsing test
            String xmlfile;
            firstXmlContent = Content.Load<string>(@"xmlfile");
            int[] enemy = new int[3];
            int[] Object = new int[4];
            */


            testAnimTex.ResetTimer(TimeSpan.Zero);
            testAnimTex.ReStartAnimation(TimeSpan.Zero);
            testAnimTex.SetFrameRate(30.0);

            testSS = new AnimatedSpriteSheet(TimeSpan.Zero, true, content.Load<Texture2D>(@"testAnim1\tempAnimSS"), new Vector2(256.0f, 192.0f), 27);
            testSS.ResetTimer(TimeSpan.Zero);
            testSS.ReStartAnimation(TimeSpan.Zero);
            testSS.SetFrameRate(45.0);
            this.ScreenManager.Game.ResetElapsedTime();

            charControllers.Add(new PlayerController(new Character(new Vector2(100F, 200F), 10F, 10, 10, content, 1.0f, "Player\\player", 100), PlayerInput.get(0)));
            charControllers.Add(new AIController(new Character(new Vector2(600F,200F), 10F, 10, 10, content, 1.0f, "Enemy\\ninja", 100), this));

            /*
            testHUD = new HUD(this.ScreenManager);
            testHUD.AddHudItem(healthTex, HudItemPos.topLeft, Vector2.Zero, 0.10f, "life", 3);
            testHUD.AddHudItem(healthTex, HudItemPos.topRight, Vector2.Zero, 0.10f, "lives", 4, diagFont);
            testHUD.AddHudItem(healthTex, HudItemPos.bottomLeft, Vector2.Zero, 0.10f, "cases", 2, diagFont);
            testHUD.AddHudItem(healthTex, HudItemPos.bottomRight, Vector2.Zero, 0.10f, "ammo", 80, diagFont);
            */

            

            resolutionScaler = Matrix.CreateScale(resolution.X / baseRes.X, resolution.Y / baseRes.Y, 1f);

        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion
        
        #region Update and Draw


        /// <summary>
        /// Updates the state of the game.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive) //&& Not Paused?
            {
                //calc camera
                Character camPlayer = charControllers[0].getCharacter();
                CalculateCameraOffset(new Vector2(camPlayer.position.X + (float)camPlayer.boundingBox.Center.X, camPlayer.position.Y + (float)camPlayer.boundingBox.Center.Y));

                if (centerTile % 10 == 0 && lastTile < centerTile)
                {
                    Vector2 pos = camPlayer.position;
                    pos.Y -= 800;
                    pos.X += Utility.rand.Next(-400, 400);
                    charControllers.Add(new AIController(new Character(pos, 10F, 10, 10, content, 1.0f, "Enemy\\ninja", 100), this));
                    lastTile = centerTile;
                }

                /*
                 * Loop through AI, call perform actions
                 * Loop through player controllers, call perform actions
                 */
                foreach (CharacterController charController in charControllers)
                {

                    charController.Update(gameTime);
                    Character character = charController.getCharacter();

                    if (character.genProjectile)
                    {
                        for (int i = 0; i < projectiles.Length; i++)
                        {
                            if (!projectiles[i].isActive)
                            {
                                Vector2 snap = Vector2.Zero;
                                if (character.flipHorizontal)
                                {
                                    snap.X = character.position.X + 30;
                                    snap.Y = character.position.Y + 110;
                                }
                                else
                                {
                                    snap.X = character.position.X + 140;
                                    snap.Y = character.position.Y + 110;
                                }
                                    projectiles[i].Initialize(snap, 1.0f, character.flipHorizontal ? (new Vector2(-15.0f, -5.0f) + character.velocity) : (new Vector2(15.0f, -5.0f) + character.velocity),
                                    (int)testProjectile.getBoundingBox().X, (int)testProjectile.getBoundingBox().Y, projectileSS[rand.Next(8)], null, gameTime.TotalGameTime.Add(TimeSpan.FromSeconds(5.0)), 1.0f, character);
                                projectiles[i].rotate = true;
                                break;
                            }
                        }
                        character.genProjectile = false;
                    }

                    if (character.tossBriefcase)
                    {
                        for (int i = 0; i < projectiles.Length; i++)
                        {
                            if (!projectiles[i].isActive)
                            {
                                Vector2 snap = Vector2.Zero;
                                if (character.flipHorizontal)
                                {
                                    snap.X = character.position.X + 30;
                                    snap.Y = character.position.Y + 110;
                                }
                                else
                                {
                                    snap.X = character.position.X + 140;
                                    snap.Y = character.position.Y + 110;
                                }
                                projectiles[i].Initialize(snap, 1.0f, character.flipHorizontal ? (new Vector2(-13.0f, -4.0f) + character.velocity) : (new Vector2(13.0f, -4.0f) + character.velocity),
                                (int)testProjectile.getBoundingBox().X, (int)testProjectile.getBoundingBox().Y, briefCaseSS, null, gameTime.TotalGameTime.Add(TimeSpan.FromSeconds(3.0)), 1.0f, character);
                                projectiles[i].rotate = true;
                                break;
                            }
                        }
                        character.tossBriefcase = false;
                    }

                }

                foreach (Projectile proj in projectiles)
                {
                    if (proj.isActive)
                    {
                        proj.Update(gameTime);
                    }
                }

                checkColliders(gameTime);
            }
            else
            {
                foreach (CharacterController charController in charControllers)
                {
                    charController.getCharacter().isActive = false;
                }

            }


            // If we are in a network game, check if we should return to the lobby.
            #region networking / not doing this atm
            /*
            if ((networkSession != null) && !IsExiting)
            {
                if (networkSession.SessionState == NetworkSessionState.Lobby)
                {
                    LoadingScreen.Load(ScreenManager, true,
                                       new BackgroundScreen(),
                                       new LobbyScreen(networkSession));
                }
            }*/
            #endregion
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(PlayerInput input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.PauseGame)
            {
                // If they pressed pause, bring up the pause menu screen.
                ScreenManager.AddScreen(new PauseMenuScreen(networkSession));
            }
        }
        
        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {

                /*RenderTarget2D rtt;
                Texture2D renderTex;

                PresentationParameters pp = ScreenManager.GraphicsDevice.PresentationParameters;

                rtt = new RenderTarget2D(ScreenManager.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1, ScreenManager.GraphicsDevice.DisplayMode.Format, pp.MultiSampleType, pp.MultiSampleQuality);
                ScreenManager.GraphicsDevice.SetRenderTarget(0, rtt);
            */

                ScreenManager.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer,
                                                   Color.White, 0, 0);
                // Our player and enemy are both actually just text strings.
                SpriteBatch spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);//ScreenManager.SpriteBatch;



                spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                        SpriteSortMode.Deferred,
                        SaveStateMode.None,
                        resolutionScaler);

                DrawBackground(gameTime, spriteBatch, charControllers[0].getCharacter().position);

                //spriteBatch.Draw(bgImage, new Vector2(-curCameraOffset.X, -curCameraOffset.Y), Color.White);

                //DrawDebugInfo(gameTime, spriteBatch);

                foreach (CharacterController charController in charControllers)
                {
                    charController.getCharacter().Draw(spriteBatch, gameTime, curCameraOffset, DEBUG, diagFont);
                }

                foreach (Projectile proj in projectiles)
                {
                    if (proj.isActive)
                    {
                        proj.draw(spriteBatch, curCameraOffset, gameTime, 1.0f, true);
                    }
                }


                DrawDebugInfo(gameTime, spriteBatch);
                spriteBatch.End();
            /*
                ScreenManager.GraphicsDevice.SetRenderTarget(0, null);

                renderTex = rtt.GetTexture();

                spriteBatch.Begin();

                spriteBatch.Draw(renderTex, Vector2.Zero, Color.White);
                //spriteBatch.Draw(border, Vector2.Zero, Color.White);
                //testHUD.draw(spriteBatch);
                DrawDebugInfo(gameTime, spriteBatch);
                spriteBatch.End();
            */
                

                // If the game is transitioning on or off, fade it out to black.
                if (TransitionPosition > 0)
                    ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }

        private void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, Vector2 playerPos)
        {
            float ourY = -curCameraOffset.Y - 1000.0f;
            centerTile = (int)((playerPos.X / 25200.0f) * 70);

            if (centerTile <= 2)
            {
                centerTile = 2;
            }
            if (centerTile >= 66)
            {
                centerTile = 66;
            }

            for (int i = centerTile - 2 ; i <= centerTile +3; i++)
            {
                spriteBatch.Draw(backgroundTiles[i, 0], new Vector2(-curCameraOffset.X + (i * 360.0f), ourY), Color.White);
                spriteBatch.Draw(backgroundTiles[i, 1], new Vector2(-curCameraOffset.X + (i * 360.0f), ourY + 360.0f), Color.White);
                spriteBatch.Draw(backgroundTiles[i, 2], new Vector2(-curCameraOffset.X + (i * 360.0f), ourY + 720.0f), Color.White);
                spriteBatch.Draw(backgroundTiles[i, 3], new Vector2(-curCameraOffset.X + (i * 360.0f), ourY + 1080.0f), Color.White);
                spriteBatch.Draw(backgroundTiles[i, 4], new Vector2(-curCameraOffset.X + (i * 360.0f), ourY + 1440.0f), Color.White);
            }

            spriteBatch.DrawString(gameFont, centerTile.ToString(), new Vector2(400f, 200f), Color.Red);

        }

        private void DrawDebugInfo(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (DEBUG)
            {


                spriteBatch.DrawString(diagFont, "Is Game Running Slowly: " + gameTime.IsRunningSlowly.ToString()+ "\n" + 
                                                 "Camera Offset: " + curCameraOffset.X + " , " + curCameraOffset.Y + "\n"+
                                                 "Current Camera Target: " + curTarget + "\n" +
                                                 "Collision Type: " +Collision.collisionType +"\n" +
                                                 "Num Collisions w/Player 1: " +numCollisionSolutions + "\n" +
                                                 (collisionAngle!=null?"Collision Angle = " +collisionAngle.Value.ToString() +"\n":"no Collision angle\n"),
                                                 new Vector2(10, 10), Color.Red);

                foreach (Rectangle bb in levelSolidBBs)
                {
                    util.drawBounding(new Rectangle((int)(bb.X - curCameraOffset.X), (int)(bb.Y - curCameraOffset.Y), bb.Width, bb.Height), false, spriteBatch, 5);
                }

                foreach (Rectangle bb in levelTopBBs)
                {
                    util.drawBounding(new Rectangle((int)(bb.X - curCameraOffset.X), (int)(bb.Y - curCameraOffset.Y), bb.Width, bb.Height), false, spriteBatch, 5);
                }

                DrawNetworkInfo(spriteBatch);

            }
        }

        private void DrawNetworkInfo(SpriteBatch spriteBatch)
        {
                if (networkSession != null)
                {
                    string message = "Players: " + networkSession.AllGamers.Count;
                    Vector2 messagePosition = new Vector2(100, 480);
                    spriteBatch.DrawString(gameFont, message, messagePosition, Color.White);

                }
        }

        private void checkColliders(GameTime time)
        {
            numCollisionSolutions = 0;
            Vector2 collisionSolution = Vector2.Zero;
            Collision.CollisionSolver solver;

            for (int i = 0; i < charControllers.Count; i++)
            {
                collisionSolution = Vector2.Zero;
                //CharacterController char1 = (CharacterController)charControllers.GetValue(i);
                #region junk
                /*
                for (int j = i + 1; j < charControllers.Length; j++)
                {
                    CharacterController char2 = (CharacterController)charControllers.GetValue(j);

                    solver = Collision.CheckCollision(char1.getCharacter().position, char2.getCharacter().position, char1.getCharacter(), char2.getCharacter(), time.TotalGameTime, false);

                    if (solver.collided)
                    {
                        char1.getCharacter().hasCollided = true;
                        char2.getCharacter().hasCollided = true;

                        collisionSolution += solver.correctionVector;

                        numCollisionSolutions++;
                        hasCollided = solver.collided;
                    }
                }
                


                if (hasCollided)
                {
                    if (Math.Abs(collisionSolution.X) > Math.Abs(collisionSolution.Y))
                    {
                        char1.getCharacter().position.Y += collisionSolution.Y;
                        char1.getCharacter().velocity.Y *= -0.001f;
                    }
                    else
                    {
                        char1.getCharacter().position.X += collisionSolution.X;
                        char1.getCharacter().velocity.X *= -0.001f;
                    }
                }
                 */
                #endregion
                for (int j = 0; j < projectiles.Length; j++)
                {
                    if (projectiles[j].isActive)
                    {
                        foreach (CharacterController char1 in charControllers)
                        {
                            if (projectiles[j].owner != char1.getCharacter())
                            {
                                solver = Collision.CheckCollision(projectiles[j].position, char1.getCharacter().position, (InteractiveObject)projectiles[j], (InteractiveObject)char1.getCharacter(), time.TotalGameTime, false);

                                if (solver.collided)
                                {
                                    char1.getCharacter().hasCollided = true;
                                    projectiles[j].Destroy();

                                    if (char1 == charControllers[0])
                                    {
                                        char1.getCharacter().health -= 3.0f;
                                    }
                                    else
                                    {
                                        char1.getCharacter().health -= 34.0f;
                                    }
                                    
                                    numCollisionSolutions++;
                                }
                            }
                        }
                    }
                }

                foreach (Rectangle bb in levelSolidBBs)
                {
                    for (int j = 0; j < projectiles.Length; j++)
                    {
                        if (projectiles[j].isActive)
                        {
                            foreach (CharacterController char1 in charControllers)
                            {
                                solver = Collision.CheckCollisionCustomBB(projectiles[j].position, new Vector2(bb.X, bb.Y), (InteractiveObject)projectiles[j], bb, time.TotalGameTime, false);

                                if (solver.collided)
                                {
                                    projectiles[j].Destroy();
                                    
                                    numCollisionSolutions++;
                                }
                            }
                        }

                        foreach (CharacterController char1 in charControllers)
                        {
                            solver = Collision.CheckCollisionCustomBB(char1.getCharacter().position, new Vector2(bb.X, bb.Y), char1.getCharacter(), bb, time.TotalGameTime, false);
                            if (solver.collided)
                            {
                                char1.getCharacter().hasCollided = true;


                                if (Math.Abs(solver.correctionVector.X) > Math.Abs(solver.correctionVector.Y))
                                {

                                        char1.getCharacter().position.Y -= solver.correctionVector.Y;
                                        char1.getCharacter().velocity.Y *= -0.01f;
                                }
                                else
                                {
                                    char1.getCharacter().position.X += solver.correctionVector.X;
                                    char1.getCharacter().velocity.X *= -0.001f;
                                }

                                numCollisionSolutions++;

                                solver.collided = false;
                            }
                        }
                    }
                }
                foreach (Rectangle bb in levelTopBBs)
                {
                    for (int j = 0; j < projectiles.Length; j++)
                    {
                        if (projectiles[j].isActive)
                        {
                            foreach (CharacterController char1 in charControllers)
                            {
                                solver = Collision.CheckCollisionCustomBB(projectiles[j].position, new Vector2(bb.X, bb.Y), (InteractiveObject)projectiles[j], bb, time.TotalGameTime, false);

                                if (solver.collided)
                                {
                                    projectiles[j].Destroy();

                                    numCollisionSolutions++;
                                }
                            }
                        }

                        foreach (CharacterController char1 in charControllers)
                        {

                            solver = Collision.CheckCollisionCustomBB(char1.getCharacter().position, new Vector2(bb.X, bb.Y), char1.getCharacter(), bb, time.TotalGameTime, false);
                            if (solver.collided)
                            {
                                char1.getCharacter().hasCollided = true;
                                if (Math.Abs(solver.correctionVector.X) > Math.Abs(solver.correctionVector.Y))
                                {
                                    if (solver.correctionVector.Y < 0.0f && (Math.Abs(solver.correctionVector.Y) < 40.0f))
                                    {
                                        char1.getCharacter().position.Y += solver.correctionVector.Y;
                                        char1.getCharacter().velocity.Y *= -0.01f;
                                    }
                                }
                                numCollisionSolutions++;

                            }
                       }
                    }
                }
            }
        }

        public void CalculateCameraOffset(Vector2 target)
        {

            //HOLY SHIT LOTS OF MAGIC NUMBERS, need some splanin to do.

            float Xdifference = target.X - ((resolution.X / 1.95f) + curCameraOffset.X); //General placement of player on screen 1/3.5 from left
            float Ydifference = target.Y - ((resolution.Y / 1.0f) + curCameraOffset.Y); // 1/1.65 from bottom

            curCameraOffset.X = curCameraOffset.X + Xdifference / 10;   // simple interpolation, the higher the divisor,
            curCameraOffset.Y = curCameraOffset.Y + Ydifference / 5;   // the slower the camera will meet that point.
    
        }

        #endregion
    }
}
