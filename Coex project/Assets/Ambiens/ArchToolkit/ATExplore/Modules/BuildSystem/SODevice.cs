using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit.BuildSystem
{
    [CreateAssetMenu(fileName = "Device", menuName = "ArchToolkit/AT+Explore/Device")]
    public class SODevice : ScriptableObject
    {
        public string Name;

        public List<string> DeviceID;


        public bool isVR;
        public bool RoomScale;
        public bool isMobileVR;
        public bool addHeightDifference = false;

        public GameObject LeftHand;
        public GameObject RightHand;

        
    }
}