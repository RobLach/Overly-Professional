using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WhiteBoard
{
    public enum HudItemPos
    {
        topLeft,
        topRight,
        topCenter,
        bottomLeft,
        bottomRight,
        bottomCenter,
        centerLeft,
        centerRight,
        center
    }

    class HUD
    {
        ScreenManager screenManager;
        List<HUDItem> hudItems;
        
        public HUD(ScreenManager scnm)
        {
            screenManager = scnm;
            hudItems = new List<HUDItem>(12);
        
        }


        // I screwed this up in the merge D:....
        // So i'm adding some crap value to at least get it compiling
        // -rob lach

        public void AddHudItem(Texture2D hudTexture, HudItemPos position, Vector2 offset, float percentageHeight)
        {
            hudItems.Add(new HUDItem(hudTexture.ToString(), hudTexture, position, percentageHeight, offset,1, 1 ,1,1));
        }

        public void AddHudItem(Texture2D hudTexture, HudItemPos position, Vector2 offset, float percentageHeight, string name)
        {
            hudItems.Add(new HUDItem(name, hudTexture, position,  percentageHeight, offset,1, 1 ,1, 1));
        }

        public void draw(SpriteBatch batch)
        {

            int screenWidth = screenManager.GraphicsDevice.Viewport.Width;
            int screenHeight = screenManager.GraphicsDevice.Viewport.Height;
            Rectangle titleSafe = screenManager.GraphicsDevice.Viewport.TitleSafeArea;

            foreach (HUDItem item in hudItems)
            {
                int width = item.tex.Width;
                int height = item.tex.Height;
                float newHeight = titleSafe.Height * item.percentH;
                float ratio = newHeight / height;
                float newWidth = width * ratio;
                Color halfWhite = new Color(Color.White, 0.5f);
                
                switch (item.itemPos)
                {
                    case HudItemPos.bottomCenter:
                        batch.Draw(item.tex, new Rectangle((int)(titleSafe.X + (int)(titleSafe.Width / 2) - (int)(newWidth / 2)), (int)(titleSafe.Y + titleSafe.Height - newHeight), (int)newWidth, (int)newHeight), halfWhite);
                        break;

                    case HudItemPos.bottomLeft:
                        batch.Draw(item.tex, new Rectangle((int)(titleSafe.X), (int)(titleSafe.Y + titleSafe.Height - newHeight), (int)newWidth, (int)newHeight), halfWhite);
                        break;

                    case HudItemPos.bottomRight:
                        batch.Draw(item.tex, new Rectangle((int)(titleSafe.X + titleSafe.Width - newWidth), (int)(titleSafe.Y + titleSafe.Height - newHeight), (int)newWidth, (int)newHeight), halfWhite);
                        break;

                    case HudItemPos.center:
                        batch.Draw(item.tex, new Rectangle((int)(titleSafe.X + (int)(titleSafe.Width / 2) - (int)(newWidth / 2)), (int)(titleSafe.Y + titleSafe.Height/2 - newHeight/2), (int)newWidth, (int)newHeight), halfWhite);
                        break;

                    case HudItemPos.centerLeft:
                        batch.Draw(item.tex, new Rectangle((int)(titleSafe.X), (int)(titleSafe.Y + titleSafe.Height / 2 - newHeight / 2), (int)newWidth, (int)newHeight), halfWhite);
                        break;

                    case HudItemPos.centerRight:
                        batch.Draw(item.tex, new Rectangle((int)(titleSafe.X + (int)(titleSafe.Width) - (int)(newWidth)), (int)(titleSafe.Y + titleSafe.Height / 2 - newHeight / 2), (int)newWidth, (int)newHeight), halfWhite);
                        break;

                    case HudItemPos.topCenter:
                        batch.Draw(item.tex, new Rectangle((int)(titleSafe.X + (int)(titleSafe.Width / 2) - (int)(newWidth / 2)), (int)(titleSafe.Y), (int)newWidth, (int)newHeight), halfWhite);
                        break;

                    case HudItemPos.topLeft:
                        batch.Draw(item.tex, new Rectangle((int)(titleSafe.X), (int)(titleSafe.Y), (int)newWidth, (int)newHeight), halfWhite);
                        break;

                    case HudItemPos.topRight:
                        batch.Draw(item.tex, new Rectangle((int)(titleSafe.X + (int)(titleSafe.Width) - (int)(newWidth)), (int)(titleSafe.Y), (int)newWidth, (int)newHeight), halfWhite);
                        break;
                      
                }
            }
        }
    }
}
