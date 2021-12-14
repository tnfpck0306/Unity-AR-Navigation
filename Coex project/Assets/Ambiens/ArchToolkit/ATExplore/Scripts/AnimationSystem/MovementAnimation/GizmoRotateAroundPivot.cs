using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit.AnimationSystem
{

    public class GizmoRotateAroundPivot : MonoBehaviour
    {

        public Vector3 position;
        public Vector3 direction;
        public float amount;
        public Transform myT;
        public Transform directionTrans;

        void OnDrawGizmos()
        {
            if (myT == null) myT = this.transform;

            if (this.myT == null && this.directionTrans == null)
                return;

            var vectorDirection = directionTrans.position - this.myT.position;
            this.amount = vectorDirection.magnitude;
            this.myT.up = vectorDirection.normalized;

            var rotationMatrix = Matrix4x4.TRS(myT.position, myT.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;
            //Gizmos.DrawMesh(mesh, Vector3.zero, Quaternion.identity, new Vector3(0.4f, 0.4f, 0.4f));
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Vector3.zero, 0.1f);


            var rotationMatrix1 = Matrix4x4.TRS(myT.position + this.myT.up * this.amount / 2, myT.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix1;
            //   Gizmos.color = new Color32(238, 233, 66, 200);

            Gizmos.DrawCube(Vector3.zero, new Vector3(0.01f, this.amount, 0.01f));

            this.position = this.myT.position;
            this.direction = vectorDirection;
        }

    }
}
