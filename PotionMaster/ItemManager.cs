using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class ItemManager
    {
        private Dictionary<int, Item> items;
        private Dictionary<int, List<string>> itemData;

        private Item CreateItem(int i)
        {
            if (itemData.TryGetValue(i, out List<string> data))
            {
                if (data[0] == "Type=Seed")
                {
                    return new Seed(data);
                }
                else if (data[0] == "Type=Potion")
                {
                    return new Potion(data);
                }
            }
            return null;
        }

        public ItemManager()
        {
            items = new Dictionary<int, Item>();
            itemData = Game1.content.Load<Dictionary<int, List<string>>>("items/itemData");
        }

        public Item GetItem(int i)
        {
            if (!items.TryGetValue(i, out Item item))
            {
                item = CreateItem(i);
                if (item != null) items.Add(i, item);
            }
            return item;
        }
    }
}
