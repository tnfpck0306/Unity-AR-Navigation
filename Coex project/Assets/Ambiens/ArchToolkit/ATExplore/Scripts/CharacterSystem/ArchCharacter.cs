using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ArchToolkit.InputSystem;
using UnityEngine.XR;
using ArchToolkit.VR;

namespace ArchToolkit.Character
{
    public enum MovementType
    {
        FlyCam,
        Classic
    }

    public enum LockMovementTo
    {
        None,
        FlyCam,
        Classic
    }

    public enum MobileMovementType
    {
        Joystick,
        Swipe
    }

    public class ArchCharacter : ArchCharacterBase
    {
        public Action<MovementType> OnMovementTypeChanged;

        public LockMovementTo LockMovement
        {
            get
            {
                return this._lockMovementTo;
            }

            set
            {
                this._lockMovementTo = value;
            }
        }

        public MovementType MovementType
        {
            get { return this._movementType; }

            set
            {
                this._movementType = value;

                if (this.OnMovementTypeChanged != null)
                    this.OnMovementTypeChanged(this._movementType);

            }
        }

        [Header("Input")]
        public InputRaycaster raycaster;

        [Header("Layer")]
        public LayerMask walkableLayers = 0;

        [Header("Body part")]
        public GameObject Head;

        [Space]
        [Space]

        [Header("Movement Fields")]

        [SerializeField]
        private float RunSpeed;

        [SerializeField]
        private float RunFaster;

        [SerializeField]
        private float movementSpeed;

        [SerializeField]
        private float clumbSpeed;

        public Action<ARaycastTool> OnChangeRaycastTool;
        private ARaycastTool __currentRaycastTool;
        public ARaycastTool CurrentRaycastTool
        {
            get
            {
                if (__currentRaycastTool == null)
                {
                    __currentRaycastTool = this.DefaultRaycastTool;
                    StartCoroutine(this.delayedInitDefaultRayCastTool());
                }
                return __currentRaycastTool;
            }
            set
            {
                if (value != this.__currentRaycastTool)
                {
                    if (this.__currentRaycastTool != null) this.__currentRaycastTool.Disable();
                    this.__currentRaycastTool = value;
                    if (this.__currentRaycastTool != null) this.__currentRaycastTool.Init();

                    if (this.OnChangeRaycastTool != null) this.OnChangeRaycastTool(this.__currentRaycastTool);

                }
            }
        }
        IEnumerator delayedInitDefaultRayCastTool()
        {
            yield return new WaitForEndOfFrame();
            __currentRaycastTool.Init();
        }
        protected ARaycastTool DefaultRaycastTool;
        public void SetRaycastTool(ARaycastTool tool)
        {
            this.CurrentRaycastTool = tool;
        }
        //[SerializeField]
        //private float rotationSpeed;

        [Space]
        [Space]

        [Header("Movement Type")]
        [SerializeField]
        private MovementType _movementType = MovementType.FlyCam; // Default is Flycam
        [SerializeField]
        private MobileMovementType _mobileMovementType = MobileMovementType.Swipe;
        [SerializeField]
        private LockMovementTo _lockMovementTo = LockMovementTo.None; // Default is None
        private float rotationX = 0;
        private float rotationY = 0;
        private float initialSpeed;

        private Vector3 latestPosition = Vector3.zero;

        public Action<float> onHeightDifferenceChange;

        protected float ___heightDifference = 1.6f;
        public float HeightDifference
        {
            get
            {
                return ___heightDifference;
            }
            set
            {
                this.___heightDifference = value;
                if (onHeightDifferenceChange != null) onHeightDifferenceChange(this.___heightDifference);
            }
        }

        private Vector3 switchVectorPosition;
        [SerializeField]
        private Vector3 startingHeadPos;
        private Collider CharacterCollider
        {
            get
            {
                if (__characterCollider == null) __characterCollider = GetComponent<Collider>();
                return __characterCollider;
            }
        }

        private Collider __characterCollider;
        private Rigidbody rb;


        public bool CanMovePosition { get; private set; } = true;

