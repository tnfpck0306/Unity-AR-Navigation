using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Utils/Repeat for n times")]
    public class RepeatForNTimes : AAction
    {
        public int repetitions = 2;
        public float delay=1;
        private int currentRepetition = 0;
        private float currentTime = 0;
        private bool isRunning = false;
        protected override void _RuntimeInit()
        {
            
        }

        protected override bool _StartAction()
        {
            currentTime = 0;
            currentRepetition = 0;
            isRunning = true;

            return false;
        }

        public override void ManagedUpdate(float deltaTime)
        {
            if(isRunning){
                
                if(currentTime == 0f || currentTime>=delay){
                    
                    if(this.currentRepetition<=this.repetitions){
                        this.currentRepetition++;
                        this.currentTime = 0;
                        this.StartNext();
                    }
                    else {
                        isRunning = false;
                        this.OnComplete();
                    }
                }
                currentTime += deltaTime;
            }
        }
    }

}
