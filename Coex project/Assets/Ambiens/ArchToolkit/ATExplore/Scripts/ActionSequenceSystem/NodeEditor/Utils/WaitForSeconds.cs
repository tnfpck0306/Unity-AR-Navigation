using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Utils/Wait for seconds")]
    public class WaitForSeconds : AAction
    {

        public float delay=1;
        private float currentTime = 0;
        private bool isRunning = false;
        protected override void _RuntimeInit()
        {
            
        }

        protected override bool _StartAction()
        {
            currentTime = 0;
            isRunning = true;
            Debug.Log("WaitForSeconds - start");

            return false;
        }

        public override void ManagedUpdate(float deltaTime)
        {
            if(isRunning){
                currentTime += deltaTime;
                if(currentTime>=delay){
                    isRunning = false;
                    this.OnComplete();
                }
            }
        }
    }

}
