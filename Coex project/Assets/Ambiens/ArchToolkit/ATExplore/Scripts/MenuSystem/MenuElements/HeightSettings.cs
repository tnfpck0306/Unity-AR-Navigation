using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ArchToolkit.MenuSystem{
    public class HeightSettings : MenuElementBase 
    {
        public override string GetPath(){
            return "/Settings/heightSettings";
        }

        public override GameObject GetUI(RectTransform parent)
        {
            var go=GameObject.Instantiate(Resources.Load<GameObject>("MainMenuPrefabs/HeightSettings"),Vector3.zero, Quaternion.identity,parent);
            return go;
        }
    }
}