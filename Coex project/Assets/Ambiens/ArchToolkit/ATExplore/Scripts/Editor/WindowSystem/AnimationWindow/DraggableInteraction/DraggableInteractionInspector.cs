using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit.AnimationSystem;
using ArchToolkit.Utils;

namespace ArchToolkit.Editor.Window
{
    public class DraggableInteractionInspector : ArchInspectorBase
    {
        public DraggableInteractionInspector(string name) : base(name)
        {

        }
        public static void AddDraggable(GameObject selected)
        {

            var draggable = selected.GetComponent<DraggableObjectInteractable>();
            if (draggable == null) draggable = selected.AddComponent<DraggableObjectInteractable>();

            var coll = selected.GetComponent<BoxCollider>();
            if (coll == null) coll=selected.AddComponent<BoxCollider>();
            var bounds= ArchToolkitProgrammingUtils.GetBounds(selected);
            coll.size = bounds.size;
            coll.center = Vector3.up*bounds.size.y*0.5f;

            draggable.TargetList.Add(selected);
            draggable.SetHandle(Vector3.zero);
            
            SetChildStatic(selected.transform);

            if (!selected.CompareTag(ArchToolkitDataPaths.ARCHINTERACTABLETAG)) selected.tag = ArchToolkitDataPaths.ARCHINTERACTABLETAG;
        }

        
    }
}