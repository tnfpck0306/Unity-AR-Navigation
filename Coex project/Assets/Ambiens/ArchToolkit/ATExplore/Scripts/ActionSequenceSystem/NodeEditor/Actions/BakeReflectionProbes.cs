using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Bake Reflection Probes")]
    public class BakeReflectionProbes : AAction
    {
        public List<ReflectionProbe> probes;
        protected override void _RuntimeInit()
        {
            if (!this.InitSceneReferences())
            {
                return;
            }

            probes = new List<ReflectionProbe>();
            foreach (var go in this.SceneReferences)
            {
                var p = go.GetComponent<ReflectionProbe>();
                if (p != null)
                {
                    probes.Add(p);
                }
            }
            
        }

        protected override bool _StartAction()
        {
            
            foreach (var p in this.probes)
            {
                p.RenderProbe();
            }
            
            return true;
        }
        
    }
}