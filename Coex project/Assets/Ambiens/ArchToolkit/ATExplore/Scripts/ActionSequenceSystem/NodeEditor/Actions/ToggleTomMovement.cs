using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Toggle Tom Movement")]

    public class ToggleTomMovement : AAction
    {
        public enum ForceTomMovementType
        {
            toggle,
            enabled,
            disabled
        }
        public ForceTomMovementType ForceMovement = ForceTomMovementType.toggle;

        protected override void _RuntimeInit()
        {
            //DO Nothing
        }

        protected override bool _StartAction()
        {
            if (ForceMovement == ForceTomMovementType.toggle)
            {
                if(ArchToolkit.ArchToolkitManager.Instance.visitor.CanMovePosition)
                    ArchToolkit.ArchToolkitManager.Instance.visitor.LockPosition();
                else ArchToolkit.ArchToolkitManager.Instance.visitor.UnlockPosition();
            }
            else if (ForceMovement == ForceTomMovementType.enabled)
                ArchToolkit.ArchToolkitManager.Instance.visitor.UnlockPosition();
            else if (ForceMovement == ForceTomMovementType.disabled)
                ArchToolkit.ArchToolkitManager.Instance.visitor.LockPosition();
            return true;
        }
    }
}
