using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;


namespace PotionMaster
{
    public class Location
    {
        private TiledMap tileMap;
        //private Character binch;
        //private Character testNPC;
        //private Enemy spoder;
        //private LocationObject bag;
        private List<Interactable> interactables;
        private List<Character> characters;
        private List<Plant> plants;
        private List<Projectile> projectiles;
        public string Name { get; set; }
        private Dictionary<string, Warp> warps;
        //private string beefsauce;

        private static readonly int[] plantableTiles = { 2, 3 };

        // TODO probably need to make these (GetPlantAt) use a 2d map so we can locate by reference instead of doing a lengthy search

        private Plant GetPlantAt(Rectangle box)
        {
            foreach (Plant p in plants)
            {
                if (p.GetCollisionBox().Intersects(box))
                {
                    return p;
                }
            }
            return null;
        }

        private Plant GetPlantAt(int x, int y)
        {
            foreach (Plant p in plants)
            {
                if (p.GetCollisionBox().Contains(x, y))
                {
                    return p;
                }
            }
            return null;
        }

        public Location(List<string> data)
        {
            Name = data[0];
            tileMap = Game1.content.Load<TiledMap>("tiledMaps/"+data[0]);
            //binch = Game1.dataManager.CreateCharacter(2, Game1.tileSize * 15, Game1.tileSize * 8);
            //testNPC = Game1.dataManager.CreateCharacter(4, Game1.tileSize * 14, Game1.tileSize * 4);
            //bag = new LocationObject(Game1.dataManager.GetItem(1));
            //spoder = (Enemy)Game1.dataManager.CreateCharacter(3, Game1.tileSize * 20, Game1.tileSize * 13);
            plants = new List<Plant>();
            projectiles = new List<Projectile>();
            interactables = new List<Interactable>();
            characters = new List<Character>();
            warps = new Dictionary<string, Warp>();
            foreach (TiledMapObject obj in tileMap.ObjectLayers[0].Objects)
            {
                if (obj.Type == "warp")
                {
                    Warp warp = new Warp(obj, this);
                    warps.Add(warp.Name, warp);
                }
                if (obj.Type == "npc")
                {
                    Character character = Game1.dataManager.CreateCharacter(int.Parse(obj.Name), 30 * Game1.tileSize, 10 * Game1.tileSize, this);
                    characters.Add(character);
                }
            }
        }

        public int TileMapW()
        {
            return tileMap.Width;
        }

        public int TileMapH()
        {
            return tileMap.Height;
        }

        public Interactable GetCollidingObject(Rectangle box, Collidable obj = null) 
        {
            foreach (KeyValuePair<string, Warp> w in warps)
            {
                if ((w.Value.GetCollisionBox().Intersects(box)) && (obj != w.Value)) return w.Value;
            }
            foreach (Interactable i in interactables)
            {
                if ((i.GetCollisionBox().Intersects(box)) && (obj != i)) return i;
            }
            foreach (Character c in characters)
            {
                if ((c.GetCollisionBox().Intersects(box)) && (obj != c)) return c;
            }
            return null;
            // if (binch.GetCollisionBox().Intersects(box))
            // {
            //     return binch;
            // }
            // else if (testNPC.GetCollisionBox().Intersects(box))
            // {
            //     return testNPC;
            // }
            // else if (bag.GetCollisionBox().Intersects(box))
            // {
            //     return bag;
            // }
            // else if ((spoder.Active) && (spoder.GetCollisionBox().Intersects(box)))
            // {
            //     return spoder;
            // }
            // else
            // {
           
            //    return GetPlantAt(box);
          //  }
        }

        public bool IsCollidingWithAnotherObject(Rectangle box, Collidable obj = null)
        {
            foreach (Interactable i in interactables)
            {
                if ((i.GetCollisionBox().Intersects(box)) && (obj != i)) return true;
            }
            foreach (Character c in characters)
            {
                if ((c.GetCollisionBox().Intersects(box)) && (obj != c)) return true;
            }
            return false;
        }

