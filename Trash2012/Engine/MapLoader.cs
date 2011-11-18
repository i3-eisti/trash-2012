using System;
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
            private static int MapCorrespondor(int tileCode)
            {
                switch (tileCode)
                {
                    case 0:
                         return (int)BackgroundTile.BackgroundType.Plain;
                    case 1 :
                         return (int)RoadTile.RoadType.Horizontal;
                    case 2:
                         return (int)RoadTile.RoadType.Vertical;
                    case 3:
                         return (int)RoadTile.RoadType.TopLeft;
                    case 4:
                         return (int)RoadTile.RoadType.TopRight;
                    case 5:
                         return (int)RoadTile.RoadType.BottomLeft;
                    case 6:
                         return (int)RoadTile.RoadType.BottomRight; 
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
                Regex tileExtractor = new Regex(@"(\d+)-(\d+)-(\d+)");
                MatchCollection tileMatchs = tileExtractor.Matches(line);

                foreach(Match tileMatch in tileMatchs)
                {
                    //read tile code, house flag and garbage amount
                    int tileCode = int.Parse( tileMatch.Groups[1].Value );
                    bool hasHouse = int.Parse( tileMatch.Groups[2].Value ) == 1;
                    int garbageAmount = int.Parse( tileMatch.Groups[3].Value );


                    IMapTile tile; //empty tile
                    var tileType = MapCorrespondor(tileCode); //tile orientation
                    if(tileCode == 0) //Plain
                    {
                        tile = new BackgroundTile((BackgroundTile.BackgroundType)tileType);
                    }
                    else
                    {
                        if(hasHouse)
                        {
                            tile = new HouseTile(
                                (RoadTile.RoadType)tileType,
                                TrashType.Paper,
                                garbageAmount);
                        }
                        else
                        {
                            tile = new RoadTile(
                                (RoadTile.RoadType)tileType);
                        }
                    }
                    AddTile(tile);
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
            using (Stream stream = new FileStream(filepath,FileMode.Open))
            {
                return loadMap(stream);
            }
        }

        public static IMapTile[][] loadDefaultMap()
        {
            //return loadMap(new MemoryStream(Resources.DefaultMap));
            return loadMap(new MemoryStream(Resources.CustomMap));
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
            using (StreamReader stream = new StreamReader(inputStream))
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
