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

        public Projectile(Character caster, Location loc) : base(loc)
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

        public override void Collide(Collidable obj)
        {

        }

        public new void Update()
        {
            if (Active)
            { 
                posX += (int)(velocityX * Game1.dt);
                posY += (int)(velocityY * Game1.dt);
                velocityX *= (float)(0.95);
                velocityY *= (float)(0.95);

                if (Math.Abs(velocityX) < 0.1) velocityX = 0;
                if (Math.Abs(velocityY) < 0.1) velocityY = 0;


                if (location != null) 
                {
                    if (location.IsCollidingWithImpassibleTile(GetCollisionBox(0, 0)))
                    {
                        Active = false;
                    }

                    Interactable i = location.GetCollidingObject(GetCollisionBox());

                    if (i != null)
                    {
                        if (i.GetType() == typeof(Enemy))
                        {
                            ((Enemy)i).Active = false;
                            Active = false;
                        }
                    }
                    if ((velocityX == 0) && (velocityY == 0))
                    {
                        Active = false;
                    }
                }                

                //sprite.Rotation -= 0.03f * Game1.dt;
                //sprite.Scale += new Vector2(0.005f * Game1.dt, 0.005f * Game1.dt);
                base.Update();
            }
        }
    }
}
