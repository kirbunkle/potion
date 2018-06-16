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
        private AnimatedSprite sprite;
        private SpriteSheetAnimationFactory spriteFactory;
        private string curSprite;
        private Rectangle collisionBox;
        protected int posX;
        protected int posY;
        private float velocityX;
        private float velocityY;

        protected Direction facingDirection;
        

        private Rectangle MakeCollisionBoundingBox(AnimatedSprite s)
        {
            return new Rectangle(
                (Game1.tileSize / 8) - ((int)sprite.BoundingRectangle.Width / 2),
                ((int)sprite.BoundingRectangle.Height / 2) + (Game1.tileSize / 8) - (int)sprite.BoundingRectangle.Width,
                (int)sprite.BoundingRectangle.Width - ((Game1.tileSize / 8) * 2),
                (int)sprite.BoundingRectangle.Width - ((Game1.tileSize / 8)));
        }

        public Character()
        {
            velocityX = 0;
            velocityY = 0;
            if (GetType() == typeof(PlayerCharacter))
            {
                var characterTexture = Game1.content.Load<Texture2D>("spriteSheets/main2");
                var characterMap = Game1.content.Load<Dictionary<string, Rectangle>>("animations/test1");
                var characterAtlas = new TextureAtlas("test1", characterTexture, characterMap);
                spriteFactory = new SpriteSheetAnimationFactory(characterAtlas);

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
                var characterTexture = Game1.content.Load<Texture2D>("spriteSheets/io1");
                var characterMap = Game1.content.Load<Dictionary<string, Rectangle>>("animations/io1");
                var characterAtlas = new TextureAtlas("io1", characterTexture, characterMap);
                spriteFactory = new SpriteSheetAnimationFactory(characterAtlas);
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
            posX += Game1.tileSize * 2;
            posY += Game1.tileSize * 2;
        }

        public Rectangle GetCollisionBox(int mx = 0, int my = 0)
        {
            return new Rectangle(
                collisionBox.X + posX + mx, 
                collisionBox.Y + posY + my, 
                collisionBox.Width, 
                collisionBox.Height);
        }
        
        public Vector2 GetPosition()
        {
            return new Vector2(posX, posY);
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

        public void Update()
        {
            sprite.Position = GetPosition();
            sprite.Update(Game1.gt);
        }

        public void Draw()
        {
            Game1.spriteBatch.Draw(sprite);
        }
    }
}
