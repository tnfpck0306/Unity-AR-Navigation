using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArchToolkit.InputSystem
{
    public static class InputListener
    {
        public enum ATInputNames
        {
            Horizontal=0,
            Vertical=1,
            MouseScrollWeel=2,
            MouseX=3,
            MouseY=4,
            TriggerRight=5,
            TriggerLeft=6,
            Fire1 =7,
            Escape=8,

            VR_Horizontal=9,
            VR_Vertical=10,
            
            JOY_MouseX = 11,
            JOY_MouseY = 12,

            Fly = 101,
        }
        static string[] ATInputMap = new string[]
        {
            "AT_Horizontal",
            "AT_Vertical",
            "AT_Mouse ScrollWeel",
            "AT_Mouse X",
            "AT_Mouse Y",
            "AT_TriggerRight",
            "AT_TriggerLeft",
            "AT_Fire1",
            "AT_Escape",

            "AT_VR_Horizontal",
            "AT_VR_Vertical",

            "AT_JOY_MouseX",
            "AT_JOY_MouseY",
        };
        
        private static Dictionary<ATInputNames, float> VirtualAxis;

        public static string InputMap(ATInputNames n) { return ATInputMap[(int)n]; }
        public static bool SpacePressed
        {
            get { return Input.GetKeyDown(KeyCode.Space); }
        }

        public static bool HorizontalDown
        {
            get { return Input.GetButtonDown(InputMap(ATInputNames.Horizontal)); }
        }

        public static bool HorizontalHold
        {
            get { return Input.GetKey(InputMap(ATInputNames.Horizontal)); }
        }
        public static bool EscapePressed
        {
            get { return Input.GetButtonDown(InputMap(ATInputNames.Escape)); }
        }
        public static bool VerticalDown
        {
            get { return Input.GetKeyDown(InputMap(ATInputNames.Vertical)); }
        }

        public static bool VerticalHold
        {
            get { return Input.GetKey(InputMap(ATInputNames.Vertical)); }
        }

        public static bool MouseButtonLeftUp
        {
            get { return Input.GetMouseButtonUp(0); }
        }

        public static bool MouseButtonRightUp
        {
            get { return Input.GetMouseButtonUp(1); }
        }

        public static bool MouseButtonLeftDown
        {
            get { return (Input.GetMouseButtonDown(0) || Input.GetButtonDown("AT_Fire1")); }
        }

        public static bool MouseButtonLeftHold
        {
            get { return Input.GetMouseButton(0); }
        }

        public static bool MouseButtonRightDown
        {
            get { return Input.GetMouseButtonDown(1); }
        }

        public static bool MouseButtonRightHold
        {
            get { return Input.GetMouseButton(1); }
        }

        public static float MouseWheel
        {
            get { return Input.GetAxis(InputMap(ATInputNames.MouseScrollWeel)); }
        }
        public static bool FlyUp
        {
            get { return GetVirtualAxis(ATInputNames.Fly)>0 || Input.GetKey(KeyCode.Q); }
        }
        public static bool FlyDown
        {
            get { return GetVirtualAxis(ATInputNames.Fly)<0 || Input.GetKey(KeyCode.E);  }
        }
        static float lastTapTime=0;
        public static bool DoubleTap
        {
            get {
                if (IsPointerOverUIObject()) return false;
                
                if(lastTapTime==0) {
                    if(Input.GetMouseButtonUp(0) || (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Ended ))
                    {
                        lastTapTime=Time.time;
                    }
                    return false;
                }
                else{
                    if(Input.GetMouseButtonUp(0) || (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Ended ))
                    {
                        if(Time.time - lastTapTime<0.3f)
                            return true;
                        else lastTapTime=Time.time;
                    }
                }
                return false;
            
            }
        }
        public static float MouseX
        {
            get
            {

                if (Input.GetAxis(InputMap(ATInputNames.JOY_MouseX)) != 0)
                {
                    return Input.GetAxis(InputMap(ATInputNames.JOY_MouseX)) * ArchToolkitManager.Instance.settings.MouseRotationSpeed;
                }
                if (IsPointerOverUIObject()) return 0;
                if (Input.touches.Length > 0 )
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                        return Input.GetTouch(0).deltaPosition.x*ArchToolkitManager.Instance.settings.TouchRotationSpeed;
                    if (Input.touches.Length > 1)
                    {
                        if (Input.GetTouch(1).phase == TouchPhase.Moved)
                            return Input.GetTouch(1).deltaPosition.x*ArchToolkitManager.Instance.settings.TouchRotationSpeed;
                    }
                }
                else
                {
                    if (ArchToolkitManager.Instance.settings.UseRightClickToRotate)
                    {
                        if (Input.GetMouseButton(1))
                        {
                            return Input.GetAxis(InputMap(ATInputNames.MouseX)) * ArchToolkitManager.Instance.settings.MouseRotationSpeed;
                        }
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        return Input.GetAxis(InputMap(ATInputNames.MouseX)) * ArchToolkitManager.Instance.settings.MouseRotationSpeed;
                    }
                }
                /*else if (Input.GetMouseButton(0))
                {
                    return Input.GetAxis(InputMap(ATInputNames.MouseX))*ArchToolkitManager.Instance.settings.MouseRotationSpeed;
                }*/
                
                return 0;
            }
        }

        public static float MouseY
        {
            get
            {
                if (Input.GetAxis(InputMap(ATInputNames.JOY_MouseY)) != 0)
                {
                    return Input.GetAxis(InputMap(ATInputNames.JOY_MouseY)) * ArchToolkitManager.Instance.settings.MouseRotationSpeed;
                }
                if (IsPointerOverUIObject()) return 0;
                if (Input.touches.Length > 0)
                {
                    if(Input.GetTouch(0).phase == TouchPhase.Moved)
                        return Input.GetTouch(0).deltaPosition.y*ArchToolkitManager.Instance.settings.TouchRotationSpeed;
                    if (Input.touches.Length > 1)
                    {
                        if (Input.GetTouch(1).phase == TouchPhase.Moved)
                            return Input.GetTouch(1).deltaPosition.y*ArchToolkitManager.Instance.settings.TouchRotationSpeed;
                    }
                }
                else
                {
                    if (ArchToolkitManager.Instance.settings.UseRightClickToRotate)
                    {
                        if (Input.GetMouseButton(1))
                        {
                            return Input.GetAxis(InputMap(ATInputNames.MouseY)) * ArchToolkitManager.Instance.settings.MouseRotationSpeed;
                        }
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        return Input.GetAxis(InputMap(ATInputNames.MouseY)) * ArchToolkitManager.Instance.settings.MouseRotationSpeed;
                    }
                }
                /*else if (Input.GetMouseButton(0))
                {
                    return Input.GetAxis(InputMap(ATInputNames.MouseY))*ArchToolkitManager.Instance.settings.MouseRotationSpeed;
                }*/
                
                return 0;
            }
        }
        public static float JoyMouseX
        {
            get
            {
                return Input.GetAxis(InputMap(ATInputNames.JOY_MouseX)) * ArchToolkitManager.Instance.settings.MouseRotationSpeed;

            }
        }
        public static float JoyMouseY
        {
            get
            {
                return Input.GetAxis(InputMap(ATInputNames.JOY_MouseY)) * ArchToolkitManager.Instance.settings.MouseRotationSpeed;
                
            }
        }
        public static bool MovementFromJoyPad()
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                foreach(var s in Input.GetJoystickNames())
                    if (s.ToLower().Contains("xbox"))
                    {
                        return (Input.GetAxis(InputMap(ATInputNames.MouseY)) == 0) && (Input.GetAxis(InputMap(ATInputNames.MouseX)) == 0);
                    }
            }
            return false;
        }

        public static float TriggerRight
        {
            get {
#if ATE_STEAMVR
                if (Valve.VR.SteamVR_Actions._default.InteractUI.GetStateUp(Valve.VR.SteamVR_Input_Sources.RightHand)) return 1;
#endif
                return Input.GetAxis(InputMap(ATInputNames.TriggerRight));
            }
        }

        public static float TriggerLeft
        {
            get
            {
#if ATE_STEAMVR
                if (Valve.VR.SteamVR_Actions._default.InteractUI.GetStateUp(Valve.VR.SteamVR_Input_Sources.LeftHand) ) return 1;
#endif
                return Input.GetAxis(InputMap(ATInputNames.TriggerLeft));
            }
        }
        public static float VRHorizontalAxis
        {
            get
            {
                return GetVirtualAxis(ATInputNames.VR_Horizontal) ;
            }

        }
        public static float HorizontalAxis
        {
            get
            {
                return GetVirtualAxis(ATInputNames.Horizontal) + Input.GetAxis(InputMap(ATInputNames.Horizontal)) ;
            }

        }

        public static float VerticalAxis
        {
            get
            {   
                return GetVirtualAxis(ATInputNames.Vertical) + Input.GetAxis(InputMap(ATInputNames.Vertical)) + Input.GetAxis(InputMap(ATInputNames.MouseScrollWeel));
            }
        }

        private static float GetVirtualAxis(ATInputNames name){
            if(VirtualAxis==null) VirtualAxis= new Dictionary<ATInputNames, float>();
            if(!VirtualAxis.ContainsKey(name)) return 0;
            return VirtualAxis[name];
        }
        public static void SetVirtualAxis(ATInputNames name, float val){
            if(VirtualAxis==null) VirtualAxis= new Dictionary<ATInputNames, float>();
            if(!VirtualAxis.ContainsKey(name)) VirtualAxis.Add(name,val);
            VirtualAxis[name]=val;
        }
        private static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            if (EventSystem.current == null)
            {
                EventSystem.current = GameObject.FindObjectOfType<EventSystem>();
            }
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
