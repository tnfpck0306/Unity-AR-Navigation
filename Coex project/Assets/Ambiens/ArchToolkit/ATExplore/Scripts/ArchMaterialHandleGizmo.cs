using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ArchToolkit.ArchGizmos
{
    /*
     * Deprecated 20200420
    [Serializable]
    public class ArchMaterialHandleGizmo : ArchGizmosBase
    {
        [ExecuteInEditMode]
        protected override void DrawEditorArchGizmo()
        {
            Gizmos.DrawIcon(this.transform.position, "icon_product.png", true);
        }

        protected override void DrawArchGizmo()
        {
            base.DrawArchGizmo();
        }


    }

    [Serializable][ExecuteInEditMode]
    public class ArchGizmosBase : MonoBehaviour
    {
        [Range(0,100)]
        public float RotationSpeed = 0;
        
        [SerializeField]
        public GameObject GizmoinGame;

        [ExecuteInEditMode]
        private void OnDrawGizmos()
        {
            //this.DrawEditorArchGizmo();
        }

        //[SerializeField][HideInInspector]
        //private bool isDefaultInstatiated = false;
        
        private void Start()
        {
            this.DrawArchGizmo();
        }

        protected virtual void DrawArchGizmo()
        {
            //if (this.GizmoinGame == null)
            //{
            //    this.GizmoinGame = GameObject.Instantiate(Resources.Load<GameObject>(ArchToolkitDataPaths.RESOURCESMATERIALHANDLE), Vector3.zero, Quaternion.identity, this.transform);
            //    this.isDefaultInstatiated = true;
            //}
            //else if (this.GizmoinGame != null && !this.isDefaultInstatiated)
            //    this.GizmoinGame = GameObject.Instantiate(this.GizmoinGame, Vector3.zero, Quaternion.identity, this.transform);
            //
            //this.GizmoinGame.transform.localPosition = Vector3.zero;
        }
        
        private void Update()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                return;
#endif

            if(this.GizmoinGame != null)
            {
                transform.Rotate(Vector3.up * (this.RotationSpeed * Time.deltaTime));
            }
        }

        protected virtual void DrawEditorArchGizmo()
        {

        }
    }
    */
}