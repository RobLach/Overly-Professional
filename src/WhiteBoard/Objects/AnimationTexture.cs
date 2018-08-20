using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WhiteBoard.Objects
{
    public class AnimationTexture
    {
        #region Fields
        private double frameRate = 3.0; //Frames/Second 
        private Queue<Texture2D> textures;
        private TimeSpan timeToNextFrame = TimeSpan.Zero;
        private Texture2D firstFrame;
        private Texture2D lastFrame;
        private Boolean looping;
        #endregion

        public AnimationTexture(TimeSpan now, Boolean loop)
        {
            textures = new Queue<Texture2D>();
            looping = loop;
            timeToNextFrame = now.Add(TimeSpan.FromSeconds(1.0 / frameRate));
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

        public void AddFrame(Texture2D tex)
        {
            lastFrame = tex;
            textures.Enqueue(tex);
            if (textures.Count == 1)
            {
                firstFrame = textures.Peek();
            }
        }

        public void ReStartAnimation(TimeSpan now)
        {
            RotateToFirstFrame();
            timeToNextFrame = now.Add(TimeSpan.FromSeconds(1.0 / frameRate));

        }

        public Texture2D getCurrentFrame(TimeSpan now)
        {
            if (timeToNextFrame < now)
            {
                timeToNextFrame = now.Add(TimeSpan.FromSeconds(1.0 / frameRate));
                if (!looping)
                {
                    if (textures.Peek() == lastFrame)
                    {
                        return textures.Peek();
                    }
                }
                textures.Enqueue(textures.Dequeue());
            }

            return textures.Peek();
        }

        public void drawCurrentFrame(TimeSpan now, SpriteBatch batch, Vector2 position)
        {
            batch.Draw(getCurrentFrame(now), position, Color.White);
        }

        public void drawCurrentFrame(TimeSpan now, SpriteBatch batch, Vector2 position, float scaler, bool flipHorizontal, Color color, float rotation)
        {
            batch.Draw(getCurrentFrame(now), position, null, color, rotation, Vector2.Zero, scaler, flipHorizontal ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
        
        }

        private void RotateToFirstFrame()
        {
            while (!textures.Peek().Equals((Texture2D)firstFrame))
            {
                textures.Enqueue(textures.Dequeue());
            }
        }

        public Vector2 getBoundingBox()
        {
            return new Vector2(firstFrame.Width, firstFrame.Height);
        }

    }
}
