using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.Character
{
    public class HUDMobile : HUD
    {
        protected  bool camUp;

        protected bool camDown;


        protected override void Awake()
        {
            base.Awake();

            this.camDown = false;
            this.camUp = false;
        }

        protected virtual void Update()
        {
            if (this.camUp || this.camDown)
                this.CamUpAndDown();
        }

        public void SwitchMovement()
        {
            switch (this.visitor.MovementType)
            {
                case MovementType.FlyCam:
                    this.visitor.MovementType = MovementType.Classic;
                    break;
                case MovementType.Classic:
                    this.visitor.MovementType = MovementType.FlyCam;
                    break;
                default:
                    break;
            }
        }

        public void CamUpAndDown()
        {
            if (this.visitor != null)
            {
                if (this.visitor.MovementType == MovementType.FlyCam && camUp)
                    this.visitor.GoUp();

                if (this.visitor.MovementType == MovementType.FlyCam && camDown)
                    this.visitor.GoDown();
            }
        }

        public void CamDown()
        {
            this.camDown = true;
            this.camUp = false;
        }

        public void CamUp()
        {
            this.camUp = true;
            this.camDown = false;
        }

        public void ResetCamUpDown()
        {
            this.camDown = false;
            this.camUp = false;
        }
    }
}