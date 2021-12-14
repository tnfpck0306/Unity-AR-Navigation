using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace ArchToolkit.AnimationSystem
{
    [System.Serializable]
    [ExecuteInEditMode]
    public abstract class AInteractable : MonoBehaviour
    {
        [SerializeField]
        public Action onDestroy;
        public string UniqueID;
        public enum StartWithType
        {
            onclick,
            onstart,
            waitcall
        }
        public enum LoopType
        {
            none,
            loop,
            pingpong,
        }
        public StartWithType StartWith;
        public bool loop = false;
        public LoopType loopType = LoopType.none;

        public List<GameObject> TargetList = new List<GameObject>();
        public List<AInteractable> CallAfterComplete = new List<AInteractable>();

        public abstract void StartAnimation();

        protected void Start()
        {
#if UNITY_EDITOR
            EditorUpdateManager.Instance.AddToUpdate(this.EditorUpdate);
#endif
            if (this.StartWith == StartWithType.onstart)
            {
                this.StartAnimation();
            }
        }

        protected abstract void EditorUpdate();

        public abstract bool AnimationOn();

        protected virtual void Update()
        {

        }

        public virtual void CompleteAnimation()
        {
            //RIABILITA COLLIDER
            foreach (var inter in CallAfterComplete)
            {
                inter.StartAnimation();
            }
        }


        public bool isHandleAlreadySetted
        {
            get
            {
                return basicHandle != null;
            }
        }

        public ArchBasicHandle GetHandleSetted
        {
            get
            {
                return basicHandle;
            }
        }

        [SerializeField]
        protected ArchBasicHandle basicHandle;

        public int ID;

        public virtual ArchBasicHandle SetHandle(Vector3 position)
        {
            return null;
        }


        private void OnDestroy()
        {
#if UNITY_EDITOR
            EditorUpdateManager.Instance.RemoveFromUpdate(this.EditorUpdate);
#endif

            if (this.onDestroy != null)
                this.onDestroy();
        }


    }
}