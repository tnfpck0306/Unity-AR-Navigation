using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ArchToolkit.MenuSystem{
    public class PPSettings : MenuElementBase 
    {
        public override string GetPath(){
            return "/Settings/PPSettings";
        }

        public override GameObject GetUI(RectTransform parent)
        {
            var go=GameObject.Instantiate(Resources.Load<GameObject>("MainMenuPrefabs/PPSettings"),Vector3.zero, Quaternion.identity,parent);
            return go;
            
        }
    }
}