using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Reflection;

namespace Graph
{
    /// <summary>
    /// XAML Control providing a graphical representation for a model Graph
    /// </summary>
    public class GraphCanvas : Canvas, IDisposable
    {
        /// <summary>
        /// Function delegate for creating a graphical representation for a node
        /// </summary>
        /// <param name="node">Node to represent</param>
        /// <returns>Graphical representation</returns>
        public delegate ContentControl UINodeCreator(Graph.Node node);

        /// <summary>
        /// Function delegate for creating a graphical representation for an edge
        /// </summary>
        /// <param name="edge">Edge to represent</param>
        /// <returns>Graphical representation</returns>
        public delegate Line UIEdgeCreator(Graph.Edge edge);

        /// <summary>
        /// Function delegate for updating a node's appearance
        /// </summary>
        /// <param name="node"> Node to be updated</param>
        /// <returns>Graphical representation of the updated node</returns>
        public delegate ContentControl UINodeUpdater(Graph.Node node, ContentControl UInode);

        Random _random = new Random(DateTime.Now.Millisecond);
        Graph _model;
        Dictionary<Graph.Node, ContentControl> _UInodes = new Dictionary<Graph.Node, ContentControl>();
        Dictionary<Graph.Edge, Line> _UIedges = new Dictionary<Graph.Edge, Line>();
        

        HashSet<Graph.Node> _lockedNodes = new HashSet<Graph.Node>();

        #region Events

        /// <summary>
        /// Delegate for the NodeMouse event
        /// </summary>
        /// <param name="node">Node entered</param>
        /// <param name="UInode">Graphical representation of node entered</param>
        public delegate void NodeMouseHandler(Graph.Node node, ContentControl UInode);


        /// <summary>
        /// Triggered when a node is clicked
        /// </summary>
        public event NodeMouseHandler NodeClicked;

        /// <summary>
        /// Triggered when a node is double-clicked
        /// </summary>
        public event NodeMouseHandler NodeDoubleClicked;

        /// <summary>
        /// Triggered when the mouse enters over a node
        /// </summary>
        public event NodeMouseHandler NodeMouseEnter;

        /// <summary>
        /// Triggered when the mouse leaves a node
        /// </summary>
        public event NodeMouseHandler NodeMouseLeave;

        #endregion

        /// <summary>
        /// The graph data structure to represent. Setting this synchronizes the structure with
        /// the representation.
        /// </summary>
        public Graph Model
        {
            get { return _model; }
            set
            {
                _model = value;

                Sync();

                _model.NodeAdded += new Graph.NodeAddedHandler(OnNodeAdded);
                _model.NodeRemoved += new Graph.NodeRemovedHandler(OnNodeRemoved);
                _model.NodesConnected += new Graph.NodesConnectedHandler(OnNodesConnected);
                _model.NodesDisconnected += new Graph.NodesDisconnectedHandler(OnNodesDisconnected);
                _model.EdgeUpdated += new Graph.EdgeUpdatedHandler(OnEdgeUpdated);
            }
        }

        void OnEdgeUpdated(Graph.Edge edge)
        {
            Dispatcher.Invoke(new Action(delegate
            {
                OnEdgeUpdatedDispatched(edge);
            }), null);
        }
        void OnEdgeUpdatedDispatched(Graph.Edge edge)
        {
            Children.Remove(_UIedges[edge]);
            _UIedges.Remove(edge);
            OnNodesConnected(edge);
        }

        /// <summary>
        /// Function to use for creating graphical representations of nodes
        /// </summary>
        public UINodeCreator NodeCreator { get; set; }

        /// <summary>
        /// Function to use for creating graphical representations of edges
        /// </summary>
        public UIEdgeCreator EdgeCreator { get; set; }

        /// <summary>
        /// Fuction to use for updating the graphical representation of a node
        /// </summary>
        
        public UINodeUpdater NodeUpdate { get; set; }
      
        /// <summary>
        /// Default constructor
        /// </summary>
        public GraphCanvas()
        {
            NodeCreator = GraphCanvas.DefaultNodeCreator;
            EdgeCreator = GraphCanvas.DefaultEdgeCreator;
            NodeUpdate = GraphCanvas.DefaultNodeUpdater;
        }

        void Clear()
        {
            _UInodes.Clear();
            _UIedges.Clear();
            Children.Clear();
        }

        void Sync()
        {
            Clear();

            foreach (Graph.Node node in _model.Nodes)
                OnNodeAdded(node);

            foreach (Graph.Edge edge in _model.Edges)
                OnNodesConnected(edge);
        }
       
