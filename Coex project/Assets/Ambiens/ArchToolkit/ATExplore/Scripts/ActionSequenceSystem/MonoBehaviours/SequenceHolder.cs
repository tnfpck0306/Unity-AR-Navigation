using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [ExecuteInEditMode]
    public class SequenceHolder : MonoBehaviour
    {
        public enum HandleType{
            SmallCube,
            SmallCilinder,
        }
        [SerializeField]
        public List<ActionSequence> Sequences;

        public Dictionary<string, GameObject> sceneHandles=new Dictionary<string, GameObject>();
        public Dictionary<string, GameObjectReferenceHolder> sceneReferencesList=new Dictionary<string, GameObjectReferenceHolder>();

        public Action<float> ManagedUpdate;
        public void Start()
        {
            //Sequence.sceneHolder = this;
            if(Application.isEditor && Application.isPlaying || Application.isPlaying)
            {
                this.ManagedUpdate = null;
                StartCoroutine(delayedInit());
            }
            
        }

        private void Update()
        {
            if (ManagedUpdate != null) ManagedUpdate(Time.deltaTime);
        }

        IEnumerator delayedInit(){

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            foreach(var s in this.Sequences)
            {
                s.RuntimeInit();
            }


        }

        public GameObject RequestHandle(string id, HandleType handleType)
        {
            if (sceneHandles.ContainsKey(id)){
                if(sceneHandles[id]==null)
                {
                    return findInChildrenOrCreateHandle(id, handleType);
                }
                return sceneHandles[id];
            }
            else {
                return findInChildrenOrCreateHandle(id, handleType);
            }
        }
        private GameObject findInChildrenOrCreateHandle(string id, HandleType handleType)
        {
            var found = this.transform.Find(id);
            if (found!=null)
            {
                this.CheckOrCreateHandle(found.gameObject, handleType);
                sceneHandles.Add(id, found.gameObject);
                return found.gameObject;
            }
            var go = new GameObject();
            go.transform.parent = this.transform;
            go.name = id;
            this.CheckOrCreateHandle(go, handleType);
            if (sceneHandles.ContainsKey(id)) sceneHandles[id] = go;
            else sceneHandles.Add(id, go);
            return go;
        }
        private void CheckOrCreateHandle(GameObject go, HandleType type){
            var h = go.GetComponent<InEditorHandle>();
            if (h == null) h = go.AddComponent<InEditorHandle>();
            h.type = type;
        }

        public GameObjectReferenceHolder RequestGameObjectReferences(string id)
        {
            if (sceneReferencesList.ContainsKey(id))
            {
                if (sceneReferencesList[id] == null)
                {
                    return findInChildrenOrCreateReferenceHolder(id);
                }
                return sceneReferencesList[id];
            }
            else
            {
                return findInChildrenOrCreateReferenceHolder(id);
            }
        }
        private GameObjectReferenceHolder findInChildrenOrCreateReferenceHolder(string id)
        {
            var found = this.transform.Find(id);
            if (found)
            {
                var holder=this.CheckOrCreateReference(found.gameObject);
                sceneReferencesList.Add(id, holder);
                return holder;
            }
            var go = new GameObject();
            go.transform.parent = this.transform;
            go.name = id;
            var h=this.CheckOrCreateReference(go);

            if (sceneReferencesList.ContainsKey(id)) sceneReferencesList[id] = h;
            else sceneReferencesList.Add(id, h);

            return h;
        }
        private GameObjectReferenceHolder CheckOrCreateReference(GameObject go)
        {
            var h = go.GetComponent<GameObjectReferenceHolder>();
            if (h == null) h = go.AddComponent<GameObjectReferenceHolder>();
            return h;
        }
        private void OnDestroy()
        {
            this.ManagedUpdate = null;
        }
    }
}