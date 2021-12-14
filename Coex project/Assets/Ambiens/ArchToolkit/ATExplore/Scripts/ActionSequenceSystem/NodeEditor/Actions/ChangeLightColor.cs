using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Change Light Color")]
    public class ChangeLightColor : AAction
    {
        public List<Light> lights;
        public Color color;
        protected override void _RuntimeInit()
        {
            if (!this.InitSceneReferences())
            {
                return;
            }

            lights = new List<Light>();
            foreach (var go in this.SceneReferences)
            {
                var l = go.GetComponent<Light>();
                if (l != null)
                {
                    lights.Add(l);
                }
            }
            
        }

        protected override bool _StartAction()
        {
            
            foreach (var l in this.lights)
            {
                l.color = this.color;
            }
            
            return true;
        }
        
    }
}