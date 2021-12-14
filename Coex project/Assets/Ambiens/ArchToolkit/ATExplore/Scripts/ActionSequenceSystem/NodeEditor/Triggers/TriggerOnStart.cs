using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Triggers/Trigger On Start Scene")]
    public class TriggerOnStart : ATriggerBase
    {
        protected override void _RuntimeInit()
        {
            Debug.Log("TriggerOnstart");
            
        }
        protected override void _AfterSequenceRuntimeInit()
        {
            Debug.Log("After all initialized, trigger on start ->START");
            this.OnComplete();
        }
    }
}
