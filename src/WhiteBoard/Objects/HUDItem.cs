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
    public class HUDItem
    {
        Vector2 pos;
        float width;
        float height;
        String name;
        Texture2D hudTexture;
        Color col;
        bool alignv, alignh;
        public HUDItem(Vector2 p, float w, float h, String n, Texture2D tex, bool hw, bool v)
        {
            if (w > 1 || h > 1)
                return;
            pos = p;
            width = w;
            height = h;
            name = n;
            col = new Color(.5f, .5f, .5f);
            hudTexture = tex;
            alignv = v;
            alignh = hw;
        }

        public void draw(float w, float h, SpriteBatch spritebatch)
        {
            Rectangle rect;
            if (alignv)
            {
                if (alignh)
                {
                    rect = new Rectangle((int)(pos.X * w), (int)(pos.Y * h), (int)(width * w), (int)(height * h));
            
                }
                else
                {
                    rect = new Rectangle((int)((pos.X - width) * w), (int)(pos.Y * h), (int)(width * w), (int)(height * h));
            
                }
            }
            else
            {
                if (alignh)
                {
                    rect = new Rectangle((int)(pos.X * w), (int)((pos.Y - height) * h), (int)(width * w), (int)(height * h));
            
                }
                else
                {
                    rect = new Rectangle((int)((pos.X - width) * w), (int)((pos.Y - height) * h), (int)(width * w), (int)(height * h));
            
                }
            }
            spritebatch.Draw(hudTexture, rect, col);
        }
    }
}
