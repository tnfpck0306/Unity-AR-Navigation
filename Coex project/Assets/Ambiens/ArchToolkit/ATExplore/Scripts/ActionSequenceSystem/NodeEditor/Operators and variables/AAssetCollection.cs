using System;
using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{

    public abstract class AAssetCollection: Node 
    {
        [Serializable]  
        public class AssetReference
        {
            public string ID;
            public UnityEngine.Object Asset;
            public string Title;
            public string Description;
        }
        [HideInInspector]
        public List<AssetReference> assetList= new List<AssetReference>();

        [Output]
        public AAssetCollection collection;

        public override object GetValue(NodePort port){
            if (port.fieldName == "collection") return this;
            return null;
        }

    }
}
