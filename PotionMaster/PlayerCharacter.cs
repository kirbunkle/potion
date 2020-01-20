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
        public PlayerCharacter(List<string> data, int x, int y, Location loc) : base(data, x, y, loc)
        {

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
                Interactable obj = location.GetCollidingObject(interactBoxRectangle);
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
                Game1.spriteBatch.Draw(interactBoxTexture, 
                    new Rectangle((int)sprite.BoundingRectangle.X, (int)sprite.BoundingRectangle.Y, 
                    (int)sprite.BoundingRectangle.Width, (int)sprite.BoundingRectangle.Height), Color.Indigo);
                Game1.spriteBatch.Draw(interactBoxTexture, interactToolBoxRectangle, Color.OrangeRed);
                Game1.spriteBatch.Draw(interactBoxTexture, interactBoxRectangle, Color.Purple);
            }
        }

    }


}