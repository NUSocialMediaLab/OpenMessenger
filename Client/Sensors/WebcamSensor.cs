using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace OpenMessenger.Client.Sensors
{
    /// <summary>
    /// Sensor for webcams. Relies on the avicap interface. Samples frames at an interval specified
    /// by captureInterval. Triggers the CameraFrameUpdate with the newest frame.
    /// </summary>
    public class WebcamSensor : Sensor
    {
        #region AviCap32 Interop

        const short WM_CAP = 0x400;
        const int WM_CAP_CONNECT = 1034;
        const int WM_CAP_DISCONNECT = 1035;
        const int WM_CAP_DRIVER_CONNECT = 0x40a;
        const int WM_CAP_DRIVER_DISCONNECT = 0x40b;
        const int WM_CAP_EDIT_COPY = 0x41e;
        const int WM_CAP_SET_PREVIEW = 0x432;
        const int WM_CAP_SET_OVERLAY = 0x433;
        const int WM_CAP_SET_PREVIEWRATE = 0x434;
        const int WM_CAP_SET_SCALE = 0x435;
        const int WM_CAP_GET_FRAME = 1084;
		const int WM_CAP_COPY = 1054;
        const int WS_CHILD = 0x40000000;
        const int WS_VISIBLE = 0x10000000;

        [DllImport("avicap32.dll")]
        protected static extern int capCreateCaptureWindowA([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszWindowName,
            int dwStyle, int x, int y, int nWidth, int nHeight, int hWndParent, int nID);

        [DllImport("user32", EntryPoint = "SendMessageA")]
        protected static extern int SendMessage(int hwnd, int wMsg, int wParam, [MarshalAs(UnmanagedType.AsAny)] object lParam);

        [DllImport("user32")]
        protected static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32")]
        protected static extern bool DestroyWindow(int hwnd);

        [DllImport("user32", EntryPoint = "OpenClipboard")]
		public static extern int OpenClipboard(int hWnd);

		[DllImport("user32", EntryPoint = "EmptyClipboard")]
		public static extern int EmptyClipboard();

		[DllImport("user32", EntryPoint = "CloseClipboard")]
		public static extern int CloseClipboard();

        #endregion

        /// <summary>
        /// Delegate for the CameraFrameUpdate event
        /// </summary>
        /// 
        /// <param name="frame">Frame bitmap sampeled</param>
        public delegate void CameraFrameUpdateHandler(Bitmap frame);

        /// <summary>
        /// Triggered when a new camera frame has been sampled
        /// </summary>
        public event CameraFrameUpdateHandler CameraFrameUpdate;

        /// <summary>
        /// Name of this sensor
        /// </summary>
        public override String Text { get { return "Webcam"; } }

        Size resolution = new Size(400, 300);
        int captureInterval = 2000;
        Timer captureTimer;
        int deviceIndex;
        int hDevice;

        /**
         * We need some kind of window handle to work with AviCap. We need not have
         * it visible though.
         */
        System.Windows.Forms.Form wndCapture = new System.Windows.Forms.Form();

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebcamSensor()
        {
        }

        /// <summary>
        /// Starts the sensor, fires up the camera and starts sampling at intervals
        /// </summary>
        public override void Start()
        {
            StartCamera();

            captureTimer = new Timer();
            captureTimer.Interval = captureInterval;
            captureTimer.Tick += new EventHandler(Capture);
            captureTimer.Start();
        }

        void Capture(object sender, EventArgs e)
        {
            captureTimer.Stop(); 

            if (CameraFrameUpdate != null)
            {
                try
                {
                    Application.DoEvents();
                    SendMessage(hDevice, WM_CAP_GET_FRAME, 0, 0);
                    SendMessage(hDevice, WM_CAP_COPY, 0, 0);

                    IDataObject clipData = Clipboard.GetDataObject();
                    Bitmap frame = (Bitmap)clipData.GetData(DataFormats.Bitmap);

                    frame = (Bitmap)frame.GetThumbnailImage(resolution.Width, resolution.Height,
                        null, System.IntPtr.Zero);

                    CameraFrameUpdate(frame);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error capturing frame from webcam: " + ex);
                    return;
                }
            }

            captureTimer.Start();
        }

        /// <summary>
        /// Stops this sensor, but doesn't shut down the camera
        /// </summary>
        public override void Stop()
        {
            if (captureTimer != null)
                captureTimer.Stop();
        }

        int SelectCamera()
        {
            return 0;
        }

        void StartCamera()
        {
            wndCapture.Size = resolution;

            deviceIndex = SelectCamera();
            string strDeviceIndex = deviceIndex.ToString();
            hDevice = capCreateCaptureWindowA(ref strDeviceIndex, WS_VISIBLE | WS_CHILD, 0, 0, 
                resolution.Width, resolution.Height, wndCapture.Handle.ToInt32(), 0);

            if (SendMessage(hDevice, WM_CAP_DRIVER_CONNECT, deviceIndex, 0) > 0)
            {
                SendMessage(hDevice, WM_CAP_CONNECT, 0, 0);
                SendMessage(hDevice, WM_CAP_SET_PREVIEW, 0, 0);
            }
        }

        void StopCamera()
        {
            Application.DoEvents();
            SendMessage(hDevice, WM_CAP_DISCONNECT, 0, 0);
            DestroyWindow(hDevice);
        }


    }
}
