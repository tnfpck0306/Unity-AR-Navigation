using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ArchToolkit.AnimationSystem
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class References : MonoBehaviour
    {
        [SerializeField]
        public string uniqueID;

        [SerializeField]
        public GameObject reference;

        [SerializeField][HideInInspector]
        private GameObject _refe;
        
        public string GetPrefix
        {
            get { return prefix; }
        }

        [SerializeField]
        internal string prefix;

        public void Init(GameObject reference)
        {
            this.reference = reference;

            this._refe = reference;


#if UNITY_EDITOR

            if (this.reference.GetComponent<RotateAround>())
                this.prefix = ArchToolkitText.ROTATION_PREFIX;
            else
                this.prefix = ArchToolkitText.TRANSLATION_PREFIX;

            EditorUpdateManager.Instance.AddToUpdate(EditorUpdate);

            EditorUtility.SetDirty(this);
#endif
        }

        private void Update()
        {
#if UNITY_EDITOR
            
            if(!EditorApplication.isPlaying && !EditorUpdateManager.Instance.IsActionInQueue(this.EditorUpdate))
            {
                EditorUpdateManager.Instance.AddToUpdate(this.EditorUpdate);
            }
#endif
        }

        
#if UNITY_EDITOR
        private void EditorUpdate()
        {
            if(this == null || this.gameObject == null)
            {
                EditorUpdateManager.Instance.RemoveFromUpdate(this.EditorUpdate);
                return;
            }
           
            if (this.reference == null)
            {
                GameObject.DestroyImmediate(this);
            }
            else
            {
                this.reference = this._refe;
            }

            
        }
#endif
        
        private void OnDestroy()
        {

#if UNITY_EDITOR
            EditorUpdateManager.Instance.RemoveFromUpdate(this.EditorUpdate);
#endif

            if (this.GetComponents<References>().Length == 1) // there is only this
            {
                this.gameObject.tag = ArchToolkitDataPaths.DEFAULT_TAG;
#if UNITY_EDITOR
                GameObject.DestroyImmediate(this.reference);
#endif
            }
        }
    }
}