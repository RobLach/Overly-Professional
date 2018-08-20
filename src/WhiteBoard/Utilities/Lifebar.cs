#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WhiteBoard.Objects;
#endregion

/// <summary>
/// Summary description for Class1
/// </summary>
namespace WhiteBoard
{
    public class Lifebar : HUDItem
    {

        public Character ch;

        /// <summary>
        /// Hud Item Class
        /// </summary>
        /// <param name="name">name of item</param>
        /// <param name="image">texture</param>
        /// <param name="position">position type</param>
        /// <param name="percentx">percentage width of screen</param>
        /// <param name="percenty">percentage height of screen</param>
        /// <param name="offset">offset</param>
        public Lifebar(string name, Texture2D image, HudItemPos position, float percenty, Vector2 offset, Character ch, int titleheight, int titlewidth, int titlex, int titley)
            :base(name,image,position,percenty,offset,titleheight,titlewidth,titlex,titley)
        {
            this.ch = ch;
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
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth / 2) - (int)(newWidth / 2)), (int)(ty + theight - newHeight), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.bottomLeft:
                    batch.Draw(tex, new Rectangle((int)(tx), (int)(ty + theight - newHeight), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.bottomRight:
                    batch.Draw(tex, new Rectangle((int)(tx + twidth - newWidth), (int)(ty + theight - newHeight), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.center:
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth / 2) - (int)(newWidth / 2)), (int)(ty + theight / 2 - newHeight / 2), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.centerLeft:
                    batch.Draw(tex, new Rectangle((int)(tx), (int)(ty + theight / 2 - newHeight / 2), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.centerRight:
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth) - (int)(newWidth)), (int)(ty + theight / 2 - newHeight / 2), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.topCenter:
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth / 2) - (int)(newWidth / 2)), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.topLeft:
                    if(ch.health == 3)
                        batch.Draw(tex, new Rectangle((int)(tx), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                        batch.Draw(tex, new Rectangle((int)(tx + newWidth), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                        batch.Draw(tex, new Rectangle((int)(tx + newWidth*2), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    if(ch.health == 2)
                        batch.Draw(tex, new Rectangle((int)(tx), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                        batch.Draw(tex, new Rectangle((int)(tx + newWidth), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    if(ch.health == 1)
                        batch.Draw(tex, new Rectangle((int)(tx), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    break;

                case HudItemPos.topRight:
                    batch.Draw(tex, new Rectangle((int)(tx + (int)(twidth) - (int)(newWidth)), (int)(ty), (int)newWidth, (int)newHeight), halfWhite);
                    break;
            }
        }
    }
}
