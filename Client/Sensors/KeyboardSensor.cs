using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.DirectX.DirectInput;
using System.Threading;


namespace OpenMessenger.Client.Sensors
{
    /// <summary>
    /// Sensor that captures global keyboard input at intervals specifed
    /// by updateFrequency
    /// </summary>
    public class KeyboardSensor : Sensor
    {
        /// <summary>
        /// Delegate for the KeyboardUpdate event
        /// </summary>
        /// <param name="keys">The list of keys pressed</param>
        public delegate void KeyboardUpdateHandler(Key[] keys);

        /// <summary>
        /// Triggered when the timer goes off -- sends out the keyboard data
        /// since the last time the event fired
        /// </summary>
        public event KeyboardUpdateHandler KeyboardUpdate;
         
        private const int updateFrequency = 5000;
        private System.Timers.Timer captureTimer;

        private Device keyboard;
        /// <summary>
        /// List of keys pressed
        /// </summary>
        private List<Key> state;

        /// <summary>
        /// Event for capturing keyboard data in real time
        /// </summary>
        private EventWaitHandle waitHandle;
        /// <summary>
        /// Thread that handles capturing the data, but not sending it off
        /// </summary>
        private Thread listeningThread;

        /// <summary>
        /// Name of this sensor
        /// </summary>
        public override string Text
        {
            get { return "Keyboard"; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
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

        /// <summary>
        /// Method that the listeningThread runs in to capture keyboardInput
        /// </summary>
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

        /// <summary>
        /// Starts the timer and the thread to capture input
        /// </summary>
        public override void Start()
        {
            keyboard.Acquire();
            captureTimer.Start();
            listeningThread = new Thread(new ThreadStart(ListenToKeyboard));
            listeningThread.Start();
        }

        /// <summary>
        /// Stops the thread and the timer
        /// </summary>
        public override void Stop()
        {
            listeningThread.Abort();
            captureTimer.Stop();
            keyboard.Unacquire();
        }

        /// <summary>
        /// Method called when the timer expires, sends out event
        /// with keyboard data
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
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

        /// <summary>
        /// Tells the operating system we no longer want to listen to the keyboard
        /// </summary>
        public void Dispose()
        {
            keyboard.Unacquire();
        }
    }
}
