using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArchToolkit.InputSystem;
using System.Linq;

namespace ArchToolkit.Character
{
    public class HUD : MonoBehaviour
    {
        
        public string FlyCamText = "FLYCAM";
        public string WalkText = "WALKCAM";

        public Text switchText;
        public GameObject switchTextPanel;

        protected ArchCharacter visitor;

        public GameObject TouchControls;

        public GameObject FlyKeysHUD;
        public GameObject SpaceKeysHUD;

        public GameObject MouseLeft;
        public GameObject MouseRight;
        public GameObject TouchIcon;


        public virtual void Exit()
        {
            if (ArchToolkitManager.IsInstanced())
            {
                ArchToolkitManager.Instance.DoExit();
            }
        }

        protected virtual void Awake()
        {
            this.InitializeHUD();

            ArchToolkitManager.Instance.OnVisitorCreated += this.InitializeHUD;
        }

        protected virtual void InitializeHUD()
        {
            if (ArchToolkitManager.IsInstanced() && ArchToolkitManager.Instance.visitor != null)
            {
                this.visitor = ArchToolkitManager.Instance.visitor;

                this.switchTextPanel.SetActive((this.visitor.LockMovement != LockMovementTo.None));
                
                this.SwitchText(this.visitor.MovementType);

                this.TouchControls.SetActive(ArchToolkitManager.Instance.settings.UIShowTouchControls);

                this.SpaceKeysHUD.SetActive(this.visitor.LockMovement == LockMovementTo.None);

                this.visitor.OnMovementTypeChanged += this.SwitchText;

                this.MouseLeft.SetActive(!ArchToolkitManager.Instance.settings.UseRightClickToRotate);
                this.MouseRight.SetActive(ArchToolkitManager.Instance.settings.UseRightClickToRotate);

            }
        }

        private void SwitchText(MovementType movementType)
        {
            if (this.switchText == null)
                return;

            this.FlyKeysHUD.SetActive((movementType == MovementType.FlyCam));
            this.switchText.text = (movementType == MovementType.FlyCam) ? this.FlyCamText : this.WalkText;
            
        }

       
    }
}
