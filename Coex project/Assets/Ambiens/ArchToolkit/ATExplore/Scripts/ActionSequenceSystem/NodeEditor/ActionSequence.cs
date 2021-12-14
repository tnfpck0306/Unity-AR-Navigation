using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem{
	
    [CreateAssetMenu]
    public class ActionSequence : NodeGraph
    {
        public void RuntimeInit()
        {
            var startNodes = this.nodes.Where(n => n is ATriggerBase);
            foreach (ATriggerBase triggerNode in startNodes) 
                triggerNode.RuntimeInit();
        }


    }

}
