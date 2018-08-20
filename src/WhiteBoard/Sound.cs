using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace NetworkStateManagement
{
    class Sound
    {
                //sounds stuff
        AudioEngine audioengine;
        SoundBank soundBank;
        WaveBank wavebank;

        // more sounds stuff
        AudioEmitter emitter;
        AudioListener listener;
        Cue cue;

        float maxEmitterDistance = 150.0f;
        float maxVelocity = 30.0f;


        public Sound()
        {
            audioengine = new AudioEngine("Content\\audio.xgs");
            wavebank = new WaveBank(audioengine, "Content\\Wave Bank.xwb");
            soundBank = new SoundBank(audioengine, "Content\\Sound Bank.xsb");
            emitter = new AudioEmitter();
            listener = new AudioListener();
            cue = soundBank.GetCue("anxious");
            cue.Apply3D(listener, emitter);
            cue.Play();

        }
        /*
        AudioEngine audioengine;
        SoundBank soundbank;
        WaveBank wavebank;
        AudioCategory soundCategory;

        public Sound()
        {
            audioengine = new AudioEngine("Content\\audio.xgs");
            wavebank = new WaveBank(audioengine, "Content\\Wave Bank.xwb");
            soundbank = new SoundBank(audioengine, "Content\\Sound Bank.xsb");
            soundCatagory = audioengine.GetCategory("Default");
        }


        public void playsound(Vector2 soundpos, Vector2 playerpos, String soundname)
        {
            float distance = Math.Sqrt(soundpos * soundpos + playerpos * playerpos);
            soundCatagory.SetVolume(1 - distance/(30+distance));
            soundbank.PlayCue(soundname);
        }

        public void update()
        {
            audioengine.Update();
        }*/
    }

}

