using Trash2012.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Trash2012.Model;

namespace TestTrash2012
{
    
    
    /// <summary>
    ///This is a test class for MapLoaderTest and is intended
    ///to contain all MapLoaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MapLoaderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for loadMap
        ///</summary>
        [TestMethod()]
        public void loadDefaultMapTest()
        {
            IMapTile[][] expected = new IMapTile[5][];
            expected[0] = new IMapTile[6];
            expected[1] = new IMapTile[6];
            expected[2] = new IMapTile[6];
            expected[3] = new IMapTile[6];
            expected[4] = new IMapTile[6];
            expected[0][0] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[0][1] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[0][2] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[0][3] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[0][4] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[0][5] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[1][0] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[1][1] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[1][2] = new RoadTile(RoadTile.RoadType.TopRight);
            expected[1][3] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[1][4] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[1][5] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[2][0] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[2][1] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[2][2] = new RoadTile(RoadTile.RoadType.Vertical);
            expected[2][3] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[2][4] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[2][5] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[3][0] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[3][1] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[3][2] = new RoadTile(RoadTile.RoadType.BottomLeft);
            expected[3][3] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[3][4] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[3][5] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[4][0] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[4][1] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[4][2] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[4][3] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[4][4] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[4][5] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);

            IMapTile[][] actual = MapLoader.loadDefaultMap();

            Assert.IsTrue(actual.Length > 0 && actual[0].Length > 0, "Map has not a valid dimension");
            Assert.AreEqual(expected.Length, actual.Length, "Constructed map has a different height");
            Assert.AreEqual(expected[0].Length, actual[0].Length, "Constructed map has a different width");

            for (int i = actual.Length; i-- > 0; )
            {
                for (int j = actual[0].Length; j-- > 0; )
                {
                    int hashA = expected[i][j].GetHashCode();
                    int hashB = actual[i][j].GetHashCode();
                    Assert.AreEqual(expected[i][j], actual[i][j], "Constructed map got a different tile at (" + i + "," + j + ")");
                }
            }
        }

        [TestMethod()]
        public void loadMapFromFileTest()
        {
            IMapTile[][] expected = new IMapTile[6][];
            expected[0] = new IMapTile[6];
            expected[1] = new IMapTile[6];
            expected[2] = new IMapTile[6];
            expected[3] = new IMapTile[6];
            expected[4] = new IMapTile[6];
            expected[5] = new IMapTile[6];
            expected[0][0] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[0][1] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[0][2] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[0][3] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[0][4] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[0][5] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);

            expected[1][0] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[1][1] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[1][2] = new RoadTile(RoadTile.RoadType.TopRight);
            expected[1][3] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[1][4] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[1][5] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);

            expected[2][0] = new RoadTile(RoadTile.RoadType.TopLeft);
            expected[2][1] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[2][2] = new RoadTile(RoadTile.RoadType.BottomRight);
            expected[2][3] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[2][4] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[2][5] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);

            expected[3][0] = new RoadTile(RoadTile.RoadType.BottomLeft);
            expected[3][1] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[3][2] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[3][3] = new RoadTile(RoadTile.RoadType.TopRight);
            expected[3][4] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[3][5] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);

            expected[4][0] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[4][1] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[4][2] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[4][3] = new RoadTile(RoadTile.RoadType.Vertical);
            expected[4][4] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[4][5] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);

            expected[5][0] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[5][1] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[5][2] = new BackgroundTile(BackgroundTile.BackgroundType.Plain);
            expected[5][3] = new RoadTile(RoadTile.RoadType.BottomLeft);
            expected[5][4] = new RoadTile(RoadTile.RoadType.Horizontal);
            expected[5][5] = new RoadTile(RoadTile.RoadType.Horizontal);

            string filepath = @"..\..\..\Trash2012\Resources\custom.trash-map";
            IMapTile[][] actual = MapLoader.loadMapFromFile(filepath);

            Assert.IsTrue(actual.Length > 0 && actual[0].Length > 0, "Map has not a valid dimension");
            Assert.AreEqual(expected.Length, actual.Length, "Constructed map has a different height");
            Assert.AreEqual(expected[0].Length, actual[0].Length, "Constructed map has a different width");

            for (int i = actual.Length; i-- > 0; )
            {
                for (int j = actual[0].Length; j-- > 0; )
                {
                    int hashA = expected[i][j].GetHashCode();
                    int hashB = actual[i][j].GetHashCode();
                    Assert.AreEqual(expected[i][j], actual[i][j], "Constructed map got a different tile at (" + i + "," + j + ")");
                }
            }
        }

    }
}
