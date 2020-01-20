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
        private Vector2 centerPos;

        public WarpEvent(string mapNameIn, Vector2 centerPosIn)
        {
            mapName = mapNameIn;
            centerPos = centerPosIn;
        }

        public override void Update()
        {
            Game1.WarpPlayer(mapName, centerPos);
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
