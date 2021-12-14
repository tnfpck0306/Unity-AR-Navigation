using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ArchToolkit.Character
{

    public class HUDDesktop : HUD
    {
        public GameObject hud;
        public GameObject topHUD;

        protected override void Awake()
        {
            base.Awake();
            this.toggleHUD(ArchToolkitManager.Instance.settings.UIShowControlsGuide?1:0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H) && this.hud != null)
                this.toggleHUD();
        }

        private void toggleHUD(int force=-1)
        {
            if (force == -1)
            {
                this.hud.SetActive(!this.hud.activeInHierarchy);
                this.topHUD.SetActive(!this.topHUD.activeInHierarchy);
            }
            else if (force == 1) 
            {
                this.hud.SetActive(true);
                this.topHUD.SetActive(true);
            }
            else
            {
                this.hud.SetActive(false);
                this.topHUD.SetActive(false);
            }

        }
    }
}