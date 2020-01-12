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
        
        public LocationObject(Item i)
        {
            item = i;
            sprite = Game1.CreateSingleAnimatedSprite(item.SpriteName(), item.AnimationName(), item.AnimationFrames(), isLooping: false);
            posX = Game1.tileSize * 4;
            posY = Game1.tileSize * 5;
            collisionBox = MakeCollisionBoundingBox();
        }

        public override void Interact()
        {
            Game1.inventory.AddItem(item);
        }

        public override void Collide(Collidable obj)
        {
            
        }
    }
}