        void OnNodeRemoved(Graph.Node node)
        {
            //ConsoleWriteLine("OnNodeRemoved");
            if (NodeLocked(node))
                UnlockNodeLocation(node);
            Children.Remove(_UInodes[node]);
            _UInodes.Remove(node);
            foreach (Graph.Edge edge in node.IncidentEdges)
            {
                OnNodesDisconnected(edge);
            }
            _model.RemoveNode(node);
        }

        void OnNodesDisconnected(Graph.Edge edge)
        {
            //ConsoleWriteLine("OnNodesDisconnected");
            Children.Remove(_UIedges[edge]);
            _UIedges.Remove(edge);
            
        }

        void OnNodesConnected(Graph.Edge edge)
        {
            // TODO: this code was keep throwing targetInvocationException at random...
            // i took out the below line out for now to debug.

            OnNodesConnectedDispatched(edge);
            try
            {
               // Dispatcher.Invoke(new Action(delegate
                //{
                //    OnNodesConnectedDispatched(edge);
                //}), System.Windows.Threading.DispatcherPriority.Normal, null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
                
        }

        void OnNodesConnectedDispatched(Graph.Edge edge)
        {
                if (!_UIedges.ContainsKey(edge))
                {
                    OnNodeAdded(edge.From);
                    OnNodeAdded(edge.To);
                    Line UIedge = EdgeCreator(edge);
                    if (edge.To != c)
                        UIedge.Width = 0;
                    _UIedges.Add(edge, UIedge);
                    UpdateEdgeLocation(edge);
                    Children.Add(UIedge);
                }
            
        }

        void UpdateEdgeLocation(Graph.Edge edge)
        {
            Line UIedge = _UIedges[edge];

            if (UIedge != null)
            {
                Point from = GetNodeLocation(edge.From);
                Point to = GetNodeLocation(edge.To);

                UIedge.X1 = from.X + _UInodes[edge.From].Width / 2;
                UIedge.Y1 = from.Y + _UInodes[edge.From].Height / 2;
                
                UIedge.X2 = to.X +_UInodes[edge.To].Width / 2;
                UIedge.Y2 = to.Y +_UInodes[edge.To].Height / 2;
            }
            else
            {
                //ConsoleWriteLine("##EdgeUpdate with Null Edge!");
            }
        }

        void OnNodeAdded(Graph.Node node)
        {
            // TODO: this code was keep throwing targetInvocationException at random...
            // i took out the below line out for now to debug.

            OnNodeAddedDispatched(node);

            //Dispatcher.Invoke(new Action(delegate
            //{
             //   OnNodeAddedDispatched(node);
            //}), null);
        }

        void OnNodeAddedDispatched(Graph.Node node)
        {
            if (!_UInodes.ContainsKey(node))
            {
                ContentControl UInode = NodeCreator(node);
                UInode.Tag = node;
                
                UInode.MouseUp += new MouseButtonEventHandler(UInode_MouseUp);
                UInode.MouseDoubleClick += new MouseButtonEventHandler(UInode_MouseDoubleClick);
                UInode.MouseEnter += new MouseEventHandler(UInode_MouseEnter);
                UInode.MouseLeave += new MouseEventHandler(UInode_MouseLeave);
                
                _UInodes.Add(node, UInode);
                
                Point location = GetRandomLocation();
                SetLeft(UInode, location.X);
                SetTop(UInode, location.Y);

                Children.Add(UInode);
            }
        }

        void UInode_MouseLeave(object sender, MouseEventArgs e)
        {
            if (NodeMouseLeave != null)
            {
                ContentControl UInode = (ContentControl)sender;
                Graph.Node node = (Graph.Node)UInode.Tag;

                NodeMouseLeave(node, UInode);
            }
        }

        void UInode_MouseEnter(object sender, MouseEventArgs e)
        {
            if (NodeMouseEnter != null)
            {
                ContentControl UInode = (ContentControl)sender;
                Graph.Node node = (Graph.Node)UInode.Tag;
                
                NodeMouseEnter(node, UInode);
            }
        }

        void UInode_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (NodeClicked != null)
            {
                ContentControl UInode = (ContentControl)sender;
                Graph.Node node = (Graph.Node)UInode.Tag;

                NodeClicked(node, UInode);
            }
        }

