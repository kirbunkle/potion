using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;

namespace PotionMaster
{
    public abstract class Interactable : Drawable
    {
        protected Rectangle collisionBox;

        protected Rectangle MakeCollisionBoundingBox(AnimatedSprite s)
        {
            return new Rectangle(
                (Game1.tileSize / 8) - ((int)sprite.BoundingRectangle.Width / 2),
                ((int)sprite.BoundingRectangle.Height / 2) + (Game1.tileSize / 8) - (int)sprite.BoundingRectangle.Width,
                (int)sprite.BoundingRectangle.Width - ((Game1.tileSize / 8) * 2),
                (int)sprite.BoundingRectangle.Width - ((Game1.tileSize / 8)));
        }

        public Interactable()
        {

        }

        public abstract void Interact();

        public Rectangle GetCollisionBox(int mx = 0, int my = 0)
        {
            return new Rectangle(
                collisionBox.X + posX + mx,
                collisionBox.Y + posY + my,
                collisionBox.Width,
                collisionBox.Height);
        }
    }
}
