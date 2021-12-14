using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.MenuSystem{
    public class ExitButton : MenuElementBase 
    {
        
        public override string GetPath(){
            return "/exit";
        }

        public override GameObject GetUI(RectTransform parent)
        {
            var go=GameObject.Instantiate(Resources.Load<GameObject>("MainMenuPrefabs/ExitButton"),Vector3.zero, Quaternion.identity,parent);
            go.GetComponent<Button>().onClick.AddListener(this.Exit);
            return go;
        }
        void Exit(){
            ArchToolkitManager.Instance.DoExit();
        }
    }
}