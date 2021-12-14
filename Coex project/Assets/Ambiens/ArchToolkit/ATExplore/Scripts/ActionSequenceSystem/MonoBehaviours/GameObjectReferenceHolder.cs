using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [ExecuteInEditMode]
    public class GameObjectReferenceHolder : MonoBehaviour
    {
        [SerializeField]
        public List<GameObject> gameObjects = new List<GameObject>();

        public GameObject GetGameObject(string name)
        {
            var found = this.transform.Find(name);
           
            if (found)
            {
                
                return found.gameObject;
            }
           
            var go = new GameObject();
            go.transform.parent = this.transform;
            go.name = name;
            return go;
        }

    }
}