using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.BitmapFonts;

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

        public Character()
        {
            velocityX = 0;
            velocityY = 0;
            if (GetType() == typeof(PlayerCharacter))
            {
                spriteFactory = Game1.CreateAnimationFactory("spriteSheets/main2", "animations/test1");

                spriteFactory.Add("down", new SpriteSheetAnimationData(new[] { 0, 1, 2, 1 }, isLooping: true));
                spriteFactory.Add("left", new SpriteSheetAnimationData(new[] { 3, 4, 5, 4 }, isLooping: true));
                spriteFactory.Add("right", new SpriteSheetAnimationData(new[] { 6, 7, 8, 7 }, isLooping: true));
                spriteFactory.Add("up", new SpriteSheetAnimationData(new[] { 9, 10, 11, 10 }, isLooping: true));

                spriteFactory.Add("down_idle", new SpriteSheetAnimationData(new[] { 1 }, isLooping: false));
                spriteFactory.Add("left_idle", new SpriteSheetAnimationData(new[] { 4 }, isLooping: false));
                spriteFactory.Add("right_idle", new SpriteSheetAnimationData(new[] { 7 }, isLooping: false));
                spriteFactory.Add("up_idle", new SpriteSheetAnimationData(new[] { 10 }, isLooping: false));

                curSprite = "down_idle";
                posX = 300;
                posY = 300;
                sprite = new AnimatedSprite(spriteFactory, curSprite);
                collisionBox = MakeCollisionBoundingBox(sprite);
            }
            else
            {
                spriteFactory = Game1.CreateAnimationFactory("spriteSheets/io1", "animations/io1");
                spriteFactory.Add("down_idle", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4, 5, 6, 7 }, isLooping: true, frameDuration: 0.1f));
                curSprite = "down_idle";
                posX = 528;
                posY = 272;
                sprite = new AnimatedSprite(spriteFactory, curSprite);
            }
            facingDirection = Direction.Down;
            collisionBox = MakeCollisionBoundingBox(sprite);
        }

        public override void Interact()
        {
            Game1.gameDateTime.AdvanceDay();
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
                sprite = new AnimatedSprite(spriteFactory, curSprite);

            velocityX += mx;
            velocityY += my;

            if (Game1.currentLocation.IsColliding(GetCollisionBox((int)velocityX, 0)))
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

            if (Game1.currentLocation.IsColliding(GetCollisionBox(0, (int)velocityY)))
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
