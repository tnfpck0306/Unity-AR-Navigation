using System;
using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using ArchToolkit.AnimationSystem;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Info Panel")]
    public class ShowInfoPanel : AAction
    {

        public string title;
        public string description;
        public Texture2D image;
        [SerializeField]
        [HideInInspector]
        public List<ArchToolkit.InputSystem.InfoDialog.KeyVal> data = new List<ArchToolkit.InputSystem.InfoDialog.KeyVal>();
        //[Input] //TODO
        //public AAssetCollection collection;
        [Input]
        public Transform handleTransform;

        protected override void _RuntimeInit()
        {
            
        }

        protected override bool _StartAction()
        {
            this.handleTransform= ArchToolkit.ArchToolkitManager.Instance.visitor.raycaster.LatestClickTransform;
            
            ArchToolkit.InputSystem.InfoDialog.Instance.InitMenu(this.title, this.description, this.image,this.data, null, handleTransform);

            return true;
        }
        
    }
}
