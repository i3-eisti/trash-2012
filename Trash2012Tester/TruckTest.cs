using Trash2012.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Trash2012Tester
{
    
    
    /// <summary>
    ///This is a test class for TruckTest and is intended
    ///to contain all TruckTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TruckTest
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
        ///A test for IsHandle
        ///</summary>
        [TestMethod()]
        public void IsHandleTest()
        {
            Truck paperTruck = new Truck(TrashType.Paper,100,1.0f);
            Truck organicTruck = new Truck(TrashType.Organic, 100, 1.0f);

            Assert.IsTrue(paperTruck.CanHandle(TrashType.Paper));
            Assert.IsFalse(paperTruck.CanHandle(TrashType.Metal));

            Assert.IsTrue(organicTruck.CanHandle(TrashType.Paper));
            Assert.IsTrue(organicTruck.CanHandle(TrashType.Plastic));
            Assert.IsTrue(organicTruck.CanHandle(TrashType.Organic));
            Assert.IsFalse(organicTruck.CanHandle(TrashType.Metal));
        }

    }
}
