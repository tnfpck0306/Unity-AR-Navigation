using System;
using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Triggers/Trigger On Object Pointer Exit")]
	public class TriggerOnObjectPointerExit : ATriggerBase
    {
        
        private List<GameObject> sceneReferences;
        protected override void _RuntimeInit()
        {
            var holder = GameObject.FindObjectOfType<SequenceHolder>();
            if(holder==null){

                Debug.LogError("There's no sequenceHolder in the scene!");
                return;
            }

            this.sceneReferences = holder.RequestGameObjectReferences(this.ID).gameObjects;

            if(this.sceneReferences.Count==0){
                Debug.LogWarning("Trigger ON Object Click: there are no reference in the scene, did you added the trigger?");
            }

            ArchToolkit.ArchToolkitManager.Instance.visitor.raycaster.OnExitSensibleObject += this.OnExitObject;

            foreach (var r in sceneReferences) 
            {
                r.tag = ArchToolkit.ArchToolkitDataPaths.ARCHINTERACTABLETAG;
                var coll = r.GetComponent<Collider>();
                if (coll == null) r.AddComponent<BoxCollider>();
            }

        }

        private void OnExitObject( RaycastHit h)
        {
            if (h.transform != null)
            {
                if (!sceneReferences.Contains(h.transform.gameObject))
                    this.OnComplete();
            }
            
        }
    }
}