        void UInode_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (NodeDoubleClicked != null)
            {
                ContentControl UInode = (ContentControl)sender;
                Graph.Node node = (Graph.Node)UInode.Tag;

                NodeDoubleClicked(node, UInode);
            }
        }

        Point GetRandomLocation()
        {
            return new Point(_random.Next((int)Width - 100), _random.Next((int)Height - 100));
        }

        Point GetNodeLocation(Graph.Node node)
        {
            return new Point(GetLeft(_UInodes[node]), GetTop(_UInodes[node]));
        }

        void SetNodeLocation(Graph.Node node, Point location)
        {
            if (!NodeLocked(node))
            {
                
                SetLeft(_UInodes[node], location.X);
                SetTop(_UInodes[node], location.Y);
            }
        }

        /// <summary>
        /// Locks a node to a specific location
        /// </summary>
        /// <param name="node">The node to lock</param>
        /// <param name="location">Location to lock to</param>
        public void LockNodeLocation(Graph.Node node, Point location)
        {
            SetNodeLocation(node, location);
            _lockedNodes.Add(node);
        }

        /// <summary>
        /// Unlocks a locked node from a location
        /// </summary>
        /// <param name="node">Node to unlock</param>
        public void UnlockNodeLocation(Graph.Node node)
        {
            _lockedNodes.Remove(node);
        }

        /// <summary>
        /// Checks if a node is locked to a location
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <returns>True if node is locked</returns>
        public bool NodeLocked(Graph.Node node)
        {
            return _lockedNodes.Contains(node);
        }

        /// <summary>
        /// Centers and locks a node in the canvas
        /// </summary>
        /// <param name="node">Node to center</param>
        public void Center(Graph.Node node)
        {
            Point center = new Point((Width / 2) - (_UInodes[node].ActualWidth / 2), 
                                     (Height/2) - (_UInodes[node].ActualHeight / 2));

            UnlockNodeLocation(node);
            LockNodeLocation(node, center);
        }

        #region Default UI Elements

        private static ContentControl DefaultNodeCreator(Graph.Node node)
        {
            ContentControl UInode = new ContentControl();
            Label label = new Label();
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.Background = Brushes.Blue;
            label.BorderThickness = new Thickness(3);
            label.BorderBrush = Brushes.Chartreuse;
            label.Width = 100;
            label.Height = 40;
            label.Content = node.Content.ToString();
            
            UInode.Width = 100;
            UInode.Height = 40;
            UInode.Background = Brushes.Blue;
            
            UInode.Content = label;
            SetZIndex(UInode, 1);
            
            return UInode;
        }

        private static Line DefaultEdgeCreator(Graph.Edge edge)
        {
            Line line = new Line();

            line.Stroke = Brushes.DimGray;
            line.StrokeThickness = 1000;
            SetZIndex(line, 0);

            return line;
        }

        private static ContentControl DefaultNodeUpdater(Graph.Node node, ContentControl UInode)
        {
            return UInode;
        }

        #endregion

        #region Radial Layout
        public Dictionary<Graph.Edge, Vector> vectDict = new Dictionary<Graph.Edge, Vector>();
        public Dictionary<Graph.Node, Point> targetDict = new Dictionary<Graph.Node, Point>();
        public Graph.Node c;
        
        /// <summary>
        /// Renders a radial layout (star layout) of the graph.
        /// </summary>
        /// <param name="center">Node to center</param>
        /// <param name="EdgeRadius">Function determining the radius of an edge</param>
        /// <param name="EdgeColor">Function determining the color of an edge</param>
        public void RadialLayout(Graph.Node center, Func<Graph.Edge, double> EdgeRadius, Func<Graph.Edge, Brush> EdgeColor)
        {
            Center(center);
            c = center;
            int index = 0;
            int total = center.Degree;

            Point centerPoint = GetNodeLocation(center);

            foreach (Graph.Edge edge in center.IncidentEdges)
            {
                _UIedges[edge].Stroke = EdgeColor(edge);

                Graph.Node neighbor = (edge.From == center) ? edge.To : edge.From;
                
                double degree = (((double)index) / ((double)total)) * (2f * Math.PI);
                Vector vector = new Vector((Math.Cos(degree)), Math.Abs((Math.Sin(degree))));
                vectDict.Add(edge, vector);

                Point pos = centerPoint + (Height * vector);

                targetDict.Add(neighbor, pos);

                SetNodeLocation(neighbor, centerPoint);
                UpdateEdgeLocation(edge);

                index++;
            }
        }

        #endregion 

        #region Force-Directed Layout

