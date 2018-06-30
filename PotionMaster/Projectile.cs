using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class Projectile : Collidable
    {
        private float velocityX;
        private float velocityY;
        private float speed;
        public bool Active { get; set; }

        public Projectile(Character caster)
        {
            speed = 0.4f;
            switch (caster.FacingDirection())
            {
                case Direction.Right: { velocityX = speed; velocityY = 0; break; }
                case Direction.Left: { velocityX = -speed; velocityY = 0; break; }
                case Direction.Down: { velocityX = 0; velocityY = speed; break; }
                case Direction.Up: { velocityX = 0; velocityY = -speed; break; }
            }
            //Vector2 v = caster.Velocity();
            //velocityX = (velocityX + v.X) / 2;
            //velocityY = (velocityY + v.Y) / 2;
            sprite = Game1.CreateSingleAnimatedSprite("spriteSheets/potions/firepot", "animations/firepot", new int[] { 0 }, isLooping: false);
            Vector2 v = caster.GetPosition();
            posX = (int)v.X;
            posY = (int)v.Y;
            collisionBox = MakeCollisionBoundingBox();
            Active = true;
        }

        public new void Update()
        {
            if (Active)
            { 
                posX += (int)(velocityX * Game1.dt);
                posY += (int)(velocityY * Game1.dt);
                if (Game1.currentLocation.IsCollidingWithImpassibleTile(GetCollisionBox(0, 0)))
                {
                    Active = false;
                }

                Interactable i = Game1.currentLocation.GetCollidingObject(GetCollisionBox());

                if ((i != null) && (i.GetType() == typeof(Enemy)))
                {
                    ((Enemy)i).Active = false;
                    Active = false;
                }

                //sprite.Rotation -= 0.03f * Game1.dt;
                //sprite.Scale += new Vector2(0.005f * Game1.dt, 0.005f * Game1.dt);
                base.Update();
            }
        }
    }
}
