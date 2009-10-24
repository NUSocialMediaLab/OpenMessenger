﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Diagnostics;

using OpenMessenger.Events;

namespace OpenMessenger.Client
{
    /// <summary>
    /// The Omni window, displaying the focus graph of the contact set. (incomplete)
    /// </summary>
    public partial class OmniWindow : Form
    {

        ClientWindow _clientWindow;

        System.Timers.Timer focusTimer;
        int focusIncreaseInterval = 1 * 1000;
        Contact currentFocusContact = null;

        private int xRes;
        /// <summary>
        /// Horizontal resolution of the display that hosts the OmniWindow
        /// </summary>
        public int XRes
        {
            get { return xRes; }
            set { xRes = value; }
        }

        private int yRes;
        /// <summary>
        /// Vertical resolution of the display that hosts the OmniWindow
        /// </summary>
        public int YRes
        {
            get { return yRes; }
            set { yRes = value; }
        }

        private float boundTopLeftX = -26.5f;
        /// <summary>
        /// Horizontal value of the top left corner of the screen used to display the OmniWindow,
        /// in eye tracker units
        /// 
        /// The values are taken from from the EyeTrac6000Net application settings and should be
        /// updated if the room setup changes
        /// </summary>
        public float BoundTopLeftX
        {
            get { return boundTopLeftX; }
            set { boundTopLeftX = value; }
        }

        private float boundTopLeftY = -19.5f;
        /// <summary>
        /// Vertical value of the top left corner of the screen used to display the OmniWindow,
        /// in eye tracker units
        /// 
        /// The values are taken from from the EyeTrac6000Net application settings and should be
        /// updated if the room setup changes
        /// </summary>
        public float BoundTopLeftY
        {
            get { return boundTopLeftY; }
            set { boundTopLeftY = value; }
        }

        private float boundBottomRightX = 26.5f;
        /// <summary>
        /// Horizontal value of the bottom right corner of the screen used to display the OmniWindow,
        /// in eye tracker units
        /// 
        /// The values are taken from from the EyeTrac6000Net application settings and should be
        /// updated if the room setup changes
        /// </summary>
        public float BoundBottomRightX
        {
            get { return boundBottomRightX; }
            set { boundBottomRightX = value; }
        }

        private float boundBottomRightY = 19.5f;
        /// <summary>
        /// Vertical value of the bottom right corner of the screen used to display the OmniWindow,
        /// in eye tracker units
        /// 
        /// The values are taken from from the EyeTrac6000Net application settings and should be
        /// updated if the room setup changes
        /// </summary>
        public float BoundBottomRightY
        {
            get { return boundBottomRightY; }
            set { boundBottomRightY = value; }
        }

