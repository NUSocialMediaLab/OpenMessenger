using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.DirectX.DirectInput;
using System.Threading;


namespace OpenMessenger.Client.Sensors
{
    public class KeyboardSensor : Sensor
    {
        public delegate void KeyboardUpdateHandler(Key[] keys);

        public event KeyboardUpdateHandler KeyboardUpdate;
         
        private const int updateFrequency = 5000;
        private System.Timers.Timer captureTimer;

        private Device keyboard;
        private List<Key> state;

        private EventWaitHandle waitHandle;
        private Thread listeningThread;

        public override string Text
        {
            get { return "Keyboard"; }
        }

        public KeyboardSensor()
        {
            state = new List<Key>();

            captureTimer = new System.Timers.Timer(updateFrequency);
            captureTimer.Elapsed += new ElapsedEventHandler(CaptureKeys);

            keyboard = new Device(SystemGuid.Keyboard);
            if (keyboard == null)
                throw new Exception("No keyboard found.");

            keyboard.SetCooperativeLevel(null, CooperativeLevelFlags.NonExclusive |
                CooperativeLevelFlags.Background);
            keyboard.SetDataFormat(DeviceDataFormat.Keyboard);
            waitHandle = new EventWaitHandle
                (false, System.Threading.EventResetMode.AutoReset);
            keyboard.SetEventNotification(waitHandle);
        }

        private void ListenToKeyboard()
        {
            while (captureTimer.Enabled)
            {
                waitHandle.WaitOne();
                lock (state){
                    foreach(Key k in keyboard.GetPressedKeys())
                    {
                        state.Add(k);
                    }
                }
            }
        }

        public override void Start()
        {
            keyboard.Acquire();
            captureTimer.Start();
            listeningThread = new Thread(new ThreadStart(ListenToKeyboard));
            listeningThread.Start();
        }

        public override void Stop()
        {
            listeningThread.Abort();
            captureTimer.Stop();
            keyboard.Unacquire();
        }

        private void CaptureKeys(object sender, ElapsedEventArgs e)
        {
            lock (state)
            {
                if (KeyboardUpdate != null)
                {
                    KeyboardUpdate(state.ToArray());
                    state.Clear();
                }
            }
        }

        public void Dispose()
        {
            keyboard.Unacquire();
        }
    }
}
