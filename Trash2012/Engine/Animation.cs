
using System;
using System.Drawing;
using Trash2012.Model;
using Trash2012.Visual;

namespace Trash2012.Engine
{
    public enum TruckAnimation
    {
        Left2Right,
        Left2Bottom,
        Left2Top,
        Right2Left,
        Right2Top,
        Right2Bottom,
        Top2Left,
        Top2Bottom,
        Top2Right,
        Bottom2Left,
        Bottom2Top,
        Bottom2Right
    }

    public class Animations
    {

        public static Bitmap FindResource(TruckAnimation animation)
        {
            switch (animation)
            {
                case TruckAnimation.Left2Right:
                    return Properties.Resources.TruckLeftRight;
                case TruckAnimation.Left2Top:
                    return Properties.Resources.TruckLetfTop;
                case TruckAnimation.Left2Bottom:
                    return Properties.Resources.TruckLeftBottom;
                
                case TruckAnimation.Right2Left:
                    return Properties.Resources.TruckRightLeft;
                case TruckAnimation.Right2Top:
                    return Properties.Resources.TruckRightTop;
                case TruckAnimation.Right2Bottom:
                    return Properties.Resources.TruckRightBottom;

                case TruckAnimation.Top2Left:
                    return Properties.Resources.TruckTopLeft;
                case TruckAnimation.Top2Bottom:
                    return Properties.Resources.TruckTopBottom;
                case TruckAnimation.Top2Right:
                    return Properties.Resources.TruckTopRight;

                case TruckAnimation.Bottom2Left:
                    return Properties.Resources.TruckBottomLeft;
                case TruckAnimation.Bottom2Top:
                    return Properties.Resources.TruckBottomTop;
                case TruckAnimation.Bottom2Right:
                    return Properties.Resources.TruckBottomRight;

                default:
                    Console.Error.WriteLine("Unhandled animation: " + animation);
                    return Properties.Resources.Missing; //FIXME Not the good one !!!
            }
        }

        public static Bitmap FindResource(BackgroundTile.BackgroundType animation)
        {
            switch (animation)
            {
                case BackgroundTile.BackgroundType.Plain:
                    return Properties.Resources.TilePlain;
                case BackgroundTile.BackgroundType.BlueHouse:
                    return Properties.Resources.BlueHouseTop;
                case BackgroundTile.BackgroundType.RedHouse:
                    return Properties.Resources.RedHouseTop;
                case BackgroundTile.BackgroundType.BrownHouse:
                    return Properties.Resources.BrownHouse;

                case BackgroundTile.BackgroundType.ForestTopLeft:
                    return Properties.Resources.ForestTopLeft;
                case BackgroundTile.BackgroundType.ForestTopMiddle:
                    return Properties.Resources.ForestTopMiddle;
                case BackgroundTile.BackgroundType.ForestTopRight:
                    return Properties.Resources.ForestTopRight;

                case BackgroundTile.BackgroundType.ForestMiddleLeft:
                    return Properties.Resources.ForestMiddleLeft;
                case BackgroundTile.BackgroundType.ForestMiddle:
                    return Properties.Resources.ForestMiddle;
                case BackgroundTile.BackgroundType.ForestMiddleRight:
                    return Properties.Resources.ForestMiddleRight;

                case BackgroundTile.BackgroundType.ForestBottomLeft:
                    return Properties.Resources.ForestBottomLeft;
                case BackgroundTile.BackgroundType.ForestBottomMiddle:
                    return Properties.Resources.ForestBottomMiddle;
                case BackgroundTile.BackgroundType.ForestBottomRight:
                    return Properties.Resources.ForestBottomRight;

                case BackgroundTile.BackgroundType.ForestSolo:
                    return Properties.Resources.ForestSolo;
                case BackgroundTile.BackgroundType.DechetterieTopLeft:
                    return Properties.Resources.DechetterieTopLeft;
                case BackgroundTile.BackgroundType.DechetterieTopRight:
                    return Properties.Resources.DechetterieTopRight;

                default:
                    Console.Error.WriteLine("Unhandled animation: " + animation);
                    return Properties.Resources.Missing; //FIXME Not the good one !!!
            }
        }