        const double hooke = 0.005f; // hooke's coefficient
        const double coulomb = 1.0f; // coulomb's coefficient
        const double wall = 0.1f;
        Thread layoutThread;
        bool layoutRunning;

        /// <summary>
        /// Starts continuously updating the force-directed layout
        /// </summary>
        /// <param name="velocity"></param>
        public void StartLayout(double velocity)
        {
            layoutRunning = true;
            layoutThread = new Thread(new ParameterizedThreadStart(LayoutThread));
            layoutThread.Start(velocity);
        }

        /// <summary>
        /// Stops drawing the force-directed layout
        /// </summary>
        public void StopLayout()
        {
            layoutRunning = false;
        }

        void LayoutThread(object velocity)
        {
            while (layoutRunning)
            {
                Dispatcher.Invoke(
                    (Action)delegate() { NextLayout(1, (double)velocity); },
                    null);

                Thread.Sleep(100);
            }               
        }

        /// <summary>
        /// Simulate a number of steps of the force directed layout
        /// </summary>
        /// <param name="numSteps">Number of steps to simulate</param>
        /// <param name="velocity">Velocity of the nodes in the system</param>
        public void NextLayout(int numSteps, double velocity)
        {
            Dispatcher.Invoke(new Action(delegate
            {
                NextLayoutDispatched(numSteps, velocity);
            }), null);
        }

        //void UpdateGraph(Graph.Node cent, int numSteps, double velocity)
        //{
        //    Random r = new Random();
        //    foreach (Graph.Node node in cent.Neighbors)
        //    {
        //        Vector displacement = vectDict[_model.GetEdge(c, node)] * (r.Next(10) - 4);
        //        SetNodeLocation(node, GetNodeLocation(node) + displacement);    
        //    }
        //    foreach (Graph.Edge edge in _model.Edges)
        //        UpdateEdgeLocation(edge);
        //}

        void NextLayoutDispatched(int numSteps, double velocity)
        {
            for (int i = 0; i < numSteps; i++)
            {
                Random r = new Random();
                foreach (Graph.Node node in _model.Nodes)
                {
                    if (_model.GetEdge(c, node)==null)
                    {
                        //ConsoleWriteLine("#### No edge between nodes?");
                    }
                    else
                    {
                        //Vector displacement = velocity * EvaluateForces(node);
                        Vector displacement = vectDict[_model.GetEdge(c, node)] * (r.Next(10)-4);
                        SetNodeLocation(node, GetNodeLocation(node) + displacement);
                    }
                }

                foreach (Graph.Edge edge in _model.Edges)
                    UpdateEdgeLocation(edge);
            }
        }

        Vector EvaluateForces(Graph.Node node)
        {
            Vector sum = new Vector();
            
            // sum up forces from edges (hooke)
            foreach (Graph.Node neighbor in node.Neighbors)
                sum += UnitVector(node, neighbor) * HookeForce(node, neighbor);
          
            // sum up forces from nodes (coulomb)
            foreach (Graph.Node other in _model.Nodes)
            {
                if (node != other)
                    sum += UnitVector(node, other) * CoulombForce(node, other);
            }

            // sum up wall forces (to bounce off walls)
            sum += WallForceVector(node);

            if (sum.Length != 0)
                sum.Normalize();

            return sum;
        }

        Vector UnitVector(Graph.Node from, Graph.Node to)
        {
            Vector vec = new Vector(0, 0);

            Point locA = GetNodeLocation(from);
            Point locB = GetNodeLocation(to);

            vec.X = locB.X - locA.X;
            vec.Y = locB.Y - locA.Y;

            if (vec.Length != 0)
                vec.Normalize();

            return vec;
        }

        double HookeForce(Graph.Node nodeA, Graph.Node nodeB)
        {
            return hooke * (nodeA | nodeB).Weight * NodeDistance(nodeA, nodeB);
        }

        double CoulombForce(Graph.Node nodeA, Graph.Node nodeB)
        {
            return (-1f) * coulomb * Math.Pow(100f / NodeDistance(nodeA, nodeB), 2f);
        }

        Vector WallForceVector(Graph.Node node)
        {
            ContentControl UInode = _UInodes[node];
            Vector vec = new Vector();

            double left = GetLeft(UInode);
            double right = left + UInode.Width;
            double top = GetTop(UInode);
            double bottom = top + UInode.Height;
            
            vec.X += wall * Math.Pow(100f / left, 3f);
            vec.X -= wall * Math.Pow(100f / (Width - right), 3f);

            vec.Y += wall * Math.Pow(100f / top, 3f);
            vec.Y -= wall * Math.Pow(100f / (Height - bottom), 3f);;

            return vec;
        }

