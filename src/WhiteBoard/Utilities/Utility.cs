#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
#endregion
namespace WhiteBoard
{
    class Utility
    {
        //Content Loader and Blank Texture
        ContentManager content;
        Texture2D blank;
        private AudioSFX SoundEffects;
        Song backgroundMusic;
        Song song1;
        Song song2;
        Song song3;
        public static int musicVolume;
        public static Random rand = new Random();
        int currentSong = 1;

        public Utility(ContentManager contents)
        {

            content = contents;

            blank = content.Load<Texture2D>("blank");
            backgroundMusic = content.Load<Song>(@"Audio\BackgroundMusic");
            SoundEffects = new AudioSFX(content);
            song1 = content.Load<Song>(@"Audio\song1");
            song2 = content.Load<Song>(@"Audio\song2");
            song3 = content.Load<Song>(@"Audio\song3");

            MediaPlayer.Play(song1);
            MediaPlayer.Volume = 1.0f;
            MediaPlayer.IsRepeating = false;

        }

        public void playSFX(Vector2 listenerpos, Vector2 emitterpos, Vector2 listenervel,
                            Vector2 emittervel, int index)
        {
            //SoundEffects.playSFX(listenerpos, emitterpos, listenervel, emittervel, index);
        }

        public void update()
        {
            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
            if (MediaPlayer.State == MediaState.Stopped)
            {
                currentSong++;
                switch (currentSong)
                {
                    case 1:
                        MediaPlayer.Play(song1);
                        break;
                    case 2:
                        MediaPlayer.Play(song2);
                        break;
                    case 3:
                        MediaPlayer.Play(song3);
                        break;
                }
            }

        }


        public void drawBounding(Rectangle boundBox, Boolean isHit, SpriteBatch spriteBatch, int thick)
        {
            //Creation of the 1 pixel wide/tall lines to draw the rectangle
            Rectangle top = new Rectangle(boundBox.X, boundBox.Y, boundBox.Width, thick);
            Rectangle bottom = new Rectangle(boundBox.X, boundBox.Y + boundBox.Height - 1, boundBox.Width + thick - 1, thick);
            Rectangle left = new Rectangle(boundBox.X, boundBox.Y, thick, boundBox.Height);
            Rectangle right = new Rectangle(boundBox.X + boundBox.Width -1, boundBox.Y, thick, boundBox.Height);

            Color col = Color.Blue;
            //Turns color red if the boxes are hitting
            if (isHit)
                col = Color.Red;
            spriteBatch.Draw(blank, top, col);
            spriteBatch.Draw(blank, bottom, col);
            spriteBatch.Draw(blank, left, col);
            spriteBatch.Draw(blank, right, col);
        }

        public void pause()
        {
            MediaPlayer.Pause();
        }

        public void play()
        {
            MediaPlayer.Resume();
        }
    }
}