        public void LockPosition()
        {
            if (this.CharacterCollider != null)
            {
                this.CharacterCollider.enabled = false;
            }

            this.CanMovePosition = false;
        }
        public void UnlockPosition()
        {
            if (this.CharacterCollider != null)
            {
                this.CharacterCollider.enabled = true;
            }
            this.CanMovePosition = true;
        }
        public bool CanRotateHead { get; private set; } = true;
        public void LockRotation()
        {
            this.CanRotateHead = false;
        }
        public void UnlockRotation()
        {
            this.CanRotateHead = true;
        }
        protected virtual void Awake()
        {

            this.SetValues();

            this.rb = this.GetComponent<Rigidbody>();

            this.initialSpeed = this.movementSpeed;

            this.startingHeadPos = this.Head.transform.localPosition;

            ArchToolkitManager.Instance.settings.OnExit += this.OnExit;

            raycaster.OnHoverFloor += this.onHoverFloor;
            raycaster.OnRaycastNull += this.OnRaycastNull;
            raycaster.OnHover += this.OnHoverInteractable;
            raycaster.OnExitSensibleObject += this.OnExitSensigleObject;

        }



        List<LayerMask> prevLayer = new List<LayerMask>();
        public void SetVisibleLayer(LayerMask mask)
        {
            prevLayer.Clear();
            var cameras = this.GetComponentsInChildren<Camera>();
            foreach (var c in cameras)
            {
                prevLayer.Add(c.cullingMask);
                c.cullingMask = mask;
            }
        }
        public void ResetVisibleLayers()
        {
            var cameras = this.GetComponentsInChildren<Camera>();
            var i = 0;
            foreach (var c in cameras)
            {
                c.cullingMask = prevLayer[i];
                i++;
            }
        }
        protected override void Start()
        {
            this.OnMovementTypeChanged += this.ResetHeadOnSwitch;
            
            if (ArchToolkitManager.Instance.movementTypePerPlatform == MovementTypePerPlatform.FullScreen360)
            {
                GameObject.Instantiate(Resources.Load<GameObject>(ArchToolkitDataPaths.RESOURCES_HUD_PATH));
            }

            if (ArchToolkitManager.Instance.movementTypePerPlatform != MovementTypePerPlatform.VR)
                this.Head.transform.localRotation = Quaternion.Euler(Vector3.zero);

            this.rotationX = this.transform.eulerAngles.y;
            this.rotationY = this.transform.eulerAngles.x;

            if (this.MovementType == MovementType.Classic)
            {
                if (this.CharacterCollider != null)
                    this.CharacterCollider.enabled = true;
            }
            else if (this.MovementType == MovementType.FlyCam)
            {
                if (this.CharacterCollider != null)
                    this.CharacterCollider.enabled = false;
            }


            this.DefaultRaycastTool = new DefaultRaycastTool();

        }

        protected virtual void OnExit()
        {
            this.SetCursor(CursorLockMode.None, true);
        }

        protected virtual void SetValues()
        {
            if (!ArchToolkitManager.IsInstanced())
                ArchToolkitManager.Factory();

            this.CanMovePosition = true;

            this.movementSpeed = ArchToolkitManager.Instance.settings.movementSpeed;
            this.RunFaster = ArchToolkitManager.Instance.settings.RunFaster;
            this.RunSpeed = ArchToolkitManager.Instance.settings.RunSpeed;
            this._lockMovementTo = ArchToolkitManager.Instance.settings.lockMovementTo;
            this._mobileMovementType = ArchToolkitManager.Instance.settings.mobileMovementType;
            this.MovementType = ArchToolkitManager.Instance.settings.movementType;

            this.clumbSpeed = ArchToolkitManager.Instance.settings.clumbSpeed;
            this.walkableLayers = ArchToolkitManager.Instance.settings.walkableLayers;

        }

        private void ResetHeadOnSwitch(MovementType movementType)
        {
            // every time that the movement is classic, reset position to head position
            if (movementType == MovementType.Classic)
            {
                if (this.CharacterCollider != null)
                    this.CharacterCollider.enabled = true;
                this.Head.transform.localPosition = this.startingHeadPos;
                if (this.rb != null)
                {
                    this.rb.velocity = Vector3.zero;
                }
            }
            else
            {
                if (this.CharacterCollider != null)
                    this.CharacterCollider.enabled = false;
                if (this.rb != null)
                {
                    this.rb.velocity = Vector3.zero;
                }
            }
        }

