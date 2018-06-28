using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private int currentItem;
        private Texture2D selectBoxTexture;
        private Rectangle selectBoxRectangle;
        private Vector2 selectedItemNamePosition;

        private void DeleteItem(int pos)
        {
            if ((pos >= 0) && (pos < items.Count)) items.RemoveAt(pos);
        }

        public Inventory()
        {
            items = new List<InventoryStack>();
            currentItem = -1;
            selectBoxTexture = Game1.content.Load<Texture2D>("spriteSheets/simplebox");
            selectBoxRectangle = new Rectangle();
            selectedItemNamePosition = new Vector2(Game1.tileSize, Game1.screenH - (Game1.tileSize * 2));
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
            if (currentItem < 0)
            {
                currentItem = 0;
            }
        }

        public void AddItem(int i)
        {
            AddItem(Game1.itemManager.GetItem(i));
        }

        public void DecrementItem(Item item)
        {
            if (items.Count() <= 0) return;
            int i = currentItem;
            do
            {
                if (items[i].Item == item)
                {
                    items[i].Count--;
                    if (items[i].Count <= 0)
                    {
                        items.RemoveAt(i);
                        if (items.Count <= 0) currentItem = -1;
                        else currentItem %= items.Count;
                    }
                    break;
                }
                i++;
                if (i >= items.Count()) i = 0;
            }
            while (i != currentItem);
        }

        public Item GetCurrentItem()
        {
            if (items.Count > 0)
                return items[currentItem].Item;
            return null;
        }
        
        public void SwapItems(int n)
        {
            if (items.Count > 0)
            {
                currentItem += n;
                currentItem %= items.Count;
            }
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

            //if (items.Count <= 0) currentItem = -1;

            if (currentItem >= 0)
                selectBoxRectangle = new Rectangle(Game1.tileSize + (currentItem * Game1.tileSize * 2), posY, Game1.tileSize * 2, Game1.tileSize);
        }

        public void DrawHud()
        {
            if (currentItem >= 0)
            {
                Game1.spriteBatch.Draw(selectBoxTexture, selectBoxRectangle, Color.Coral);
                Game1.spriteBatch.DrawString(Game1.font, items[currentItem].Item.Name(), selectedItemNamePosition, Color.Turquoise);
            }
            foreach (InventoryStack i in items)
            {
                i.Draw();
                Game1.spriteBatch.DrawString(Game1.font, i.Count.ToString(), i.GetPosition() + new Vector2(Game1.tileSize, 0), Color.White);
            }
        }
    }
}
