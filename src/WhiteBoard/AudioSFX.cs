#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace WhiteBoard
{
    class AudioSFX
    {
        public SoundEffect[] noises;
        public static int sfxVolume = 30;


        public AudioSFX(ContentManager content)
        {
            noises = new SoundEffect[1];
            noises[0] = content.Load<SoundEffect>(@"Audio\noise");
        }

      

        public void playSFX(Vector2 listenerpos, Vector2 emitterpos, Vector2 listenervel,
    Vector2 emittervel, int index)
        {
            float panning = ((float)listenerpos.X - (float)emitterpos.X) /
                  ((float)Math.Abs(((float)listenerpos.X - (float)emitterpos.X))+50);

            float distance = (float)Math.Sqrt(((float)listenerpos.X - (float)emitterpos.X) *
                                       ((float)listenerpos.X - (float)emitterpos.X) +
                                       ((float)listenerpos.Y - (float)emitterpos.Y) *
                                       ((float)listenerpos.Y - (float)emitterpos.Y) );
            float volume = 100.0f / (100.0f + distance) * (float)(sfxVolume/30);

            noises[index].Play(volume, 0.0f, panning, false);
        }

    }
}