        private byte owSceneNum;
        /// <summary>
        /// Scene number that corresponds to the screen where the OmniWindow is displayed
        /// 
        /// The values are taken from from the EyeTrac6000Net application settings and should be
        /// updated if the room setup changes
        /// </summary>
        public byte OwSceneNum
        {
            get { return owSceneNum; }
            set { owSceneNum = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientWindow">Parent window</param>
        public OmniWindow(ClientWindow clientWindow)
        {
            this._clientWindow = clientWindow;

            InitializeComponent();

            GetScreenResolution();

            ClientController client = ClientController.GetInstance();


            focusTimer = new System.Timers.Timer(focusIncreaseInterval);
            focusTimer.Elapsed += new System.Timers.ElapsedEventHandler(focusTimer_Elapsed);

            canvas.NodeCreator = CreateContactNode;
            canvas.EdgeCreator = CreateEdge;
            canvas.Model = client.Contacts.FocusGraph;
            canvas.NodeUpdate = UpdateContactNode;

            canvas.NodeMouseEnter += new Graph.GraphCanvas.NodeMouseHandler(NodeMouseEnter);
            canvas.NodeMouseLeave += new Graph.GraphCanvas.NodeMouseHandler(NodeMouseLeave);
            canvas.NodeClicked += new Graph.GraphCanvas.NodeMouseHandler(NodeClicked);
            canvas.NodeDoubleClicked += new Graph.GraphCanvas.NodeMouseHandler(NodeDoubleClicked);

            client.Contacts.ContactUpdated += new ContactSet.ContactUpdatedHandler(OnGraphChanged);
            client.Contacts.ContactRemoved += new ContactSet.ContactRemovedHandler(OnGraphChanged);
            client.Contacts.FocusUpdated += new ContactSet.FocusUpdatedHandler(FocusUpdate);
            canvas.ArcLayout(client.Contacts.MyNode, new Func<Graph.Graph.Edge, double>(EdgeRadius),
              new Func<Graph.Graph.Edge, System.Windows.Media.Brush>(EdgeBrush));

        }

        public void StartTimer()
        {
            focusTimer.Start();
        }

        public void StopTimer()
        {
            focusTimer.Stop();
        }

        private void GetScreenResolution()
        {
            //Two screens
            if (System.Windows.Forms.Screen.AllScreens.Count() == 2)
            {
                //Note: this assumes that the projector is set up as the secondary screen
                XRes = System.Windows.Forms.Screen.AllScreens[1].Bounds.Width;
                YRes = System.Windows.Forms.Screen.AllScreens[1].Bounds.Height;
                owSceneNum = 2;
                boundTopLeftX = -26.5f;
                boundTopLeftY = -19.5f;
                boundBottomRightX = 26.5f;
                boundBottomRightY = 19.5f;
            }
            //One screen
            else
            {
                XRes = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                YRes = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                owSceneNum = 1;
                boundTopLeftX = -8.5f;
                boundTopLeftY = -7f;
                boundBottomRightX = 8.5f;
                boundBottomRightY = 7f;
            }
        }

        /// <summary>
        /// Detect which avatar is being looked at on the screen
        /// </summary>
        /// <param name="owPos">The position object that is generated from the eye tracker data</param>
        /// <returns>A contact object that represents the avatar being looked at, or null if the location
        /// does not contain any avatars</returns>
        /// 
        private delegate void InputHitTestD(System.Windows.Point pt);

        public Contact DetectAvatar(OmniWindowPos owPos)
        {
            System.Windows.Point pt = new System.Windows.Point(owPos.XPx, owPos.YPx);
            System.Windows.IInputElement elem = null;

            canvas.Dispatcher.Invoke(
            System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
                delegate()
                {
                    elem = canvas.InputHitTest(pt);
                }
            ));

            //System.Windows.IInputElement elem = canvas.InputHitTest(pt);

            if (elem != null)
            {
                //System.Windows.IInputElement elem = canvas.InputHitTest(new System.Windows.Point(owPos.XPx, owPos.YPx));
                if (elem is ContentControl)
                {
                    ContentControl ctrl = (ContentControl)elem;

                    if (ctrl.Tag is Graph.Graph.Node)
                    {
                        Graph.Graph.Node node = (Graph.Graph.Node)ctrl.Tag;
                        ClientController client = ClientController.GetInstance();
                        return client.Contacts[(Guid)node.Content];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            
        }

        void FocusUpdate(Guid contactA, Guid contactB, double level)
        {
            //TODO: this was originally meant to update the UI only, but since the UI currently bases all
            // locations on the Client's ContactSet (ClientController's _contacts) such a method is currently
            // not needed, but future changes to the UI (and whether it ultimately directly displays focus
            // levels or shows something only based on the actual levels) might benefit from a reconstruction 
            // that separates the OmniWindow from ContactSet.
            // UPDATE: Ok now parts of this are used, but it still doesn't do all it was suppoesed to originally...
        }


        void ShiftFocus(Contact target, double change)
        {
            ClientController client = ClientController.GetInstance();
            Guid i = client.Me.Id;

            foreach (Graph.Graph.Node n in client.Contacts.MyNode.Neighbors)
            {
                Guid cur = (Guid)n.Content;
                double oldFoc = client.Contacts.GetFocus(i, cur);

                if (target!=null && cur == target.Id)
                {
                    client.SetFocus(cur, oldFoc+change*0.8);
                }
                else
                    client.SetFocus(cur, oldFoc-change*0.2);

            }
        }

        void NodeMouseEnter(Graph.Graph.Node node, ContentControl UInode)
        {
            ClientController client = ClientController.GetInstance();
            currentFocusContact = client.Contacts[(Guid)node.Content];

            if (currentFocusContact == client.Me)
                currentFocusContact = null;
        }

        void focusTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Guid i = ClientController.GetInstance().Me.Id;
            ShiftFocus(currentFocusContact, 1);
        }

        void NodeMouseLeave(Graph.Graph.Node node, ContentControl UInode)
        {
            currentFocusContact = null;
        }

        void NodeClicked(Graph.Graph.Node node, ContentControl UInode)
        {
            /* doesn't really do anything meaningful...
            if ((Guid)node.Content == ClientController.GetInstance().Me.Id)
            {
                ShiftFocus(null, 1);
            }
            Guid i = ClientController.GetInstance().Me.Id;
            Update();
             */
        }

        void NodeDoubleClicked(Graph.Graph.Node node, ContentControl UInode)
        {
            
            ClientController client = ClientController.GetInstance();
            Guid i = client.Me.Id;

            if ((Guid)node.Content!= i)
                client.ShowConversationDialog((Guid)node.Content);
        }

        void OnGraphChanged(Contact contact)
        {
            Update();
        }

        /// <summary>
        /// Update the graph canvas when the screen is resized
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            canvas.Width = elementHost.Width;
            canvas.Height = elementHost.Height;

            Update();
        }

        /// <summary>
        /// Update the canvas on repaint. This seemed to fix a bug when the canvas was initially drawn,
        /// not sure why or how useful it is.
        /// </summary>
        /// <param name="e">The base class argument</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Update();
        }

        /// <summary>
        /// Update/Redraw the Omni window, called in response to changes
        /// </summary>
        new private void Update()
        {
            if (canvas.Model != null)
            {
                ClientController client = ClientController.GetInstance();
                canvas.UpdateArc(client.Contacts.MyNode, new Func<Graph.Graph.Edge, double>(EdgeRadius),
                    new Func<Graph.Graph.Edge, System.Windows.Media.Brush>(EdgeBrush));
            }
        }

        double EdgeRadius(Graph.Graph.Edge edge)
        {
            Guid i = ClientController.GetInstance().Me.Id;
            Guid neighborID = (Guid) ((((Guid)edge.To.Content) == i) ? edge.From.Content : edge.To.Content);
            Graph.Graph.Node other = (edge.To.Content.Equals(i)) ? edge.From : edge.To;
            return ClientController.GetInstance().Contacts.GetFocus((Guid)edge.From.Content, i);
        }

        Brush EdgeBrush(Graph.Graph.Edge edge)
        {
            Graph.Graph.Node myNode = ClientController.GetInstance().Contacts.MyNode;
            Graph.Graph.Node neighbor = (edge.To == myNode) ? edge.From : edge.To;
            if (currentFocusContact != null && currentFocusContact.Id == (Guid)neighbor.Content)
                return Brushes.Red;
            else
                return Brushes.DarkGray;
        }

        ContentControl CreateContactNode(Graph.Graph.Node node)
        {
            Contact contact = ClientController.GetInstance().Contacts[(Guid)node.Content];

            OmniContactNode UInode = new OmniContactNode(contact);
            Canvas.SetZIndex(UInode, 1);
            return UInode;
        }

        Line CreateEdge(Graph.Graph.Edge edge)
        {
            Line line = new Line();
            line.Stroke = Brushes.DimGray;
            Canvas.SetZIndex(line, 0);
            return line;
        }
        
        /// <summary>
        /// This method gets set to the delegate NodeUpdater of GraphCanvas. All it does is check to see that
        /// the correct optional data is displayed on the node.
        /// </summary>
        /// <param name="node"> Sadly the node needs to be passed in to get the focus level...</param>
        /// <param name="UInode"> The UInode to be updated.</param>
        /// <returns> The updated OmniContactNode.</returns>
        ContentControl UpdateContactNode(Graph.Graph.Node node, ContentControl UInode)
        {
            Guid myId = ClientController.GetInstance().Me.Id;
            OmniContactNode temp = (OmniContactNode)UInode;
            
            double focusLevel = ClientController.GetInstance().Contacts.GetFocus(myId, ((Guid)node.Content));
            if (focusLevel >= 3)
                temp.ShowInfo("");
            else temp.HideInfo();
            return temp;
        }

        private void OmniWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // hide the omni window instead of disposing it
            _clientWindow.SetOmniVisible(false);
            this.StopTimer();
            e.Cancel = true;
        }


