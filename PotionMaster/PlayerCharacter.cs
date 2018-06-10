using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace PotionMaster
{
    class PlayerCharacter : Character
    {
        private float speed;

        public PlayerCharacter()
        {
            speed = 0.15f;
        }

        public new void Update()
        {
            float moveX = 0;
            float moveY = 0;
            if (Game1.keyboardInput.IsKeyDown(Keys.Left))
            {
                moveX -= speed * Game1.dt;
            }
            if (Game1.keyboardInput.IsKeyDown(Keys.Right))
            {
                moveX += speed * Game1.dt;
            }
            if (Game1.keyboardInput.IsKeyDown(Keys.Up))
            {
                moveY -= speed * Game1.dt;
            }
            if (Game1.keyboardInput.IsKeyDown(Keys.Down))
            {
                moveY += speed * Game1.dt;
            }
            if ((moveX != 0) && (moveY != 0))
            {
                moveX *= 0.7f;
                moveY *= 0.7f;
            }
            Move(moveX, moveY);
            base.Update();
        }
    }


}