
using System;
using Trash2012.Model;
using System.Drawing;

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

    public enum BackgroundAnimation
    {
        House1
    }

    public class Animations
    {
        public static Bitmap FindResource(BackgroundAnimation animation)
        {
            switch (animation)
            {
                case BackgroundAnimation.House1:
                    return Properties.Resources.House1_top;
                default:
                    throw new ArgumentException("Animation introuvable: " + animation);
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
