using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;


namespace WhiteBoard.Objects
{
    class Projectile : InteractiveObject
    {
        public TimeSpan ttl;
        public bool isActive = false;
        public Character owner;
        public float rotVal = 0.0f;


        public Projectile()
            :base(Vector2.Zero, 0.0f, Vector2.Zero, 0,0, (AnimatedSpriteSheet)null)
        {
            this.aT = null;
            this.isActive = false;
            this.scaler = 1.0f;
        }

        public Projectile(Vector2 pos, float mass, Vector2 velocity,
                                int boundingBoxWidth, int boundingBoxHeight,
                                AnimatedSpriteSheet animSS, TimeSpan timeToLive, float scaler)
            :base(pos,mass,velocity,boundingBoxWidth,boundingBoxHeight,animSS)
        {
            this.ttl = timeToLive;
            this.owner = null;
        }

        public Projectile(Vector2 pos, float mass, Vector2 velocity,
                                int boundingBoxWidth, int boundingBoxHeight,
                                AnimationTexture animT, TimeSpan timeToLive, float scaler)
            : base(pos, mass, velocity, boundingBoxWidth, boundingBoxHeight, animT)
        {
            this.ttl = timeToLive;
            this.owner = null;
        }

        public void Initialize(Vector2 pos, float mass, Vector2 velocity,
                                int boundingBoxWidth, int boundingBoxHeight,
                                AnimatedSpriteSheet animSS, AnimationTexture animTex,
                                TimeSpan timeToLive, float scaler, Character owner)
        {
            this.position = pos;
            this.mass = mass;
            this.velocity = velocity;
            if (animSS != null)
            {
                Vector2 temp = animSS.getBoundingBox();
                this.boundingBox.X = (int)pos.X;
                this.boundingBox.Y = (int)pos.Y;
                this.boundingBox.Width = (int)(temp.X*scaler);
                this.boundingBox.Height = (int)(temp.Y * scaler);
                this.aSS = animSS;
               
            }
            else if (animTex != null)
            {
                Vector2 temp = animTex.getBoundingBox();
                this.boundingBox.X = (int)pos.X;
                this.boundingBox.Y = (int)pos.Y;
                this.boundingBox.Width = (int)(temp.X * scaler);
                this.boundingBox.Height = (int)(temp.Y * scaler);
                this.aT = animTex;
            }

            this.isActive = true;
            this.ttl = timeToLive;
            this.scaler = scaler;
            this.owner = owner;
            this.currentRotation = MathHelper.ToRadians(random.Next(75));
            this.rotVal += MathHelper.ToRadians(random.Next(7) + 1);
        }

        public void Destroy()
        {
            this.isActive = false;
            this.rotVal = 0.0f;
        }

        public void Update(GameTime gameTime)
        {
            this.position = this.position + this.velocity;

                this.velocity.Y += 0.32f; // 9.8 / 30

                if (this.rotate)
                {
                    this.currentRotation += rotVal;
                }

            if (gameTime.TotalGameTime.CompareTo(ttl) > 0)
            {
                Destroy();
            }

        }

    }
}
