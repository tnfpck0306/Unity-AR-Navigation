using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Close Multi Choice Panel")]

    public class CloseMultiChoicePanel : AAction
    {
        
        protected override void _RuntimeInit()
        {
            //DO Nothing
        }

        protected override bool _StartAction()
        {
            ArchToolkit.InputSystem.ImageSelectedDialog.Instance.Close();
            return true;
        }
    }
}