        Transform targetPosition;
        public void SetTargetPosition(Vector3 position)
        {
            if (targetPosition != null)
            {
                targetPosition.position = position;
            }
            else
            {
                var go = new GameObject("TargetPosition");
                go.transform.position = position;
                this.targetPosition = go.transform;
            }
        }

        public virtual void Teleport(Vector3 point)
        {
            Vector3 dir = (this.transform.position - this.Head.transform.position).normalized;

            float dist = Vector3.Distance(this.Head.transform.position, this.transform.position);

            Vector3 finale = dir * dist;

            finale = new Vector3(finale.x, 0f, finale.z);

            if (ArchToolkitManager.Instance.movementTypePerPlatform == MovementTypePerPlatform.VR)
                this.transform.position = new Vector3(point.x, point.y + this.HeightDifference, point.z);
            else
            {
                this.transform.position = new Vector3(point.x, point.y, point.z);
            }

            this.transform.position += finale;

        }

        protected override void Update()
        {
            if (this.Head == null)
            {
                //Debug.LogError("Head is null,maybe you forgot to link it, in the inspector");
                return;
            }

            if (this.CurrentRaycastTool.Update(Time.deltaTime))
            {
                this.CheckShift();

                if (InputListener.DoubleTap && this.latestFloorPosition != Vector3.zero)
                {
                    this.SetTargetPosition(this.latestFloorPosition);
                }
                else
                {
                    if (InputListener.SpacePressed && this._lockMovementTo == LockMovementTo.None) // if space is pressed and, the movement isn't locked
                    {
                        if (this.MovementType == MovementType.FlyCam)
                            this.MovementType = MovementType.Classic;
                        else
                            this.MovementType = MovementType.FlyCam;
                    }

                    switch (this.MovementType)
                    {
                        case MovementType.FlyCam:
                            this.FlyCamMovement();
                            break;
                        case MovementType.Classic:
                            this.ClassicMovement();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected Vector3 latestFloorPosition;
        protected void onHoverFloor(RaycastHit hit)
        {
            latestFloorPosition = hit.point;
        }
        private void OnRaycastNull(Ray obj)
        {
            latestFloorPosition = Vector3.zero;
        }
        private void OnHoverInteractable(Ray arg1, RaycastHit arg2)
        {
            latestFloorPosition = Vector3.zero;
        }
        private void OnExitSensigleObject(RaycastHit obj)
        {
            latestFloorPosition = Vector3.zero;
        }
        protected void CheckGround(Action<RaycastHit> onHit, Transform defaultTrasform = null)
        {
            RaycastHit hit;
            if (defaultTrasform == null) defaultTrasform = this.Head.transform;
            Ray ray = new Ray(defaultTrasform.position, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction, Color.blue);
            if (Physics.Raycast(ray, out hit, 100, this.walkableLayers))
            {
                if (onHit != null)
                    onHit(hit);
            }
            else
            {
                if (onHit != null)
                    onHit(hit);
            }
        }

        public void RotateCameraOrBody(Transform cameraTransform = null, Transform bodyTransform = null)
        {
            if (!this.CanRotateHead) return;

            if (cameraTransform == null) cameraTransform = this.Head.transform;
            if (bodyTransform == null) bodyTransform = this.transform;

            rotationX += InputListener.MouseX * Time.deltaTime;
            rotationY -= InputListener.MouseY * Time.deltaTime;

            rotationY = Mathf.Clamp(rotationY, -90, 90);

            bodyTransform.rotation = Quaternion.Euler(new Vector3(0, rotationX, 0));
            cameraTransform.localRotation = Quaternion.Euler(new Vector3(rotationY, 0, 0));
        }

        public void ForcePositionAndForward(Vector3 position, Vector3 forward)
        {
            this.transform.position = position;

            this.Head.transform.localRotation = Quaternion.identity;
            this.Head.transform.rotation = Quaternion.identity;
            this.transform.forward = forward;
        }
        private void FlyCamMovement()
        {
            if (this.CanMovePosition)
            {
                this.transform.Translate(new Vector3((InputListener.HorizontalAxis * this.movementSpeed * Time.deltaTime),
                                             0, InputListener.VerticalAxis * this.movementSpeed * Time.deltaTime), this.Head.transform);

                if (InputListener.FlyUp) { this.GoUp(); }
                if (InputListener.FlyDown) { this.GoDown(); }


                if (HUDJoystickMobile.MovementMobileContainer != null)
                {
                    if (HUDJoystickMobile.MovementMobileContainer.InputDirection.magnitude > 0.70f)
                    {
                        this.transform.Translate(new Vector3(HUDJoystickMobile.MovementMobileContainer.InputDirection.x * this.movementSpeed * Time.deltaTime,
                                                        0,
                                                        HUDJoystickMobile.MovementMobileContainer.InputDirection.y * this.movementSpeed * Time.deltaTime), this.Head.transform);
                    }
                }
            }

            this.RotateCameraOrBody(this.Head.transform, this.transform);

        }

        public void GoUp()
        {
            this.transform.position += this.transform.up * this.clumbSpeed * Time.deltaTime;
        }

        public void GoDown()
        {
            this.transform.position -= this.transform.up * this.clumbSpeed * Time.deltaTime;
        }

        protected virtual void ClassicMovement()
        {
            if (this.CanMovePosition)
            {
                this.CheckGround((RaycastHit hit) =>
                {
                    if (hit.transform != null)
                    {
                        var headLocalPos = this.Head.transform.localPosition;
                        headLocalPos.y = this.HeightDifference;
                        this.Head.transform.localPosition = headLocalPos;

                        Vector3 movement = new Vector3(InputListener.HorizontalAxis * this.movementSpeed * Time.deltaTime,
                                             0, InputListener.VerticalAxis * this.movementSpeed * Time.deltaTime);

                        if (this.targetPosition != null)
                        {

                            if (movement.sqrMagnitude > 0)
                            {
                                GameObject.Destroy(this.targetPosition.gameObject);
                            }
                            else
                            {
                                var distance = this.targetPosition.position - this.transform.position;

                                distance = Vector3.ProjectOnPlane(distance, Vector3.up);
                                if (distance.magnitude < 0.1f)
                                {
                                    GameObject.Destroy(this.targetPosition.gameObject);
                                }

                                distance.Normalize();

                                distance = distance * this.movementSpeed * Time.deltaTime;

                                movement = distance;

                                this.transform.position = this.transform.position + movement;
                            }
                        }
                        else
                        {
                            this.transform.Translate(movement, this.Head.transform);

                            if (movement.sqrMagnitude < 0.01f)
                            {
                                if (this.rb != null)
                                {
                                    this.rb.velocity = Vector3.zero;
                                }
                            }
                        }
                        // Force Body on the floor
                        this.transform.position = new Vector3(this.transform.position.x, hit.point.y, this.transform.position.z);
                    }
                    else
                    {
                        // Wait 2 seconds, and if the player dont fall on the ground switch the movement to flycam
                        this.StartCoroutine(this.StartTimerFalling(2, () =>
                        {
                            if (hit.transform == null)
                            {
                                this.MovementType = MovementType.FlyCam;
                            }
                        }));
                    }
                });

            }

            this.RotateCameraOrBody(this.Head.transform, this.transform);
        }

        private IEnumerator StartTimerFalling(float seconds, Action onTimeFinish)
        {
            transform.Translate(Vector3.down * 3 * Time.deltaTime, Space.World);

            yield return new WaitForSecondsRealtime(seconds);

            if (onTimeFinish != null)
                onTimeFinish();
        }

        private void CheckShift()
        {
            if (Input.GetKey(KeyCode.LeftShift))
                this.movementSpeed = this.RunSpeed + this.initialSpeed;
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                this.movementSpeed = this.RunFaster + this.initialSpeed;
            else
                this.movementSpeed = initialSpeed;
        }

        private void SetCursor(CursorLockMode cursorLockMode, bool visible)
        {
            Cursor.lockState = cursorLockMode;
            Cursor.visible = visible;
        }
        public void TeleportWithFade()
        {
            this.Teleport(this.raycaster.hit.point);

            if (ArchToolkitManager.Instance.settings.VRFadeTime > 0)
                VRFade.Instance.StartFade(this.transform.position, ArchToolkitManager.Instance.settings.VRFadeTime, Color.black);
        }
        public bool CanTeleport(RaycastHit hit, float maxAngleDegree)
        {
            var canTeleport = Utils.ArchToolkitProgrammingUtils.CanTeleport(hit, maxAngleDegree);

            //if (this.OnCheckVRInteraction != null)
            //    this.OnCheckVRInteraction(canTeleport);

            return canTeleport;

        }
    }
}
