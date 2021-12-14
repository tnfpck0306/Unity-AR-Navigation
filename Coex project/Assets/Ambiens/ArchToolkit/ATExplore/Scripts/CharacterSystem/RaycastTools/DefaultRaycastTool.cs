using ArchToolkit.InputSystem;
using ArchToolkit.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ArchToolkit.Character
{
    public class DefaultRaycastTool : ARaycastTool
    {

        public override bool Update(float time)
        {
            if (ArchToolkitManager.Instance.movementTypePerPlatform == MovementTypePerPlatform.VR)
            {

                this.VRMovement();
            }
            
            return true;
        }

        protected override void OnClick(Transform transform)
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
            
        }

        protected override void OnRaycastHit(Ray ray, RaycastHit hit)
        {
            
        }

        protected override void OnRaycastNull(Ray ray)
        {
            
        }

        protected override void OnTimerClickOnFloor()
        {
            if (this.raycaster.UseTimer) //Special Case: onClick is triggered by the raycaster when there's the timer
            {
                this.character.TeleportWithFade();
            }
        }

        void VRMovement()
        {
            if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager != null &&
                ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.isVrRunning)
            {

                if (this.raycaster != null && this.raycaster.hit.transform != null)
                {

                    if (this.character.CanTeleport(this.raycaster.hit, 45f))
                    {
                        if (!this.raycaster.UseTimer)
                        {
                            if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.HasRightHand())
                            {
                                ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.rightController.OnTriggerPressed(() =>
                                {
                                    //this.character.Teleport(this.raycaster.hit.point);
                                    //VRFade.Instance.StartFade(this.character.transform.position, 0.35f, Color.black);
                                    this.character.TeleportWithFade();

                                }, 0.95f);
                            }
                        }
                        else
                        {
                            if (!this.raycaster.UseTimer)
                            {
                                if (InputListener.MouseButtonLeftDown)
                                {
                                    this.character.TeleportWithFade();
                                }
                            }
                        }
                    }
                }
                /*else
                {
                    if (this.OnCheckVRInteraction != null)
                        this.OnCheckVRInteraction(false);
                }*/
                
            }
        }
    }
}