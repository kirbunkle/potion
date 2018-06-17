using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PotionMaster
{
    public class LocationObject : Interactable
    {
        private Item item;
        
        public LocationObject()
        {
            item = new Item();
            sprite = Game1.CreateSingleAnimatedSprite(item.SpriteName, item.AnimationName, item.AnimationFrames, isLooping: false);
            posX = (int)(Game1.tileSize * 3.5);
            posY = (int)(Game1.tileSize * 4.5);
            collisionBox = MakeCollisionBoundingBox(sprite);
        }

        public override void Interact()
        {
            Game1.inventory.AddItem(item);
        }
    }
}
