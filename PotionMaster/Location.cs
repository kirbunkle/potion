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
        private string beefsauce;

        public Location()
        {
            tileMap = Game1.content.Load<TiledMap>("tiledMaps/dumb_grass");
            beefsauce = "bbb";
        }

        public bool IsCollidingAtPoint(int x, int y)
        {
            if ((x < 0) || (y < 0) || (x > (tileMap.Width * Game1.tileSize)) || (y > (tileMap.Height * Game1.tileSize)))
                return true;

            int sampleX = x / Game1.tileSize;
            int sampleY = y / Game1.tileSize;
            if ((tileMap.TileLayers[0].TryGetTile(sampleX, sampleY, out TiledMapTile? tile)) && (tile.HasValue))
            {
                beefsauce = tile.Value.GlobalIdentifier.ToString() + " " + x.ToString() + " " + y.ToString();
                if (tile.Value.GlobalIdentifier == 0)
                {
                    return true;
                }
                return false;
            }
            beefsauce = "None " + x.ToString() + " " + y.ToString();
            return true;
        }

        public bool IsColliding(Rectangle box)
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

        public void Update()
        {
            Game1.mapRenderer.Update(tileMap, Game1.gt);
        }

        public void Draw()
        {
            Game1.mapRenderer.Draw(tileMap, Game1.camera.GetViewMatrix());
        }

        public void DrawHud()
        {
            Game1.spriteBatch.DrawString(Game1.font, beefsauce, new Vector2(10, 10), Color.AntiqueWhite);
        }
    }
}
