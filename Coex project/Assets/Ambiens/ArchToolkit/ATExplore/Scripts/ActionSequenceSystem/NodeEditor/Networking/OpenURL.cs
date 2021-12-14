using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Networking/Open URL")]

    public class OpenURL : AAction
    {
        [Input]
        public string url = "https://www.ambiensvr.com";
        protected override void _RuntimeInit()
        {
            //DO Nothing
        }

        protected override bool _StartAction()
        {
            var i = this.GetInputPort("url");
            if (i.IsConnected)
            {
                this.url = this.GetInputValue<string>("url");
            }

            Application.OpenURL(this.url);
            
            return true;
        }
    }
}
