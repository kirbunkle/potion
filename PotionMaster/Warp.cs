using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    class Warp : Interactable
    {
        public Vector2 OffsetPos { get; set; }
        public string DestinationMap { get; set; }
        public string DestinationWarp { get; set; }
        public string Name { get; set; }
        
        public Warp(TiledMapObject ob, Location loc) : base(loc)
        {
            collisionBox = new Rectangle(0, 0, (int)ob.Size.Width, (int)ob.Size.Height);
            posX = (int)ob.Position.X;
            posY = (int)ob.Position.Y;
            var pos = new Vector2(float.Parse(ob.Properties["xPos"]), float.Parse(ob.Properties["yPos"]));         
            OffsetPos = pos;
            DestinationMap = ob.Properties["destMap"];
            DestinationWarp = ob.Properties["destWarpName"];
            Name = ob.Name;
        }

        public override void Collide(Collidable obj)
        {
            if ((obj == Game1.playerCharacter) && (Game1.locations.TryGetValue(DestinationMap, out Location loc)))
            {
                var pos = loc.GetWarpPos(DestinationWarp);
                pos.X += OffsetPos.X;
                pos.Y += OffsetPos.Y;
                if (OffsetPos.X > 0) pos.X += (int)(collisionBox.Width * 1.5); else if (OffsetPos.X < 0) pos.X -= (int)(Game1.playerCharacter.GetCollisionBox().Width * 1.5);
                if (OffsetPos.Y > 0) pos.Y += (int)(collisionBox.Height * 1.5); else if (OffsetPos.Y < 0) pos.Y -= (int)(Game1.playerCharacter.GetCollisionBox().Height * 1.5);
                Game1.PushEvent(new Fade(false));
                Game1.PushEvent(new WarpEvent(DestinationMap, pos));
                Game1.PushEvent(new Fade(true));
            }
        }

        public override void Interact()
        {
            
        }
    }
}
