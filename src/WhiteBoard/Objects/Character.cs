using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;
using WhiteBoard.Controller;

namespace WhiteBoard.Objects
{
    public class Character : InteractiveObject
    {
        #region Fields
        //Player Attributes and stuff.

        public float acceleration = 0.5f;

        public bool genProjectile = false;
        public bool tossBriefcase = false;
        public bool deadFlag = false;

        public bool jumping = false;
        public bool freeFalling = false;
        public bool shooting = false;
        public bool newShot = false;
        public bool punching = false;

        //Animation Stuff        
        AnimationTexture idle;
        AnimationTexture run;
        AnimationTexture punch;
        AnimationTexture throwCase;
        AnimationTexture throwItem;
        AnimationTexture jump;

        private string previousAnim = "Idle";

        public TimeSpan shootCoolDown = TimeSpan.Zero;
        public TimeSpan punchCoolDown = TimeSpan.Zero;

        public enum GameAction
        {
            walkLeft, walkRight,
            jump, melee, shoot, toss, none
        }

        public GameAction[] defaultAction = new GameAction[1] { GameAction.none };
        private GameAction[] lastActions;

        public float health;

        #endregion

        public Character(Vector2 pos, float mass, int width, int height, ContentManager content, float scaler, String ani_prefix, float h)
        {
            this.LoadTextures(content, ani_prefix);
            this.position = pos;
            this.aT = run;
            this.scaler = scaler;
            this.health = h;
            base.construct(mass, Vector2.Zero, width, height);
        }

        public void initialize(Vector2 pos, float mass, int width, int height, ContentManager content, float scaler, String ani_prefix, float h)
        {
            this.LoadTextures(content, ani_prefix);
            this.position = pos;
            this.aT = run;
            this.scaler = scaler;
            this.health = h;
            base.construct(mass, Vector2.Zero, width, height);
            this.isActive = true;
        }

        public void destroy()
        {
            this.isActive = false;
        }

        public override void Update(GameTime time)
        {
            if (deadFlag || health < 0)
            {
                deadFlag = true;
                this.isActive = false;
                return;
            }

            this.isActive = true;

            if (velocity.X > 0.0f)
            {
                this.flipHorizontal = false;
            }
            else if (velocity.X < 0.0f)
            {
                this.flipHorizontal = true;
            }

            if (newShot)
            {
                shootCoolDown = time.TotalGameTime.Add(TimeSpan.FromSeconds(0.3));
                newShot = false;
            }

            if (shooting)
            {
                if (this.currentAnim != "Shoot")
                {
                    this.previousAnim = this.currentAnim;
                    this.currentAnim = "Shoot"; 
                    this.punchCoolDown = time.TotalGameTime.Add(TimeSpan.FromSeconds(0.3));

                }
                if (shootCoolDown < time.TotalGameTime)
                {
                    this.currentAnim = this.previousAnim;
                    shooting = false;
                }
            }
            else if (jumping)
            {
                if (this.currentAnim != "Jump")
                {
                    this.animationLibrary["Jump"].ResetTimer(time.TotalGameTime);
                    this.animationLibrary["Jump"].ReStartAnimation(time.TotalGameTime);
                    this.velocity.Y = -12.0f;
                }

                this.currentAnim = "Jump";

                if (Math.Abs(this.velocity.Y) <= 0.1f)
                {
                    this.jumping = false;
                    this.freeFalling = true;
                }

            }
            else if (freeFalling)
            {
                this.currentAnim = "Jump";
                if (Math.Abs(this.velocity.Y) <= 2.0f)
                {
                    this.freeFalling = false;
                    this.currentAnim = "Idle";
                }
            }
            else if (punching)
            {
                if (this.currentAnim != "Punch")
                {
                    this.currentAnim = "Punch";
                    this.animationLibrary["Punch"].ReStartAnimation(time.TotalGameTime);
                    shootCoolDown = time.TotalGameTime.Add(TimeSpan.FromSeconds(0.3));
                }

                if (time.TotalGameTime < shootCoolDown)
                {
                    this.currentAnim = "Punch";
                    punching = true;
                }
                else
                {
                    punching = false;
                }
               
            }
            else if (Math.Abs(velocity.X) > 1.5f)
            {
                this.currentAnim = "Walk";
            }
            else
            {
                this.currentAnim = "Idle";
            }

            this.position = this.position + this.velocity;

            /*if (this.position.Y < 400.0f)
            {*/
                this.velocity.Y += 0.32f; // 9.8 / 30
            /*}
            else
            {
                this.velocity.Y = 0.0f;
            }*/

            //Collision checks, other stuff, etc.

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 curCameraOffset, bool DEBUG, SpriteFont diagFont)
        {

            if (isActive)
            {
                draw(spriteBatch, curCameraOffset, gameTime, 1.0f, false);

                Vector2 posMinusOffset = position - curCameraOffset;
                spriteBatch.DrawString(diagFont, "health: " + health,
                        Vector2.Add(posMinusOffset, new Vector2(50, -15)), Color.Red);

                if (DEBUG)
                {
                    //utility.drawBounding(new Rectangle((int)posMinusOffset.X, (int)posMinusOffset.Y, (int)getBoundingBox().X, (int)getBoundingBox().Y),
                    //                                    hasCollided, spriteBatch, 5); //Bounding Boxes

                    hasCollided = false;

                    spriteBatch.DrawString(diagFont, velocity.ToString(),
                        Vector2.Add(posMinusOffset, new Vector2(50, -25)), Color.Red);

                    String actions = "";
                    foreach (Character.GameAction act in getLastActions())
                        actions = actions + act.ToString() + ",";

                    spriteBatch.DrawString(diagFont, actions,
                        Vector2.Add(posMinusOffset, new Vector2(50, -35)), Color.Red);
                }
            }
        }

