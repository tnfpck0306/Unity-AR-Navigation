using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.MenuSystem{
    public class MainMenu : SubMenu
    {
        public List<MenuElementBase> registeredMenuElements;

        protected override void Start()
        {

            this.subMenuButtonPrefab=Resources.Load<GameObject>("MainMenuPrefabs/SubMenuButtonsPrefab");
            this.registeredMenuElements=new List<MenuElementBase>();

            this.registeredMenuElements.Add(new HeightSettings());
            this.registeredMenuElements.Add(new SwitchCamera());
            this.registeredMenuElements.Add(new PhotoCamera());

            //this.registeredMenuElements.Add(new PPSettings());

            this.registeredMenuElements.Add(new ExitButton());
            
            this.RefreshButtons();
            this.ForceMenuVisibility(false);
            
        }
        void RefreshButtons()
        {
            foreach(var b in this.registeredMenuElements)
            {
                var p= b.GetPath();
                string[] pathArr = p.Split(new []{'/'},StringSplitOptions.RemoveEmptyEntries);
                if(pathArr.Length==1)
                {
                    var go=b.GetUI(this.ButtonContainer);
                    if (go != null) 
                    {
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localRotation = Quaternion.identity;
                    }
                    
                }
                else{
                    this.AddSubMenu(pathArr, 0, b);
                }
            }
        }

        public void CloseAll(){
            this.CloseAllSubMenu();
            this.ForceMenuVisibility(false);
        }
        public void CloseAllSubMenu(){
            foreach(var s in this.ButtonContainer.GetComponentsInChildren<SubMenu>()){
                s.ForceMenuVisibility(false);
            }
        }
       
    }
}

