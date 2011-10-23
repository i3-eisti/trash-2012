using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Trash2012.Properties;
using System.Drawing;

namespace Trash2012.Model
{
    public class City
    {
        public MapTile[,] Map { get; private set; }

        public City(MapTile[,] cityMap)
        {
            this.Map = cityMap;
        }
    }

    public class MapTile
    {
        public enum Type
        {
            Plain
        }

        private static Bitmap selectTile(MapTile.Type type)
        {
            switch (type)
            {
                case Type.Plain:
                    return Resources.TilePlain;
                default:
                    throw new ArgumentException("Unknown Map Type");
            }
        }

        public Bitmap Tile { get; private set; }

        protected MapTile(Bitmap img)
        {
            Tile = img;
        }

        public MapTile(Type type) : this(selectTile(type))
        {

        }
    }

    class RoadTile : MapTile
    {
        new public enum Type
        {
            Horizontal,
            Vertical,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        private static Bitmap selectTile(RoadTile.Type dir)
        {
            switch (dir)
            {
                case Type.Horizontal:
                    return Resources.TileRoadHorizontal;
                case Type.Vertical:
                    return Resources.TileRoadVertical;
                case Type.TopLeft:
                    return Resources.TileRoadTopLeft;
                case Type.TopRight:
                    return Resources.TileRoadTopRight;
                case Type.BottomLeft:
                    return Resources.TileRoadBottomLeft;
                case Type.BottomRight:
                    return Resources.TileRoadBottomRight;
                default:
                    throw new ArgumentException("Unknown Road Direction");
            }
        }

        public RoadTile(RoadTile.Type dir) : base(selectTile(dir))
        {
        }
    }

    class HouseTile : MapTile
    {
        new public enum Type
        {
            Horizontal,
            Vertical,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        private static Bitmap selectTile(HouseTile.Type dir)
        {
            throw new NotImplementedException("House tile icon not ready yet.");
            //switch (dir)
            //{
            //    case Type.Horizontal:
            //        return Resources.TileHouseRoadHorizontal;
            //    case Type.Vertical:      
            //        return Resources.TileHouseRoadVertical;
            //    case Type.TopLeft:       
            //        return Resources.TileHouseRoadTopLeft;
            //    case Type.TopRight:      
            //        return Resources.TileHouseRoadTopRight;
            //    case Type.BottomLeft:    
            //        return Resources.TileHouseRoadBottomLeft;
            //    case Type.BottomRight:   
            //        return Resources.TileHouseRoadBottomRight;
            //    default:
            //        throw new ArgumentException("Unknown House Type");
            //}
        }

        public HouseTile(HouseTile.Type type) : base(selectTile(type))
        {
        }
    }


}
