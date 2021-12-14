using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArchToolkit.Utils;
using ArchToolkit.Character;
using System;

namespace ArchToolkit.VR
{

    public class ArchGazeCanvas : MonoBehaviour
    {
        public bool influenceScale;

        private Vector3 startScale = Vector3.one;
        private Quaternion targetRotation = Quaternion.identity;
        public Image image;
        private bool Initialized = false;
        public LineRenderer line;
        private InputRaycaster raycaster;
        void Awake()
        {
            Initialized = false;

            this.startScale = this.transform.localScale;
            this.line = this.GetComponent<LineRenderer>();
            this.raycaster = GameObject.FindObjectOfType<InputRaycaster>();
            if (this.raycaster != null)
            {
                this.raycaster.OnHover += this.OnHover;
                this.raycaster.OnRayCastHit += this.OnRaycastHit;
                this.raycaster.OnExitSensibleObject += this.OnExitSensibleObject;
                this.raycaster.OnClick += this.OnClick;
                this.raycaster.OnRaycastNull += this.OnRaycastNull;
                this.raycaster.OnHoverFloor += this.OnHoverFloor;
            }
        }

        private void OnRaycastHit(Ray ray, RaycastHit hit)
        {

            SetLine(ray.origin, hit.point);
            this.SetTransform(hit.point + hit.normal * 0.01f, hit.normal, hit.distance);
        }

        private void OnRaycastNull(Ray ray)
        {
            SetLine(ray.origin, ray.origin + ray.direction * 10);
        }
        private void OnClick(Transform obj)
        {

        }
        private void OnExitSensibleObject(RaycastHit obj)
        {

        }
        private void OnHoverFloor(RaycastHit hit)
        {
            
            this.SetTransform(hit.point + hit.normal * 0.05f, hit.normal, hit.distance);
        }
        private void OnHover(Ray ray, RaycastHit hit)
        {
            SetLine(ray.origin, hit.point);
            this.SetTransform(hit.point + hit.normal * 0.05f, hit.normal, hit.distance);
        }
        protected virtual void SetLine(Vector3 start, Vector3 end)
        {
            if (!this.raycaster.UseTimer && this.line != null)
            {
                this.line.SetPosition(0, start);
                this.line.SetPosition(1, end);
            }
        }
        void Update()
        {
            if (!Initialized)
            {
                this.TryInit();
            }
            else
            {
                if (this.transform.rotation != targetRotation)
                {
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 10f);
                }
            }

        }

        void TryInit()
        {
            if (ArchToolkitManager.Instance.movementTypePerPlatform == MovementTypePerPlatform.VR)
            {
                if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.ConnectedHeadset != null)
                {
                    if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.ConnectedHeadset.isMobileVR)
                    {
                        this.line.enabled = false;
                        Initialized = true;
                    }
                    else
                    {
                        Initialized = true;
                    }
                }
                /*
                if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.ConnectedDevice != ConnectedDevice.Not_Established)
                {
                    if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.ConnectedDevice == ConnectedDevice.Mobile_Cardboard)
                    {
                        this.line.enabled = false;
                        Initialized = true;
                    }
                    else
                    {
                        Initialized = true;
                    }
                }
                */
            }
            else
            {
                this.Initialized = true;
            }
        }

        public void SetTransform(Vector3 hitPosition, Vector3 hitNormal, float distance)
        {
            this.transform.position = hitPosition;
            this.targetRotation = Quaternion.FromToRotation(Vector3.forward, hitNormal);
            //if (this.influenceScale)
            //    this.transform.localScale = startScale * distance;
        }
    }
}
