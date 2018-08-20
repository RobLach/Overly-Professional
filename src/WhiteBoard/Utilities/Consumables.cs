#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

/// <summary>
/// Summary description for Class1
/// </summary>
namespace WhiteBoard
{
    public class Consumables : HUDItem
    {

        public int amount;
        public SpriteFont font;

        /// <summary>
        /// Hud Item Class
        /// </summary>
        /// <param name="name">name of item</param>
        /// <param name="image">texture</param>
        /// <param name="position">position type</param>
        /// <param name="percentx">percentage width of screen</param>
        /// <param name="percenty">percentage height of screen</param>
        /// <param name="offset">offset</param>
        public Consumables(string name, Texture2D image, HudItemPos position, float percenty, Vector2 offset, int lifeAmount, int titleheight, int titlewidth, int titlex, int titley, SpriteFont lifeFont)
            : base(name, image, position, percenty, offset, titleheight, titlewidth, titlex, titley)
        {
            amount = lifeAmount;
            font = lifeFont;
        }

        public override void update(int amounts)
        {
            amount = amounts;
        }

        public override void draw(SpriteBatch batch)
        {
            int width = tex.Width;
            int height = tex.Height;
            float newHeight = theight * percentH;
            float ratio = newHeight / height;
            float newWidth = width * ratio;
            Color halfWhite = new Color(Color.White, 1f);
            switch (itemPos)
            {
                case HudItemPos.bottomCenter:
                    //if(life == 3)
                    //else if(life == 2)
                    //else if(life == 1)
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth / 2) - (int)(newWidth / 2)), (int)(ty + theight - newHeight), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.bottomLeft:
                    //if(life == 3)
                    //else if(life == 2)
                    //else if(life == 1)
                    batch.Draw(tex, new Rectangle((int)(tx), (int)(ty + theight - newHeight), (int)newWidth, (int)newHeight), halfWhite);
                    batch.DrawString(font, "x   " + amount + "", new Vector2((int)(tx) + (int)(newHeight) + (int)(newHeight/4), (int)(ty + theight - newHeight)), halfWhite);
                    break;

                case HudItemPos.bottomRight:
                    //if(life == 3)
                    //else if(life == 2)
                    //else if(life == 1)
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth) - (int)(newWidth) - (int)(newWidth / 2)), (int)(ty + theight - newHeight), (int)newWidth, (int)newHeight), halfWhite);
                    batch.DrawString(font, "x   " + amount + "", new Vector2((int)tx + (int)(twidth) - (int)(newWidth / 2), (int)(ty + theight - newHeight)), halfWhite);
                    break;

                case HudItemPos.center:
                    //if(life == 3)
                    //else if(life == 2)
                    //else if(life == 1)
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth / 2) - (int)(newWidth / 2)), (int)(ty + theight / 2 - newHeight / 2), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.centerLeft:
                    //if(life == 3)
                    //else if(life == 2)
                    //else if(life == 1)
                    batch.Draw(tex, new Rectangle((int)(tx), (int)(ty + theight / 2 - newHeight / 2), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.centerRight:
                    //if(life == 3)
                    //else if(life == 2)
                    //else if(life == 1)
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth) - (int)(newWidth) - (int)(newWidth / 2)), (int)(ty + theight / 2 - newHeight / 2), (int)newWidth, (int)newHeight), halfWhite);
                    batch.DrawString(font, "x   " + amount + "", new Vector2((int)tx + (int)(twidth) - (int)(newWidth / 2), (int)(ty + theight / 2 - newHeight / 2)), halfWhite);
                    break;

                case HudItemPos.topCenter:
                    //if(life == 3)
                    //else if(life == 2)
                    //else if(life == 1)
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth / 2) - (int)(newWidth / 2)), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.topLeft:
                    //if (life == 3)
                      //  batch.Draw(tex, new Rectangle((int)(tx), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    //batch.Draw(tex, new Rectangle((int)(tx + newWidth), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    //batch.Draw(tex, new Rectangle((int)(tx + newWidth * 2), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    //if (life == 2)
                        //batch.Draw(tex, new Rectangle((int)(tx), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    //batch.Draw(tex, new Rectangle((int)(tx + newWidth), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    //if (life == 1)
                        batch.Draw(tex, new Rectangle((int)(tx), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.topRight:
                    //if(life == 3)
                    //else if(life == 2)
                    //else if(life == 1)
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth) - (int)(newWidth) - (int) (newWidth/2)), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    batch.DrawString(font,"x   " + amount + "", new Vector2((int)tx + (int)(twidth) - (int)(newWidth/2), (int)(ty)), halfWhite);
                    break;

            }
        }
    }
}
