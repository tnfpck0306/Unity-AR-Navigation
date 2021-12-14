using UnityEngine;

namespace ArchToolkit.AnimationSystem
{
    [System.Serializable]
    public class Variant
    {
        [SerializeField]
        public UnityEngine.Object linkedObject;

        [SerializeField]
        public Texture2D preview;

        public string ID;

        public string name;

        public string description;

        public int index;

        public Variant(UnityEngine.Object linkedObject, string ID, string name,Texture2D preview = null, string description = " ")
        {
            this.linkedObject = linkedObject;
            this.ID = ID;
            this.name = name;
            this.description = description;
            this.preview = preview;
        }
    }
}