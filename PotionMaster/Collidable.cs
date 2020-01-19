using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public abstract class Collidable : Drawable
    {
        protected Rectangle collisionBox;
        protected Location location;

        protected Rectangle MakeCollisionBoundingBox()
        {
            return new Rectangle(
                (Game1.tileSize / 6),
                ((int)sprite.BoundingRectangle.Height / 2) + (Game1.tileSize / 6) - ((int)sprite.BoundingRectangle.Width / 2),
                (int)sprite.BoundingRectangle.Width - ((Game1.tileSize / 6) * 2),
                (int)sprite.BoundingRectangle.Width - ((Game1.tileSize / 6)) - 1);
        }

        public Collidable(Location loc)
        {
            location = loc;
        }

        public void SetLocation(Location loc)
        {
            location = loc;
        }

        public Location GetLocation()
        {
            return location;
        }
        
        public Rectangle GetCollisionBox(int mx = 0, int my = 0)
        {
            return new Rectangle(
                collisionBox.X + posX + mx,
                collisionBox.Y + posY + my,
                collisionBox.Width,
                collisionBox.Height);
        }

        public abstract void Collide(Collidable obj);
    }
}
