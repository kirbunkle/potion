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
        
        public Warp(TiledMapObject ob)
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
                if (OffsetPos.X > 0) pos.X += collisionBox.Width; else if (OffsetPos.X < 0) pos.X -= Game1.playerCharacter.GetCollisionBox().Width;
                if (OffsetPos.Y > 0) pos.Y += collisionBox.Height; else if (OffsetPos.Y < 0) pos.Y -= Game1.playerCharacter.GetCollisionBox().Height;
                Game1.WarpPlayer(DestinationMap, pos);
            }
        }

        public override void Interact()
        {
            
        }
    }
}
