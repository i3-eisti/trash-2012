using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Trash2012.Model
{
    public class Travel : IEnumerable<IRoadTile>
    {
        private List<IRoadTile> tiles;

        public int Count
        {
            get
            {
                return tiles.Count;
            }
        }

        public enum Extremity
        {
            Top,
            Bottom,
            Left,
            Right
        }

        public Travel()
        {
            tiles = new List<IRoadTile>();
        }

        public bool Add(IMapTile tile)
        {
            if (tile is IRoadTile)
            {
                IRoadTile road = ((IRoadTile)tile);
                if (tiles.Count == 0)
                {
                    tiles.Add(road);
                    return true;
                }
                if (IsPositionCompatible(tiles.Last(), road))
                {
                    if (IsTypeCompatible(tiles.Last(), road))
                    {
                        if (!this.Contains(road))
                        {
                            tiles.Add(road);
                            Console.WriteLine("Ajout OK : Count = " + tiles.Count);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Compares the position of IRoadTile to know if they are stick to each other
        /// </summary>
        /// <param name="a">IRoadTile</param>
        /// <param name="b">IRoadTile</param>
        /// <returns>true is IRoadTiles are stick to each other, else false</returns>
        public static bool IsPositionCompatible(IRoadTile a, IRoadTile b)
        {
            int diffX = Math.Abs(a.Position.X - b.Position.X);
            int diffY = Math.Abs(a.Position.Y - b.Position.Y);
            if (diffX < -1 || diffX > 1 || diffY < -1 || diffY > 1)
            {
                return false;
            }

            // L : J'ai changé les tests qui m'avaient l'air bizares.
            if (diffX == 0)
            {
                if (diffY == -1 || diffY == 1)
                {
                    return true;
                }

            }
            else if (diffY == 0)
            {
                if (diffX == -1 || diffX == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Compares two IRoadTile to know if they are bound to each other
        /// </summary>
        /// <param name="a">IRoadTile</param>
        /// <param name="b">IRoadTile</param>
        /// <returns>true is IRoadTiles are bound to each other, else false</returns>
        public static bool IsTypeCompatible(IRoadTile a, IRoadTile b)
        {
            Extremity a_commun_border_with_b;
            int diffX = a.Position.X - b.Position.X;
            int diffY = a.Position.Y - b.Position.Y;
            if (diffX == 0)
            {
                if (diffY == 1)
                {
                    a_commun_border_with_b = Extremity.Top;
                }
                else
                {
                    a_commun_border_with_b = Extremity.Bottom;
                }
            }
            else
            {
                if (diffX == 1)
                {
                    a_commun_border_with_b = Extremity.Left;
                }
                else
                {
                    a_commun_border_with_b = Extremity.Right;
                }
            }

            switch (a_commun_border_with_b)
            {
                case Extremity.Top:
                    if (HasExtremity(a, Extremity.Top) && HasExtremity(b, Extremity.Bottom))
                        return true;
                    break;
                case Extremity.Bottom:
                    if (HasExtremity(a, Extremity.Bottom) && HasExtremity(b, Extremity.Top))
                        return true;
                    break;
                case Extremity.Left: //LEFT
                    if (HasExtremity(a, Extremity.Left) && HasExtremity(b, Extremity.Right))
                        return true;
                    break;
                case Extremity.Right: //RIGHT
                    if (HasExtremity(a, Extremity.Right) && HasExtremity(b, Extremity.Left))
                        return true;
                    break;
            }
            return false;
        }

        public static bool HasExtremity(IRoadTile a, Extremity x)
        {
            switch (x)
            {
                case Extremity.Top:
                    return
                        (a.Type == RoadTile.RoadType.Vertical
                        || a.Type == RoadTile.RoadType.TopLeft
                        || a.Type == RoadTile.RoadType.TopRight
                        || a.Type == RoadTile.RoadType.TopBottomLeft
                        || a.Type == RoadTile.RoadType.TopBottomRight
                        || a.Type == RoadTile.RoadType.TopLeftRight);

                case Extremity.Bottom:
                    return
                        (a.Type == RoadTile.RoadType.Vertical
                        || a.Type == RoadTile.RoadType.BottomLeft
                        || a.Type == RoadTile.RoadType.BottomRight
                        || a.Type == RoadTile.RoadType.TopBottomLeft
                        || a.Type == RoadTile.RoadType.TopBottomRight
                        || a.Type == RoadTile.RoadType.BottomLeftRight);

                case Extremity.Left:
                    return
                        (a.Type == RoadTile.RoadType.Horizontal
                        || a.Type == RoadTile.RoadType.TopLeft
                        || a.Type == RoadTile.RoadType.BottomLeft
                        || a.Type == RoadTile.RoadType.TopBottomLeft
                        || a.Type == RoadTile.RoadType.TopLeftRight
                        || a.Type == RoadTile.RoadType.BottomLeftRight);

                case Extremity.Right:
                    return
                        (a.Type == RoadTile.RoadType.Horizontal
                        || a.Type == RoadTile.RoadType.TopRight
                        || a.Type == RoadTile.RoadType.BottomRight
                        || a.Type == RoadTile.RoadType.TopBottomRight
                        || a.Type == RoadTile.RoadType.TopLeftRight
                        || a.Type == RoadTile.RoadType.BottomLeftRight);
            }
            return false;

        }

        public bool Contains(IMapTile tile)
        {
            if (tile is IRoadTile)
            {
                IRoadTile road = ((IRoadTile)tile);
                foreach (IRoadTile item in tiles)
                {
                    if (item.Position.X == tile.Position.X && item.Position.Y == tile.Position.Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int ContainsAndIndex(IMapTile tile)
        {
            Console.WriteLine("Position : " + tile.Position.X + " - " + tile.Position.Y);
            if (tile is IRoadTile)
            {
                IRoadTile road = ((IRoadTile)tile);
                for (int i = 0; i < tiles.Count; i++)
                {
                    IRoadTile item = tiles[i];
                    if (item.Position.X == tile.Position.X && item.Position.Y == tile.Position.Y)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public bool Remove(IMapTile tile)
        {
            if (tile is IRoadTile)
            {
                IRoadTile road = ((IRoadTile)tile);
                int i = this.ContainsAndIndex(road);
                if (i == 0)
                {
                    tiles.RemoveAt(0);
                    return true;
                }
                if (i == tiles.Count - 1)
                {
                    tiles.RemoveAt(tiles.Count - 1);
                    return true;
                }
            }
            return false;
        }

        public IMapTile Get(int i)
        {
            return tiles[i];
        }

        public IRoadTile this[int idx]
        {
            get { return tiles[idx]; }
        }

        public IEnumerator<IRoadTile> GetEnumerator()
        {
            return tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
