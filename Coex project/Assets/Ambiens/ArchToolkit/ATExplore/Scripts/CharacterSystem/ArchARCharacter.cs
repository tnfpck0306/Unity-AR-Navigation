using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ArchToolkit.InputSystem;
using UnityEngine.XR;
using ArchToolkit.VR;
using UnityEngine.SceneManagement;
using ambiens.archtoolkit;
using ambiens.archtoolkit.atexplore.ar;

namespace ArchToolkit.Character
{

    public class ArchARCharacter : ArchCharacter
    {

        public Vector3 positionOffset;
        public Quaternion quaternionOffset;

        protected override void Awake()
        {
            this.SetValues();

            //raycaster.OnHoverFloor += this.onHoverFloor;

            StartCoroutine(this.InitRoot());

        }
        protected override void Start()
        {
            this.DefaultRaycastTool = new DefaultRaycastTool();

        }
        public Transform root;
        private List<GameObject> rootGameObjects;
        public IEnumerator InitRoot()
        {
            if (this.rootGameObjects == null)
            {
                Scene s = SceneManager.GetSceneAt(0);

                this.rootGameObjects = new List<GameObject>(s.GetRootGameObjects());
                rootGameObjects.Remove(this.gameObject);
                rootGameObjects.Remove(ArchToolkitManager.Instance.gameObject);
                var sh = FindObjectOfType<ambiens.archtoolkit.atexplore.actionSystem.SequenceHolder>();
                if (sh != null) rootGameObjects.Remove(sh.gameObject);

               /* var reference = FindObjectsOfType<ATImageMarkerReference>();
                if (reference != null)
                {
                    foreach(var r in reference)
                        rootGameObjects.Remove(r.gameObject);
                }*/
            }
            
            this.root = new GameObject("root").transform;

            foreach (var go in this.rootGameObjects)
            {
                go.transform.parent = this.root;
            }
            yield return new WaitForEndOfFrame();

            var bounds = this.root.EncapsulateBounds(true);
            foreach (var go in this.rootGameObjects)
            {
                go.transform.parent = null;
            }
            yield return new WaitForEndOfFrame();

            this.root.transform.position = new Vector3(bounds.center.x, bounds.center.y, bounds.center.z);

            foreach (var go in this.rootGameObjects)
            {
                go.transform.parent = this.root;
            }
            yield return new WaitForEndOfFrame();

            this.root.gameObject.SetActive(false);
        }

        public IEnumerator SetRootPositionAndRotation( Vector3 position, Quaternion rotation )
        {
            foreach (var go in this.rootGameObjects)
            {
                go.transform.parent = null;
            }
            yield return new WaitForEndOfFrame();

            this.root.transform.position = position;
            //this.root.transform.rotation = rotation;

            yield return new WaitForEndOfFrame();

            foreach (var go in this.rootGameObjects)
            {
                go.transform.parent = this.root;
            }
            yield return new WaitForEndOfFrame();

        }
        

        protected override void SetValues()
        {
            base.SetValues();
            

        }

        protected override void Update()
        {
            if (this.Head == null)
            {
                Debug.LogError("Null Head");
                return;
            }

            this.CurrentRaycastTool.Update(Time.deltaTime);
            
        }

    }
}
