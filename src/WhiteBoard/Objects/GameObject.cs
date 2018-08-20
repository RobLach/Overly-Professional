#region Using Statements
using System;
using System.Threading;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;
#endregion

namespace WhiteBoard.Objects
{
    public class GameObject
    {
        #region Fields
        public Vector2 position;
        public AnimatedSpriteSheet aSS = null;
        public AnimationTexture aT = null;
        public bool isActive = false;
        public bool hasCollided = false;
        public Color color = Color.White;
        public bool rotate = false;
        public float currentRotation = 0.0f;
        public bool flipHorizontal = false;
        public float scaler;
        public string currentAnim = "Idle";

        public Random random;

        public System.Collections.Generic.Dictionary<string, AnimationTexture> animationLibrary;
        #endregion

        public GameObject()
        {
            this.animationLibrary = new System.Collections.Generic.Dictionary<string, AnimationTexture>(6);
            random = new Random();
        }

        public GameObject(Vector2 pos, AnimationTexture animTex)
            : this()
        {
            this.position = pos;
            this.aT = animTex;
            scaler = 1.0f;
        }

        public GameObject(Vector2 pos, AnimatedSpriteSheet animSS)
            : this()
        {
            this.position = pos;
            this.aSS = animSS;
            scaler = 1.0f;
        }

        public GameObject(Vector2 pos, AnimationTexture animTex, float scaler):this(pos,animTex)
        {
            this.scaler = scaler;
        }

        public GameObject(Vector2 pos, AnimatedSpriteSheet animSS, float scaler):this(pos, animSS)
        {
            this.scaler = scaler;
        }

        public virtual void addAnimation(string animName, AnimationTexture animTex)
        {
            this.animationLibrary.Add(animName, animTex);
            
        }

        public virtual void Update(GameTime time)
        {
        }

        /// <summary>
        /// draws game object
        /// REMINDER: Spritebatch must already be running.
        /// </summary>
        /// <param name="spriteBatch">batch passed in</param>
        public virtual void draw(SpriteBatch spriteBatch, Vector2 cameraOffset, GameTime time, bool realCenter)
        {
            if (aSS != null)
            {
                aSS.drawCurrentFrame(time.TotalGameTime, spriteBatch, this.position - cameraOffset, scaler, flipHorizontal, color, currentRotation, realCenter);
            }
            else if (aT != null)
            {
                aT.drawCurrentFrame(time.TotalGameTime, spriteBatch, this.position - cameraOffset, scaler, flipHorizontal, color, currentRotation);
            }
        }

        /// <summary>
        /// draws game object
        /// REMINDER: Spritebatch must already be running.
        /// </summary>
        /// <param name="spriteBatch">batch passed in</param>
        public virtual void draw(SpriteBatch spriteBatch, Vector2 cameraOffset, GameTime time, float scaler, bool realCenter)
        {
            if (aSS != null)
            {
                aSS.drawCurrentFrame(time.TotalGameTime, spriteBatch, this.position - cameraOffset, scaler, flipHorizontal, color, currentRotation, realCenter);
            }
            else if (aT != null)
            {
                
                    //aT.drawCurrentFrame(time.TotalGameTime, spriteBatch, this.position - cameraOffset, scaler, flipHorizontal, color, currentRotation);
                this.animationLibrary[currentAnim].drawCurrentFrame(time.TotalGameTime, spriteBatch, this.position - cameraOffset, scaler, flipHorizontal, color, this.currentRotation);

            }
        }

        public Vector2 getBoundingBox()
        {
            if (aSS != null)
            {
                return aSS.getBoundingBox();
            }
            else if (aT != null)
            {
                return aT.getBoundingBox();
            }
            return Vector2.Zero;
        }

        
    }
}
