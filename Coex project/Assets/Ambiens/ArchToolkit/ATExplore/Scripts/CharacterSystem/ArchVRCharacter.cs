using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ArchToolkit.InputSystem;
using ArchToolkit.VR;

namespace ArchToolkit.Character
{

    public class ArchVRCharacter : ArchCharacter
    {

        public event Action<bool> OnCheckVRInteraction;

        protected override void Awake()
        {
            this.SetValues();

            this.onHeightDifferenceChange += this.onVRHeightDifferenceChange;
            raycaster.OnHoverFloor += this.onHoverFloor;
            string deviceName = UnityEngine.XR.XRSettings.loadedDeviceName.ToLower();
#if !UNITY_2020_1_OR_NEWER
            string deviceModel = UnityEngine.XR.XRDevice.model.ToLower();
            Debug.Log(deviceName + " " + deviceModel);
#else
            Debug.Log(deviceName );
#endif


        }

        private void onVRHeightDifferenceChange(float height)
        {
            this.CheckGround((RaycastHit hit) => {
                this.transform.position = new Vector3(hit.point.x, hit.point.y + this.HeightDifference, hit.point.z);
            }, this.transform);
        }

        protected override void Start()
        {
            this.DefaultRaycastTool = new DefaultRaycastTool();
        }

        protected override void SetValues()
        {
            base.SetValues();

        }
        private bool heightInitialized = false;

        void InitHeight()
        {
            if (heightInitialized) return;
            if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.ConnectedHeadset == null) return;

            if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.ConnectedHeadset.addHeightDifference)
            {
                this.HeightDifference = 1.6f;
            }
            else
            {
                this.___heightDifference = 0;
#if UNITY_2020_1_OR_NEWER
                List<UnityEngine.XR.XRInputSubsystem> subsystems = new List<UnityEngine.XR.XRInputSubsystem>();
                SubsystemManager.GetInstances(subsystems);

                foreach (var subsystem in subsystems)
                {
                    if (subsystem.TrySetTrackingOriginMode(UnityEngine.XR.TrackingOriginModeFlags.Floor))
                    {

                    }
                }
#endif
            }
            heightInitialized = true ;
        }

        protected override void Update()
        {
            if (this.Head == null)
            {
                Debug.LogError("Null Head");
                return;
            }
            this.InitHeight();

            this.CurrentRaycastTool.Update(Time.deltaTime);

            if (ArchToolkitManager.Instance.settings.EnableVRSmoothMovement)
                this.ClassicMovement();

        }

        protected override void ClassicMovement()
        {

            base.ClassicMovement();
        }

    }
}

