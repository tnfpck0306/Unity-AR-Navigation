using System;
using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using ArchToolkit.AnimationSystem;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Show Multi-Choice Dialog")]
    public class ShowMultiChoicePanel : AAction
    {
        [HideInInspector]
        public List<Variant> variants=new List<Variant>();
        //[Input] //TODO
        //public AAssetCollection collection;
        [Input]
        public Transform handleTransform;

        public static string VARIANT_PORT_NAME = "ACTION";
        public static string OUTPUT_PORT_NAME = "CurrentSelected";
        protected override void _RuntimeInit()
        {
            
        }

        protected override bool _StartAction()
        {
            this.handleTransform= ArchToolkit.ArchToolkitManager.Instance.visitor.raycaster.LatestClickTransform;
            
            ArchToolkit.InputSystem.ImageSelectedDialog.Instance.InitMenu(0, 
                this.variants, this.OnChange, null, handleTransform);

            return true;
        }
        Variant currentSelectedVariant;
        private void OnChange(Variant obj)
        {
            this.currentSelectedVariant = obj;

            var port=this.GetOutputPort(obj.ID);
            if(port.IsConnected)
            {
                for(int i=0; i< port.ConnectionCount; i++)
                {
                    var p = port.GetConnection(i);
                    if (p != null)
                    {
                        AAction next = (AAction)p.node;
                        next.RuntimeInit();
                        next.StartAction();
                    }
                    
                }
                
            }
        }

        public override object GetValue(NodePort port)
        {
            if(port.fieldName==OUTPUT_PORT_NAME){
                return currentSelectedVariant.linkedObject;
            }
            return false;

        }
    }
}
