using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class Enemy : Character
    {
        public bool Active { get; set; }

        public Enemy(List<string> data, int x, int y, Location loc) : base(data, x, y, loc) //number one
        {
            Active = true;
        }        
    }
}
