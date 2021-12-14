using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ambiens.archtoolkit.atexplore.actionSystem{
    [ExecuteInEditMode]
    public class InEditorHandle : MonoBehaviour
    {

        public SequenceHolder.HandleType type;

        Mesh __mesh = null;
        

        Mesh mesh
        {
            get
            {
                if (__mesh == null)
                {

                    var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    __mesh = go.GetComponent<MeshFilter>().mesh;
                    GameObject.DestroyImmediate(go);
                }
                return __mesh;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            switch(type){
                case SequenceHolder.HandleType.SmallCube:
                    Gizmos.DrawCube(this.transform.position, new Vector3(1, 0.1f, 1));
                    break;
                case SequenceHolder.HandleType.SmallCilinder:
                    var rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                    Gizmos.matrix = rotationMatrix;
                    Gizmos.DrawMesh(mesh, Vector3.zero, Quaternion.identity, new Vector3(.5f, 0.05f, .5f));
                    break;
            }
        }
    }
}

