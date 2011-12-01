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
        HouseTile.THouse HouseType { get; }
        Trash Garbage { get; set; }
    }

    //All interface above map to corresponding class

    public abstract class AbstractTile<TileType>
    {
        /// <summary>
        /// Position of the tile in the Map (grid)
        /// </summary>
        public Point Position { get; set; }
        /// <summary>
        /// Tile's type
        /// </summary>
        public TileType Type { get; private set; }

        protected AbstractTile(TileType t)
        {
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
            Plain,
            BlueHouse
        }

        public override int GetHashCode()
        {
            return Type.ToString().GetHashCode() * 31 + 11;
        }

        public BackgroundTile(BackgroundType type) : base(type) { }
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
            Horizontal,         //              TOP
            Vertical,           //            |     |
            TopLeft,            //            |     |
            TopRight,           //      ______|     |______
            BottomLeft,         // LEFT                     RIGHT
            BottomRight,        //      ______       ______      
            TopBottomLeft,      //            |     |
            TopBottomRight,     //            |     |        
            TopLeftRight,       //            |     |
            BottomLeftRight     //            BOTTOM 
        }

        public override int GetHashCode()
        {
            return Type.ToString().GetHashCode() * 31 + 13;
        }

        public RoadTile(RoadTile.RoadType dir) : base(dir) { }
    }

    /// <summary>
    /// Specific MapTile which represents House tile
    /// </summary>
    public class HouseTile : RoadTile, IHouseTile
    {
        public enum THouse
        {
            Normal,
            Blue
        }

        public override int GetHashCode()
        {
            return Type.ToString().GetHashCode() * 31 + 17;
        }

        public Trash Garbage { get; set; }
        public THouse HouseType { get; private set; }

        public HouseTile(RoadType t, THouse ht, TrashType ttype, int quantity = 0) : base(t)
        {
            Garbage = new Trash(ttype, quantity);
            HouseType = ht;
        }
    }

}
