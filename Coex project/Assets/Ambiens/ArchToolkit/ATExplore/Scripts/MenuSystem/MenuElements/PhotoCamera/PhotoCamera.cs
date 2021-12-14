using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.MenuSystem{
    public class PhotoCamera : MenuElementBase 
    {
        MainMenu menu;
        public override string GetPath(){
            return "/Tools/PhotoCamera";
        }

        public override GameObject GetUI(RectTransform parent)
        {
            var go=GameObject.Instantiate(Resources.Load<GameObject>("MainMenuPrefabs/PhotoCamera"),Vector3.zero, Quaternion.identity,parent);
            go.GetComponent<Button>().onClick.AddListener(this.InstantiateCam);
            this.menu = go.GetComponentInParent<MainMenu>();
            return go;
        }

        void InstantiateCam()
        {
            var cam=GameObject.Instantiate(Resources.Load<GameObject>("MainMenuPrefabs/PhotoCameraUI"));
            this.menu.CloseAll();
        }
    }
}