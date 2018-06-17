using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PotionMaster
{
    public class Inventory
    {
        private List<InventoryStack> items;

        public Inventory()
        {
            items = new List<InventoryStack>();
        }

        public void AddItem(Item i)
        {
            foreach (InventoryStack s in items)
            {
                if (i == s.Item)
                {
                    s.Count += 1;
                    return;
                }
            }
            items.Add(new InventoryStack(i));
        }

        public void Update()
        {
            int posX = Game1.tileSize;
            int posY = Game1.screenH - Game1.tileSize;

            foreach (InventoryStack i in items)
            {
                i.SetPosition(posX, posY);
                i.Update();
                posX += Game1.tileSize * 2;
            }
        }

        public void DrawHud()
        {
            foreach (InventoryStack i in items)
            {
                i.Draw();
                Game1.spriteBatch.DrawString(Game1.font, i.Count.ToString(), i.GetPosition() + new Vector2(Game1.tileSize, 0), Color.White);
            }
        }
    }
}