        /// <summary>
        /// An inner class used to determine the position on the OmniWindow where a user is looking. It
        /// takes the raw eye tracker readings and converts them to corresponding pixel coordinates based
        /// on the screen bounds and the screen resolution
        /// </summary>
        public class OmniWindowPos
        {
            /// <summary>
            /// A reference to the OmniWindow is necessary since the OmniWindow contains bounds and resolution
            /// information
            /// </summary>
            private OmniWindow ow;
            private byte sceneNum;
            private Single xIn;
            private Single yIn;
            private int xPx;
            private int yPx;
            private Contact avatarHit;
            private bool isAvatarHitSet = false;

            /// <summary>
            /// Scene number where the user is looking
            /// </summary>
            public byte SceneNum
            {
                get
                {
                    return sceneNum;
                }
                set
                {
                    sceneNum = value;
                }
            }


            /// <summary>
            /// The horizontal eye position, in eye tracker units (inches)
            /// </summary>
            public Single XIn
            {
                get
                {
                    return xIn;
                }
                set
                {
                    xIn = value;
                }
            }

            /// <summary>
            /// The horizontal eye position, in pixels relative to the screen size of the display that shows
            /// the OmniWindow
            /// </summary>
            public int XPx
            {
                get
                {
                    return xPx;
                }
                set
                {
                    xPx = value;
                }
            }

