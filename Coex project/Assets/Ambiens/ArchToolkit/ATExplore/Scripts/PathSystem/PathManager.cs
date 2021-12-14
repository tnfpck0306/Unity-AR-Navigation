using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace ArchToolkit.Navigation
{
    [Serializable]
    public class PathManager
    {

        public List<PathPoint> PathPoints
        {
            get
            {
                this._pathPoints.RemoveAll(go => go == null);

                foreach (var pp in GameObject.FindObjectsOfType<PathPoint>())
                {
                    if (pp == null)
                        continue;

                    if (this._pathPoints.Contains(pp))
                        continue;

                    this._pathPoints.Add(pp);
                }

                return this._pathPoints;
            }
        }
        
        [SerializeField]
        private List<PathPoint> _pathPoints = new List<PathPoint>();

        private GameObject pathPointsContainer
        {
            get
            {
                if (this._pathPointsContainer != null)
                    return this._pathPointsContainer;
                else
                {
                    this._pathPointsContainer = GameObject.Find("Path Container");
                }

                return this._pathPointsContainer;
            }
            set
            {
                this._pathPointsContainer = value;
            }
        }

        private GameObject _pathPointsContainer;
   
        public void SetStartingPoint()
        {
#if UNITY_EDITOR
            
            // Check if pathlist is empty
            if (this.PathPoints != null && this.PathPoints.Count == 0)
            {
                this.CheckContainer();

                var tom = GameObject.Instantiate(Resources.Load<GameObject>(ArchToolkitDataPaths.RESOURCES_TOM_PATH),this.pathPointsContainer.transform);


                Selection.activeObject = SceneView.lastActiveSceneView;
                Camera sceneCam = SceneView.lastActiveSceneView.camera;
                if (sceneCam != null)
                {
                    Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 5));
                    tom.transform.position = spawnPos;
                    tom.transform.rotation = Quaternion.Euler(0,sceneCam.transform.rotation.eulerAngles.y,0);
                }
                else
                    tom.transform.position = Vector3.one;

                var path = tom.AddComponent<PathPoint>();

                if (path != null)
                    path.Init(0);

                if(!this._pathPoints.Contains(path))
                    this._pathPoints.Add(path);

                Selection.activeGameObject = tom;
            }
            
            // TODO: Create entry point for adjacent points.
#else
            // TODO: Maybe we need to replace tom with arrow or point.
#endif
        }
        
        private void CheckContainer()
        {
            if (this.pathPointsContainer == null)
            {
                this.pathPointsContainer = new GameObject("Path Container");

                this.pathPointsContainer.transform.position = Vector3.zero;
            }
        }
#if UNITY_EDITOR
        public void AddPoint()
        {
            this.CheckContainer();

            var point = GameObject.Instantiate(Resources.Load<GameObject>(ArchToolkitDataPaths.RESOURCES_TELEPORT_POINT));

            Selection.activeObject = SceneView.lastActiveSceneView;
            Camera sceneCam = SceneView.lastActiveSceneView.camera;

            if(this.pathPointsContainer != null)
            {
                point.transform.SetParent(this.pathPointsContainer.transform);
            }

            if (sceneCam != null)
            {
                Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 5));
                point.transform.position = spawnPos;
            }
            else
                point.transform.position = Vector3.one;

            var path = point.AddComponent<PathPoint>();

            var handle = point.AddComponent<AnimationSystem.ArchBasicHandle>();

            handle.animationToOpen = point.AddComponent<AnimationSystem.ArchTeleportAnimation>();

            if (path != null)
                path.Init(this.PathPoints.Count + 1);

            if (!this._pathPoints.Contains(path))
                this._pathPoints.Add(path);

            Selection.activeGameObject = point;
        }
#endif
        public void EnableAllPoint(bool enable)
        {
            foreach (var path in this.PathPoints)
            {
                if (path == null)
                    continue;

                if (path.ID == 0)
                    continue;

                path.gameObject.SetActive(enable);
            }
        }
    }
}