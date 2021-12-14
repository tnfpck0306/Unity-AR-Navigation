using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit.InputSystem
{

    public class ArchUIImageSelectedDialogClose : ArchUIButton
    {
        public override void OnClick(Transform hitted)
        {
            if (hitted.gameObject != this.transform.gameObject)
                return;

            ImageSelectedDialog.Instance.Close();
        }

        public override void OnExitSensibleObject(RaycastHit hitted)
        {
            
        }

        public override void OnHover(Ray ray, RaycastHit hitted)
        {
            if (hitted.transform.gameObject != this.transform.gameObject)
                return;

            if(this.Thumbnail != null)
            {
                this.Thumbnail.color = Color.yellow;
            }
        }
    }
}
