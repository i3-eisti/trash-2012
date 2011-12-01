
using System;
using System.Drawing;
using Trash2012.Model;

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
                case TruckAnimation.Left2Bottom:
                    return Properties.Resources.TruckLeftBottom;
                case TruckAnimation.Top2Bottom:
                    return Properties.Resources.TruckTopBottom;
                case TruckAnimation.Top2Right:
                    return Properties.Resources.TruckTopRight;
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
    }

}