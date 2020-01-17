using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PotionMaster
{
    public class Dialogue : Event
    {
        private string text;
        private float posX;
        private float posY;

        public Dialogue()
        {
            text = "Hey hey, you're gay.";
            posX = Game1.screenW / 2;
            posY = Game1.screenH / 2;
        }

        public override void Update()
        {
            if ((Game1.input.ButtonPressed(GameButtons.A)) || (Game1.input.ButtonPressed(GameButtons.B)))
            {
                Complete = true;
            }
        }

        public override void Draw()
        {

        }

        public override void DrawHud()
        {
            Game1.spriteBatch.DrawString(Game1.font, text, new Vector2(posX, posY), Color.Crimson);
        }
    }
}
