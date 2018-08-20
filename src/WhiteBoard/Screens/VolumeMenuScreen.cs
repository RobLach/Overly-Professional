#region File Description
//-----------------------------------------------------------------------------
// VolumeMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace WhiteBoard
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class VolumeMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry musicUpMenuEntry;
        MenuEntry sfxUpMenuEntry;
        MenuEntry musicDownMenuEntry;
        MenuEntry sfxDownMenuEntry;




        static int musicVolume = 30;
        static int sfxVolume = 30;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public VolumeMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            musicUpMenuEntry = new MenuEntry(string.Empty);
            musicDownMenuEntry = new MenuEntry(string.Empty);
            sfxUpMenuEntry = new MenuEntry(string.Empty);
            sfxDownMenuEntry = new MenuEntry(string.Empty);

            // Flag that there is no need for the game to transition
            // off when the pause menu is on top of it.
            IsPopup = true;


            SetMenuEntryText();

            MenuEntry backMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            musicUpMenuEntry.Selected += MusicUpMenuEntrySelected;
            sfxUpMenuEntry.Selected += SFXUpMenuEntrySelected;
            musicDownMenuEntry.Selected += MusicDownMenuEntrySelected;
            sfxDownMenuEntry.Selected += SFXDownMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(musicUpMenuEntry);
            MenuEntries.Add(musicDownMenuEntry);
            MenuEntries.Add(sfxUpMenuEntry);
            MenuEntries.Add(sfxDownMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            musicUpMenuEntry.Text = "Music Volume Up: " + musicVolume;
            musicDownMenuEntry.Text = "Music Volume Down: " + musicVolume;
            sfxUpMenuEntry.Text = "SFX Volume Up: " + sfxVolume;
            sfxDownMenuEntry.Text = "SFX Volume Down: " + sfxVolume;
        }


        #endregion

        #region Handle Input



        void MusicUpMenuEntrySelected(object sender, EventArgs e)
        {
            musicVolume++;
            MathHelper.Clamp(musicVolume, 0, 30);
            Utility.musicVolume = musicVolume;
            SetMenuEntryText();
        }

        void SFXUpMenuEntrySelected(object sender, EventArgs e)
        {
            sfxVolume++;
            MathHelper.Clamp(sfxVolume, 0, 30);
            AudioSFX.sfxVolume = sfxVolume;
            SetMenuEntryText();
        }

        void MusicDownMenuEntrySelected(object sender, EventArgs e)
        {
            musicVolume--;
            MathHelper.Clamp(musicVolume, 0, 30);
            Utility.musicVolume = musicVolume;
            SetMenuEntryText();
        }

        void SFXDownMenuEntrySelected(object sender, EventArgs e)
        {
            sfxVolume--;
            MathHelper.Clamp(sfxVolume, 0, 30);
            AudioSFX.sfxVolume = sfxVolume;
            SetMenuEntryText();
        }

        #endregion


    }
}
