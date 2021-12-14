using System.Collections;
using System.Collections.Generic;
using ArchToolkit.Character;
using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.MenuSystem{
    public class ToggleUI : MenuElementBase 
    {
        public override string GetPath(){
            return "/Settings/toggleUI";
        }

        public override GameObject GetUI(RectTransform parent)
        {
            var go=GameObject.Instantiate(Resources.Load<GameObject>("MainMenuPrefabs/ToggleUI"),Vector3.zero, Quaternion.identity,parent);
            var b=go.GetComponent<Button>();
            b.onClick.AddListener(this.toggleUI);
            return go;
           
        }
       
        public void toggleUI()
        {
            switch (ArchToolkitManager.Instance.visitor.MovementType)
            {
                case MovementType.FlyCam:
                    ArchToolkitManager.Instance.visitor.MovementType = MovementType.Classic;
                    break;
                case MovementType.Classic:
                    ArchToolkitManager.Instance.visitor.MovementType = MovementType.FlyCam;
                    break;
                default:
                    break;
            }
        }

    }
}