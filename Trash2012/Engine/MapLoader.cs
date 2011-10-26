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
            private static Dictionary<int, IMapTile> buildTileCorrespondor()
            {
                Dictionary<int, IMapTile> correspondance = new Dictionary<int, IMapTile>();
                correspondance.Add(0, new BackgroundTile(BackgroundTile.BackgroundType.Plain));
                correspondance.Add(1, new RoadTile(RoadTile.RoadType.Horizontal));
                correspondance.Add(2, new RoadTile(RoadTile.RoadType.Vertical));
                correspondance.Add(3, new RoadTile(RoadTile.RoadType.TopLeft));
                correspondance.Add(4, new RoadTile(RoadTile.RoadType.TopRight));
                correspondance.Add(5, new RoadTile(RoadTile.RoadType.BottomLeft));
                correspondance.Add(6, new RoadTile(RoadTile.RoadType.BottomRight));
                return correspondance;
            }

            public static readonly Dictionary<int, IMapTile> MapCorrespondor =
                buildTileCorrespondor();

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
                    AddTile(MapCorrespondor[tileCode]);
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


        // Summary:
        //      Read a file map and build corresponding city
        // Returns:
        //      New city with map initialized
        public static IMapTile[][] loadMap(string filepath)
        {
            //test buffer
            string lineBuffer = null;
            IMapBuilder<TileSwallower> swallower = new DimensionSwallower();

            //swallow that file !
            using(StreamReader stream = new System.IO.StreamReader(filepath))
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
