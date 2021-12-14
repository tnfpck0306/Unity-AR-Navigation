using System;
using UnityEngine;

namespace ArchToolkit.Editor.Window
{
    [System.Serializable]
    public class EditorInteractable 
    {

        [SerializeField]
        public int ID;
        [SerializeField]
        public UnityEngine.Object linkedObject;

        [SerializeField]
        public string name;
        [SerializeField]
        public string description;



        public EditorInteractable(UnityEngine.Object linkedObject, string name, int ID, string description = "")
        {

            this.linkedObject = linkedObject;
            this.name = name;
            this.description = description;
            this.ID = ID;

        }
    }
}