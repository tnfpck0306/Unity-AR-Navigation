using UnityEngine;
using System.Linq;

namespace ArchToolkit.Character
{
    public class HUDJoystickMobile : HUDMobile
    {

        public static VJHandler MovementMobileContainer;

        public static VJHandler RotationMobileContainer;

        [SerializeField]
        private GameObject switchWalkButton;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void InitializeHUD()
        {   
            base.InitializeHUD();

            if (ArchToolkitManager.IsInstanced() && ArchToolkitManager.Instance.movementTypePerPlatform == MovementTypePerPlatform.FullScreen360)
            {
                if (ArchToolkitManager.Instance.visitor.LockMovement != LockMovementTo.None)
                {
                    if (this.switchWalkButton != null)
                        this.switchWalkButton.SetActive(false);
                }

                var handlers = GameObject.FindObjectsOfType<VJHandler>();

                MovementMobileContainer = handlers.ToList().Find(h => h.handlerType == HandlerType.Movement);
                RotationMobileContainer = handlers.ToList().Find(h => h.handlerType == HandlerType.Rotation);


            }
        }


        protected override void Update()
        {
            base.Update();
        }

    }
}
