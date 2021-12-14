using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Rotate Objects Around Pivot")]
    public class RotateAroundPivot : ATransformAnimation
    {
        
        public override void animateFunction(float amount)
        {
            foreach (var targ in this.SceneReferences)
            {
                targ.transform.RotateAround(this.PivotPosition, this.PivotDirection, amount);
            }
        }

        public override void calculateAnimationAmount(float amount)
        {

        }

    }
}
