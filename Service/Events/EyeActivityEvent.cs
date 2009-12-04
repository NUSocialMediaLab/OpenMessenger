using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OpenMessenger.Events
{

    /// <summary>
    /// Event representing information about eye location as received from EyeTrackerSensor and processed by
    /// EyeActivityMonitor
    /// 
    /// Refer to OmniWindow.OmniWindowPos inner class for detailed explanations of the fields
    /// </summary>
    [DataContract]
    public class EyeActivityEvent : Event
    {
        private byte sceneNum;
        private int xPx;
        private int yPx;
        private float xIn;
        private float yIn;
        //private Contact avatarHit;
        private string avatarHitName;

        [DataMember]
        public byte SceneNum
        {
            get { return sceneNum; }
            set { sceneNum = value; }
        }

        [DataMember]
        public int XPx
        {
            get { return xPx; }
            set { xPx = value; }
        }

        [DataMember]
        public int YPx
        {
            get { return yPx; }
            set { yPx = value; }
        }

        [DataMember]
        public float XIn
        {
            get { return xIn; }
            set { xIn = value; }
        }

        [DataMember]
        public float YIn
        {
            get { return yIn; }
            set { yIn = value; }
        }

        /*
        [DataMember]
        public Contact AvatarHit
        {
            get { return avatarHit; }
            set { avatarHit = value; }
        }
        */

        /// <summary>
        /// A string representing the name of the avatar that was matched on the OmniWindow
        /// </summary>
        [DataMember]
        public string AvatarHitName
        {
            get { return avatarHitName; }
            set { avatarHitName = value; }
        }


        //FIXME: Which focus level to use?
        public EyeActivityEvent(Guid sender, byte sceneNum, float xIn, float yIn, int xPx, int yPx, string avatarHitName /*Contact avatarHit*/)
            : base(sender, 2f)
        {
            this.sceneNum = sceneNum;
            this.xIn = xIn;
            this.yIn = yIn;
            this.xPx = xPx;
            this.yPx = yPx;
            //this.avatarHit = avatarHit;
            this.avatarHitName = avatarHitName;
        }

        /// <summary>
        /// Textual representation of the eye activity event
        /// </summary>
        public override string ToString()
        {
            return avatarHitName;

            //return "Eye Activity. Scene number: " + sceneNum + "  X: " + xPx + " px  Y: " + yPx + " px  Avatar Hit: " + avatarHit==null ? "None" : avatarHit.Name;
            //return "Eye Activity. Scene number: " + sceneNum + "  X: " + xPx + " px  Y: " + yPx + " px  Avatar Hit: " + avatarHitName;
        }
    }
}