        public int GetTileTypeIDByTileIndex(int x, int y)
        {
            if ((tileMap.TileLayers[0].TryGetTile(x, y, out TiledMapTile? tile)) && tile.HasValue)
            {
                return tile.Value.GlobalIdentifier;
            }
            return -1;
        }

        public int GetTileTypeID(int x, int y)
        {
            int sampleX = x / Game1.tileSize;
            int sampleY = y / Game1.tileSize;
            return GetTileTypeIDByTileIndex(sampleX, sampleY);
        }

        public bool IsCollidingAtPoint(int x, int y)
        {
            return (x <= 0)
                || (y <= 0)
                || (x >= tileMap.WidthInPixels)
                || (y >= tileMap.HeightInPixels)
                || (GetTileTypeID(x, y) <= 0);
        }

        public bool IsColliding(Rectangle box, Collidable obj = null)
        {
            return IsCollidingAtPoint(box.X, box.Y)
                || IsCollidingAtPoint(box.X + box.Width, box.Y)
                || IsCollidingAtPoint(box.X, box.Y + box.Height)
                || IsCollidingAtPoint(box.X + box.Width, box.Y + box.Height)
                || IsCollidingWithAnotherObject(box, obj);
        }

        public bool IsCollidingWithImpassibleTile(Rectangle box)
        {
            return IsCollidingAtPoint(box.X, box.Y)
                || IsCollidingAtPoint(box.X + box.Width, box.Y)
                || IsCollidingAtPoint(box.X, box.Y + box.Height)
                || IsCollidingAtPoint(box.X + box.Width, box.Y + box.Height);
        }

        public int WidthInPixels()
        {
            return tileMap.WidthInPixels;
        }

        public int HeightInPixels()
        {
            return tileMap.HeightInPixels;
        }

        public bool PlantExistsAtPosition(int x, int y)
        {
            return GetPlantAt(x, y) != null;
        }

        public bool IsValidPlantableLocation(Rectangle loc)
        {
            Point p = loc.Center;
            return (plantableTiles.Contains(GetTileTypeID(p.X, p.Y)) && !PlantExistsAtPosition(p.X, p.Y));
        }

        public void Plant(Rectangle loc, Seed seed)
        {
            plants.Add(new Plant(loc, seed, this));
        }

        public void Harvest(Plant plant)
        {
            plants.Remove(plant);
        }

        public void AddProjectile(Projectile p)
        {
            projectiles.Add(p);
        }

        public void RemoveProjectile(Projectile p)
        {
            projectiles.Remove(p);
        }

        public Vector2 GetCenter()
        {
            return new Vector2(tileMap.WidthInPixels / 2, tileMap.HeightInPixels / 2);
        }

        public Vector2 GetWarpPos(string name)
        {
            if (warps.TryGetValue(name, out Warp w))
            {
                return w.GetPosition();
            }
            else
            {
                return new Vector2(0, 0);
            }
        }

        public void Update()
        {
            Game1.mapRenderer.Update(tileMap, Game1.gt);
            foreach (Interactable i in interactables)
            {
                i.Update();
            }
            foreach (Plant p in plants)
            {
                p.Update();
            }
            foreach (Character c in characters)
            {
                c.Update();
            }
            for (int i = projectiles.Count-1; i >= 0; i--)
            {
                Projectile p = projectiles[i];
                if (p.Active)
                    p.Update();
                else
                    projectiles.Remove(p);
            }
        }

        public void Draw()
        {
            Game1.mapRenderer.Draw(tileMap, Game1.camera.GetViewMatrix());
            foreach (Interactable i in interactables)
            {
                i.Draw();
            }
            foreach (Character c in characters)
            {
                c.Draw();
            }
            foreach (Plant p in plants)
            {
                p.Draw();
            }
            foreach (Projectile p in projectiles)
            {
                p.Draw();
            }
        }

        public void DrawHud()
        {
            //Game1.spriteBatch.DrawString(Game1.font, beefsauce, new Vector2(500, 10), Color.AntiqueWhite);
        }
    }
}
