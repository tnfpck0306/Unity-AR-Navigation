
using ArchToolkit.BuildSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_WEBGL && !UNITY_STANDALONE_OSX && IS_LEGACY_INPUT_INSTALLED
using UnityEngine.SpatialTracking;
#endif
using UnityEngine.XR;

namespace ArchToolkit.VR
{

    public class ArchToolkitVRManager : MonoBehaviour
    {

        private SODevice _connectedHeadset;
        public SODevice ConnectedHeadset
        {
            get
            {
                return this._connectedHeadset;
            }
            protected set
            {
                this._connectedHeadset = value;

                if (this._connectedHeadset != null)
                {
                    if (this.OnHeadsetConnected != null)
                        this.OnHeadsetConnected(this._connectedHeadset);
                }
            }
        }
        public Action<SODevice> OnHeadsetConnected;



        public bool isDeviceConnected;

        public bool isVrRunning;

        public ArchVRControllerBase leftController;

        public ArchVRControllerBase rightController;

        public bool isTrackingAcquired = false;

        // public List<string> supportedDevices = new List<string>();

        public bool HasRightHand()
        {
            return this.rightController != null && this.rightController.TrackingAcquired;
        }

        protected List<string> sdkSupported = new List<string> {
#if UNITY_ANDROID || UNITY_IOS
            "Cardboard",
#else
            "OpenVR","Oculus" 
#endif

            };

        private int sdkLoadedIndex = 0;

        protected virtual void Awake()
        {
            InputTracking.trackingLost += InputTracking_trackingLost;
            InputTracking.trackingAcquired += InputTracking_trackingAcquired;

            ArchToolkitManager.Instance.OnVisitorCreated += this.InitializeVR;
        }

        public virtual void InitializeVR()
        {
#if ATE_STEAMVR
            Valve.VR.SteamVR.Initialize();
#endif

            StartCoroutine(this.SetDevice());

        }

        protected virtual void OnDestroy()
        {
            InputTracking.trackingLost -= InputTracking_trackingLost;

            InputTracking.trackingAcquired -= InputTracking_trackingAcquired;

            //this.joysticksConnected.Clear();

            if (ArchToolkitManager.IsInstanced())
                ArchToolkitManager.Instance.OnVisitorCreated -= this.InitializeVR;

            this.StopCoroutine(this.SetDevice());
        }

        protected virtual void OnApplicationQuit()
        {
            InputTracking.trackingLost -= InputTracking_trackingLost;

            InputTracking.trackingAcquired -= InputTracking_trackingAcquired;
        }

        protected virtual void InputTracking_trackingAcquired(XRNodeState obj)
        {

            Debug.Log("Tracking Acquired " + obj.nodeType);

            if (obj.nodeType == XRNode.Head)
                this.isTrackingAcquired = true;
            if (obj.nodeType == XRNode.LeftHand)
            {
                if (this.leftController != null)
                    this.leftController.TrackingAcquired = true;
            }
            if (obj.nodeType == XRNode.RightHand)
            {
                if (this.rightController != null)
                    this.rightController.TrackingAcquired = true;
            }
        }

        protected virtual void InputTracking_trackingLost(XRNodeState obj)
        {
            if (obj.nodeType == XRNode.Head)
                this.isTrackingAcquired = false;

            if (obj.nodeType == XRNode.LeftHand)
            {
                if (this.leftController != null)
                    this.leftController.TrackingAcquired = false;
            }
            if (obj.nodeType == XRNode.RightHand)
            {
                if (this.rightController != null)
                    this.rightController.TrackingAcquired = false;
            }
        }

        protected virtual void Update()
        {
            this.isDeviceConnected = isXRDevicePresent();

            this.isVrRunning = XRSettings.isDeviceActive;
        }
        public static string XRDeviceModel()
        {
            var DeviceName = SystemInfo.deviceName+" "+SystemInfo.deviceModel+" ";

#if ATE_STEAMVR
            
            return DeviceName+Valve.VR.SteamVR.instance.hmd_TrackingSystemName;

#elif UNITY_2020_1_OR_NEWER
            var devices = new List<InputDevice>();
            InputDevices.GetDevices(devices);
            
            foreach (var d in devices)
            {
                DeviceName+=" "+d.name;
            }
            return DeviceName;
#else
            return DeviceName+" "+XRDevice.model;
#endif
        }
        public static bool isXRDevicePresent()
        {
#if UNITY_2020_1_OR_NEWER
            var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
            foreach (var xrDisplay in xrDisplaySubsystems)
            {
                if (xrDisplay.running)
                {
                    return true;
                }
            }
            return false;
#else
            return XRDevice.isPresent;
#endif
        }

        public virtual string GetCurrentDevice()
        {
            string device = "None - "+ SystemInfo.deviceName;

            Debug.Log("Device => XRDevice.Model " + XRDeviceModel() +
                " , XRSettings.loadedDeviceName " + XRSettings.loadedDeviceName +
                " , SystemInfo.deviceName " + SystemInfo.deviceName);

            if (isXRDevicePresent())
                device = XRDeviceModel();

            if (XRSettings.loadedDeviceName == "cardboard") // cardboard is identified with loadedDeviceName and not with XRDevice.model
                device = "Cardboard";

            Debug.Log("Device XR is " + device);

#if UNITY_EDITOR
            Debug.Log("Current Device is " + device);
#endif

            return device;
        }


        protected virtual IEnumerator SetDevice()
        {
            while (this.ConnectedHeadset == null)
            {
                var deviceID = this.GetCurrentDevice();
                Debug.Log("device ID " + deviceID);
                SODevice headset = null;
                foreach (var d in ArchToolkitManager.Instance.settings.buildTemplate.Devices)
                {
                    foreach (var dID in d.DeviceID)
                    {
                        if (deviceID.ToLower().Contains(dID.ToLower()) || dID.ToLower().Contains(deviceID.ToLower()))
                        {
                            Debug.Log("Device Found " + d.name);
                            headset = d;
                        }
                    }
                }
                //var headset=ArchToolkitManager.Instance.settings.buildTemplate.Devices.Find(d => d.DeviceID.Contains(deviceID) );

                if (headset != null)
                {
                    this.ConnectedHeadset = headset;
#if !UNITY_2020_1_OR_NEWER
                    if (this.ConnectedHeadset.RoomScale)
                    {
                        XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
                    }
#endif
                }
                else
                {
                    this.ConnectedHeadset = null;

                    if (this.sdkLoadedIndex < this.sdkSupported.Count)
                    {
                        var sdk = this.sdkSupported[this.sdkLoadedIndex];

                        yield return LoadDevice(sdk, true);

                        sdkLoadedIndex++;
                    }
                    else this.sdkLoadedIndex = 0;
                }

                yield return new WaitForEndOfFrame();
            }
        }


        public virtual IEnumerator LoadDevice(string newDevice, bool enable)
        {
            Debug.Log("Loading Device " + newDevice);
#if UNITY_2020_1_OR_NEWER 
            //XRSettings.LoadDeviceByName(newDevice);
            List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
            SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);
            for (int i = 0; i < subsystems.Count; i++)
            {
                subsystems[i].TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
            }
#else
            XRSettings.LoadDeviceByName(newDevice);
#endif
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
#if UNITY_2020_1_OR_NEWER
            
#else
            XRSettings.enabled = enable;
#endif


            Debug.Log("Loaded Device " + XRDeviceModel());

        }


    }


}
