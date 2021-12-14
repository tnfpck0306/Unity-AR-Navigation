using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Triggers/Trigger On Handle Click")]

    public class TriggerOnHandleClick : ATriggerBase
    {
       
        private List<GameObject> sceneReferences;

        protected override void _RuntimeInit()
        {
            var holder = GameObject.FindObjectOfType<SequenceHolder>();
            if (holder == null)
            {
                Debug.LogError("There's no sequenceHolder in the scene!");
                return;
            }
            this.sceneReferences = holder.RequestGameObjectReferences(this.ID).gameObjects;
            ArchToolkit.ArchToolkitManager.Instance.visitor.raycaster.OnClick += this.OnClick;

        }
        private void OnClick(Transform obj)
        {
            if (sceneReferences.Contains(obj.gameObject))
                this.OnComplete();
        }
    }
}
