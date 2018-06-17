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

        public Item()
        {
            Name = "Ifir Blossom Seed";
            SpriteName = "spriteSheets/plants/fireplant";
            AnimationName = "animations/fireplant";
            AnimationFrames = new int[] { 5 };
        }
    }
}
