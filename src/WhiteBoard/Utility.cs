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


        public Utility(ContentManager contents)
        {

            content = contents;

            blank = content.Load<Texture2D>("blank");
            backgroundMusic = content.Load<Song>(@"Audio\BackgroundMusic");
            SoundEffects = new AudioSFX(content);
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;

        }

        public void playSFX(Vector2 listenerpos, Vector2 emitterpos, Vector2 listenervel,
                            Vector2 emittervel, int index)
        {
            SoundEffects.playSFX(listenerpos, emitterpos, listenervel, emittervel, index);
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
    }

    public class DoubleHashtable<TKey, TKey2, TValue>
    {
        Dictionary<TKey, Dictionary<TKey2, TValue>> hash;

        public DoubleHashtable()
        {
            hash = new Dictionary<TKey, Dictionary<TKey2, TValue>>();
        }

        public bool containsKey(TKey key1, TKey2 key2)
        {
            if (!hash.ContainsKey(key1))
                return false;
            Dictionary<TKey2, TValue> temp = hash[key1];
            if (temp.ContainsKey(key2))
                return true;
            return false;
        }

        public TValue get(TKey key1, TKey2 key2)
        {
            if (!hash.ContainsKey(key1))
                throw new InvalidOperationException("Key is not in the table");

            Dictionary<TKey2, TValue> temp = hash[key1];
            if (!temp.ContainsKey(key2))
                throw new InvalidOperationException("Key is not in the table");

            return temp[key2];
        }

        public void Add(TKey key1, TKey2 key2, TValue value)
        {
            if (!hash.ContainsKey(key1))
                hash.Add(key1, new Dictionary<TKey2, TValue>());

            Dictionary<TKey2, TValue> temp = hash[key1];

            temp.Add(key2, value);
        }

        public bool Remove(TKey key1, TKey2 key2)
        {
            if (!hash.ContainsKey(key1))
                return false;
            return hash[key1].Remove(key2);
        }
    }
}