using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Windows.Forms;

namespace OpenMessenger.Server
{
    /// <summary>
    /// Main window for the server
    /// </summary>
    public class Server
    {
        #region Events

        /// <summary>
        /// Delegate for the ServerStatusEvent
        /// </summary>
        /// <param name="message">Message reported by the server</param>
        public delegate void ServerStatusHandler(string message);

        /// <summary>
        /// Triggers when the server decides to report something to the UI
        /// </summary>
        public event ServerStatusHandler ServerStatus;

        #endregion

        static Server _instance;

        Uri _url;
        ServiceHost _host;
        
        /// <summary>
        /// True if the service host is running
        /// </summary>
        public bool IsRunning
        {
            get { return _host != null && _host.State == CommunicationState.Opened; }
        }

        /// <summary>
        /// Gets the controller for the service controller
        /// </summary>
        public ServiceController Controller
        {
            get { return ((IService)_host.SingletonInstance).Controller; }
        }

        /// <summary>
        /// Gets the singleton instance of the server
        /// </summary>
        /// <returns>The server singleton instance</returns>
        public static Server GetInstance()
        {
            if (_instance == null)
                _instance = new Server();

            return _instance;
        }

        /// <summary>
        /// Start hosting the OM service
        /// </summary>
        /// <param name="url">Url of listener</param>
        /// <returns>True if successful</returns>
        public bool Start(string url)
        {
            if (IsRunning)
                Stop();

            _url = new Uri(url);
            _host = new ServiceHost(typeof(OpenMessenger.Service), _url);

            try
            {
                _host.AddServiceEndpoint(
                    typeof(OpenMessenger.IService),
                    new WSHttpBinding(),
                    _url);

                // enable metadata exchange.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                _host.Description.Behaviors.Add(smb);

                // wire up service host events for forwarding
                _host.Opening += new EventHandler(ServiceHostOpeningHandler);
                _host.Opened += new EventHandler(ServiceHostOpenedHandler);
                _host.Closing += new EventHandler(ServiceHostClosingHandler);
                _host.Closed += new EventHandler(ServiceHostClosedHandler);

                _host.Open();
            }
            catch (CommunicationException ce)
            {
                MessageBox.Show("Communcation Error: {0}", ce.Message);
                _host.Abort();
            }

            return IsRunning;
        }

        /// <summary>
        /// Stops hosting the OM service
        /// </summary>
        /// <returns>True if successful</returns>
        public bool Stop()
        {
            _host.Close();

            return !IsRunning;
        }

        #region Event Handlers

        void ServiceHostClosedHandler(object sender, EventArgs e)
        {
            if (ServerStatus != null)
                ServerStatus("Server stopped!");
        }

        void ServiceHostClosingHandler(object sender, EventArgs e)
        {
            if (ServerStatus != null)
                ServerStatus("Server closing...");
        }

        void ServiceHostOpenedHandler(object sender, EventArgs e)
        {
            if (ServerStatus != null)
                ServerStatus("Server started!");
        }

        void ServiceHostOpeningHandler(object sender, EventArgs e)
        {
            if (ServerStatus != null)
                ServerStatus("Server starting...");
        }

        #endregion
    }
}
