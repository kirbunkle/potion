using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class Item
    {
        public string Name { get; }
        public string SpriteName { get; }
        public string AnimationName { get; }
        public int[] AnimationFrames { get; }
        public int[] PlantAnimationFrames { get; }
        public int[] PlantSchedule { get; }

        public Item()
        {
            Name = "Ifir Blossom Seed";
            SpriteName = "spriteSheets/plants/fireplant";
            AnimationName = "animations/fireplant";
            AnimationFrames = new int[] { 5 };
            PlantAnimationFrames = new int[] { 0, 1, 2, 3, 4 };
            PlantSchedule = new int[] { 0, 1, 3, 5, 6 };
        }

        public void Use()
        {
            if (Game1.currentLocation.IsValidPlantableLocation(Game1.playerCharacter.GetToolRectangle()))
            {
                Game1.currentLocation.Plant(Game1.playerCharacter.GetToolRectangle(), this);
                Game1.inventory.DecrementItem(this);
            }
        }
    }
}
