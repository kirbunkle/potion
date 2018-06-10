using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;

namespace PotionMaster
{
    class Character
    {
        private AnimatedSprite sprite;
        private SpriteSheetAnimationFactory spriteFactory;
        public Vector2 position { get; set; }
        protected float y;
        private string curSprite;


        public Character()
        {
            var characterTexture = Game1.content.Load<Texture2D>("main2");
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
            sprite = new AnimatedSprite(spriteFactory, curSprite);
            position = new Vector2(300, 300);
        }

        public void Move(float mx, float my)
        {
            string oldSprite = curSprite;
            if (my > 0) 
                curSprite = "down";
            else if (my < 0)
                curSprite = "up";
            else if (mx < 0)
                curSprite = "left";
            else if (mx > 0)
                curSprite = "right";
            else if (!curSprite.Contains("idle"))
                curSprite += "_idle";

            if (oldSprite != curSprite)
                sprite = new AnimatedSprite(spriteFactory, curSprite);

            position = new Vector2(position.X + mx, position.Y + my);
        }

        public void Update()
        {
            var pos = position;
            pos.X = (float)Math.Floor(pos.X);
            pos.Y = (float)Math.Floor(pos.Y);
            sprite.Position = pos;
            sprite.Update(Game1.gt);
        }

        public void Draw()
        {
            Game1.spriteBatch.Draw(sprite);
        }
    }
}