        public static Bitmap FindResource(RoadTile.RoadType type)
        {
            switch (type)
            {
                case RoadTile.RoadType.Horizontal:
                    return Properties.Resources.TileRoadHorizontal;
                case RoadTile.RoadType.Vertical:
                    return Properties.Resources.TileRoadVertical;
                case RoadTile.RoadType.TopLeft:
                    return Properties.Resources.TileRoadTopLeft;
                case RoadTile.RoadType.TopRight:
                    return Properties.Resources.TileRoadTopRight;
                case RoadTile.RoadType.BottomLeft:
                    return Properties.Resources.TileRoadBottomLeft;
                case RoadTile.RoadType.BottomRight:
                    return Properties.Resources.TileRoadBottomRight;
                case RoadTile.RoadType.TopBottomLeft:
                    return Properties.Resources.TileRoadTopBottomLeft;
                case RoadTile.RoadType.TopBottomRight:
                    return Properties.Resources.TileRoadTopBottomRight;
                case RoadTile.RoadType.TopLeftRight:
                    return Properties.Resources.TileRoadTopLeftRight;
                case RoadTile.RoadType.BottomLeftRight:
                    return Properties.Resources.TileRoadBottomLeftRight;
                default:
                    throw new ArgumentException("Unhandled animation bitmap tile: " + type);
            }
        }

        public static Bitmap FindResource(HouseTile.THouse type)
        {
            switch (type)
            {
                case HouseTile.THouse.Normal:
                    return Properties.Resources.NormalHouse;
                case HouseTile.THouse.Blue:
                    return Properties.Resources.BlueHouseBot;
                case HouseTile.THouse.Red:
                    return Properties.Resources.RedHouseBot;
                case HouseTile.THouse.TrashFirm:
                    return Properties.Resources.DechetterieBottomLeft;
                default:
                    Console.Error.WriteLine("Unhandled animation: " + type);
                    return Properties.Resources.Missing; //FIXME Not the good one !!!
            }
        }

        public static TruckAnimation FindNext(
            Travel.Extremity from,
            Travel.Extremity to) 
        {
            switch (from)
            {
                case Travel.Extremity.Top:
                    switch (to)
                    {
                        case Travel.Extremity.Bottom:
                            return TruckAnimation.Top2Bottom;
                        case Travel.Extremity.Left:
                            return TruckAnimation.Top2Left;
                        case Travel.Extremity.Right:
                            return TruckAnimation.Top2Right;
                        default:
                            throw new ArgumentException("Can't move from top to top");
                    }
                case Travel.Extremity.Bottom:
                    switch (to)
                    {
                        case Travel.Extremity.Top:
                            return TruckAnimation.Bottom2Top;
                        case Travel.Extremity.Left:
                            return TruckAnimation.Bottom2Left;
                        case Travel.Extremity.Right:
                            return TruckAnimation.Bottom2Right;
                        default:
                            throw new ArgumentException("Can't move from bottom to bottom");
                    }
                case Travel.Extremity.Left:
                    switch (to)
                    {
                        case Travel.Extremity.Top:
                            return TruckAnimation.Left2Top;
                        case Travel.Extremity.Right:
                            return TruckAnimation.Left2Right;
                        case Travel.Extremity.Bottom:
                            return TruckAnimation.Left2Bottom;
                        default:
                            throw new ArgumentException("Can't move from left to left");
                    }
                case Travel.Extremity.Right: 
                    switch (to)
                    {
                        case Travel.Extremity.Top:
                            return TruckAnimation.Right2Top;
                        case Travel.Extremity.Left:
                            return TruckAnimation.Right2Left;
                        case Travel.Extremity.Bottom:
                            return TruckAnimation.Right2Bottom;
                        default:
                            throw new ArgumentException("Can't move from right to right");
                    }
                default:
                    throw new ArgumentException("Impossible case: " + from);
            } 
        }

