using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.BitmapFonts;
using System.Linq;

namespace PotionMaster
{
    public enum Direction { Down, Left, Right, Up };

    public class Character : Interactable
    {
        private SpriteSheetAnimationFactory spriteFactory;
        private string curSprite;
        private float velocityX;
        private float velocityY;

        protected Direction facingDirection;        

        public Character(List<string> data, int x, int y, Location loc) : base(loc)
        {
            velocityX = 0;
            velocityY = 0;

            curSprite = "";

            spriteFactory = Game1.CreateAnimationFactory(data[2], data[3]);
            float frameDuration = float.Parse(data[4]);
            for (int i = 5; i < data.Count; i++)
            {
                var indexes = data[i].Split(',');
                int[] animationFrames = new int[indexes.Count()-1];
                for (int j = 0; j < indexes.Count() - 1; j++)
                {
                    animationFrames[j] = Int32.Parse(indexes[j+1]);
                }
                spriteFactory.Add(indexes[0], new SpriteSheetAnimationData(animationFrames, frameDuration, (animationFrames.Count() > 1)));
            }
            curSprite = "down_idle";
            facingDirection = Direction.Down;
            sprite = Game1.CreateAnimatedSprite(spriteFactory, curSprite);
            collisionBox = MakeCollisionBoundingBox();
            posX = x;
            posY = y;
        }

        public override void Interact()
        {
            Game1.gameDateTime.AdvanceDay();
        }

        public override void Collide(Collidable obj)
        {

        }

        public Vector2 Velocity()
        {
            return new Vector2(velocityX, velocityY);
        }

        public Direction FacingDirection()
        {
            return facingDirection;
        }

        public void Move(float mx, float my)
        {
            string oldSprite = curSprite;
            if (my > 0)
            {
                curSprite = "down";
                facingDirection = Direction.Down;
            }
            else if (my < 0)
            {
                curSprite = "up";
                facingDirection = Direction.Up;
            }
            else if (mx < 0)
            {
                curSprite = "left";
                facingDirection = Direction.Left;
            }
            else if (mx > 0)
            {
                curSprite = "right";
                facingDirection = Direction.Right;
            }
            else if (!curSprite.Contains("idle"))
            {
                curSprite += "_idle";
            }

            if (oldSprite != curSprite)
                sprite = Game1.CreateAnimatedSprite(spriteFactory, curSprite); //TODO should we just store this?

            velocityX += mx;
            velocityY += my;

            if (location != null)
            {
                var obj = location.GetCollidingObject(GetCollisionBox((int)velocityX, (int)velocityY));
                if (obj != null)
                {
                    obj.Collide(this);
                }
            }

            if ((location != null) && (location.IsColliding(GetCollisionBox((int)velocityX, 0))))
            {
                velocityX /= 16;
            }
            else if (Math.Abs(velocityX) < 1)
            {
                velocityX = 0;
            }
            else
            {
                posX = posX + (int)velocityX;
                velocityX %= 1;
            }

            if ((location != null) && (location.IsColliding(GetCollisionBox(0, (int)velocityY))))
            {
                velocityY /= 16;
            }
            else if (Math.Abs(velocityY) < 1)
            {
                velocityY = 0;
            }
            else
            {
                posY = posY + (int)velocityY;
                velocityY %= 1;
            }
        }
    }
}
