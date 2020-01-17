using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class Fade : Event
    {
        private double speed;
        private double value;
        private Boolean inverse;
        private Texture2D texture;

        public Fade(Boolean doFadeOut)
        {
            speed = 0.005;
            inverse = !doFadeOut;
            value = 0;
            texture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] colorData = new Color[1];

            colorData[0] = Color.Black;
            texture.SetData(colorData);
        }

        public override void Update()
        {
            value += speed * Game1.dt;
            if (value > 1)
            {
                value = 1;
                Complete = true;
            }
        }

        public override void Draw()
        {
            
        }

        public override void DrawHud()
        {
            double fadeValue = value;
            if (inverse)
            {
                fadeValue = 1 - fadeValue;
            }
            Game1.spriteBatch.Draw(texture, new Rectangle(0, 0, Game1.screenW, Game1.screenH), new Color(Color.Black, (float)fadeValue)); 
        }
    }
}
