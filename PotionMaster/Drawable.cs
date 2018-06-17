using Microsoft.Xna.Framework;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class Drawable
    {
        protected AnimatedSprite sprite;
        protected int posX;
        protected int posY;

        public Drawable()
        {

        }

        public Vector2 GetPosition()
        {
            return new Vector2(posX, posY);
        }

        public void SetPosition(int x, int y)
        {
            posX = x;
            posY = y;
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
