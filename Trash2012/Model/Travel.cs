using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trash2012.Model
{
    public class Travel
    {
        private List<IRoadTile> tiles;

        public enum Extremity{
            TOP = 1,
            BOTTOM = 2,
            LEFT = 3,
            RIGHT = 4
        }

        public Travel()
        {
            tiles = new List<IRoadTile>();
        }

        public bool Add(IMapTile tile)
        {
            if (tile is IRoadTile)
            {
                IRoadTile road = ((IRoadTile) tile);
                if (tiles.Count == 0)
                {
                    tiles.Add(road);
                    return true;
                }
                if (IsPositionCompatible(tiles.Last(), road))
                {
                    if (IsTypeCompatible(tiles.Last(), road))
                    {
                        tiles.Add(road);
                        return true;
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
            if (diffX == 0)
            {
                if (diffY != 0)
                {
                    return true;
                }
            }
            else
            {
                if (diffY == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Compares two IRoadTile to knwo if they are bound each other
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
                    a_commun_border_with_b = Extremity.TOP;
                }
                else
                {
                    a_commun_border_with_b = Extremity.BOTTOM;
                }
            }
            else
            {
                if (diffX == 1)
                {
                    a_commun_border_with_b = Extremity.LEFT;
                }
                else
                {
                    a_commun_border_with_b = Extremity.RIGHT;
                }
            }

            switch (a_commun_border_with_b)
            {
                case Extremity.TOP:
                    if (HasExtremity(a, Extremity.TOP) && HasExtremity(b, Extremity.BOTTOM))
                        return true;
                    break;
                case Extremity.BOTTOM:
                    if (HasExtremity(a, Extremity.BOTTOM) && HasExtremity(b, Extremity.TOP))
                        return true;
                    break;
                case Extremity.LEFT: //LEFT
                    if (HasExtremity(a, Extremity.LEFT) && HasExtremity(b, Extremity.RIGHT))
                        return true;
                    break;
                case Extremity.RIGHT: //RIGHT
                    if (HasExtremity(a, Extremity.RIGHT) && HasExtremity(b, Extremity.LEFT))
                        return true;
                    break;
            }
            return false;
        }

        public static bool HasExtremity(IRoadTile a, Extremity x)
        {
            switch (x)
            {
                case Extremity.TOP:
                    return
                        (a.Type == RoadTile.RoadType.Vertical
                        || a.Type == RoadTile.RoadType.TopLeft
                        || a.Type == RoadTile.RoadType.TopRight
                        || a.Type == RoadTile.RoadType.TopBottomLeft
                        || a.Type == RoadTile.RoadType.TopBottomRight
                        || a.Type == RoadTile.RoadType.TopLeftRight);

                case Extremity.BOTTOM:
                    return
                        (a.Type == RoadTile.RoadType.Vertical
                        || a.Type == RoadTile.RoadType.BottomLeft
                        || a.Type == RoadTile.RoadType.BottomRight
                        || a.Type == RoadTile.RoadType.TopBottomLeft
                        || a.Type == RoadTile.RoadType.TopBottomRight
                        || a.Type == RoadTile.RoadType.BottomLeftRight);
                    
                case Extremity.LEFT:
                    return
                        (a.Type == RoadTile.RoadType.Horizontal
                        || a.Type == RoadTile.RoadType.TopLeft
                        || a.Type == RoadTile.RoadType.BottomLeft
                        || a.Type == RoadTile.RoadType.TopBottomLeft
                        || a.Type == RoadTile.RoadType.TopLeftRight
                        || a.Type == RoadTile.RoadType.BottomLeftRight);

                case Extremity.RIGHT:
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
    }
}
