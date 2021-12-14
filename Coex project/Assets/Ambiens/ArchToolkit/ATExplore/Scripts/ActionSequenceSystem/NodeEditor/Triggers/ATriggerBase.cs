
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{

    public abstract class ATriggerBase : AActionNodeBase
    {
        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "Caller") return this;
            return null;
        }
    }
}
