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

        public Enemy(List<string> data, int x, int y) : base(data, x, y) //number one
        {
            Active = true;
        }        
    }
}
