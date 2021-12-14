using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit.AnimationSystem
{

    [ExecuteInEditMode][System.Serializable]
    public class GizmoRotateAroundDirection : MonoBehaviour
    {
        private Mesh mesh;

        public enum rotDirType{
            rotation,
            translation
        }
        public rotDirType type = rotDirType.rotation;
        public float animationAmount;

        [SerializeField][HideInInspector]
        private Translate translate;

        [SerializeField][HideInInspector]
        private RotateAround rotate;

        private void Start()
        {
            GameObject app;

            this.rotate = this.transform.GetComponentInParent<RotateAround>();
            this.translate = this.transform.GetComponentInParent<Translate>();

            if (this.translate != null) type = rotDirType.translation;
            else if (this.rotate != null) type = rotDirType.rotation;

            if (this.type == rotDirType.translation)
            {
                app = GameObject.Instantiate(Resources.Load(ArchToolkitDataPaths.RESOURCESPATH_TRANSLATE_ARROW)) as GameObject;
            }
            else
            {
                app = GameObject.Instantiate(Resources.Load(ArchToolkitDataPaths.RESOURCEPATH_ROTATION_ARROW)) as GameObject;
            }

            mesh = app.GetComponentInChildren<MeshFilter>().sharedMesh;
            DestroyImmediate(app);
        }

        Vector3 dir;

        void OnDrawGizmos()
        {
           
            var rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;
            // Gizmos.color = new Color32(238, 233, 66, 200);
            Gizmos.color = Color.red;

            Gizmos.DrawMesh(mesh, Vector3.zero, Quaternion.identity, new Vector3(0.1f, 0.1f, 0.1f));

            var pivot = transform.parent.Find("pivot");

            if (pivot == null)
            {
                Debug.LogWarning("Warning: this action will remove the selected interaction.");
                return;
            }
            //if(this.animationAmount!=0){
                if (this.type == rotDirType.translation)
                {
                    dir = pivot.transform.position - this.transform.position;
                    transform.rotation = Quaternion.LookRotation(-dir * -180);
                    this.animationAmount = dir.magnitude;
                }
                else if (this.type == rotDirType.rotation)
                {
                    dir = pivot.transform.position - this.transform.position;
                    if(this.rotate!=null){
                        this.animationAmount = rotate.animationAmount;
                    }
                    transform.rotation = Quaternion.LookRotation(dir * this.animationAmount);
                }
            //}
            
        }
    }
}