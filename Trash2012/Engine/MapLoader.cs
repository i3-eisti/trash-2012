using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trash2012.Model;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using Trash2012.Properties;

namespace Trash2012.Engine
{
    public class MapLoader
    {
        #region MapSwallower

        abstract class MapBuilder {}

        interface IMapBuilder<out MapBuilder>
        {
            MapBuilder swallow(string line);
            IMapTile[][] Build();
        }

        //Dimension swallower attempt to read map dimension
        class DimensionSwallower : MapBuilder, IMapBuilder<TileSwallower>
        {
            public TileSwallower swallow(string line)
            {
                //Attempt to extract map dimension from string like '5 5' or '   45 23 '
                Regex r = new Regex(@"^\s*(\d+)\s+(\d+)\s*$");
                Match m = r.Match(line);

                //extraction success
                if (m.Success) 
                {
                    try
                    {
                        int width = int.Parse(m.Groups[1].Value);
                        int height = int.Parse(m.Groups[2].Value);
                        return new TileSwallower(width, height);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException(
                            "Given map file contains wrong dimension, please check your file");
                    }
                }
                //extracton fail
                else 
                {
                    throw new ArgumentException(
                        "Given map file contains no dimension data, please check your file");
                }
            }

            public IMapTile[][] Build()
            {
                throw new NotSupportedException(
                    "Only tile swallower can build a city, " +
                    "please swallow dimension line then map tile definition");
            }
        }

        //Tile swallower attempt to read map tile
        class TileSwallower : MapBuilder, IMapBuilder<TileSwallower>
        {
            private static IMapTile MapCorrespondor(int tileCode)
            {
                switch (tileCode)
                {
                    case 0:
                         return new BackgroundTile(BackgroundTile.BackgroundType.Plain);
                    case 1 : 
                         return new RoadTile(RoadTile.RoadType.Horizontal);
                    case 2:
                         return new RoadTile(RoadTile.RoadType.Vertical);
                    case 3:
                         return new RoadTile(RoadTile.RoadType.TopLeft);
                    case 4:
                         return new RoadTile(RoadTile.RoadType.TopRight);
                    case 5:
                         return new RoadTile(RoadTile.RoadType.BottomLeft);
                    case 6:
                         return new RoadTile(RoadTile.RoadType.BottomRight); 
                    default:
                         throw new ArgumentException("Unknown tile code : " + tileCode);
                }
            }

            private int width;
            private int height;
            //tiles buffer
            private IMapTile[][] tiles;
            //check if buffer is totally filled
            private int remainingTilesToComplete;
            private int currentLine;
            private int currentColumn;

            public TileSwallower(int width, int height)
            {
                this.width = width;
                this.height = height;
                remainingTilesToComplete = width * height;
                tiles = new IMapTile[height][];
                currentLine = currentColumn = 0;
                tiles[currentLine] = new IMapTile[width];
            }

            private void AddTile(IMapTile tile)
            {
                if (currentColumn == width)
                {
                    currentColumn = 0;
                    currentLine += 1;
                    tiles[currentLine] = new IMapTile[width];
                }
                //add corresponding tile to buffer
                tile.Position = new Point(currentColumn, currentLine);
                this.tiles[currentLine][currentColumn] = tile;
                currentColumn += 1;
            }

            public TileSwallower swallow(string line)
            {
                // '\d' represents a single so that we can extract a single 
                Regex tileExtractor = new Regex(@"(\d)");
                MatchCollection tileMatchs = tileExtractor.Matches(line);

                foreach(Match tileMatch in tileMatchs)
                {
                    //read tile code
                    int tileCode = int.Parse( tileMatch.Value );
                    AddTile(MapCorrespondor(tileCode));
                    //decrease remaining needed item
                    remainingTilesToComplete--;
                }

                //chained call so return yourself
                return this;
            }

            public IMapTile[][] Build()
            {
                if (remainingTilesToComplete != 0)
                    throw new Exception("Not enough data to build a complete city");
                return tiles;
            }

        }

        #endregion

        public static IMapTile[][] loadMapFromFile(string filepath)
        {
            //swallow that file !
            using (Stream stream = new System.IO.FileStream(filepath,FileMode.Open))
            {
                return loadMap(stream);
            }
        }

        public static IMapTile[][] loadDefaultMap()
        {
            return loadMap(new MemoryStream(Resources.DefaultMap));
        }

        // Summary:
        //      Read a file map and build corresponding city
        // Returns:
        //      New city with map initialized
        private static IMapTile[][] loadMap(Stream inputStream)
        {
            //test buffer
            string lineBuffer = null;
            IMapBuilder<TileSwallower> swallower = new DimensionSwallower();

            //swallow that file !
            using (StreamReader stream = new System.IO.StreamReader(inputStream))
            {
                //iterate through lines
                while((lineBuffer = stream.ReadLine()) != null)
                {
                    //line is a comment
                    if (lineBuffer.StartsWith("#"))
                    {
                        continue;
                    }
                    //line needs to be parsed
                    else
                    {
                        //swallower handle incorrect data in map file and throw 'ArgumentException'
                        swallower = swallower.swallow(lineBuffer);
                    }
                }
            }

            //throw 'NotSupportedException' if given file doesn't provide tile data
            return swallower.Build();
        }

        private MapLoader() { }
    }
}