        public void handleActions(GameAction[] gameActs)
        {
            lastActions = gameActs;
            foreach (GameAction gAct in gameActs)
                handleAction(gAct);
        }

        public GameAction[] getLastActions()
        {
            if (lastActions == null)
                return this.defaultAction;
            return lastActions;
        }

        public void handleAction(GameAction gAct)
        {
            if ((shooting || punching) && Math.Abs(this.velocity.Y) < 0.1f)
            {
                if ((gAct != GameAction.shoot) || (gAct != GameAction.melee))
                {
                    gAct = GameAction.none;
                }
            }
            switch (gAct)
            {

                case GameAction.none:
                    if (Math.Abs(this.velocity.X) > 0.15f)
                    {
                        this.velocity.X *= this.friction;
                    }
                    else
                    {
                        this.velocity.X = 0;
                    }
                    break;

                //When you jump too much and land, you go through the floor!
                case GameAction.jump:

                    if ((this.velocity.Y < 1.4f) && (this.velocity.Y > -1.4f)) // currently this, but should be if(collidingBottom == true) or something liek that.
                    {

                        if (!jumping || !freeFalling)
                        {
                            jumping = true;
                        }

                    }
                    break;

                case GameAction.walkLeft:
                    if (this.velocity.X > 0)
                    {
                        this.velocity.X *= this.friction;
                    }

                    if (this.velocity.X > -maxSpeed)
                    {
                        this.velocity.X -= acceleration;
                    }

                    break;

                case GameAction.walkRight:
                        if (this.velocity.X < 0)
                        {
                            this.velocity.X *= this.friction;
                        }
                        if (this.velocity.X < maxSpeed)
                        {
                            this.velocity.X += acceleration;
                        } 
                    break;

                case GameAction.shoot:
                    genProjectile = true;
                    shooting = true;
                    newShot = true;
                    if (!jumping)
                    {
                        goto case GameAction.none;
                    }
                    else
                    { break; }

                case GameAction.melee:
                    punching = true;
                    break;

                case GameAction.toss:
                    tossBriefcase = true;
                    shooting = true;
                    newShot = true;
                    if (!jumping)
                    {
                        goto case GameAction.none;
                    }
                    break;
            }

        }

        public void LoadTextures(ContentManager content, String prefix)
        {
            //String prefix = "Player\\player";

            idle = new AnimationTexture(TimeSpan.Zero, true);
            idle.AddFrame(content.Load<Texture2D>(@prefix+"_idle"));
            idle.ResetTimer(TimeSpan.Zero);
            idle.SetFrameRate(0.0001);
            idle.ReStartAnimation(TimeSpan.Zero);

            run = new AnimationTexture(TimeSpan.Zero, true);
            try
            {
                run.AddFrame(content.Load<Texture2D>(@prefix+"_run2"));
                run.AddFrame(content.Load<Texture2D>(@prefix+"_run1"));
                run.AddFrame(content.Load<Texture2D>(@prefix+"_run2"));
                run.AddFrame(content.Load<Texture2D>(@prefix+"_run3"));
                run.SetFrameRate(5.0);
                run.ReStartAnimation(TimeSpan.Zero);
                run.ResetTimer(TimeSpan.Zero);
            }
            catch (Exception e)
            {   run = idle; }

            try
            {
                jump = new AnimationTexture(TimeSpan.Zero, false);
                jump.AddFrame(content.Load<Texture2D>(@prefix+"_jump1"));
                jump.AddFrame(content.Load<Texture2D>(@prefix+"_jump2"));
                jump.AddFrame(content.Load<Texture2D>(@prefix+"_jump3"));
                jump.AddFrame(content.Load<Texture2D>(@prefix+"_jump4"));
                jump.SetFrameRate(15.0);
                jump.ReStartAnimation(TimeSpan.Zero);
                jump.ResetTimer(TimeSpan.Zero);
            }
            catch (Exception e)
            {   jump = idle; }

            try
            {
                throwItem = new AnimationTexture(TimeSpan.Zero, true);
                throwItem.AddFrame(content.Load<Texture2D>(@prefix+"_shoot"));
                throwItem.SetFrameRate(0.0001);
                throwItem.ResetTimer(TimeSpan.Zero);
                throwItem.ReStartAnimation(TimeSpan.Zero);
            }
            catch (Exception e)
            { throwItem = idle; }

            try
            {
                punch = new AnimationTexture(TimeSpan.Zero, false);
                punch.AddFrame(content.Load<Texture2D>(@prefix+"_throwing3"));
                punch.AddFrame(content.Load<Texture2D>(@prefix+"_throwing2"));
                punch.AddFrame(content.Load<Texture2D>(@prefix+"_throwing1"));
                punch.AddFrame(content.Load<Texture2D>(@prefix+"_throwing4"));
                punch.SetFrameRate(10.0);
                punch.ResetTimer(TimeSpan.Zero);
                punch.ReStartAnimation(TimeSpan.Zero);
                punch.SetFrameRate(10.0);
            }
            catch (Exception e)
            { punch = idle; }

            addAnimation("Idle", idle);
            addAnimation("Walk", run);
            addAnimation("Jump", jump);
            addAnimation("Shoot", throwItem);
            addAnimation("Punch", punch);
        }
    }
}
