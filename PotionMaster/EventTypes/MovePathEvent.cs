using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Priority_Queue;

namespace PotionMaster
{
    public enum TileDirections { up, down, left, right };

    public class TilePos : FastPriorityQueueNode
    {
        public int X { set; get; }
        public int Y { set; get; }
        public int Score { set; get; }
        public TilePos Parent { set; get; }
        public TilePos(TilePos parent, int xIn, int yIn, int startX, int startY)
        {
            X = xIn;
            Y = yIn;
            Parent = parent;
            Score = Math.Abs(xIn - startX) + Math.Abs(yIn - startY);
        }
        public Boolean Equals(TilePos t)
        {
            return ((t.X == X) && (t.Y == Y));
        }
    }

    public class MovePathEvent : Event
    {
        private Location location;
        private Character character;
        private int startX;
        private int startY;
        private int destX;
        private int destY;
        private int tileMapW;
        private int tileMapH;
        private TilePos curTile;
        private Boolean[,] checkedTiles;

        public MovePathEvent(Location loc, Character c, int startXTile, int startYTile, int destXTile, int destYTile)
        {
            location = loc;
            character = c;
            startX = startXTile;
            startY = startYTile;
            destX = destXTile;
            destY = destYTile;
            tileMapH = location.TileMapH();
            tileMapW = location.TileMapW();
            curTile = null;
            checkedTiles = new Boolean[tileMapW, tileMapH];
            for (int i = 0; i < tileMapW; i++)
            {
                for (int j = 0; j < tileMapH; j++)
                {
                    checkedTiles[i, j] = false;
                }
            }
            checkedTiles[destX, destY] = true;
            GamePaused = false;

            FindPath();
        }

        private void QueueUp(TilePos aTile, FastPriorityQueue<TilePos> priorityQueue)
        {
            if ((aTile.X < 0)
            || (aTile.X >= tileMapW)
            || (aTile.Y < 0)
            || (aTile.Y >= tileMapH)
            || (checkedTiles[aTile.X, aTile.Y])
            || (location.GetTileTypeIDByTileIndex(aTile.X, aTile.Y) <= 0)) return;

            checkedTiles[aTile.X, aTile.Y] = true;
            priorityQueue.Enqueue(aTile, aTile.Score);
        }

        private void FindPath()
        {
            if (((startX == destX) && (startY == destY))
            || (startX < 0)
            || (startX >= tileMapW)
            || (startY < 0)
            || (startY >= tileMapH)
            || (destX < 0)
            || (destX >= tileMapW)
            || (destY < 0)
            || (destY >= tileMapH)
            || (location.GetTileTypeIDByTileIndex(destX, destY) <= 0)) return;

            curTile = new TilePos(null, destX, destY, startX, startY);

            FastPriorityQueue<TilePos> priorityQueue = new FastPriorityQueue<TilePos>(tileMapW * tileMapH);
            while (!((curTile.X == startX) && (curTile.Y == startY)))
            {
                // check the four sides
                TilePos leftTile = new TilePos(curTile, curTile.X - 1, curTile.Y, startX, startY);
                TilePos rightTile = new TilePos(curTile, curTile.X + 1, curTile.Y, startX, startY);
                TilePos topTile = new TilePos(curTile, curTile.X, curTile.Y - 1, startX, startY);
                TilePos bottomTile = new TilePos(curTile, curTile.X, curTile.Y + 1, startX, startY);

                if (curTile.Parent != null)
                {
                    if ((curTile.Parent.Equals(leftTile)) || (curTile.Parent.Equals(rightTile)))
                    {
                        topTile.Score += 1;
                        bottomTile.Score += 1;
                    }
                    else if ((curTile.Parent.Equals(bottomTile)) || (curTile.Parent.Equals(topTile)))
                    {
                        rightTile.Score += 1;
                        leftTile.Score += 1;
                    }
                }

                QueueUp(leftTile, priorityQueue);
                QueueUp(rightTile, priorityQueue);
                QueueUp(bottomTile, priorityQueue);
                QueueUp(topTile, priorityQueue);

                curTile = priorityQueue.Dequeue();
            }
        }   

        public override void Update()
        {
            if (curTile == null)
            {
                Complete = true;
            }
            else
            {
                Vector2 v = character.WalkToward(curTile.X * Game1.tileSize, curTile.Y * Game1.tileSize);
                while ((v.X == 0) && (v.Y == 0))
                {
                    if (curTile.Parent == null)
                    {
                        v = character.WalkToward(curTile.X * Game1.tileSize, curTile.Y * Game1.tileSize, true);
                        curTile = curTile.Parent;
                        Complete = true;
                        break;
                    }
                    else
                    {
                        curTile = curTile.Parent;
                        v = character.WalkToward(curTile.X * Game1.tileSize, curTile.Y * Game1.tileSize);
                    }
                }
            }
        }

        public override void Draw()
        {

        }

        public override void DrawHud()
        {

        }
    }
}
