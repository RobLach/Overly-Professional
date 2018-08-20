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
    public class InteractiveObject : GameObject
    {
        #region Fields
        public float mass;
        public float friction = 0.9f;
        /// <summary>
        /// Units per tick
        /// </summary>
        public Vector2 velocity;
        public Rectangle boundingBox;

        public float maxSpeed = 9.0f;

        #endregion

        public InteractiveObject() { }

        /* Don't think we need this, but i dunno so commented out
         * 
        public InteractiveObject(float mass, Vector2 velocity, int boundingBoxWidth, int boundingBoxHeight)
        {
            this.position = new Vector2(0, 0);
            this.textureName = "NULL";
            this.mass = mass;
            this.velocity = velocity;
            this.boundingBox.X = 0;
            this.boundingBox.Y = 0;
            this.boundingBox.Width = boundingBoxWidth;
            this.boundingBox.Height = boundingBoxHeight;
            this.friction = 0.0f;
            this.fromSpriteSheet = false;
            
        }*/

        /// <summary>
        /// Good STUFF!
        /// Constructor for proper interactive objects frmo spritesheets.
        /// </summary>
        public InteractiveObject(Vector2 pos,  float mass, Vector2 velocity, int boundingBoxWidth, int boundingBoxHeight, AnimatedSpriteSheet animSS)
            :base(pos, animSS)
        {
            construct(mass, velocity, boundingBoxWidth, boundingBoxHeight);
        }



        /// <summary>
        /// Good STUFF!
        /// Constructor for proper interactive objects frmo animatedTextures.
        /// </summary>
        public InteractiveObject(Vector2 pos, float mass, Vector2 velocity, int boundingBoxWidth, int boundingBoxHeight, AnimationTexture animT)
            :base(pos, animT)
        {
            construct(mass, velocity, boundingBoxWidth, boundingBoxHeight);
        }

        protected void construct(float mass, Vector2 velocity, int boundingBoxWidth, int boundingBoxHeight)
        {
            this.mass = mass;
            this.velocity = velocity;
            this.boundingBox = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), boundingBoxWidth, boundingBoxHeight);
        }

        public InteractiveObject(Vector2 pos, float mass, Vector2 velocity, int boundingBoxWidth, int boundingBoxHeight, AnimationTexture animT, float scaler)
            :this(pos, mass,velocity,boundingBoxWidth, boundingBoxHeight, animT)
        {
            this.scaler = scaler;
        }


        public InteractiveObject(Vector2 pos, float mass, Vector2 velocity, int boundingBoxWidth, int boundingBoxHeight, AnimatedSpriteSheet animSS, float scaler)
            : this(pos, mass, velocity, boundingBoxWidth, boundingBoxHeight, animSS)
        {
            this.scaler = scaler;
        }
        /// <summary>
        /// UpdatesObject
        /// </summary>
        /// <param name="time"></param>
        public override void Update(GameTime time)
        {
            this.position = this.position + this.velocity;

            if (rotate)
            {
                this.currentRotation += MathHelper.ToRadians(random.Next(3)+1);
            }

            if (this.position.Y < 400.0f)
            {
                this.velocity.Y += 0.32f; // 9.8 / 30
            }
            else
            {
                this.velocity.Y = 0.0f;
            }
            //Collision checks, other stuff, etc.

        }

        /* BIG BATCH OF AWESOME (OPEN TO SEE)
         
|                                                                              |
|                                                                              |
|        -------------------------------                                       |
|        |                             |                                       |
|        |     JoJo's Coffee Shop      |                                       |
|        |                             |                                       |
|        ------------------------------                                        |                               
|                                                                              |
|     ___                                                    /\/\/\/|          |
|    /   \                                                   |      |          |
|   / / **      ______                       ________        (<)(<)--3         |
|   | \ -/     /     /                       \       \       |      |          |
|    \| |   < /  @  /  ) ) )           ( ( (  \   @   \ > >  | ---  |    ~     |
|    /   \ / ------                            --------  \ \  \____/    [ ]=   |
|----------------------                     ------------------------------------
|----------------------                     ------------------------------------ 
|    \---|--         | |                    | |          ------     |          |
|    |    \ \        | |                    | |          |  --------|          |
|                                                           |                  |
--------------------------------------------------------------------------------                                                                              |     |
|  Ethereal Packet Analyzer                                                    |
|==============================================================================|
|                                                                              |
|  0000   00 03 52 02 c1 73 00 0b 7d 0b a6 b5 08 00 45 00  Alicia232: Hey,..   |
|  0010   02 57 c6 0e 40 00 80 06 32 91 c0 a8 00 2b 45 24  what's up?.......   |
|  0020   fa 09 05 b4 00 50 03 6a 97 14 63 65 57 e4 50 18  ..BuckeyeKim:....   |
|  0030   3f 74 7a 2d 00 00 50 4f 53 54 20 2f 6d 63 6d 64  Not much, there..   |
|  0040   2f 73 65 6e 64 20 48 54 54 50 2f 31 2e 31 0d 0a  's this really...   |
|  0050   48 6f 73 74 3a 20 77 77 77 6c 2e 6d 65 65 62 6f  cute guy across..   |
|  0060   2e 63 6f 6d 0d 0a 55 73 65 72 2d 41 67 65 6e 74  from me on his...   |
|  0070   3a 20 4d 6f 7a 69 6c 6c 61 2f 35 2e 30 20 28 57  laptop. He looks.   |
|  0080   69 6e 64 6f 77 73 3b 20 55 3b 20 57 69 6e 64 6f  so into what.....   |
|  0090   77 73 20 4e 54 20 35 2e 31 3b 20 65 6e 2d 55 53  he's doing. I....   |
|  00a0   3b 20 72 76 3a 31 2e 38 2e 30 2e 39 29 20 47 65  think I wanna....   |
|  00b0   63 6b 6f 2f 32 30 30 36 31 32 30 36 20 46 69 72  do him...........   |
|  00c0   65 66 6f 78 2f 31 2e 35 2e 30 2e 39 0d 0a 41 63  efox/1.5.0.9..Ac    |
|  00d0   63 65 70 74 3a 20 61 70 70 6c 69 63 61 74 69 6f  cept: applicatio    |
|  00e0   6e 2f 78 2d 73 68 6f 63 6b 77 61 76 65 2d 66 6c  n/x-shockwave-fl    |
|  00f0   61 73 68 2c 74 65 78 74 2f 78 6d 6c 2c 61 70 70  ash,text/xml,app    |
|  0100   6c 69 63 61 74 69 6f 6e 2f 78 6d 6c 2c 61 70 70  lication/xml,app    |
|  0110   6c 69 63 61 74 69 6f 6e 2f 78 68 74 6d 6c 2b 78  lication/xhtml+x    |
|  0120   6d 6c 2c 74 65 78 74 2f 68 74 6d 6c 3b 71 3d 30  ml,text/html;q=0    |
|  0130   2e 39 2c 74 65 78 74 2f 70 6c 61 69 6e 3b 71 3d  .9,text/plain;q=    |
|                                                                              |
|                                                                              |
--------------------------------------------------------------------------------
|                                                                              |
|                                                                              |
|                                                                              |
|     ___                                                     /\/\/\/|         |
|    /   \                                                    |      |         |
|   / / **      ______                       /                (!)(!)--3        |
|   | \ -/     /     /                  /   /                 |      |         |
|    \| |   < /  @  /  ) ) )       /                     \ \  | --   |         |
|    /   \ / ------                    /                  \ \  \____/          |
|----------------------             /       ------------------------------------
|----------------------     ________    /   ------------------------------------ 
|    \---|--         | |    \       \       | |          ------     |          |
|    |    \ \        | |     \   @   \  [ ] | |          |  --------|          |
|                             --------  ~~~ | |             |                  |
|                                       ~ ~                                    |
|                                        ~                                     |
*/

    }
}
