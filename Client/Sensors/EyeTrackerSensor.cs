using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using ASLSERIALOUTLIB2Lib;
using System.Runtime.InteropServices;


namespace OpenMessenger.Client.Sensors
{

    /// <summary>
    /// Sensor that connects to the eye tracker and reads eye tracker data periodically. Note that the
    /// eye tracker software must be installed in order for the libraries to be linked in correctly.
    /// In order to understand this code in detail, it is very helpful to look at the SDK document
    /// for the eye tracker.
    /// </summary>
    public class EyeTrackerSensor : Sensor
    {
        /// <summary>
        /// Delegate for the EyeTrackerUpdate event
        /// </summary>
        /// <param name="screenshot"></param>
        public delegate void EyeTrackerUpdateHandler(byte sceneNum, float xIn, float yIn);

        /// <summary>
        /// Triggered when new data from the eye tracker is received
        /// </summary>
        public event EyeTrackerUpdateHandler EyeTrackerUpdate;

        /// <summary>
        /// The main object responsible for communicating with the eye tracker
        /// </summary>
        private IASLSerialOutPort3 reader;

        /// <summary>
        /// This configuration file specifies some of the parameters used to communicate with the eye
        /// tracker, such as the fields that we are interested in receiving.
        /// It is usually generated when you run the SerialOutClient sample application that is provided
        /// as a sample of the eye tracker SDK
        /// </summary>
        private string cfgFile = "C:/Program Files/ASL Eye Tracker 6000/SDK/VisualCPP/SerialOutClient/Release/ETSerialPortViewer.cfg";

        /// <summary>
        /// After connecting to the eye tracker, this array indicates the names and indices of the
        /// values received
        /// </summary>
        private Array itemNames = null;

        /// The three constants below store the names of the fields that we are interested in
        /// from the eye tracker
        private const String SCENE_NUM_FIELD = "EH_scene_number";
        private const String XPOS_FIELD = "EH_horz_gaze_coord";
        private const String YPOS_FIELD = "EH_vert_gaze_coord";

        private const int INDEX_NOT_INITIALIZED = -1;

        /// The three constants below store the indices of the fields that we are interested in
        /// from the eye tracker
        private static int sceneNumIndex = INDEX_NOT_INITIALIZED;
        private static int xPosIndex = INDEX_NOT_INITIALIZED;
        private static int yPosIndex = INDEX_NOT_INITIALIZED;

        /// <summary>
        /// The timer responsible for periodically calling read operations on the eye tracker
        /// </summary>
        private System.Timers.Timer eyePosReadTimer;

        private int updateFreq = 500;
        /// <summary>
        /// The time period (in ms) for reading data from the eye tracker
        /// </summary>
        public int UpdateFreq
        {
            get { return updateFreq; }
            set { updateFreq = value; }
        }

        /// <summary>
        /// Name of this sensor
        /// </summary>
        public override string Text
        {
            get { return "EyeTracker"; }
        }

        /// <summary>
        /// Default constructor - sets up the timer object (but does not start it)
        /// </summary>
        public EyeTrackerSensor()
        {
            eyePosReadTimer = new System.Timers.Timer(updateFreq);
            eyePosReadTimer.Elapsed += new System.Timers.ElapsedEventHandler(ReadPosition);
        }

        /// <summary>
        /// Start the sensor, reading data from the eye tracker
        /// </summary>
        public override void Start()
        {
            StartEyeTracker();
            eyePosReadTimer.Start();
        }

        /// <summary>
        /// Logic for setting up the initial connection to the eye tracker. Reading the eye tracker SDK
        /// will help better understand this method.
        /// </summary>
        private void StartEyeTracker()
        {
            reader = new ASLSerialOutPort3Class();
            
            bool eyeHeadIntegration = true;
            int port = 1;
            int baudRate = 57600;
            int updateRate = 60;
            //Note: we do not enable streaming mode because we want to specify our own frequency and because
            //we do not need the very high frequency that streaming mode provides.
            bool streamingMode = false;
            int itemCount;

            try
            {
                reader.Connect(cfgFile, port, eyeHeadIntegration, out baudRate, out updateRate,
                    out streamingMode, out itemCount, out itemNames);

                //Find the position in the array received from the eye tracker where relevant values are stored
                if (sceneNumIndex == INDEX_NOT_INITIALIZED || xPosIndex == INDEX_NOT_INITIALIZED ||
                    yPosIndex == INDEX_NOT_INITIALIZED)
                {
                    if (itemNames != null)
                    {
                        for (int i = 0; i < itemCount; i++)
                        {
                            string curField = (string)itemNames.GetValue(i);
                            if (SCENE_NUM_FIELD == curField)
                            {
                                sceneNumIndex = i;
                            }

                            if (XPOS_FIELD == curField)
                            {
                                xPosIndex = i;
                            }

                            if (YPOS_FIELD == curField)
                            {
                                yPosIndex = i;
                            }
                        }
                    }

                }
            }
            catch (COMException)
            {
                string errorMsg;
                reader.GetLastError(out errorMsg);
                //FIXME: Should we be writing this to some log file?
                //ConsoleWriteLine("Could not connect to eye tracker: " + errorMsg);
                eyePosReadTimer.Stop();
            }
            
        }

        /// <summary>
        /// Stop the sensor, reading data from the eye tracker
        /// </summary>
        public override void Stop()
        {
            eyePosReadTimer.Stop();
            reader.Disconnect();
        }

        /// <summary>
        /// Get readings from the eye tracker. In order for this method to work, StartEyeTracker() must
        /// be called first in order to initialize the connection. Reading the eye tracker SDK
        /// will help better understand this method.
        /// </summary>
        private void ReadPosition(object sender, System.Timers.ElapsedEventArgs e)
        {

            eyePosReadTimer.Stop();

            if (EyeTrackerUpdate != null)
            {
                Array tempItems = null;
                int count;
                bool available = false;
                try
                {
                    reader.GetScaledData(out tempItems, out count, out available);
                }
                catch (COMException)
                {
                    string errorMsg;
                    reader.GetLastError(out errorMsg);
                    //FIXME: This should go to the event log instead.
                    //ConsoleWriteLine("Error reading data from eye tracker: " + errorMsg);
                }

                if (available)
                {
                    byte sceneNum = (byte)tempItems.GetValue(sceneNumIndex);
                    Single xIn = (Single)tempItems.GetValue(xPosIndex);
                    Single yIn = (Single)tempItems.GetValue(yPosIndex);
                    //Raise event that the EyeActivityMonitor listens for. It includes the raw readings of
                    //the eye location, without any conversions into screen coordinates
                    //Console.WriteLine("x: " + xIn +" y: " + yIn + " scene: " + sceneNum);
                    for (int i = 0; i < tempItems.Length; i++)
                    {
                        Console.Write(tempItems.GetValue(i)+" ");
                    }
                    Console.WriteLine();
                    EyeTrackerUpdate(sceneNum, xIn, yIn);
                }
                
            }

            eyePosReadTimer.Start();
            
        }
    }
}
