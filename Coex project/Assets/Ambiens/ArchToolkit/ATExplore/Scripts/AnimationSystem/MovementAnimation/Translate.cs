using UnityEngine;

namespace ArchToolkit.AnimationSystem
{

    public class Translate : ATransformInteractable
    {

        public bool test = false;
        public bool reset = false;
        
        public override void animateFunction(float amount)
        {
            foreach (var targ in this.TargetList)
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