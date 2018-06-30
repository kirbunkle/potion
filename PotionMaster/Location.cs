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
        private Character binch;
        private Enemy spoder;
        private LocationObject bag;
        private List<Plant> plants;
        private List<Projectile> projectiles;
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

        public Location()
        {
            tileMap = Game1.content.Load<TiledMap>("tiledMaps/dumb_grass");
            binch = new Character();
            bag = new LocationObject(Game1.itemManager.GetItem(1));
            spoder = new Enemy();
            plants = new List<Plant>();
            projectiles = new List<Projectile>();
        }

        public Interactable GetCollidingObject(Rectangle box) 
        {
            if (binch.GetCollisionBox().Intersects(box))
            {
                return binch;
            }
            else if (bag.GetCollisionBox().Intersects(box))
            {
                return bag;
            }
            else if ((spoder.Active) && (spoder.GetCollisionBox().Intersects(box)))
            {
                return spoder;
            }
            else
            {
                return GetPlantAt(box);
            }
        }

        public bool IsCollidingWithAnotherObject(Rectangle box)
        {
            return binch.GetCollisionBox().Intersects(box) || bag.GetCollisionBox().Intersects(box);
        }

        public int GetTileTypeID(int x, int y)
        {
            int sampleX = x / Game1.tileSize;
            int sampleY = y / Game1.tileSize;
            if ((tileMap.TileLayers[0].TryGetTile(sampleX, sampleY, out TiledMapTile? tile)) && tile.HasValue)
            {
                return tile.Value.GlobalIdentifier;
            }
            return -1;
        }

        public bool IsCollidingAtPoint(int x, int y)
        {
            return (x <= 0)
                || (y <= 0)
                || (x >= tileMap.WidthInPixels)
                || (y >= tileMap.HeightInPixels)
                || (GetTileTypeID(x, y) <= 0);
        }

        public bool IsColliding(Rectangle box)
        {
            return IsCollidingAtPoint(box.X, box.Y)
                || IsCollidingAtPoint(box.X + box.Width, box.Y)
                || IsCollidingAtPoint(box.X, box.Y + box.Height)
                || IsCollidingAtPoint(box.X + box.Width, box.Y + box.Height)
                || IsCollidingWithAnotherObject(box);
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

        public void Update()
        {
            Game1.mapRenderer.Update(tileMap, Game1.gt);
            binch.Update();
            bag.Update();
            if (spoder.Active) spoder.Update();
            foreach (Plant p in plants)
            {
                p.Update();
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
            binch.Draw();
            bag.Draw();
            if (spoder.Active) spoder.Draw();
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
