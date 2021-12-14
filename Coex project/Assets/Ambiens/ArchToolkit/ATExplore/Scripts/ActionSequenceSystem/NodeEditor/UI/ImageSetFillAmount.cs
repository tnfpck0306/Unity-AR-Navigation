using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;
using UnityEngine.UI;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("UI/Image Set Fill Amount")]
    public class ImageSetFillAmount : AAction
    {
        [Input]
        public float amount;

        private List<Image> images;
        protected override void _RuntimeInit()
        {
            if (!this.InitSceneReferences())
            {
                return;
            }

            images = new List<Image>();
            foreach (var go in this.SceneReferences)
            {
                var p = go.GetComponent<Image>();
                if (p != null)
                {
                    images.Add(p);
                }
            }
        }

        protected override bool _StartAction()
        {
            if (this.images == null) return true;

            var n=this.GetInputPort("amount");
            if (n.IsConnected)
            {
                this.amount = this.GetInputValue<float>("amount");
            }

            this.SetAmount(this.amount);

            return true;
        }
        private void SetAmount(float f)
        {

            foreach (var i in this.images) i.fillAmount = f;
        }
    }
}
