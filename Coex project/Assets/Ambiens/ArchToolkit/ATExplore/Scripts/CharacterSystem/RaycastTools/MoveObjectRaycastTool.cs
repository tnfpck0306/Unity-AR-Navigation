using ArchToolkit.InputSystem;
using ArchToolkit.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ArchToolkit.Character
{
    public class MoveObjectRaycastTool : ARaycastTool
    {
        private Transform ToMove;
        private TouchZoomAndRotateBehaviour touchMan;
        public MoveObjectRaycastTool(Transform tToMove):base()
        {
            this.ToMove = tToMove;
            this.targetPosition = this.ToMove.position;
            this.targetRotation = this.ToMove.transform.localEulerAngles.y;
        }

        public Vector3 targetPosition;
        public float targetRotation;

        public override bool Update(float time)
        {
            if (ArchToolkitManager.Instance.movementTypePerPlatform == MovementTypePerPlatform.VR)
            {
                if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.rightController != null)
                {
                    ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.rightController.OnTriggerPressed(() =>
                    {
                        ArchToolkitManager.Instance.visitor.SetRaycastTool(null);
                    });
                }
                if (InputListener.HorizontalAxis != 0)
                {
                    targetRotation = this.ToMove.localEulerAngles.y + InputListener.HorizontalAxis * time * 200f;
                }
            }
            else
            {
                if (touchMan == null) touchMan = new TouchZoomAndRotateBehaviour();

                if (touchMan.ManageTouchGestures())
                {
                    targetRotation = this.ToMove.localEulerAngles.y + touchMan.RotationAmount * time * 200f;
                }
                else if (InputListener.MouseButtonLeftDown)
                {
                    ArchToolkitManager.Instance.visitor.SetRaycastTool(null);
                }
                else if (InputListener.MouseWheel != 0)
                {
                    targetRotation = this.ToMove.localEulerAngles.y + InputListener.MouseWheel * time * 300f;
                }
                else if (InputListener.MovementFromJoyPad())
                {
                    ArchToolkitManager.Instance.visitor.RotateCameraOrBody();
                }
            }
            this.ToMove.position = Vector3.Lerp(this.ToMove.position, this.targetPosition,0.1f);
            this.ToMove.localEulerAngles = new Vector3(
                this.ToMove.localEulerAngles.x, 
                 this.targetRotation,
                this.ToMove.localEulerAngles.z);
            
            return false;
        }

        protected override void OnClick(Transform obj)
        {
            
        }

        protected override void OnExitSensibleObject(RaycastHit obj)
        {
            
        }

        protected override void OnHover(Ray ray, RaycastHit hit)
        {
            
        }

        protected override void OnHoverFloor(RaycastHit hit)
        {
            this.targetPosition = hit.point+Vector3.up*0.01f;
        }

        protected override void OnRaycastHit(Ray ray, RaycastHit hit)
        {
            
        }

        protected override void OnRaycastNull(Ray ray)
        {
            
        }

        protected override void OnTimerClickOnFloor()
        {
            ArchToolkitManager.Instance.visitor.SetRaycastTool(null);
        }
    }
}