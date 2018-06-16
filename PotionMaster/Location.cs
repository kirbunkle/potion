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

        public Location()
        {
            tileMap = Game1.content.Load<TiledMap>("tiledMaps/dumb_grass");
            binch = new Character();
        }

        public Interactable GetCollidingObject(Rectangle box) // TODO: make generic for interactible types
        {
            if (binch.GetCollisionBox().Intersects(box))
            {
                return binch;
            }
            return null;
        }

        public bool IsCollidingWithAnotherObject(Rectangle box)
        {
            return binch.GetCollisionBox().Intersects(box);
        }

        public bool IsCollidingAtPoint(int x, int y)
        {
            int sampleX = x / Game1.tileSize;
            int sampleY = y / Game1.tileSize;
            return (x < 0)
                || (y < 0)
                || (x > (tileMap.Width * Game1.tileSize))
                || (y > (tileMap.Height * Game1.tileSize))
                || (!tileMap.TileLayers[0].TryGetTile(sampleX, sampleY, out TiledMapTile? tile))
                || (!tile.HasValue)
                || (tile.Value.GlobalIdentifier == 0);
        }

        public bool IsColliding(Rectangle box)
        {
            return IsCollidingAtPoint(box.X, box.Y)
                || IsCollidingAtPoint(box.X + box.Width, box.Y)
                || IsCollidingAtPoint(box.X, box.Y + box.Height)
                || IsCollidingAtPoint(box.X + box.Width, box.Y + box.Height)
                || IsCollidingWithAnotherObject(box);
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
            binch.Update();
        }

        public void Draw()
        {
            Game1.mapRenderer.Draw(tileMap, Game1.camera.GetViewMatrix());
            binch.Draw();
        }

        public void DrawHud()
        {
            //Game1.spriteBatch.DrawString(Game1.font, beefsauce, new Vector2(10, 10), Color.AntiqueWhite);
        }
    }
}
