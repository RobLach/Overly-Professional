using WhiteBoard.Objects;
using System;

namespace WhiteBoard.Controller
{
    class AIStrategy
    {
        public /* abstract */ Character.GameAction[] getAction(/* state */)
        {
            // This ai can have its own states as well as looking at the game state
            // it will use those 2 to come up with a set of actions to perform
            //return new Character.GameAction[] { Character.GameAction.none };

            Character.GameAction[] arr = new Character.GameAction[1];
            int rand = 6;// (new Random()).Next(4);

            if (rand == 1)
            {
                arr[0] = Character.GameAction.walkLeft;
            }
            else if (rand == 2)
            {
                arr[0] = Character.GameAction.walkRight;
            }
            else if (rand == 3)
            {
                arr[0] = Character.GameAction.jump;
            }
            else
            {
                arr[0] = Character.GameAction.none;
            }

            return arr;

        }
    }
}
