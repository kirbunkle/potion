using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PotionMaster
{
    public class PlayerCharacter : Character
    {
        private float speed;
        private bool drawInteractBox;
        private Texture2D interactBoxTexture;
        private Rectangle interactBoxRectangle;

        private Rectangle FindInteractBoxRectangle()
        {
            int x = (posX / Game1.tileSize) * Game1.tileSize;
            int y = (posY / Game1.tileSize) * Game1.tileSize;
            switch (facingDirection)
            {
                case Direction.Down:
                    y += Game1.tileSize;
                    break;
                case Direction.Up:
                    y -= Game1.tileSize;
                    break;
                case Direction.Left:
                    x -= Game1.tileSize;
                    break;
                case Direction.Right:
                    x += Game1.tileSize;
                    break;
            }
            return new Rectangle(x, y, Game1.tileSize, Game1.tileSize);
        }

        public PlayerCharacter()
        {
            speed = 0.15f;
            drawInteractBox = true;
            interactBoxTexture = Game1.content.Load<Texture2D>("spriteSheets/simplebox");
            interactBoxRectangle = FindInteractBoxRectangle();
        }

        public new void Update()
        {
            float moveX = 0;
            float moveY = 0;
            if (Game1.keyboardInput.IsKeyDown(Keys.Left)) // TODO: make a wrapper of the key inputs for configurable keys
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
            if (Game1.keyboardInput.IsKeyDown(Keys.Space))
            {
                Interactable obj = Game1.currentLocation.GetCollidingObject(interactBoxRectangle); // TODO: make generic for interactible 
                if (obj != null)
                {
                    obj.Interact();
                }
            }
            Move(moveX, moveY);
            base.Update();
            interactBoxRectangle = FindInteractBoxRectangle();
        }

        public new void Draw()
        {
            base.Draw();
            if (drawInteractBox)
            {
                Game1.spriteBatch.Draw(interactBoxTexture, GetCollisionBox(), Color.YellowGreen);
                Game1.spriteBatch.Draw(interactBoxTexture, interactBoxRectangle, Color.OrangeRed);
            }
        }
    }


}