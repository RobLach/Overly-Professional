using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WhiteBoard.Controller;
using WhiteBoard.Objects;
using System.Linq;
using System.Text;

namespace WhiteBoard.Collisions
{
    public static class Collision
    {
        public struct CollisionSolver
        {
            public bool collided;
            public Vector2 correctionVector;
        }

        public static String collisionType = "Bounding Box";

        public static CollisionSolver CheckCollision(Vector2 pos1, Vector2 pos2, Character char1, Character char2, TimeSpan now, bool perPixel)
        {
            CollisionSolver solver = new CollisionSolver();
            collisionType = "Bounding-Box";
            if (char1.aSS != null)
            {
                if (char2.aSS != null)
                {
                    return CheckCollision(pos1, pos2, char1.aSS, char2.aSS, now, perPixel);
                }
                else if (char2.aT != null)
                {
                    return CheckCollision(pos1, pos2, char1.aSS, char2.animationLibrary[char2.currentAnim], now, perPixel);
                }
            }
            else
            {
                if (char2.aSS != null)
                {
                    return CheckCollision(pos1, pos2, char1.animationLibrary[char1.currentAnim], char2.aSS, now, perPixel);
                }
                else if (char2.aT != null)
                {
                    return CheckCollision(pos1, pos2, char1.animationLibrary[char1.currentAnim], char2.animationLibrary[char2.currentAnim], now, perPixel);
                }
            }

            solver.collided = false;
            return solver;
        }

        public static CollisionSolver CheckCollision(Vector2 pos1, Vector2 pos2, InteractiveObject char1, InteractiveObject char2, TimeSpan now, bool perPixel)
        {
            CollisionSolver solver = new CollisionSolver();
            collisionType = "Bounding-Box";
            if (char1.aSS != null)
            {
                if (char2.aSS != null)
                {
                    return CheckCollision(pos1, pos2, char1.aSS, char2.aSS, now, perPixel);
                }
                else if (char2.aT != null)
                {
                    return CheckCollision(pos1, pos2, char1.aSS, char2.animationLibrary[char2.currentAnim], now, perPixel);
                }
            }
            else
            {
                if (char2.aSS != null)
                {
                    return CheckCollision(pos1, pos2, char1.animationLibrary[char1.currentAnim], char2.aSS, now, perPixel);
                }
                else if (char2.aT != null)
                {
                    return CheckCollision(pos1, pos2, char1.animationLibrary[char1.currentAnim], char2.animationLibrary[char2.currentAnim], now, perPixel);
                }
            }

            solver.collided = false;
            return solver;
        }

        public static CollisionSolver CheckCollision(Vector2 pos1, Vector2 pos2, Texture2D collider1, Texture2D collider2, bool perPixel)
        {
            CollisionSolver solver = new CollisionSolver();
            collisionType = "Bounding-Box";
            Rectangle collider1Range = new Rectangle(0, 0, collider1.Width, collider1.Height);
            Rectangle collider2Range = new Rectangle(0, 0, collider2.Width, collider2.Height);
            collider1Range.X = (int)pos1.X;
            collider1Range.Y = (int)pos1.Y;
            collider2Range.X = (int)pos2.X;
            collider2Range.Y = (int)pos2.Y;

            if ((collider1Range.Left > collider2Range.Right && collider1Range.Right > collider2Range.Left) ||
                (collider2Range.Left > collider1Range.Right && collider2Range.Right > collider1Range.Left) ||
                (collider1Range.Top < collider2Range.Bottom && collider1Range.Bottom < collider2Range.Top) ||
                (collider2Range.Top < collider1Range.Bottom && collider2Range.Bottom < collider1Range.Top))
            {
                solver.collided = false;
                return solver;
            }
            else
            {
                solver.collided = true;

                if (collider1Range.Right > collider2Range.Left && collider1Range.Left < collider2Range.Right)
                {
                    if (collider1Range.Center.X < collider2Range.Center.X)
                    {
                        solver.correctionVector.X = (collider1Range.Right - collider2Range.Left) * -1.0f;
                    }
                    else
                    {
                        solver.correctionVector.X = (collider2Range.Right - collider1Range.Left);
                    }
                }

                if (collider1Range.Bottom > collider2Range.Top && collider1Range.Top < collider2Range.Bottom)
                {
                    if (collider1Range.Center.Y > collider2Range.Center.Y)
                    {
                        solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom) * -1.0f;
                    }
                    else
                    {
                        solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom);
                    }

                }

