using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace PotionMaster
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private Vector2 location;

        public Sprite()
        {
            // TODO sprite content loader, pass in sprite name, look up dimensions, pass in position
            Texture = Game1.content.Load<Texture2D>("main2");
            Rows = 8;
            Columns = 12;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            location = new Vector2(200, 200);
        }

        public void Update(float moveX, float moveY)
        {
            if (moveX < 0)
            {
                currentFrame++;
                if (currentFrame == totalFrames) currentFrame = 0;
            }
            if (moveX > 0)
            {
                currentFrame--;
                if (currentFrame < 0) currentFrame = totalFrames - 1;
            }
            location.X += moveX;
            location.Y += moveY;
        }

        public void Draw()
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, (int)(width * (location.Y / 200)), (int)(height * (location.Y / 200)));

            Game1.spriteBatch.Begin();
            Game1.spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            Game1.spriteBatch.End();
        }
    }
}