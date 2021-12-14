using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{

    public abstract class ALoopableAction : AAction
    {
        public enum LoopType{
            PingPong=0,
            Simple=1
        }
        public bool AutoLoop = false;
        public LoopType loopType;

        protected float currentSign = 1;

    }
}
