using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit.Navigation
{
    [System.Serializable]
    public class PathPoint : MonoBehaviour
    {
        [SerializeField]
        public int ID;

        [HideInInspector]
        public List<PathPoint> neighboors = new List<PathPoint>();

        public void Init(int ID, List<PathPoint> neighboors = null)
        {
            this.ID = ID;

            if (neighboors != null)
                this.neighboors = neighboors;
        }

        private void Start()
        {
            if(this.ID == 0)
            {
                var mFilter = this.GetComponent<MeshFilter>();

                if (mFilter == null)
                    mFilter = this.GetComponentInChildren<MeshFilter>();

                if (mFilter != null)
                    mFilter.sharedMesh = null;
            }
        }
    }
}
