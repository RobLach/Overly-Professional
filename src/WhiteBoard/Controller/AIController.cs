using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhiteBoard;
using WhiteBoard.Objects;
using Microsoft.Xna.Framework;

namespace WhiteBoard.Controller
{
    class AIController : CharacterController
    {
        GameplayScreen gscreen;
        AiState currState = AiState.idle;
        public TimeSpan shootCoolDown = TimeSpan.Zero;
        public TimeSpan meleeCoolDown = TimeSpan.Zero;

        public enum AiState
        {
            following, melee, jumping, idle, shoot
        }

        public AIController(Character ch, GameplayScreen gs) : base(ch)
        {
            gscreen = gs;
        }

        public override Character.GameAction[] getAction(GameTime gameTime)
        {
            Character pl = gscreen.charControllers[0].getCharacter();
            Character.GameAction[] prevActs = pl.getLastActions();
            int threshold = 100;
            List<Character.GameAction> actions = new List<Character.GameAction>();
            actions.Add(Character.GameAction.none);

            float pos = this.character.position.X - pl.position.X;
            if (pos > 0)
                character.flipHorizontal = true;
            else
                character.flipHorizontal = false;

            if (currState == AiState.jumping)
            {
                if (Math.Abs(character.velocity.Y) < 0)
                    currState = AiState.idle;
                else
                    return actions.ToArray();
            }

            if (shootCoolDown < gameTime.TotalGameTime || meleeCoolDown < gameTime.TotalGameTime)
            {
                currState = AiState.idle;
            }

            if ((currState == AiState.idle || currState == AiState.following) && Math.Abs(pos) > (2 * threshold) && Utility.rand.NextDouble() < .03)
            {
                currState = AiState.shoot;
                this.shootCoolDown = gameTime.TotalGameTime.Add(TimeSpan.FromSeconds(3));
                actions.Add(Character.GameAction.shoot);
            }

            //check projectiles
            if ((currState == AiState.idle || currState == AiState.following) && Math.Abs(pos) > threshold)
            {
                foreach (Projectile obj in gscreen.projectiles)
                {
                    if (!obj.isActive)
                        continue;

                    float distance = obj.position.X - this.character.position.X;

                    if ((distance < -threshold && obj.velocity.X > 0) || (distance > threshold && obj.velocity.X < 0))
                        if (prevActs.Contains(Character.GameAction.jump))
                            actions.Add(Character.GameAction.jump);
                }
            }
            else if (Math.Abs(pos) < threshold && Utility.rand.NextDouble() < .3)
            {
                currState = AiState.melee;
                this.meleeCoolDown = gameTime.TotalGameTime.Add(TimeSpan.FromSeconds(1));
                actions.Add(Character.GameAction.melee);
            }

            if ((currState == AiState.idle || currState == AiState.following) && pos > threshold)
            {
                currState = AiState.following;
                actions.Add(Character.GameAction.walkLeft);
            }
            else if ((currState == AiState.idle || currState == AiState.following) && pos < -threshold)
            {
                currState = AiState.following;
                actions.Add(Character.GameAction.walkRight);
            }
            
            return actions.ToArray();
        }
    }
}
