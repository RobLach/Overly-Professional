using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using WhiteBoard.Objects;

namespace WhiteBoard.Controller
{

    public class PlayerInput
    {
        private static float lowthreshhold = .2F;
        private static float highthreshhold = .85F;

        private static PlayerInput[] players;
        public const int MaxInputs = 4;

        private KeyboardState currentKeyboardStates;
        private GamePadState currentGamePadStates;

        private KeyboardState previousKeyboardStates;
        private GamePadState previousGamePadStates;

        private int playerNum;

        //mapping will be loaded from somewhere, maybe a text or resourse file?
        private PlayerInput(int num)
        {
            playerNum = num;
        }

        public static PlayerInput get(int player)
        {
            if (players == null)
            {
                players = new PlayerInput[MaxInputs];
                for (int i = 0; i < MaxInputs; i++)
                    players[i] = new PlayerInput(i);
            }
            return players[player];
        }

        public void Update()
        {
            previousKeyboardStates = currentKeyboardStates;
            previousGamePadStates = currentGamePadStates;

            currentKeyboardStates = Keyboard.GetState();
            currentGamePadStates = GamePad.GetState((PlayerIndex)playerNum);
        }

        public bool IsKeyDown(Keys key)
        {
            return currentKeyboardStates.IsKeyDown(key);
        }

        public Vector2 getLeftStrick()
        {
            return normal(currentGamePadStates.ThumbSticks.Left);
        }

        public Vector2 getRightStick()
        {
            return normal(currentGamePadStates.ThumbSticks.Right);
        }

        public Vector2 normal(Vector2 pos)
        {
            Vector2 ret = new Vector2(pos.X, pos.Y);

            if (ret.X > highthreshhold)
                ret.X = 1;
            if (ret.X < lowthreshhold)
                ret.X = 0;
            if (ret.Y > highthreshhold)
                ret.Y = 1;
            if (ret.Y < lowthreshhold)
                ret.Y = 0;

            return ret;
        }

        public Character.GameAction[] getAction(Dictionary<Buttons, Character.GameAction> map,
                                                Dictionary<Keys, Character.GameAction> map2)
        {
            LinkedList<Character.GameAction> arr = new LinkedList<Character.GameAction>();
            foreach( Buttons bt in map.Keys )
            {
                if ((currentGamePadStates.IsButtonDown(bt) && IsNewButtonPress(bt))||(currentGamePadStates.IsButtonDown(bt) && bt.ToString().Contains("Thumbstick")))//&& previousGamePadStates.IsButtonUp(bt))
                    arr.AddLast(map[bt]);
            }
            foreach (Keys kt in map2.Keys)
            {
                if (currentKeyboardStates.IsKeyDown(kt))
                    arr.AddLast(map2[kt]);
            }

            return arr.ToArray();
        }

        //Would like to get rid off this mapping from the Input class and put it somewhere else

        /// <summary>
        /// Checks for a "menu up" input action, from any player,
        /// on either keyboard or gamepad.
        /// </summary>
        public bool MenuUp
        {
            get
            {
                return IsNewKeyPress(Keys.Up) ||
                       IsNewButtonPress(Buttons.DPadUp) ||
                       IsNewButtonPress(Buttons.LeftThumbstickUp);
            }
        }

        /// <summary>
        /// Checks for a "menu down" input action, from any player,
        /// on either keyboard or gamepad.
        /// </summary>
        public bool MenuDown
        {
            get
            {
                return IsNewKeyPress(Keys.Down) ||
                       IsNewButtonPress(Buttons.DPadDown) ||
                       IsNewButtonPress(Buttons.LeftThumbstickDown);
            }
        }

        /// <summary>
        /// Checks for a "menu select" input action, from any player,
        /// on either keyboard or gamepad.
        /// </summary>
        public bool MenuSelect
        {
            get
            {
                return IsNewKeyPress(Keys.Space) ||
                       IsNewKeyPress(Keys.Enter) ||
                       IsNewButtonPress(Buttons.A) ||
                       IsNewButtonPress(Buttons.Start);
            }
        }
        
        
        /// <summary>
        /// Checks for a "menu cancel" input action, from any player,
        /// on either keyboard or gamepad.
        /// </summary>
        public bool MenuCancel
        {
            get
            {
                return IsNewKeyPress(Keys.Escape) ||
                       IsNewButtonPress(Buttons.B) ||
                       IsNewButtonPress(Buttons.Back);
            }
        }

        /// <summary>
        /// Checks for a "pause the game" input action, from any player,
        /// on either keyboard or gamepad.
        /// </summary>
        public bool PauseGame
        {
            get
            {
                return IsNewKeyPress(Keys.Escape) ||
                       IsNewButtonPress(Buttons.Back) ||
                       IsNewButtonPress(Buttons.Start);
            }
        }


        /// <summary>
        /// Helper for checking if a key was newly pressed during this update,
        /// by the specified player.
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
            return (currentKeyboardStates.IsKeyDown(key) &&
                    previousKeyboardStates.IsKeyUp(key));
        }

        /// <summary>
        /// Helper for checking if a button was newly pressed during this update,
        /// by the specified player.
        /// </summary>
        public bool IsNewButtonPress(Buttons button)
        {

            if (currentGamePadStates.IsButtonDown(button) &&
                previousGamePadStates.IsButtonUp(button))
            {
                return (true);
            }

            return (false);
        }


        /// <summary>
        /// Checks for a "menu select" input action from the specified player.
        /// </summary>
        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) ||
                   IsNewKeyPress(Keys.Enter) ||
                   IsNewButtonPress(Buttons.A) ||
                   IsNewButtonPress(Buttons.Start);
        }


        /// <summary>
        /// Checks for a "menu cancel" input action from the specified player.
        /// </summary>
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape) ||
                   IsNewButtonPress(Buttons.B) ||
                   IsNewButtonPress(Buttons.Back);
        }

        public bool SwitchCameraTargets
        {
            get
            {
                return IsNewKeyPress(Keys.Space);
            }
        }

        public bool debugMode
        {
            get
            {
                return IsNewButtonPress(Buttons.LeftShoulder);
            }
        }
    }
}
