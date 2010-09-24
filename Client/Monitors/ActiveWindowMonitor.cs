using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMessenger.Client.Monitors
{
    class ActiveWindowMonitor : Monitor
    {
        public ActiveWindowMonitor()
        { }

        public override void Start()
        {
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        private void OnActiveWindowUpdateHandler(string title)
        { }

        public override string Text
        {
            get { return "Active Window"; }
        }
    }
}
