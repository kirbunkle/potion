using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{

    public abstract class Item
    {
        protected string name;
        protected string spriteName;
        protected string animationName;
        protected int[] animationFrames;

        public Item(List<string> data)
        {
            name = data[1];
            spriteName = data[2];
            animationName = data[3];
            var indexes = data[4].Split(',');
            animationFrames = new int[indexes.Count()];
            for (int i = 0; i < indexes.Count(); i++)
            {
                animationFrames[i] = Int32.Parse(indexes[i]);
            }
        }

        public string Name()
        {
            return name;
        }

        public string SpriteName()
        {
            return spriteName;
        }

        public string AnimationName()
        {
            return animationName;
        }

        public int[] AnimationFrames()
        {
            return animationFrames;
        }

        public abstract void Use();
    }
}
