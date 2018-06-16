using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PotionMaster
{
    public class Item
    {
        private Texture2D image;

        public string Name { get; set; }
        
        public Item()
        {
            Name = "Ifir Blossom Seed";
            image = Game1.content.Load<Texture2D>("");
        }
    }
}
