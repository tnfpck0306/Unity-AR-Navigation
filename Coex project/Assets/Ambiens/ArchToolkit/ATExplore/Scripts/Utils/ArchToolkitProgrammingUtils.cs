using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ArchToolkit.Utils
{

    public static class ArchToolkitProgrammingUtils
    {
        private static Vector3[] raycastRay = new Vector3[4];

        private static float SafeWallThresholds = 0.25f;

#if UNITY_EDITOR
        public static Vector3 FrontOfEditorCamera()
        {
            Selection.activeObject = SceneView.lastActiveSceneView;
            Camera sceneCam = SceneView.lastActiveSceneView.camera;
            if (sceneCam != null)
            {
                Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 2));

                return spawnPos;
            }

            return Vector3.one;
        }
#endif

        public static Bounds GetBounds(GameObject go)
        {
            var list = new List<GameObject>();

            list.Add(go);

            for (int i = 0; i < go.transform.childCount; i++)
            {
                list.Add(go.transform.GetChild(i).gameObject);
            }

            return GetBounds(list);
        }

        public static Bounds GetBounds(List<GameObject> list)
        {
            MeshRenderer rend = null;
            Bounds bounds = new Bounds();

            foreach (var gO in list)
            {
                rend = gO.GetComponent<MeshRenderer>();

                if (rend != null)
                {
                    bounds = CalculateBoundingBox(rend.gameObject);
                    //bounds.Encapsulate( rend.bounds);
                }
                else
                {
                    foreach (MeshRenderer t in gO.GetComponentsInChildren<MeshRenderer>())
                    {
                        bounds = CalculateBoundingBox(t.gameObject);
                    }
                }

            }


            return bounds;
        }

        private static Bounds CalculateBoundingBox(GameObject aObj)
        {
            if (aObj == null)
            {
                //Debug.LogError("CalculateBoundingBox: object is null");
                return new Bounds(Vector3.zero, Vector3.one);
            }
            Transform myTransform = aObj.transform;
            Mesh mesh = null;
            MeshFilter mF = aObj.GetComponent<MeshFilter>();
            if (mF != null)
                mesh = mF.sharedMesh;
            else
            {
                SkinnedMeshRenderer sMR = aObj.GetComponent<SkinnedMeshRenderer>();
                if (sMR != null)
                    mesh = sMR.sharedMesh;
            }
            if (mesh == null)
            {
                Debug.LogError("CalculateBoundingBox: no mesh found on the given object");
                return new Bounds(aObj.transform.position, Vector3.one);
            }
            Vector3[] vertices = mesh.vertices;
            if (vertices.Length <= 0)
            {
                Debug.LogError("CalculateBoundingBox: mesh doesn't have vertices");
                return new Bounds(aObj.transform.position, Vector3.one);
            }
            Vector3 min, max;
            min = max = myTransform.TransformPoint(vertices[0]);
            for (int i = 1; i < vertices.Length; i++)
            {
                Vector3 V = myTransform.TransformPoint(vertices[i]);
                for (int n = 0; n < 3; n++)
                {
                    if (V[n] > max[n])
                        max[n] = V[n];
                    if (V[n] < min[n])
                        min[n] = V[n];
                }
            }
            Bounds B = new Bounds();
            B.SetMinMax(min, max);
            return B;
        }

        public static bool CanTeleport(RaycastHit hit, float maxAngleDegree=20f)
        {
            RaycastHit hit1;

            bool teletportHitCheck = true;
            //Debug.Log(Vector3.Angle(hit.normal, Vector3.up));
            if (Mathf.Abs(Vector3.Angle(hit.normal, Vector3.up)) < maxAngleDegree)
            {
                Vector3 RayOrigin = hit.point + hit.normal * SafeWallThresholds;//new Vector3 (hit.point.x, hit.point.y + SafeWallThresholds, hit.point.z);

                raycastRay[0] = new Vector3(hit.point.x + SafeWallThresholds, hit.point.y - 0.1f, hit.point.z);
                raycastRay[1] = new Vector3(hit.point.x - SafeWallThresholds, hit.point.y - 0.1f, hit.point.z);
                raycastRay[2] = new Vector3(hit.point.x, hit.point.y - 0.1f, hit.point.z + SafeWallThresholds);
                raycastRay[3] = new Vector3(hit.point.x, hit.point.y - 0.1f, hit.point.z - SafeWallThresholds);

                foreach (var v in raycastRay)
                {
                    if (Physics.Linecast(RayOrigin, v, out hit1))
                    {
                        
                        if (Mathf.Abs(Vector3.Angle(hit1.normal, Vector3.up)) < maxAngleDegree)
                        {
                            teletportHitCheck = true;
                        }
                        else
                        {
                            teletportHitCheck = false;
                        }
                    }
                    else
                    {
                        teletportHitCheck = false;
                    }
                }
            }
            else
                teletportHitCheck = false;

            return teletportHitCheck;
        }
        
    }
}
