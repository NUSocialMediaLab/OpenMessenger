using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    /// <summary>
    /// General graph data structure
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Delegate for the NodeAdded event.
        /// </summary>
        /// <param name="node">The node added</param>
        public delegate void NodeAddedHandler(Node node);

        /// <summary>
        /// Delegate for the NodeRemoved event
        /// </summary>
        /// <param name="node">The node removed</param>
        public delegate void NodeRemovedHandler(Node node);

        /// <summary>
        /// Delegate for the NodesConnected event
        /// </summary>
        /// <param name="edge">The edge resulting from connecting the nodes</param>
        public delegate void NodesConnectedHandler(Edge edge);

        /// <summary>
        /// Delegate for the NodesDisconnected event
        /// </summary>
        /// <param name="edge">The edge removed</param>
        public delegate void NodesDisconnectedHandler(Edge edge);

        /// <summary>
        /// Delegate for the EdgeUpdated event
        /// </summary>
        /// <param name="edge">The updated edge</param>
        public delegate void EdgeUpdatedHandler(Edge edge);

        /// <summary>
        /// Represents a node in the graph data structure
        /// </summary>
        public class Node
        {
            Graph _graph;

            /// <summary>
            /// Generic content of the node
            /// </summary>
            public object Content { get; set; }

            /// <summary>
            /// Total degree of the node (in-degree + out-degree)
            /// </summary>
            public int Degree
            {
                get { return InDegree + OutDegree; }
            }

            /// <summary>
            /// Number of incoming edges for the node
            /// </summary>
            public int InDegree
            {
                get
                {
                    int count = 0;
                    foreach (Edge edge in IncidentEdges)
                    {
                        if (edge.To == this)
                            count++;
                    }

                    return count;
                }
            }

            /// <summary>
            /// Number of outgoing edges for the node
            /// </summary>
            public int OutDegree
            {
                get
                {
                    int count = 0;
                    foreach (Edge edge in IncidentEdges)
                    {
                        if (edge.From == this)
                            count++;
                    }

                    return count;
                }
            }

            /// <summary>
            /// Nodes connected on outgoing edges
            /// </summary>
            public IEnumerable<Node> Neighbors
            {
                get
                {
                    foreach (Edge edge in _graph.Edges)
                    {
                        if (edge.From == this)
                            yield return edge.To;
                    }
                }
            }

            /// <summary>
            /// All edges connected to the node (incoming and outgoing)
            /// </summary>
            public IEnumerable<Edge> IncidentEdges
            {
                get
                {
                    foreach (Edge edge in _graph.Edges)
                    {
                        if (edge.From == this || edge.To == this)
                            yield return edge;
                    }
                }
            }

            /// <summary>
            /// A basic operator for getting the edge between two nodes.
            /// e.g. nodeA | nodeB returns the edge connecting nodeA to nodeB.
            /// </summary>
            /// <param name="from">Source node</param>
            /// <param name="to">Destination node</param>
            /// <returns>The connecting edge</returns>
            public static Edge operator |(Node from, Node to)
            {
                return from._graph.GetEdge(from, to);
            }

            /// <summary>
            /// Textual representation of the node
            /// </summary>
            /// <returns>Textual representation of the node's contents</returns>
            public override string ToString()
            {
                return Content.ToString();
            }

            /// <summary>
            /// Node constructor
            /// </summary>
            /// <param name="graph">Hosting graph</param>
            /// <param name="content">Content of node</param>
            public Node(Graph graph, object content)
            {
                _graph = graph;
                Content = content;
            }
        }

        /// <summary>
        /// Represents an edge in the graph
        /// </summary>
        public class Edge
        {
            Graph _graph;
            double _weight = 1.0f;

            /// <summary>
            /// Source node
            /// </summary>
            public Node From { get; set; }

            /// <summary>
            /// Destination node
            /// </summary>
            public Node To { get; set; }

            /// <summary>
            /// Weight of the edge
            /// </summary>
            public double Weight
            {
                get { return _weight; }
                set
                {
                    _weight = value;
                    if (_graph.EdgeUpdated != null)
                        _graph.EdgeUpdated(this);
                }

            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="graph">Hosting graph</param>
            /// <param name="from">Source node</param>
            /// <param name="to">Destination node</param>
            public Edge(Graph graph, Node from, Node to)
            {
                _graph = graph;
                From = from;
                To = to;
            }
        }

        /// <summary>
        /// Triggered when a node is added
        /// </summary>
        public event NodeAddedHandler NodeAdded;

        /// <summary>
        /// Triggered when a node is removed 
        /// </summary>
        public event NodeRemovedHandler NodeRemoved;

        /// <summary>
        /// Triggered when two nodes a connected
        /// </summary>
        public event NodesConnectedHandler NodesConnected;

        /// <summary>
        /// Triggered when two nodes are disconnected
        /// </summary>
        public event NodesDisconnectedHandler NodesDisconnected;

        /// <summary>
        /// Triggered when an edge is updated (e.g. weight is updated)
        /// </summary>
        public event EdgeUpdatedHandler EdgeUpdated;

        Random random = new Random(DateTime.Now.Millisecond);

        HashSet<Node> _nodes = new HashSet<Node>();
        HashSet<Edge> _edges = new HashSet<Edge>();

        /// <summary>
        /// Enumerates all nodes in the graph
        /// </summary>
        public IEnumerable<Node> Nodes
        {
            get { return _nodes.AsEnumerable<Node>(); }
        }

        /// <summary>
        /// Enumerates all edges in the graph
        /// </summary>
        public IEnumerable<Edge> Edges
        {
            get { return _edges.AsEnumerable<Edge>(); }
        }

        /// <summary>
        /// Add a node to the graph
        /// </summary>
        /// <param name="node">Node to be added</param>
        public void AddNode(Node node)
        {
            _nodes.Add(node);

            if (NodeAdded != null) 
                NodeAdded(node);
        }

        /// <summary>
        /// Add a node to the graph by specifying the content
        /// </summary>
        /// <param name="content">Content of new node</param>
        public void AddNode(object content)
        {
            Node node = new Node(this, content);
            AddNode(node);
        }

        /// <summary>
        /// Remove a node from the graph
        /// </summary>
        /// <param name="node">Node to remove</param>
        public void RemoveNode(Node node)
        {
            if (_nodes.Contains(node))
            {
                _nodes.Remove(node);

                foreach (Edge edge in node.IncidentEdges.ToList<Edge>())
                    _edges.Remove(edge);

                if (NodeRemoved != null)
                    NodeRemoved(node);
                //else
                    //ConsoleWriteLine("Graph.NodeRemoved is null");
            }
        }

        /// <summary>
        /// Connect two nodes in the graph
        /// </summary>
        /// <param name="from">Source node</param>
        /// <param name="to">Destination node</param>
        /// <returns>Resulting edge</returns>
        public virtual Edge Connect(Node from, Node to)
        {
            if (!Connected(from, to))
            {
                Edge edge = new Edge(this, from, to);

                _edges.Add(edge);

                if (NodesConnected != null)
                    NodesConnected(edge);
                return edge;
            }
            else if (from != to)
            {
                return GetEdge(from, to);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Connect two nodes by specifying the content of the nodes to connect
        /// </summary>
        /// <param name="from">Source content</param>
        /// <param name="to">Destination content</param>
        /// <returns>Resulting edge</returns>
        public Edge Connect(object from, object to)
        {
            Node fromNode = FindNode(from);
            Node toNode = FindNode(to);

            if (fromNode != null && toNode != null)
                return Connect(fromNode, toNode);
            else
                return null;
        }

        /// <summary>
        /// Disconnect two nodes in the graph
        /// </summary>
        /// <param name="from">Source node</param>
        /// <param name="to">Destination node</param>
        public void Disconnect(Node from, Node to)
        {
            Edge edge = GetEdge(from, to);

            if (edge != null)
            {
                _edges.Remove(edge);

                if (NodesDisconnected != null)
                    NodesDisconnected(edge);
            }
        }

        /// <summary>
        /// Checks if two nodes are connected
        /// </summary>
        /// <param name="from">Source node</param>
        /// <param name="to">Destination node</param>
        /// <returns>True if the two nodes are connected</returns>
        public bool Connected(Node from, Node to)
        {
            return GetEdge(from, to) != null;
        }

        /// <summary>
        /// Retrieves an edge by specifying the consituent nodes
        /// </summary>
        /// <param name="from">Source node</param>
        /// <param name="to">Destination node</param>
        /// <returns>The connecting edge</returns>
        public Edge GetEdge(Node from, Node to)
        {
            foreach (Edge edge in _edges)
            {
                if (edge.From == from && edge.To == to)
                    return edge;
            }
            return null;
        }

        /// <summary>
        /// Retrieves an edge by specifying the consituent nodes, optionally connecting the nodes
        /// if they are not already connected
        /// </summary>
        /// <param name="from">Source node</param>
        /// <param name="to">Destination node</param>
        /// <param name="createIfNotExists">Create the edge if nodes are not connected</param>
        /// <returns>The connecting edge</returns>
        public Edge GetEdge(Node from, Node to, bool createIfNotExists)
        {
            if (!Connected(from, to) && createIfNotExists)
                return Connect(from, to);

            return GetEdge(from, to);
        }

        /// <summary>
        /// Checks if the graph contains a given node
        /// </summary>
        /// <param name="node">Node to lookup</param>
        /// <returns>True if exists in the graph</returns>
        public bool ContainsNode(Node node)
        {
            return _nodes.Contains(node);
        }

        /// <summary>
        /// Checks if a node with the specified content exists in the graph
        /// </summary>
        /// <param name="content">Content of node to lookup</param>
        /// <returns>True if exists in the graph</returns>
        public bool ContainsNode(object content)
        {
            return FindNode(content) != null;
        }

        /// <summary>
        /// Retrieve a node by specifying its content
        /// </summary>
        /// <param name="content">Content to lookup</param>
        /// <returns>The node with the given content, or null</returns>
        public Node FindNode(object content)
        {
            foreach (Node node in _nodes)
            {
                if (node.Content.Equals(content))
                    return node;
            }
            return null;
        }

        /// <summary>
        /// Samples a random node from the graph
        /// </summary>
        /// <returns>A random node</returns>
        public Node RandomNode()
        {
            return _nodes.ElementAt(random.Next(_nodes.Count));
        }

        /// <summary>
        /// Clears the whole graph of nodes and edges
        /// </summary>
        public void Clear()
        {
            List<Node> removeThese = Nodes.ToList();

            foreach (Node node in removeThese)
                RemoveNode(node);
        }

        /// <summary>
        /// A randomly grown graph, i.e. not an Erdos graph. This was inspired by the
        /// paper "Are randomly grown graphs really random?" by Callaway, Hopcroft, Kleinberg
        /// and Strogatz. (http://arxiv.org/abs/cond-mat/0104546)
        /// </summary>
        /// <param name="numNodes">Number of nodes to generate</param>
        /// <param name="pAttachment">Probability of attachment</param>
        /// <returns>A graph</returns>
        public static Graph GrowRandom(int numNodes, double pAttachment)
        {
            Graph graph = new Graph();

            for (int i = 0; i < numNodes; i++)
            {
                graph.AddNode(new Graph.Node(graph, i));

                if (graph.random.NextDouble() <= pAttachment)
                {
                    Node nodeA = graph.RandomNode();
                    Node nodeB = graph.RandomNode();

                    if (nodeA != nodeB)
                    {
                        Edge edge = graph.Connect(nodeA, nodeB);
                        edge.Weight = graph.random.Next(1, 10);
                    }
                }
            }

            return graph;
        }

        public static Graph GrowNetwork(int numClients)
        {
            Graph graph = new Graph();
            Node user = new Node(graph, 0);
            graph.AddNode(user);
            for (int i = 1; i < numClients; i++)
            {
                Node neighbor = new Node(graph, i);
                graph.AddNode(neighbor);
                graph.Connect(user, neighbor);
                graph.Connect(neighbor, user);
            }
            for (int i = 0; i < numClients; i++)
            {
                Node cur = graph._nodes.ElementAt(i);
                for (int j = i; j < numClients; j++)
                {
                    Node neigh = graph._nodes.ElementAt(j);
                    graph.Connect(cur, neigh);
                    graph.Connect(neigh, cur);
                }
            }

            return graph;
        }

        /// <summary>
        /// Grows a random star topology graph. First a center node is added, then new nodes are connected
        /// with an edge being an outgoing edge specified by the pOutEdge parameter.
        /// </summary>
        /// <param name="numNodes">Total number of nodes</param>
        /// <param name="pOutEdge">Probability of having an out edge</param>
        /// <returns>Resulting star graph</returns>
        public static Graph GrowStar(int numNodes, double pOutEdge)
        {
            Graph graph = new Graph();

            Node center = new Node(graph, 0);
            graph.AddNode(center);

            for (int i = 1; i < numNodes; i++)
            {
                Node neighbor = new Node(graph, i);
                graph.AddNode(neighbor);

                if (graph.random.NextDouble() <= pOutEdge)
                    graph.Connect(center, neighbor);
                else
                    graph.Connect(neighbor, center);
            }

            return graph;
        }
    }
}
