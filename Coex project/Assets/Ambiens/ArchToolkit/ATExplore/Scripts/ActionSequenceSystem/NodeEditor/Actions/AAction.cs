using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{

    public abstract class AAction : AActionNodeBase
    {
        [Input]
        public AActionNodeBase Caller;

        [HideInInspector]
        public List<GameObject> SceneReferences = new List<GameObject>();
        [HideInInspector]
        public GameObjectReferenceHolder ReferenceHolder;

        public void StartAction()
        {
            if (this._StartAction())
                this.OnComplete();
            else{
                this.GetSequenceHolder().ManagedUpdate -= this.ManagedUpdate;
                this.GetSequenceHolder().ManagedUpdate += this.ManagedUpdate;
            }
        }
        protected abstract bool _StartAction();

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "Caller") return this;
            return null;
        }

        public bool InitSceneReferences()
        {
            var holder = GameObject.FindObjectOfType<SequenceHolder>();
            if (holder == null)
            {
                Debug.LogError("There's no sequenceHolder in the scene!");
                return false;
            }
            this.ReferenceHolder = holder.RequestGameObjectReferences(this.ID);
            this.SceneReferences = this.ReferenceHolder.gameObjects;
            return true;
        }

        protected void CallCallback(string name)
        {
            var port = this.GetOutputPort(name);
            if (port.IsConnected)
            {
                for (int i = 0; i < port.ConnectionCount; i++)
                {
                    var p = port.GetConnection(i);
                    if (p != null)
                    {
                        AAction next = (AAction)p.node;
                        next.RuntimeInit();
                        next.StartAction();
                    }
                }
            }
        }
    }
}
