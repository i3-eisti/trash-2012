
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
        Bottom2Right,
        Missing
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

                case BackgroundTile.BackgroundType.MairieTopLeft:
                    return Properties.Resources.mairie_top_left;
                case BackgroundTile.BackgroundType.MairieTopMid:
                    return Properties.Resources.mairie_top_mid;
                case BackgroundTile.BackgroundType.MairieTopRight:
                    return Properties.Resources.mairie_top_right;
                case BackgroundTile.BackgroundType.MairieMidLeft:
                    return Properties.Resources.mairie_mid_left;
                case BackgroundTile.BackgroundType.MairieMid:
                    return Properties.Resources.mairie_mid;
                case BackgroundTile.BackgroundType.MairieMidRight:
                    return Properties.Resources.mairie_mid_right;

                case BackgroundTile.BackgroundType.HouseFlowerTop:
                    return Properties.Resources.House4_top;

                case BackgroundTile.BackgroundType.Heolienne:
                    return Properties.Resources.Heolienne;

                case BackgroundTile.BackgroundType.HousePink:
                    return Properties.Resources.House5_top;
                case BackgroundTile.BackgroundType.HouseGreen:
                    return Properties.Resources.House6_top;
                case BackgroundTile.BackgroundType.HouseWater:
                    return Properties.Resources.House7_top;

                case BackgroundTile.BackgroundType.LabTopLeft:
                    return Properties.Resources.House8_topleft;
                case BackgroundTile.BackgroundType.LabTopRight:
                    return Properties.Resources.House8_topright;

                case BackgroundTile.BackgroundType.ChurchTopLeft:
                    return Properties.Resources.Church_topleft;
                case BackgroundTile.BackgroundType.ChurchTopRight:
                    return Properties.Resources.Church_topright;
                case BackgroundTile.BackgroundType.ChurchMidLeft:
                    return Properties.Resources.Church_midleft;
                case BackgroundTile.BackgroundType.ChurchMidRight:
                    return Properties.Resources.Church_midright;

                case BackgroundTile.BackgroundType.HouseYellowTopLeft:
                    return Properties.Resources.House9_topleft;
                case BackgroundTile.BackgroundType.HouseYellowTopRight:
                    return Properties.Resources.House9_topright;


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
                case RoadTile.RoadType.TopBottomLeftRight:
                    return Properties.Resources.TileRoadTopBottomLeftRight;

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
                case HouseTile.THouse.MairieLeft:
                    return Properties.Resources.mairie_bot_left;
                case HouseTile.THouse.MairieMid:
                    return Properties.Resources.mairie_bot_mid;
                case HouseTile.THouse.MairieRight:
                    return Properties.Resources.mairie_bot_right;
                case HouseTile.THouse.HouseFlower:
                    return Properties.Resources.House4_bot;
                case HouseTile.THouse.HousePink:
                    return Properties.Resources.House5_bot;
                case HouseTile.THouse.HouseGreen:
                    return Properties.Resources.House6_bot;
                case HouseTile.THouse.HouseWater:
                    return Properties.Resources.House7_bot;
                case HouseTile.THouse.LabLeft:
                    return Properties.Resources.House8_botleft;
                case HouseTile.THouse.LabRight:
                    return Properties.Resources.House8_botright;
                case HouseTile.THouse.ChurchLeft:
                    return Properties.Resources.Church_botleft;
                case HouseTile.THouse.ChurchRight:
                    return Properties.Resources.Church_botright;
                case HouseTile.THouse.HouseYellow:
                    return Properties.Resources.House9_botleft;
                
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