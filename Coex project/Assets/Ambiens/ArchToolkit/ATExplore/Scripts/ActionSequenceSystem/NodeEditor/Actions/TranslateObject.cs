using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Translate Objects")]
    public class TranslateObject : ATransformAnimation
    {
        void Reset(){
            this.animationAmount = 1;
        }
        public override void animateFunction(float amount)
        {
            foreach (var targ in this.SceneReferences)
            {
                targ.transform.position = targ.transform.position + this.PivotDirection.normalized * amount;
            }
        }

        public override void calculateAnimationAmount(float amount)
        {
            this.animationAmount = amount;
        }
    }
}
