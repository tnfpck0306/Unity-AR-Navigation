#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ArchToolkit
{
    [Serializable]
    public class EditorUpdateManager 
    {
        [SerializeField]
        private Action EditorUpdate;

        public static Action OnCreation;

        [SerializeField]
        public int ID = 0;

        [SerializeField]
        private List<Action> actionsAdded = new List<Action>();

        [SerializeField]
        public static EditorUpdateManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EditorUpdateManager();

                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        private static EditorUpdateManager _instance;

        public static bool IsInstanced()
        {
            return _instance != null;
        }

        public static GameObject AnimationLogics
        {
            get
            {
                if(_animationLogics == null)
                {
                    _animationLogics = GameObject.Find(ArchToolkitDataPaths.AVR_LOGICS_NAME);

                    if(_animationLogics == null)
                    {
                        _animationLogics = new GameObject(ArchToolkitDataPaths.AVR_LOGICS_NAME);
                    }
                }

                return _animationLogics;
            }

            set
            {
                _animationLogics = value;
            }
        }

        public static GameObject AnimationContainer
        {
            get
            {
                if (_animationContainer == null)
                {
                    _animationContainer = GameObject.Find(ArchToolkitDataPaths.ANIMATION_CONTAINER);

                    if(_animationContainer == null)
                    {
                        _animationContainer = new GameObject(ArchToolkitDataPaths.ANIMATION_CONTAINER);

                        _animationContainer.transform.SetParent(AnimationLogics.transform);
                    }
                }

                return _animationContainer;
            }

            set
            {
                _animationContainer = value;
            }
        }
        
        public static GameObject MaterialSwitchContainer
        {
            get
            {
                if (_materialSwitchContainer == null)
                {
                    _materialSwitchContainer = GameObject.Find(ArchToolkitDataPaths.MATERIAL_CONTAINER);

                    if (_materialSwitchContainer == null)
                    {
                        _materialSwitchContainer = new GameObject(ArchToolkitDataPaths.MATERIAL_CONTAINER);

                        _materialSwitchContainer.transform.SetParent(AnimationLogics.transform);
                    }
                }

                return _materialSwitchContainer;
            }

            set
            {
                _materialSwitchContainer = value;
            }
        }

        [SerializeField]
        private static GameObject _animationLogics;
        [SerializeField]
        private static GameObject _animationContainer;
        [SerializeField]
        private static GameObject _materialSwitchContainer;


        public EditorUpdateManager()
        {
            if (_instance != null)
            {
                Debug.Log("Editor Update Manager Already exist");
                return;
            }
            if (this.ID == 0)
                EditorApplication.update += Update;

            if (this.ID == 0)
            {
                this.ID = new System.Random().Next(150, 9999999);
            }

            Instance = this;

            if (OnCreation != null)
                OnCreation();

        }

        public bool IsActionInQueue(Action action)
        {
            return (this.actionsAdded.Contains(action)) ;
        }

        public void AddToUpdate(Action action)
        {
            if (!this.actionsAdded.Contains(action))
            {
                this.actionsAdded.Add(action);
                this.EditorUpdate += action;
            }
        }


        public void RefreshAllActions()
        {
            if (this.actionsAdded.Count == 0)
                return;


            this.EditorUpdate = null;

            foreach (var action in this.actionsAdded)
            {
                if(action == null)
                {
                    continue;
                }

                this.EditorUpdate += action;
            }

            EditorApplication.update -= Update;
            EditorApplication.update += Update;
        }

        public void RemoveFromUpdate(Action action)
        {
            if (this.actionsAdded.Contains(action))
            {
                this.actionsAdded.Remove(action);
                this.EditorUpdate -= action;
            }
        }

        private void Update()
        {
            if (EditorApplication.isPlaying || EditorApplication.isPaused)
                return;

            if (this.EditorUpdate != null)
                this.EditorUpdate();
        }

    }
}
#endif