        double NodeDistance(Graph.Node nodeA, Graph.Node nodeB)
        {
            Point locA = GetNodeLocation(nodeA);
            Point locB = GetNodeLocation(nodeB);

            double distX = Math.Abs(locA.X - locB.X);
            double distY = Math.Abs(locA.Y - locB.Y);

            return Math.Sqrt(Math.Pow(distX, 2f) + Math.Pow(distY, 2f));
        }

        /// <summary>
        /// Automatic disposal of the canvas, stops a running layout.
        /// </summary>
        public void Dispose()
        {
            StopLayout();
        }

        #endregion

        #region Arc Layout

        public void BottomCenter(Graph.Node node)
        {
            Point bottom = new Point((Width / 2) - (_UInodes[node].ActualWidth / 2),
                                     (Height) - (_UInodes[node].ActualHeight / 2));

            UnlockNodeLocation(node);
            
            LockNodeLocation(node, bottom);
        }

        public Point setDestination(Graph.Node center, Graph.Edge edge)
        {
            Vector v = 5 * (1/edge.Weight) * vectDict[edge];
            
            Point destination = GetNodeLocation(center)+ v;
            return destination;
        }

        public delegate double CheckWeight(Graph.Edge edge);

        public CheckWeight WeightCheck { get; set;}

        public void ArcLayout(Graph.Node center, Func<Graph.Edge, double> EdgeRadius, Func<Graph.Edge, Brush> EdgeColor)
        {
            BottomCenter(center);
            c = center;
            int index = 0;
            int total = center.Degree;
            Point centerPoint = GetNodeLocation(center);
            
            foreach (Graph.Edge edge in center.IncidentEdges)
            {

                _UIedges[edge].Stroke = EdgeColor(edge);

                Graph.Node neighbor = (edge.From == center) ? edge.To : edge.From;
                double degree = ((1 + (double)index) / ((1 + (double)total)) * (2f * Math.PI) / 2);
                Vector vector = new Vector((Math.Cos(degree)), (-1) * Math.Abs((Math.Sin(degree))));
                vectDict.Add(edge, vector); 
                
                Point pos = centerPoint - (10*Height* vector);

                SetNodeLocation(neighbor, pos);
                UpdateEdgeLocation(edge);
                index++;
            }
            StartArcLayout(1);
        }

