using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit.AnimationSystem
{
    [ExecuteInEditMode]
    public class RotateAround : ATransformInteractable
    {
        public override void animateFunction(float amount)
        {
            foreach (var targ in this.TargetList)
            {
                targ.transform.RotateAround(this.PivotPosition, this.PivotDirection, amount);
            }
        }

        public override void calculateAnimationAmount(float amount)
        {

        }
    }
}