            /// <summary>
            /// The vertical eye position, in eye tracker units (inches)
            /// </summary>
            public Single YIn
            {
                get
                {
                    return yIn;
                }
                set
                {
                    yIn = value;
                }
            }

            /// <summary>
            /// The vertical eye position, in pixels relative to the screen size of the display that shows
            /// the OmniWindow
            /// </summary>
            public int YPx
            {
                get
                {
                    return yPx;
                }
                set
                {
                    yPx = value;
                }
            }

            /// <summary>
            /// The avatar that was matched on the OmniWindow by this reading, or null
            /// if no avatar was matched
            /// </summary>
            public Contact AvatarHit
            {
                get { return avatarHit; }
                set { avatarHit = value; }
            }

            /// <summary>
            /// A boolean used to distinguish the two possible null values of AvatarHit. If this boolean
            /// is set, it indicates that no avatar was matched. Otherwise it simply indicates that the
            /// AvatarHit value has not yet been set.
            /// </summary>
            public bool IsAvatarHitSet
            {
                get { return isAvatarHitSet; }
                set { isAvatarHitSet = value; }
            }


            /// <summary>
            /// Constructor. Takes the raw eye position readings as received from the eye tracker, stores
            /// them and converts them to corresponding pixel position
            /// </summary>
            /// <param name="ow">A reference to the OmniWindow where the user is looking</param>
            /// <param name="sceneNum">The scene number of this reading</param>
            /// <param name="xIn">The horizontal position of this reading</param>
            /// <param name="yIn">The vertical position of this reading</param>
            public OmniWindowPos(OmniWindow ow, byte sceneNum, Single xIn, Single yIn)
            {
                this.ow = ow;
                this.sceneNum = sceneNum;

                this.xIn = xIn;
                this.yIn = yIn;

                //Shift by minimum bound values to remove negative values
                xIn -= ow.BoundTopLeftX;
                yIn -= ow.BoundTopLeftY;

                //Calculate pixel equivalent
                xPx = Convert.ToInt32(xIn / (ow.BoundBottomRightX - ow.BoundTopLeftX) * ow.XRes);
                yPx = Convert.ToInt32(yIn / (ow.BoundBottomRightY - ow.BoundTopLeftY) * ow.YRes);
                

            }


        }

        private void elementHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

    }
}
