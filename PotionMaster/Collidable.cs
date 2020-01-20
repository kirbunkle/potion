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
            int t = Game1.tileSize / 6;
            int l = (int)sprite.BoundingRectangle.Width - (2 * t);

            return new Rectangle(
                t,
                (int)sprite.BoundingRectangle.Height - (l + t),
                l,
                l);
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

        public Vector2 GetCenterCollisionBox()
        {
            Rectangle box = GetCollisionBox();
            return new Vector2(box.X + (box.Width / 2), box.Y + (box.Height / 2));
        }

        public void SetCenterPosition(Vector2 v)
        {
            Vector2 centerV = GetCenterCollisionBox();
            Vector2 cornerV = GetPosition();
            centerV -= cornerV;
            SetPosition(v - centerV);
        }

        public abstract void Collide(Collidable obj);
    }
}
