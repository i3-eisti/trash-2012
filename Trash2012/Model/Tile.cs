using System;
using System.Drawing;
using Trash2012.Properties;

namespace Trash2012.Model
{
    /// <summary>
    /// General Tile
    /// </summary>
    public interface IMapTile
    {
        Bitmap Tile { get; }
        Point Position { get; set; }
    }
    /// <summary>
    /// Tile which represents background elements
    /// </summary>
    public interface IBackgroundTile : IMapTile
    {
        BackgroundTile.BackgroundType Type { get; }
    }
    /// <summary>
    /// Tile which represents road elements
    /// </summary>
    public interface IRoadTile : IMapTile
    {
        RoadTile.RoadType Type { get; }
    }
    /// <summary>
    /// Tile which represents house elements
    /// </summary>
    public interface IHouseTile : IRoadTile
    {
        Trash Garbage { get; set; }
    }

    //All interface above map to corresponding class

    public abstract class AbstractTile<TileType>
    {
        /// <summary>
        /// Tile's image
        /// </summary>
        public Bitmap Tile { get; set; }
        /// <summary>
        /// Position of the tile in the Map (grid)
        /// </summary>
        public Point Position { get; set; }
        /// <summary>
        /// Tile's type
        /// </summary>
        public TileType Type { get; private set; }

        protected AbstractTile(Bitmap img, TileType t)
        {
            Tile = img;
            Type = t;
        }

        public abstract override int GetHashCode();

        public override bool Equals(object obj)
        {
            return obj is AbstractTile<TileType> && obj.GetHashCode() == this.GetHashCode();
        }
    }

    public class BackgroundTile : AbstractTile<BackgroundTile.BackgroundType>, IBackgroundTile
    {
        /// <summary>
        /// BackgroundTile Type
        /// </summary>
        public enum BackgroundType
        {
            Plain
        }

        /// <summary>
        ///     Internal method for selecting correct Bitmap
        /// </summary>
        /// <param name="type">BackgroundTile type</param>
        /// <returns>corresponding BitMap</returns>
        private static Bitmap selectTile(BackgroundType type)
        {
            switch (type)
            {
                case BackgroundType.Plain:
                    return Resources.TilePlain;
                default:
                    throw new ArgumentException("Unknown Map BackgroundType : " + type);
            }
        }

        public override int GetHashCode()
        {
            return Type.ToString().GetHashCode() * 31 + 11;
        }

        public BackgroundTile(BackgroundType type) : base(selectTile(type), type) { }
    }

    /// <summary>
    ///  Specific MapTile which represents Road tile
    /// </summary>
    public class RoadTile : AbstractTile<RoadTile.RoadType>, IRoadTile
    {
        /// <summary>
        /// RoadTile's type
        /// </summary>
        public enum RoadType
        {
            Horizontal,
            Vertical,
            TopLeft,            //              TOP
            TopRight,           //            |     |
            BottomLeft,         //            |     |
            BottomRight,        //      ______|     |______
            TopBottomLeft,      // LEFT                     RIGHT
            TopBottomRight,     //      ______       ______              
            TopLeftRight,       //            |     |
            BottomLeftRight     //            |     |
            //            |     |
            //            BOTTOM  
        }

        /// <summary>
        ///     Internal method for selecting correct Bitmap
        /// </summary>
        /// <param name="type">MapTile type</param>
        /// <returns>corresponding BitMap</returns>
        private static Bitmap selectTile(RoadTile.RoadType dir)
        {
            switch (dir)
            {
                case RoadType.Horizontal:
                    return Resources.TileRoadHorizontal;
                case RoadType.Vertical:
                    return Resources.TileRoadVertical;
                case RoadType.TopLeft:
                    return Resources.TileRoadTopLeft;
                case RoadType.TopRight:
                    return Resources.TileRoadTopRight;
                case RoadType.BottomLeft:
                    return Resources.TileRoadBottomLeft;
                case RoadType.BottomRight:
                    return Resources.TileRoadBottomRight;
                case RoadType.TopBottomLeft:
                    return Resources.TileRoadTopBottomLeft;
                case RoadType.TopBottomRight:
                    return Resources.TileRoadTopBottomRight;
                case RoadType.TopLeftRight:
                    return Resources.TileRoadTopLeftRight;
                case RoadType.BottomLeftRight:
                    return Resources.TileRoadBottomLeftRight;
                default:
                    throw new ArgumentException("Unknown Road Direction : " + dir);
            }
        }

        public override int GetHashCode()
        {
            return Type.ToString().GetHashCode() * 31 + 13;
        }

        public RoadTile(RoadTile.RoadType dir) : base(selectTile(dir), dir) { }
    }

    /// <summary>
    /// Specific MapTile which represents House tile
    /// </summary>
    public class HouseTile : RoadTile, IHouseTile
    {
        public override int GetHashCode()
        {
            return Type.ToString().GetHashCode() * 31 + 17;
        }

        public Trash Garbage { get; set; }

        public HouseTile(RoadType t, TrashType ttype,int quantity = 0) : base(t)
        {
            Garbage = new Trash(ttype, quantity);
            switch (t)
            {
                case RoadType.Horizontal: Tile = quantity == 0 ? Resources.HouseHorizontalEmpty : Resources.HouseHorizontalFull; break;
                case RoadType.Vertical: Tile = quantity == 0 ? Resources.HouseVerticalEmpty : Resources.HouseVerticalFull; break;
            }
        }
    }

}
