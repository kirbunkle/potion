using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public abstract class Event
    {
        public Boolean Complete { set; get; }
        public Boolean GamePaused { set; get; }

        public Event()
        {
            Complete = false;
            GamePaused = true;
        }

        public abstract void Update();

        public abstract void Draw();

        public abstract void DrawHud();
    }
}
