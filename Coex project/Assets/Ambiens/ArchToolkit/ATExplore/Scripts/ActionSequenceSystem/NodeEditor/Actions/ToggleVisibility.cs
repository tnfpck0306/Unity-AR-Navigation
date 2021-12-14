using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Toggle Object Visibility")]
    public class ToggleVisibility : AAction
    {

        public enum VisibilityStartType
        {
            dontTouch,
            allEnabled,
            allDisabled
        }
        public enum ForceVisibilityType
        {
            toggle,
            enabled,
            disabled
        }
        public VisibilityStartType startVisibility = VisibilityStartType.dontTouch;
        public ForceVisibilityType ForceVisibilityTo = ForceVisibilityType.toggle;
        
        protected override void _RuntimeInit()
        {
            if (!this.InitSceneReferences())
            {
                return;
            }

            if (startVisibility != VisibilityStartType.dontTouch)
            {
                foreach (var go in this.SceneReferences)
                    go.SetActive(startVisibility == VisibilityStartType.allEnabled);
            }
            
        }
        
        protected override bool _StartAction()
        {
            foreach (var go in this.SceneReferences)
            {
                if (ForceVisibilityTo == ForceVisibilityType.toggle)
                    go.SetActive(!go.activeSelf);
                else if (ForceVisibilityTo == ForceVisibilityType.enabled)
                    go.SetActive(true);
                else if (ForceVisibilityTo == ForceVisibilityType.disabled)
                    go.SetActive(false);
            }
            
            return true;
        }
        
    }
}