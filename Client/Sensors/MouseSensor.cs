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

    class MouseSensor : Sensor
    {
        public MouseSensor()
        {
            mouseReadTimer = new System.Timers.Timer(updateFreq);
            mouseReadTimer.Elapsed += new System.Timers.ElapsedEventHandler(ReadMouse);

        }
        private System.Timers.Timer mouseReadTimer;

        private int updateFreq = 500;



        public override string Text
        {
            get { return "MouseSensor"; }
        }

        public override void Start()
        {
            mouseReadTimer.Start();
        }

        public override void Stop()
        {
            mouseReadTimer.Stop();
            
        }

        private void ReadMouse(object sender, System.Timers.ElapsedEventArgs e)
            {
                Console.WriteLine("ReadMouse");
                mouseReadTimer.Stop();

                if (MouseUpdate != null)
                {
                    Console.WriteLine("MouseUpdate not null");
                    Array tempItems = null;
                    int count;
                    bool available = true;
                    try
                    {
                        //reader.GetScaledData(out tempItems, out count, out available);
                    }
                    catch (COMException)
                    {
                        string errorMsg;
                        //reader.GetLastError(out errorMsg);
                        //FIXME: This should go to the event log instead.
                        //Console.WriteLine("Error reading data from eye tracker: " + errorMsg);
                    }

                    if (available)
                    {

                        Point p = System.Windows.Forms.Control.MousePosition;
                        
                        byte sceneNum = 1;//(byte)tempItems.GetValue(sceneNumIndex);
                        Single xIn = p.X;//(Single)tempItems.GetValue(xPosIndex);
                        Single yIn = p.Y;//(Single)tempItems.GetValue(yPosIndex);
                        //Raise event that the EyeActivityMonitor listens for. It includes the raw readings of
                        //the eye location, without any conversions into screen coordinates
                        Console.WriteLine("x: " + xIn +" y: " + yIn + " scene: " + sceneNum);
                        Console.WriteLine();
                        MouseUpdate(sceneNum, xIn, yIn);
                    }

                }

                mouseReadTimer.Start();
            }




             //<summary>
             //Delegate for the EyeTrackerUpdate event
             //</summary>
             //<param name="screenshot"></param>
            public delegate void MouseUpdateHandler(byte sceneNum, float xIn, float yIn);

             //<summary>
             //Triggered when new data from the eye tracker is received
             //</summary>
            public event MouseUpdateHandler MouseUpdate;
        //
    }
}
