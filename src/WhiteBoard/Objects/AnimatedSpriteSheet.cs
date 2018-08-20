using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WhiteBoard.Objects
{
    public class AnimatedSpriteSheet
    {
        #region Fields
        private double frameRate = 3.0; //Frames/Second 
        private Texture2D spriteSheet;
        private TimeSpan timeToNextFrame = TimeSpan.Zero;
        private Queue<Rectangle> ssCoords;
        private Rectangle firstRectangle;
        private Boolean looping;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="now"></param>
        /// <param name="loop"></param>
        /// <param name="spriteSheet"></param>
        /// <param name="spriteSize"></param>
        public AnimatedSpriteSheet(TimeSpan now, Boolean loop, Texture2D spriteSheet, Vector2 spriteSize, int numFrames)
        {
            float numWidth = spriteSheet.Width / spriteSize.X; 
            float numHeight = spriteSheet.Height / spriteSize.Y;
            ssCoords = new Queue<Rectangle>((int)(numWidth * numHeight));
            looping = loop;

            this.spriteSheet = spriteSheet;

            timeToNextFrame = now.Add(TimeSpan.FromSeconds(1.0 / frameRate));

            int framesAdded = 0;

            for (int i = 0; i < numHeight; i++)
            {
                for (int j = 0; j < numWidth; j++)
                {
                    if (framesAdded >= numFrames)
                    {
                        break;
                    }
                    Rectangle temp = new Rectangle(j * (int)spriteSize.X, i * (int)spriteSize.Y, (int)spriteSize.X, (int)spriteSize.Y);
                    if (i == 0 && j == 0)
                    {
                        firstRectangle = temp;
                    }
                    ssCoords.Enqueue(temp);
                    framesAdded++;
                }
            }
        }

        /// <summary>
        /// Resets Animation Timer
        /// </summary>
        /// <param name="now"></param>
        public void ResetTimer(TimeSpan now)
        {
            timeToNextFrame = now.Add(TimeSpan.FromSeconds(1.0 / frameRate));
        }

        public void SetFrameRate(double rate)
        {
            this.frameRate = rate;
        }

        public void ReStartAnimation(TimeSpan now)
        {
            RotateToFirstFrame();
            timeToNextFrame = now.Add(TimeSpan.FromSeconds(1.0 / frameRate));

        }

        public Rectangle  getCurrentFrame(TimeSpan now)
        {
            if (timeToNextFrame < now)
            {
                timeToNextFrame = now.Add(TimeSpan.FromSeconds(1.0 / frameRate));
                ssCoords.Enqueue(ssCoords.Dequeue());
            }
            return ssCoords.Peek();
        }

        public void drawCurrentFrame(TimeSpan now, SpriteBatch batch, Vector2 position, bool flipHorizontal)
        {
            batch.Draw(spriteSheet, position, getCurrentFrame(now), Color.White);
        }

        public void drawCurrentFrame(TimeSpan now, SpriteBatch batch, Vector2 position, float scaler, bool flipHorizontal, Color color, float rotation, bool realCenter)
        {
            if (realCenter)
            {
                batch.Draw(spriteSheet, position, getCurrentFrame(now), color, rotation, new Vector2(getCurrentFrame(now).Center.X, getCurrentFrame(now).Center.Y), scaler, flipHorizontal ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
            }
            else
            {
                batch.Draw(spriteSheet, position, getCurrentFrame(now), color, rotation, Vector2.Zero, scaler, flipHorizontal ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
            }
        }

        private void RotateToFirstFrame()
        {
            while (!ssCoords.Peek().Equals(firstRectangle))
            {
                ssCoords.Enqueue(ssCoords.Dequeue());
            }
        }

        public Vector2 getBoundingBox()
        {
            return new Vector2(firstRectangle.Width, firstRectangle.Height);

        }
        
        public Texture2D getSpriteSheet()
        {
            return spriteSheet;
            
        }
    }
}
