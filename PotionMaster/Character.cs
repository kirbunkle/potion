using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace PotionMaster
{
    class Character
    {
        private Sprite sprite;
        protected float moveX;
        protected float moveY;


        public Character()
        {
            sprite = new Sprite();
            moveX = 0;
            moveY = 0;
        }

        public void Update()
        {
            sprite.Update(moveX, moveY);
        }

        public void Draw()
        {
            sprite.Draw();
        }
    }
}
