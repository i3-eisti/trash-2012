using System;
using Trash2012.Properties;
using System.Drawing;

namespace Trash2012.Model
{
    /// <summary>
    ///     City Class.
    ///     Contains a two-dimension array acting as city map.
    /// </summary>
    public class City
    {
        //cannot access both size with [,] notation
        public IMapTile[][] Map { get; private set; }
        public int PeopleNumber { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public City(IMapTile[][] map) : this(map, map[0].Length, map.Length) { }

        public City(IMapTile[][] cityMap, int width, int height)
        {
            Map = cityMap;
            Width = width;
            Height = height;

            var plpNum = 0;
            for (var i = Height; i-- > 0; )
            {
                for (var j = Width; j-- > 0; )
                {
                    if (Map[i][j] is IHouseTile) //People is equivalent to a HouseTile
                        plpNum++;
                }
            }
            PeopleNumber = plpNum;

        }

        public override bool Equals(object obj)
        {
            if (!(obj is City))
                return false;
            City that = (City)obj;

            return GetHashCode() == that.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Width * 31 + (
                Height * 31 + (
                    Map.GetHashCode() * 31
            ));
        }
    }


}