                return solver;
            }
        }

        public static CollisionSolver CheckCollision(Vector2 pos1, Vector2 pos2, AnimatedSpriteSheet collider1, AnimatedSpriteSheet collider2, TimeSpan now, bool perPixel)
        {
            CollisionSolver solver = new CollisionSolver();
            collisionType = "Bounding-Box";
            Texture2D ss1 = collider1.getSpriteSheet();
            Texture2D ss2 = collider2.getSpriteSheet();

            Rectangle collider1Range = collider1.getCurrentFrame(now);
            Rectangle collider2Range = collider2.getCurrentFrame(now);

            collider1Range.X = (int)pos1.X;
            collider1Range.Y = (int)pos1.Y;
            collider2Range.X = (int)pos2.X;
            collider2Range.Y = (int)pos2.Y;

            if ((collider1Range.Left > collider2Range.Right && collider1Range.Right > collider2Range.Left) ||
                (collider2Range.Left > collider1Range.Right && collider2Range.Right > collider1Range.Left) ||
                (collider1Range.Top < collider2Range.Bottom && collider1Range.Bottom < collider2Range.Top) ||
                (collider2Range.Top < collider1Range.Bottom && collider2Range.Bottom < collider1Range.Top))
            {
                solver.collided = false;
                return solver;
            }
            else
            {
                solver.collided = true;

                if (collider1Range.Right > collider2Range.Left && collider1Range.Left < collider2Range.Right)
                {
                    if (collider1Range.Center.X < collider2Range.Center.X)
                    {
                        solver.correctionVector.X = (collider1Range.Right - collider2Range.Left) * -1.0f;
                    }
                    else
                    {
                        solver.correctionVector.X = (collider2Range.Right - collider1Range.Left);
                    }
                }

                if (collider1Range.Bottom > collider2Range.Top && collider1Range.Top < collider2Range.Bottom)
                {
                    if (collider1Range.Center.Y > collider2Range.Center.Y)
                    {
                        solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom) * -1.0f;
                    }
                    else
                    {
                        solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom);
                    }

                }

                return solver;
            }

    
        }

        public static CollisionSolver CheckCollision(Vector2 pos1, Vector2 pos2, AnimationTexture collider1, AnimatedSpriteSheet collider2, TimeSpan now, bool perPixel)
        {
            CollisionSolver solver = new CollisionSolver();
            collisionType = "Bounding-Box";
            Texture2D texture = collider1.getCurrentFrame(now);
            Rectangle collider1Range = new Rectangle(0, 0, texture.Width, texture.Height);


            Texture2D ss = collider2.getSpriteSheet();
            Rectangle collider2Range = collider2.getCurrentFrame(now);

            collider1Range.X = (int)pos1.X;
            collider1Range.Y = (int)pos1.Y;
            collider2Range.X = (int)pos2.X;
            collider2Range.Y = (int)pos2.Y;

            //Simple Bounding Box Collisions
            if ((collider1Range.Left > collider2Range.Right && collider1Range.Right > collider2Range.Left) ||
                (collider2Range.Left > collider1Range.Right && collider2Range.Right > collider1Range.Left) ||
                (collider1Range.Top < collider2Range.Bottom && collider1Range.Bottom < collider2Range.Top) ||
                (collider2Range.Top < collider1Range.Bottom && collider2Range.Bottom < collider1Range.Top))
            {
                solver.collided = false;
                return solver;
            }
            else
            {
                solver.collided = true;

                if (collider1Range.Right > collider2Range.Left && collider1Range.Left < collider2Range.Right)
                {
                    if (collider1Range.Center.X < collider2Range.Center.X)
                    {
                        solver.correctionVector.X = (collider1Range.Right - collider2Range.Left) * -1.0f;
                    }
                    else
                    {
                        solver.correctionVector.X = (collider2Range.Right - collider1Range.Left);
                    }
                }

                if (collider1Range.Bottom > collider2Range.Top && collider1Range.Top < collider2Range.Bottom)
                {
                    if (collider1Range.Center.Y > collider2Range.Center.Y)
                    {
                        solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom) * -1.0f;
                    }
                    else
                    {
                        solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom);
                    }

                }

                return solver;
            }
        }

        public static CollisionSolver CheckCollision(Vector2 pos1, Vector2 pos2, AnimatedSpriteSheet collider1, AnimationTexture collider2, TimeSpan now, bool perPixel)
        {
            CollisionSolver solver = new CollisionSolver();
            collisionType = "Bounding-Box";

            Texture2D ss = collider1.getSpriteSheet();
            Rectangle collider1Range = collider1.getCurrentFrame(now);

            Texture2D texture = collider2.getCurrentFrame(now);
            Rectangle collider2Range = new Rectangle(0, 0, texture.Width, texture.Height);




            collider1Range.X = (int)pos1.X;
            collider1Range.Y = (int)pos1.Y;
            collider2Range.X = (int)pos2.X;
            collider2Range.Y = (int)pos2.Y;

            //Simple Bounding Box Collisions
            if ((collider1Range.Left > collider2Range.Right && collider1Range.Right > collider2Range.Left) ||
                (collider2Range.Left > collider1Range.Right && collider2Range.Right > collider1Range.Left) ||
                (collider1Range.Top < collider2Range.Bottom && collider1Range.Bottom < collider2Range.Top) ||
                (collider2Range.Top < collider1Range.Bottom && collider2Range.Bottom < collider1Range.Top))
            {
                solver.collided = false;
                return solver;
            }
            else
            {
                solver.collided = true;

                if (collider1Range.Right > collider2Range.Left && collider1Range.Left < collider2Range.Right)
                {
                    if (collider1Range.Center.X < collider2Range.Center.X)
                    {
                        solver.correctionVector.X = (collider1Range.Right - collider2Range.Left) * -1.0f;
                    }
                    else
                    {
                        solver.correctionVector.X = (collider2Range.Right - collider1Range.Left);
                    }
                }

                if (collider1Range.Bottom > collider2Range.Top && collider1Range.Top < collider2Range.Bottom)
                {
                    if (collider1Range.Center.Y > collider2Range.Center.Y)
                    {
                        solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom) * -1.0f;
                    }
                    else
                    {
                        solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom);
                    }
                }

                return solver;
            }
        }

        public static CollisionSolver CheckCollision(Vector2 pos1, Vector2 pos2, AnimationTexture collider1, AnimationTexture collider2, TimeSpan now, bool perPixel)
        {
            return CheckCollision(pos1, pos2, collider1.getCurrentFrame(now), collider2.getCurrentFrame(now), perPixel);
        }

        public static CollisionSolver CheckCollisionCustomBB(Vector2 pos1, Vector2 pos2, InteractiveObject char1, Rectangle bb, TimeSpan now, bool perPixel)
        {
            CollisionSolver solver = new CollisionSolver();
            collisionType = "Bounding-Box";

            if (char1.aSS != null)
            {
                Texture2D ss1 = char1.aSS.getSpriteSheet();
                Rectangle collider1Range = char1.aSS.getCurrentFrame(now);
                collider1Range.X = (int)pos1.X;
                collider1Range.Y = (int)pos1.Y;


                bb.X = (int)pos2.X;
                bb.Y = (int)pos2.Y;

                Rectangle collider2Range = bb;

                if ((collider1Range.Left > collider2Range.Right && collider1Range.Right > collider2Range.Left) ||
                (collider2Range.Left > collider1Range.Right && collider2Range.Right > collider1Range.Left) ||
                (collider1Range.Top < collider2Range.Bottom && collider1Range.Bottom < collider2Range.Top) ||
                (collider2Range.Top < collider1Range.Bottom && collider2Range.Bottom < collider1Range.Top))
                {
                    solver.collided = false;
                    return solver;
                }
                else
                {
                    solver.collided = true;

                    if (collider1Range.Right > collider2Range.Left && collider1Range.Left < collider2Range.Right)
                    {
                        if (collider1Range.Center.X < collider2Range.Center.X)
                        {
                            solver.correctionVector.X = (collider1Range.Right - collider2Range.Left) * -1.0f;
                        }
                        else
                        {
                            solver.correctionVector.X = (collider2Range.Right - collider1Range.Left);
                        }
                    }

                    if (collider1Range.Bottom > collider2Range.Top && collider1Range.Top < collider2Range.Bottom)
                    {
                        if (collider1Range.Center.Y > collider2Range.Center.Y)
                        {
                            solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom) * -1.0f;
                        }
                        else
                        {
                            solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom);
                        }

                    }

                    return solver;
                }


                

            }
            else if (char1.aT != null)
            {

                Texture2D texture = char1.aT.getCurrentFrame(now);
                Rectangle collider1Range = new Rectangle(0, 0, texture.Width, texture.Height);



                bb.X = (int)pos2.X;
                bb.Y = (int)pos2.Y;

                Rectangle collider2Range = bb;


                collider1Range.X = (int)pos1.X;
                collider1Range.Y = (int)pos1.Y;
                collider2Range.X = (int)pos2.X;
                collider2Range.Y = (int)pos2.Y;

                if ((collider1Range.Left > collider2Range.Right && collider1Range.Right > collider2Range.Left) ||
                (collider2Range.Left > collider1Range.Right && collider2Range.Right > collider1Range.Left) ||
                (collider1Range.Top < collider2Range.Bottom && collider1Range.Bottom < collider2Range.Top) ||
                (collider2Range.Top < collider1Range.Bottom && collider2Range.Bottom < collider1Range.Top))
                {
                    solver.collided = false;
                    return solver;
                }
                else
                {
                    solver.collided = true;

                    if (collider1Range.Right > collider2Range.Left && collider1Range.Left < collider2Range.Right)
                    {
                        if (collider1Range.Center.X < collider2Range.Center.X)
                        {
                            solver.correctionVector.X = (collider1Range.Right - collider2Range.Left) * -1.0f;
                        }
                        else
                        {
                            solver.correctionVector.X = (collider2Range.Right - collider1Range.Left);
                        }
                    }

                    if (collider1Range.Bottom > collider2Range.Top && collider1Range.Top < collider2Range.Bottom)
                    {
                        if (collider1Range.Center.Y > collider2Range.Center.Y)
                        {
                            solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom) * -1.0f;
                        }
                        else
                        {
                            solver.correctionVector.Y = (collider2Range.Top - collider1Range.Bottom);
                        }

                    }

                    return solver;
                }

  

            }

            solver.collided = false;
            return solver;
        }

        private static CollisionSolver IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            collisionType = "Per-Pixel";
            CollisionSolver solver = new CollisionSolver();
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            if (((rectangleA.Bottom > rectangleB.Top) && (rectangleA.Top < rectangleB.Bottom)))//&& ((rectangleA.Right < rectangleB.Left) && (rectangleB.Right > rectangleA.Left)))
            {
                solver.collided = true;

                if (rectangleA.Right > rectangleB.Left && rectangleA.Left < rectangleB.Right)
                {
                    if (rectangleA.Center.X < rectangleB.Center.X)
                    {
                        solver.correctionVector.X = (rectangleA.Right - rectangleB.Left) * -1.0f;
                    }
                    else
                    {
                        solver.correctionVector.X = (rectangleB.Right - rectangleA.Left);
                    }
                }

                if (rectangleA.Bottom > rectangleB.Top && rectangleA.Top < rectangleB.Bottom)
                {
                    if (rectangleA.Center.Y > rectangleB.Center.Y)
                    {
                        solver.correctionVector.Y = (rectangleB.Top - rectangleA.Bottom) * -1.0f;
                    }
                    else
                    {
                        solver.correctionVector.Y = (rectangleB.Top - rectangleA.Bottom);
                    }

                }

                return solver;

            }
            /*
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[(x - rectangleA.Left) +
                                (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                (y - rectangleB.Top) * rectangleB.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        solver.collided = true;

                        if (rectangleA.Right > rectangleB.Left && rectangleA.Left < rectangleB.Right)
                        {
                            if (rectangleA.Center.X < rectangleB.Center.X)
                            {
                                solver.correctionVector.X = (rectangleA.Right - rectangleB.Left)*-1.0f;
                            }
                            else
                            {
                                solver.correctionVector.X = (rectangleB.Right - rectangleA.Left);
                            }
                        }

                        if (rectangleA.Bottom > rectangleB.Top && rectangleA.Top < rectangleB.Bottom)
                        {
                            if (rectangleA.Center.Y > rectangleB.Center.Y)
                            {
                                solver.correctionVector.Y = (rectangleB.Top - rectangleA.Bottom) * -1.0f;
                            }
                            else
                            {
                                solver.correctionVector.Y = (rectangleB.Top - rectangleA.Bottom);
                            }

                        }

                        return solver;
                    }
                }
            }*/
            solver.collided = false;
            return solver;
        }

    }
}
