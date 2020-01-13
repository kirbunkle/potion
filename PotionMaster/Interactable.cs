using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;

namespace PotionMaster
{
    public abstract class Interactable : Collidable
    {
        public Interactable(Location loc) : base(loc)
        {

        }

        public abstract void Interact();
    }
}
