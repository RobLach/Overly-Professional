using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using WhiteBoard.Objects;
using Microsoft.Xna.Framework;

namespace WhiteBoard.Controller
{
    class PlayerController : CharacterController
    {
        private PlayerInput plInput;
        private Dictionary<Buttons, Character.GameAction> mapping;
        private Dictionary<Keys, Character.GameAction> mapping_keys;

        public PlayerController(Character ch, PlayerInput playerInput,
                            Dictionary<Buttons, Character.GameAction> map)
            : base(ch)
        {
            mapping = map;
            plInput = playerInput;
        }

        public PlayerController(Character ch, PlayerInput playerInput) : base(ch)
        {
            plInput = playerInput;
            mapping = new Dictionary<Buttons, Character.GameAction>();
            mapping_keys = new Dictionary<Keys, Character.GameAction>();

            mapping.Add(Buttons.LeftThumbstickLeft, Character.GameAction.walkLeft);
            mapping_keys.Add(Keys.Left, Character.GameAction.walkLeft);

            mapping.Add(Buttons.LeftThumbstickRight, Character.GameAction.walkRight);
            mapping_keys.Add(Keys.Right, Character.GameAction.walkRight);

            mapping.Add(Buttons.A, Character.GameAction.jump);
            mapping_keys.Add(Keys.Up, Character.GameAction.jump);

            mapping.Add(Buttons.B, Character.GameAction.melee);
            mapping_keys.Add(Keys.LeftControl, Character.GameAction.melee);

            mapping.Add(Buttons.X, Character.GameAction.shoot);
            mapping_keys.Add(Keys.X, Character.GameAction.shoot);

            mapping.Add(Buttons.RightTrigger, Character.GameAction.toss);
            mapping_keys.Add(Keys.Z, Character.GameAction.toss);

            mapping.Add(Buttons.LeftTrigger, Character.GameAction.shoot);
        }


        public override Character.GameAction[] getAction(GameTime gameTime)
        {
            //check the mappings and create a new Game Action based on it
            Character.GameAction[] arr = plInput.getAction(mapping, mapping_keys);
            if (arr == null || arr.Length == 0)
                return new Character.GameAction[] { Character.GameAction.none };
            return arr;
        }
    }
}
