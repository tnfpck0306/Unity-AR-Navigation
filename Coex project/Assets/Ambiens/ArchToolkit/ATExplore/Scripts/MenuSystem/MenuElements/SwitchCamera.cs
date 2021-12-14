using System.Collections;
using System.Collections.Generic;
using ArchToolkit.Character;
using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.MenuSystem{
    public class SwitchCamera : MenuElementBase 
    {
        Text buttonText;
        public override string GetPath(){
            return "/Tools/switchcamera";
        }

        public override GameObject GetUI(RectTransform parent)
        {
            if(ArchToolkit.ArchToolkitManager.Instance.movementTypePerPlatform== MovementTypePerPlatform.VR)
            {
                return null;
            }
            else
            {
                var go = GameObject.Instantiate(Resources.Load<GameObject>("MainMenuPrefabs/SwitchCamera"), Vector3.zero, Quaternion.identity, parent);
                var b = go.GetComponent<Button>();
                b.onClick.AddListener(this.SwitchMovement);
                this.buttonText = go.GetComponentInChildren<Text>();
                this.SetLabel();
                return go;
            }
            
            
        }
        void SetLabel()
        {
            switch (ArchToolkitManager.Instance.visitor.MovementType)
            {
                case MovementType.FlyCam:
                    this.buttonText.text="Set Walkcam";
                    break;
                case MovementType.Classic:
                    this.buttonText.text="Set Flycam";
                    break;
                default:
                    break;
            }
        }
        public void SwitchMovement()
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
            this.SetLabel();
            this.buttonText.GetComponentInParent<MainMenu>().CloseAll();
        }

    }
}