        public static TruckAnimation FindNextFrom(
            IMapTile ModelTile,
            Travel.Extremity from)
        {
            if (ModelTile is IRoadTile)
            {
                IRoadTile Road = (IRoadTile)ModelTile;
                switch (Road.Type)
                {
                    case RoadTile.RoadType.BottomLeft:
                        if (from == Travel.Extremity.Left)
                        {
                            return TruckAnimation.Left2Bottom;
                        }
                        else
                        {
                            return TruckAnimation.Bottom2Left;
                        }
                        break;

                    case RoadTile.RoadType.BottomRight:
                        if (from == Travel.Extremity.Right)
                        {
                            return TruckAnimation.Right2Bottom;
                        }
                        else
                        {
                            return TruckAnimation.Bottom2Right;
                        }
                        break;

                    case RoadTile.RoadType.Horizontal:
                        if (from == Travel.Extremity.Right)
                        {
                            return TruckAnimation.Right2Left;
                        }
                        else
                        {
                            return TruckAnimation.Left2Right;
                        }
                        break;

                    case RoadTile.RoadType.TopLeft:
                        if (from == Travel.Extremity.Top)
                        {
                            return TruckAnimation.Top2Left;
                        }
                        else
                        {
                            return TruckAnimation.Left2Top;
                        }
                        break;

                    case RoadTile.RoadType.TopRight:
                        if (from == Travel.Extremity.Top)
                        {
                            return TruckAnimation.Top2Right;
                        }
                        else
                        {
                            return TruckAnimation.Right2Top;
                        }
                        break;

                    case RoadTile.RoadType.Vertical:
                        if (from == Travel.Extremity.Top)
                        {
                            return TruckAnimation.Top2Bottom;
                        }
                        else
                        {
                            return TruckAnimation.Bottom2Top;
                        }
                        break;

                    default:
                        throw new ArgumentException("Can't match tile and direction");
                }
            }
            else
            {
                throw new ArgumentException("Can't animate the truck somewhere else than on a road");
            }
        }

        public static TruckAnimation FindNextTo(
            IMapTile ModelTile,
            Travel.Extremity to)
        {
            if (ModelTile is IRoadTile)
            {
                IRoadTile Road = (IRoadTile)ModelTile;
                switch (Road.Type)
                {
                    case RoadTile.RoadType.BottomLeft:
                        if (to == Travel.Extremity.Left)
                        {
                            return TruckAnimation.Bottom2Left;
                        }
                        else
                        {
                            return TruckAnimation.Left2Bottom;
                        }
                        break;

                    case RoadTile.RoadType.BottomRight:
                        if (to == Travel.Extremity.Right)
                        {
                            return TruckAnimation.Bottom2Right;
                        }
                        else
                        {
                            return TruckAnimation.Right2Bottom;
                        }
                        break;

                    case RoadTile.RoadType.Horizontal:
                        if (to == Travel.Extremity.Right)
                        {
                            return TruckAnimation.Left2Right;
                        }
                        else
                        {
                            return TruckAnimation.Right2Left;
                        }
                        break;

                    case RoadTile.RoadType.TopLeft:
                        if (to == Travel.Extremity.Top)
                        {
                            return TruckAnimation.Left2Top;
                        }
                        else
                        {
                            return TruckAnimation.Top2Left;
                        }
                        break;

                    case RoadTile.RoadType.TopRight:
                        if (to == Travel.Extremity.Top)
                        {
                            return TruckAnimation.Right2Top;
                        }
                        else
                        {
                            return TruckAnimation.Top2Right;
                        }
                        break;

                    case RoadTile.RoadType.Vertical:
                        if (to == Travel.Extremity.Top)
                        {
                            return TruckAnimation.Bottom2Top;
                        }
                        else
                        {
                            return TruckAnimation.Top2Bottom;
                        }
                        break;

                    default:
                        throw new ArgumentException("Can't match tile and direction");
                }
            }
            else
            {
                throw new ArgumentException("Can't animate the truck somewhere else than on a road");
            }
        }
    }

}