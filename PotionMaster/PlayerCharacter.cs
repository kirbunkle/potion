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
        private bool drawToolInteractBox;
        private Texture2D interactBoxTexture;
        private Rectangle interactBoxRectangle;
        private Rectangle interactToolBoxRectangle;

        private Rectangle MakeInteractRectangle(int xIn, int yIn)
        {
            int x = xIn;
            int y = yIn;
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

        private Rectangle FindInteractToolBoxRectangle()
        {
            return MakeInteractRectangle(((posX + (Game1.tileSize / 2)) / Game1.tileSize) * Game1.tileSize, ((posY + (Game1.tileSize / 2)) / Game1.tileSize) * Game1.tileSize);
        }

        private Rectangle FindInteractBoxRectangle()
        {
            return MakeInteractRectangle(posX, posY);
        }

        public PlayerCharacter(List<string> data, int x, int y) : base(data, x, y)
        {
            speed = 0.15f;
            drawToolInteractBox = true;
            interactBoxTexture = Game1.content.Load<Texture2D>("spriteSheets/simplebox");
            interactToolBoxRectangle = FindInteractToolBoxRectangle();
            interactBoxRectangle = FindInteractBoxRectangle();
        }

        public Rectangle GetToolRectangle()
        {
            return interactToolBoxRectangle;
        }

        public new void Update()
        {
            float moveX = 0;
            float moveY = 0;
            if (Game1.input.ButtonDown(GameButtons.Left)) 
            {
                moveX -= speed * Game1.dt;
            }
            if (Game1.input.ButtonDown(GameButtons.Right))
            {
                moveX += speed * Game1.dt;
            }
            if (Game1.input.ButtonDown(GameButtons.Up))
            {
                moveY -= speed * Game1.dt;
            }
            if (Game1.input.ButtonDown(GameButtons.Down))
            {
                moveY += speed * Game1.dt;
            }
            if ((moveX != 0) && (moveY != 0))
            {
                moveX *= 0.7f;
                moveY *= 0.7f;
            }
            if (Game1.input.ButtonPressed(GameButtons.A))
            {
                // interact
                Interactable obj = Game1.currentLocation.GetCollidingObject(interactBoxRectangle);
                if (obj != null)
                {
                    obj.Interact();
                }
            }
            else if (Game1.input.ButtonPressed(GameButtons.B))
            {
                // use item
                Item i = Game1.inventory.GetCurrentItem();
                if (i != null) i.Use();
            }
            else if (Game1.input.ButtonPressed(GameButtons.Tab))
            {
                // swap items
                Game1.inventory.SwapItems(1);
            }
            Move(moveX, moveY);
            base.Update();
            interactToolBoxRectangle = FindInteractToolBoxRectangle();
            interactBoxRectangle = FindInteractBoxRectangle();
        }

        public new void Draw()
        {
            base.Draw();
            if (drawToolInteractBox)
            {
                //Game1.spriteBatch.Draw(interactBoxTexture, GetCollisionBox(), Color.YellowGreen);
                //Game1.spriteBatch.Draw(interactBoxTexture, 
                //    new Rectangle((int)sprite.BoundingRectangle.X, (int)sprite.BoundingRectangle.Y, 
                //    (int)sprite.BoundingRectangle.Width, (int)sprite.BoundingRectangle.Height), Color.Indigo);
                Game1.spriteBatch.Draw(interactBoxTexture, interactToolBoxRectangle, Color.OrangeRed);
                //Game1.spriteBatch.Draw(interactBoxTexture, interactBoxRectangle, Color.Purple);
            }
        }
    }


}