        /// <summary>
        /// Updates the ArcLayout after a node has been added, removed, or focused to a new level.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="EdgeRadius"></param>
        /// <param name="EdgeColor"></param>
        public void UpdateArc(Graph.Node center, Func<Graph.Edge, double> EdgeRadius, Func<Graph.Edge, Brush> EdgeColor)
        {
            BottomCenter(center);
            int total = center.Degree;
            Point centerPoint = GetNodeLocation(center);
            RedefineVectors(center);
            //ConsoleWriteLine("UpdateArc Called");
            foreach(Graph.Edge edge in center.IncidentEdges)
            {
                if (edge.To == center)
                {
                    _UIedges[edge].Stroke = EdgeColor(edge);
                    UpdateEdgeLocation(edge);
                }
                
            }
            UpdateNodes();
            //ConsoleWriteLine("UpdateArc Closing");
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateNodes()
        {
            foreach (Graph.Node node in _UInodes.Keys)
            {
                NodeUpdate(node, _UInodes[node]);
            }
        }

        /// <summary>
        /// Updates the angles of each node in the arc, relative to the center node, so that
        /// all nodes are equally distributed.
        /// This is one of a few methods that at some point might be altered for
        /// use in subarc displays.
        /// </summary>
        /// <param name="center"></param>
        void RedefineVectors(Graph.Node center)
        {
            int index = 0;
            vectDict.Clear();
            int total=center.InDegree;
            foreach(Graph.Edge edge in center.IncidentEdges)
                if (edge.To == center && edge.From != center)
                {
                    Graph.Node neighbor = (edge.From == center) ? edge.To : edge.From;

                    double degree = ((1 + (double)index) / ((1 + (double)total)) * (2f * Math.PI) / 2);
                    Vector vector = new Vector((Math.Cos(degree)), (-1) * Math.Abs((Math.Sin(degree))));
                    if (!vectDict.ContainsKey(edge))
                    {
                        vectDict.Add(edge, vector);
                    }
                    index++;
                }
        }

        public void StartArcLayout(double velocity)
        {
            layoutRunning = true;
            layoutThread = new Thread(new ParameterizedThreadStart(ArcLayoutThread));
            layoutThread.Start(velocity);
        }

        /// <summary>
        /// Stops drawing the arc-based layout
        /// </summary>
        public void StopArcLayout()
        {
            layoutRunning = false;
        }

        void ArcLayoutThread(object velocity)
        {
            while (layoutRunning)
            {
                Dispatcher.Invoke(
                    (Action)delegate() { RefreashArc(c, 1, (double)velocity); },
                    null);

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Refreashes the target points for all nodes, with respect to the
        /// node center (in the current system, center will be usually be the
        /// user's node)
        /// This is the method that should be tweaked to change the final
        /// destination points for nodes while maintaining the inverse speed
        /// of node movement.
        /// In future versions with more advanced subarcs, these method could be changed
        /// to take a target dictionary as a variable.
        /// </summary>
        /// <param name="center"> The node that will be at the center of the arc. </param>
        void ResetTargetPoints(Graph.Node center)
        {
            RedefineVectors(center);
            targetDict.Clear();
            //Point p = GetNodeLocation(center);

            foreach (Graph.Edge edge in center.IncidentEdges)
            {
               
                if (edge.To == center && edge.From != center)
                {
                    Point p = new Point((Width / 2) - (_UInodes[edge.From].ActualWidth / 2),
                                     (Height) - (_UInodes[edge.From].ActualHeight / 2));
                    targetDict.Add(edge.From, p + (vectDict[edge] * (450 - (50*(edge.Weight)))));
                }
                
            }
        }

        /// <summary>
        /// RefreashArc is the most often called method of the Arc struction, called 10 times per second.
        /// The method resets target points around the user node, and then moves each node
        /// one tenth of the distance from it's current location to its target, and updates all relevant
        /// edge/node locations
        /// </summary>
        /// <param name="center"></param>
        /// <param name="numSteps"> The number of steps, I just left this here to keep the signature similiar to other RefreashMethods</param>
        /// <param name="velocity"> Just like numsteps really... </param>
        public void RefreashArc(Graph.Node center, int numSteps, double velocity)
        {
            ResetTargetPoints(center);
            foreach (Graph.Edge edge in center.IncidentEdges)
            {
                if (edge.To == center && edge.From != center)
                {
                    Graph.Node neighbor = edge.From;
                    Vector targVect = GetNodeLocation(neighbor) - targetDict[neighbor];
                    if (!_lockedNodes.Contains(neighbor))
                        SetNodeLocation(neighbor, (targVect / (-10)) + GetNodeLocation(neighbor));
                    UpdateEdgeLocation(edge);  
                }
            }
        }

        // Right now this method doesn't get called anywhere, but
        // it's a good name for a funct and should be reincorporated...
        public void ShiftFocus(Graph.Node target, int quantity)
        {

            /// ALL OF THIS CODE is from older versions of focus systems, it is here for reference but is
            /// no longer functional.


            ////Graph.Edge connector = _model.GetEdge(target, c);
            ////foreach (Graph.Edge edge in _model.Edges)
            ////{
            ////    Console.Write("WUpdated");
            ////    if (edge.Equals(connector))
            ////        edge.Weight = (edge.Weight + 50 > 500) ? 500 : edge.Weight + 50;
            ////    else
            ////        edge.Weight = (edge.Weight - 20 < 50) ? 50 : edge.Weight - 20;
            ////}
            //////ConsoleWriteLine("Exiting ShfitFocus");
            // -----ALL STUFF FOR FAKE FOCUS, FUNCTIONAL ----------
            //if (!target.Equals(c))
            //{
            //    int old = tempFocus[target];
            //    int newF = old + quantity + 20;
            //    newF = (newF > 500) ? 500 : ((newF < 50) ? 50 : newF);

            //    tempFocus.Remove(target);
            //    tempFocus.Add(target, newF);
            //    //ConsoleWriteLine("focus shift....");
            //    foreach (Graph.Node node in _model.Nodes)
            //    {
            //        int old2 = tempFocus[node];
            //        int newF2 = old2 - 20;
            //        newF2 = (newF2 > 500) ? 500 : ((newF2 < 50) ? 50 : newF2);
            //        tempFocus.Remove(node);
            //        tempFocus.Add(node, newF2);
            //    }
            //}
        }
        #endregion
    }
}
