using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class DataManager
    {
        private Dictionary<int, Item> items;
        private Dictionary<int, List<string>> itemData;
        private Dictionary<int, List<string>> characterData;
        private Dictionary<int, List<string>> locationData;

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

        public DataManager()
        {
            items = new Dictionary<int, Item>();
            itemData = Game1.content.Load<Dictionary<int, List<string>>>("items/itemData");
            characterData = Game1.content.Load<Dictionary<int, List<string>>>("characters/characterData");
            locationData = Game1.content.Load<Dictionary<int, List<string>>>("tiledMaps/locationData");
        }

        public Item GetItem(int id)
        {
            if (!items.TryGetValue(id, out Item item))
            {
                item = CreateItem(id);
                if (item != null) items.Add(id, item);
            }
            return item;
        }

        public Character CreateCharacter(int id, int x, int y, Location loc)
        {
            if (characterData.TryGetValue(id, out List<string> data))
            {
                if (data[0] == "Type=PlayerCharacter")
                {
                    return new PlayerCharacter(data, x, y, loc);
                }
                else if (data[0] == "Type=Character")
                {
                    return new Character(data, x, y, loc);
                }
                else if (data[0] == "Type=Enemy")
                {
                    return new Enemy(data, x, y, loc);
                }
            }
            return null;
        }

        public Location CreateLocation(int id)
        {
            if (locationData.TryGetValue(id, out List<string> data))
            {
                return new Location(data);
            }
            return null;
        }
    }
}
