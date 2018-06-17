using MonoGame.Extended.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class InventoryStack : Drawable
    {
        public Item Item { get; }
        public int Count { get; set; }

        public InventoryStack(Item i, int x = 0, int y = 0)
        {
            Item = i;
            posX = x;
            posY = y;
            sprite = Game1.CreateSingleAnimatedSprite(Item.SpriteName, Item.AnimationName, Item.AnimationFrames, isLooping: false);
            Count = 1;
        }
    }
}
