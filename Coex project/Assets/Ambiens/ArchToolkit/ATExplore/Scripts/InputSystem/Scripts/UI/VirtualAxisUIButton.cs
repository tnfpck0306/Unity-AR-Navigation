using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ArchToolkit.InputSystem{
    public class VirtualAxisUIButton : MonoBehaviour
    {
        public InputListener.ATInputNames inputname;
        public float value;
        private float multiplier=0;
        void Update(){
            InputListener.SetVirtualAxis(inputname,multiplier*value);
        }
        public void SendPositiveInput(){
            multiplier=1;
        }
        public void SendNegativeInput(){
            multiplier=-1;
        }
        public void NullInput(){
            multiplier=0;
        }
    }
}

