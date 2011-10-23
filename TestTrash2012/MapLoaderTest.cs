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
        public void loadMapTest()
        {
            string filepath = @"D:\devel\Trash2012\Trash2012\Resources\default.trash-map";
            City expected = null; // TODO: Initialize to an appropriate value
            City actual;
            actual = MapLoader.loadMap(filepath);
        }
    }
}
