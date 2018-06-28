using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class Seed : Item
    {
        private int[] plantAnimationFrames;
        private int[] plantSchedule;
        private int cropYield;

        public Seed(List<string> data) : base(data)
        {
            //Type=Seed|Ifir Blossom Seed|spriteSheets/plants/fireplant|animations/fireplant|5|0,1,2,3,4|0,1,3,5,6|2

            string[] indexes = data[5].Split(',');
            plantAnimationFrames = new int[indexes.Count()];
            for (int i = 0; i < indexes.Count(); i++)
            {
                plantAnimationFrames[i] = Int32.Parse(indexes[i]);
            }
            indexes = data[6].Split(',');
            plantSchedule = new int[indexes.Count()];
            for (int i = 0; i < indexes.Count(); i++)
            {
                plantSchedule[i] = Int32.Parse(indexes[i]);
            }
            cropYield = Int32.Parse(data[7]);
        }

        public int[] PlantAnimationFrames()
        {
            return plantAnimationFrames;
        }

        public int[] PlantSchedule()
        {
            return plantSchedule;
        }

        public int CropYield()
        {
            return cropYield;
        }

        public override void Use()
        {
            if (Game1.currentLocation.IsValidPlantableLocation(Game1.playerCharacter.GetToolRectangle()))
            {
                Game1.currentLocation.Plant(Game1.playerCharacter.GetToolRectangle(), this);
                Game1.inventory.DecrementItem(this);
            }
        }
    }
}
