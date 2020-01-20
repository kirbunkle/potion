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
        private List<Event> events;

        // TODO: probably don't need these
        protected bool drawToolInteractBox;
        protected Texture2D interactBoxTexture;
        protected Rectangle interactBoxRectangle;
        protected Rectangle interactToolBoxRectangle;

        protected float speed;
        protected Direction facingDirection;

        public Character(List<string> data, int x, int y, Location loc) : base(loc)
        {
            drawToolInteractBox = true;
            interactBoxTexture = Game1.content.Load<Texture2D>("spriteSheets/simplebox");
            interactToolBoxRectangle = FindInteractToolBoxRectangle();
            interactBoxRectangle = FindInteractBoxRectangle();

            velocityX = 0;
            velocityY = 0;

            curSprite = "";
            speed = 0.15f;

            events = new List<Event>();

            spriteFactory = Game1.CreateAnimationFactory(data[2], data[3]);
            float frameDuration = float.Parse(data[4]);
            for (int i = 5; i < data.Count; i++)
            {
                var indexes = data[i].Split(',');
                int[] animationFrames = new int[indexes.Count() - 1];
                for (int j = 0; j < indexes.Count() - 1; j++)
                {
                    animationFrames[j] = Int32.Parse(indexes[j + 1]);
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

        // TODO: likewise probably dont need these things but idk
        protected Rectangle MakeInteractRectangle(int xIn, int yIn)
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

        protected Rectangle FindInteractToolBoxRectangle()
        {
            return MakeInteractRectangle(((posX + (Game1.tileSize / 2)) / Game1.tileSize) * Game1.tileSize, ((posY + (Game1.tileSize / 2)) / Game1.tileSize) * Game1.tileSize);
        }

        protected Rectangle FindInteractBoxRectangle()
        {
            return MakeInteractRectangle(posX, posY);
        }

        public override void Interact()
        {
            Game1.PushEvent(new Dialogue("I'm so busy... please leave me alone."));
            Vector2 c = GetCenterCollisionBox();
            int tileX = (int)c.X / Game1.tileSize;
            int tileY = (int)c.Y / Game1.tileSize;
            events.Add(new MovePathEvent(location, this, tileX, tileY, (tileX + Game1.gameDateTime.Minute) % location.TileMapW(), (tileY + Game1.gameDateTime.Hour) % location.TileMapH()));
        }

        public override void Collide(Collidable obj)
        {
            //Game1.PushEvent(new Dialogue("Yo, don't bump me!"));
        }

        public Vector2 Velocity()
        {
            return new Vector2(velocityX, velocityY);
        }

        public Direction FacingDirection()
        {
            return facingDirection;
        }

        public Vector2 Move(float mx, float my)
        {
            Vector2 result = new Vector2(posX, posY);
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
                var obj = location.GetCollidingObject(GetCollisionBox((int)velocityX, (int)velocityY), this);
                if (obj != null)
                {
                    obj.Collide(this);
                }
            }

            if ((location != null) && (location.IsColliding(GetCollisionBox((int)velocityX, 0), this)))
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

            if ((location != null) && (location.IsColliding(GetCollisionBox(0, (int)velocityY), this)))
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
            result.X -= posX;
            result.Y -= posY;
            return result;
        }

        public Vector2 WalkTowardCenter(int destX, int destY, Boolean finishWalkingIfZero = false)
        {
            Vector2 result = GetCenterCollisionBox();
            result.X = destX - result.X;
            result.Y = destY - result.Y;
            float myVelocity = speed * Game1.dt;
            if ((result.X != 0) && (result.Y != 0))
            {
                myVelocity *= 0.7F;
            }

            if (result.X > myVelocity) result.X = myVelocity;
            if (result.X < -myVelocity) result.X = -myVelocity;
            if (result.Y > myVelocity) result.Y = myVelocity;
            if (result.Y < -myVelocity) result.Y = -myVelocity;

            if (!((result.X == 0) && (result.Y == 0)) || finishWalkingIfZero) Move(result.X, result.Y);
            return result;
        }

        public new void Update()
        {
            while (events.Count > 0)
            {
                Event currentEvent = events[events.Count - 1];
                currentEvent.Update();
                if (currentEvent.Complete)
                {
                    events.Remove(currentEvent);
                }
                else
                {
                    break;
                }
            }
            base.Update();
        }

        public new void Draw()
        {
            base.Draw();
            Game1.spriteBatch.Draw(interactBoxTexture, GetCollisionBox(), Color.YellowGreen);
        }
    }
}
