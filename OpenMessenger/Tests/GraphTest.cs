using Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Tests
{
    
    
    /// <summary>
    ///This is a test class for GraphTest and is intended
    ///to contain all GraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GraphTest
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
        /// Tests adding and removing nodes from the graph.
        ///</summary>
        [TestMethod()]
        public void AddRemoveNodeTest()
        {
            Graph.Graph target = new Graph.Graph();
            Graph.Graph.Node node = new Graph.Graph.Node(target, 1);
            target.AddNode(node);

            Assert.AreEqual<bool>(target.ContainsNode(node), true);
            Assert.AreEqual<bool>(target.ContainsNode(1), true);
        }

        /// <summary>
        /// Tests connecting, disconnecting nodes and retrieving edges.
        ///</summary>
        [TestMethod()]
        public void ConnectionTest()
        {
            Graph.Graph target = new Graph.Graph();

            Graph.Graph.Node node1 = new Graph.Graph.Node(target, 1);
            target.AddNode(node1);

            Graph.Graph.Node node2 = new Graph.Graph.Node(target, 2);
            target.AddNode(node2);

            target.Connect(node1, node2);
            target.Connect(2, 1);

            Assert.IsTrue(target.Connected(node1, node2));
            Assert.IsTrue(target.Connected(node2, node1));

            Graph.Graph.Edge edge = target.GetEdge(node1, node2);

            Assert.IsNotNull(edge);
            Assert.AreEqual<Graph.Graph.Node>(edge.From, node1);
            Assert.AreEqual<Graph.Graph.Node>(edge.To, node2);

            Assert.AreEqual<Graph.Graph.Edge>(edge, node1 | node2);

            target.RemoveNode(node1);

            Assert.IsNull(node1 | node2);
            Assert.IsNull(node2 | node1);
        }

        /// <summary>
        /// Tests looking up a given node in the graph by its value
        ///</summary>
        [TestMethod()]
        public void FindNodeTest()
        {
            Graph.Graph target = new Graph.Graph();

            Graph.Graph.Node node = new Graph.Graph.Node(target, 7);
            target.AddNode(node);

            Graph.Graph.Node found = target.FindNode(7);

            Assert.AreEqual<Graph.Graph.Node>(node, found);
        }
    }
}
