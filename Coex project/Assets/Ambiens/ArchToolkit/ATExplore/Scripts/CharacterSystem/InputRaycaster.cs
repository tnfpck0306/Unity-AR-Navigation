using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ArchToolkit.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ArchToolkit.Character
{

    public class InputRaycaster : MonoBehaviour
    {
        public Camera currentCamera;

        public RaycastHit hit;
        public List<RaycastHit> hits;
        public Ray rayOrigin;

        public float TimeWatched
        {
            get
            {
                return this.timeWatched;
            }
        }

        public float maxTimer;

        public event Action<Transform> OnClick;
        public event Action OnTimerClickOnFloor;//TODO-> refactoring needed


        public event Action<Ray, RaycastHit> OnHover;
        public event Action<RaycastHit> OnHoverFloor;
        public event Action<RaycastHit> OnExitSensibleObject;
        public event Action<Ray ,RaycastHit> OnRayCastHit;

        public event Action<Ray> OnRaycastNull;

        public bool isPointerOverUI;

        private float timeWatched;

        private float timeBetweenClicks = 0.5f;
        private float timeSinceLastClick = 0;

        private Transform _latestClickTransform;
        public Transform LatestClickTransform{get{return _latestClickTransform;}}

        public GraphicRaycaster gRaycaster;

        private void Update()
        {
            this.maxTimer = ArchToolkitManager.Instance.settings.maxGazeTimer;

            this.isPointerOverUI = this.IsPointerOverUIObject();

            if (this.isPointerOverUI)
                return;

            if (ArchToolkitManager.Instance.movementTypePerPlatform == MovementTypePerPlatform.VR )
            {
                //We automatically use the timer if there's no controller!

                if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.HasRightHand())
                {
                    if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.rightController.rayOrigin != null)
                    {
                        this.rayOrigin = new Ray(ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.rightController.rayOrigin.transform.position,
                                                  ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.rightController.rayOrigin.transform.forward);

                        this.CheckRaycast(this.rayOrigin);
                    }
                }
                else{
                    this.rayOrigin = new Ray(this.currentCamera.transform.position, this.currentCamera.transform.forward);
                    this.CheckRaycast(this.rayOrigin);
                }
            }
            //NEW! v1.3
            else if (ArchToolkitManager.Instance.movementTypePerPlatform == MovementTypePerPlatform.AR)
            {

                this.rayOrigin = this.currentCamera.ScreenPointToRay(Input.mousePosition);
                this.CheckRaycast(this.rayOrigin);

            }
            else if (InputListener.MovementFromJoyPad())
            {
                this.rayOrigin = new Ray(this.currentCamera.transform.position, this.currentCamera.transform.forward);
                this.CheckRaycast(this.rayOrigin);
            }
            else{
                this.rayOrigin = this.currentCamera.ScreenPointToRay(Input.mousePosition);
                this.CheckRaycast(this.rayOrigin);
            }
        }

        public bool UseTimer
        {
            get
            {
                if(ArchToolkitManager.Instance.movementTypePerPlatform== MovementTypePerPlatform.VR)
                    return !ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.HasRightHand();
                return false;
            }
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            if (EventSystem.current == null)
            {
                EventSystem.current = GameObject.FindObjectOfType<EventSystem>();
            }
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count > 0)
                results.RemoveAll(r => r.gameObject.GetComponent<IgnoreUI>());

            return results.Count > 0;
        }

        private bool IsSensibleObject(GameObject gameObject)
        {
            return gameObject.CompareTag(ArchToolkitDataPaths.ARCHINTERACTABLETAG);
        }
        Vector3 TimerLastPositionForTeleport;
        private UnityEngine.Object CheckRaycast(Ray ray)
        {   
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.transform != null && hit.transform.gameObject != null)
                {
                    if(this.OnRayCastHit!=null) this.OnRayCastHit(ray, hit);

                    if(Utils.ArchToolkitProgrammingUtils.CanTeleport(hit,45f))
                    {
                        if (this.UseTimer)
                        {
                            
                            if (TimerLastPositionForTeleport.magnitude == 0) 
                            {
                                TimerLastPositionForTeleport = hit.point;
                            }
                            else if((TimerLastPositionForTeleport - hit.point).magnitude < 0.5f)
                            {
                                
                                if (this.timeWatched > this.maxTimer)
                                {
                                    _latestClickTransform = hit.transform;

                                    if (this.OnTimerClickOnFloor != null)
                                        this.OnTimerClickOnFloor();

                                    this.timeWatched = 0;
                                }
                                else
                                    this.timeWatched += Time.deltaTime;
                            }
                            else
                            {
                                TimerLastPositionForTeleport = hit.point;
                                this.timeWatched = 0;
                            }
                        }
                        else
                        {

                        }
                        if(this.OnHoverFloor!=null) this.OnHoverFloor(hit);
                    }
                    else if (this.IsSensibleObject(hit.transform.gameObject)) // if the object is marked as interactable
                    {
                        TimerLastPositionForTeleport = Vector3.zero;

                        if (this.OnHover != null)
                            this.OnHover(ray, hit);
                       
                        if (ArchToolkitManager.Instance.movementTypePerPlatform == MovementTypePerPlatform.VR 
                            && 
                            ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.HasRightHand())
                        {
                            ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.rightController.OnTriggerPressed(() =>
                            {
                                if (timeSinceLastClick == 0 || Time.time >= timeSinceLastClick + timeBetweenClicks)
                                {
                                    timeSinceLastClick = Time.time;
                                    _latestClickTransform=hit.transform;
                                    if (this.OnClick != null)
                                        this.OnClick(hit.transform);
                                }

                            });
                        }
                        else
                        {
                            if (this.UseTimer) // If input depend on timer
                            {
                                if (this.timeWatched > this.maxTimer)
                                {
                                    _latestClickTransform=hit.transform;

                                    if (this.OnClick != null)
                                        this.OnClick(hit.transform);

                                    this.timeWatched = 0;
                                }
                                else
                                    this.timeWatched += Time.deltaTime;
                            }
                            else
                            {
                                if (InputListener.MouseButtonLeftDown)
                                {
                                     _latestClickTransform=hit.transform;
                                    if (this.OnClick != null)
                                        this.OnClick(hit.transform);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.OnExitSensibleObject != null)
                            this.OnExitSensibleObject(hit);
                        TimerLastPositionForTeleport = Vector3.zero;

                        this.timeWatched = 0;
                    }
                }

            }
            else
            {
                if (this.OnRaycastNull != null)
                    this.OnRaycastNull(ray);
                
                this.timeWatched = 0;
            }
            return null;
        }

    }

}
