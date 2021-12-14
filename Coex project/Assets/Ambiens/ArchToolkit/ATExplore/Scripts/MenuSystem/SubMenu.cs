using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.MenuSystem{
    public class SubMenu : MonoBehaviour
    {
        public RectTransform ButtonContainer;
        public GameObject subMenuButtonPrefab;
        public MainMenu main;
        protected virtual void Start()
        {
            this.main=this.gameObject.GetComponentInParent<MainMenu>();
            this.ForceMenuVisibility(false);
            
        }
        protected void AddSubMenu(string[] names, int level, MenuElementBase button)
        {
            if(level==names.Length-1)
            {
                var go=button.GetUI(this.ButtonContainer);
                if (go != null)
                {
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.identity;
                }
            }
            else{
                var found=this.ButtonContainer.Find(names[level]);
                if(found==null)
                {
                    var go=GameObject.Instantiate(Resources.Load<GameObject>("MainMenuPrefabs/SubMenuButtonsPrefab"),Vector3.zero, Quaternion.identity, this.ButtonContainer);
                    go.transform.localPosition=Vector3.zero;
                    go.transform.localRotation=Quaternion.identity;

                    go.name=names[level];
                    go.GetComponentInChildren<Text>().text=names[level];
                    var subMenu=go.GetComponent<SubMenu>();
                    subMenu.AddSubMenu(names, level+1,button);
                }
                else{
                    var subMenu=found.GetComponent<SubMenu>();
                    subMenu.AddSubMenu(names, level+1,button);
                }
            }
            
        }
        public void ForceMenuVisibility(bool force){
            ButtonContainer.gameObject.SetActive(force);
        }
        public void ToggleMenu(){
            if(this.main!=null)
                this.main.CloseAllSubMenu();
            ButtonContainer.gameObject.SetActive(!ButtonContainer.gameObject.activeSelf);
        }
    }
}
