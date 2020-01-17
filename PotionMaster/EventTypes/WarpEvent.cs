using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class WarpEvent : Event
    {
        private string mapName;
        private Vector2 pos;

        public WarpEvent(string mapNameIn, Vector2 posIn)
        {
            mapName = mapNameIn;
            pos = posIn;
        }

        public override void Update()
        {
            Game1.WarpPlayer(mapName, pos);
            Complete = true;
        }

        public override void Draw()
        {
            
        }

        public override void DrawHud()
        {
            
        }
    }
}
