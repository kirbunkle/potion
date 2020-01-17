using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PotionMaster
{
    public class Dialogue : Event
    {
        private List<string> text;
        private int index;
        private float posX;
        private float posY;
        private Texture2D texture;
        private Rectangle rect;
        private Color backgroundColor;

        public Dialogue(string s)
        {
            text = new List<string>();
            posX = (float)(Game1.screenW * 0.1);
            posY = (float)(Game1.screenH * 0.7);

            index = 0;

            texture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] colorData = new Color[1];

            colorData[0] = Color.White;
            texture.SetData(colorData);

            rect = new Rectangle(0, (int)(Game1.screenH * 0.6), Game1.screenW, (int)(Game1.screenH * 0.33));
            backgroundColor = new Color(Color.BlueViolet, 0.8F);

            text.Add(s);
        }

        public override void Update()
        {
            if ((Game1.input.ButtonPressed(GameButtons.A)) || (Game1.input.ButtonPressed(GameButtons.B)))
            {
                if (index < (text.Count - 1))
                {
                    index++;
                }
                else
                {
                    Complete = true;
                }
            }
        }

        public override void Draw()
        {

        }

        public override void DrawHud()
        {
            Game1.spriteBatch.Draw(texture, rect, backgroundColor);
            Game1.spriteBatch.DrawString(Game1.font, text[index], new Vector2(posX, posY), Color.Black);
        }
    }
}
