using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Utils/Debug Log")]

    public class DebugLog : AAction
    {
        [Input]
        public string DebugString = "Debug";
        protected override void _RuntimeInit()
        {
            //DO Nothing
        }

        protected override bool _StartAction()
        {
            var i = this.GetInputPort("DebugString");
            if (i.IsConnected)
            {
                this.DebugString = this.GetInputValue<string>("DebugString");
            }

            Debug.Log(DebugString);
            return true;
        }
    }
}
