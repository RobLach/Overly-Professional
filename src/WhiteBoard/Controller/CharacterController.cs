using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhiteBoard.Objects;
using Microsoft.Xna.Framework;

namespace WhiteBoard.Controller
{
    abstract class CharacterController
    {
        protected Character character;

        public CharacterController(Character ch)
        {
            character = ch;
            character.isActive = false;
        }

        public void Update(GameTime gameTime)
        {
            this.Perform(gameTime);
            character.Update(gameTime);
        }

        /*
         * Called by the Game Loop on all caracters
         */
        protected void Perform(GameTime gameTime)
        {
            if (this.character.isActive)
                character.handleActions(getAction(gameTime));
            else
                character.handleActions(character.defaultAction);
        }

        public Character getCharacter()
        {
            return character;
        }

        public abstract Character.GameAction[] getAction(GameTime gameTime);
    }
}
