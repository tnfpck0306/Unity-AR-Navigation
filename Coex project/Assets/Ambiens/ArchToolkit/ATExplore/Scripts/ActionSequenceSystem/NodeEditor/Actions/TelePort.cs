using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Teleport to position")]
    public class TelePort : AAction
    {
        
        public Vector3 tpTargetPosition;
        protected override void _RuntimeInit()
        {
            
        }

        protected override bool _StartAction()
        {

            ArchToolkit.ArchToolkitManager.Instance.visitor.Teleport(this.tpTargetPosition);

            return true;
        }

    }
}
