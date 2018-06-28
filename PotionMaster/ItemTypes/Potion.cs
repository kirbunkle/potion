using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class Potion : Item
    {
        public Potion(List<string> data) : base(data)
        {

        }

        public override void Use()
        {
            Game1.inventory.DecrementItem(this);
        }
    }
}
