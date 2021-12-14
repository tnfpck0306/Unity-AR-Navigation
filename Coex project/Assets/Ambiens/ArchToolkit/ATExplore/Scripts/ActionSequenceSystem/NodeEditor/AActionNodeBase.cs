using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{

    public abstract class AActionNodeBase : Node
    {
        [Output(dynamicPortList = true)]
        public List<AAction> Next=new List<AAction>();
        [SerializeField]
        [HideInInspector]
        private string __ID = null;
        public string ID {
            get{
                
                if (String.IsNullOrEmpty(__ID))
                {
                    this.__ID=this.name+"-"+RandomString(10);
                }
                foreach(var n in this.graph.nodes)
                {
                    if(n!= this)
                    {
                        if(n.GetType() == typeof(AActionNodeBase))
                            if (((AActionNodeBase)n).__ID == this.__ID)
                                this.__ID = this.name + "-" + RandomString(10);
                    }
                }
                return __ID;
            }

        }
        
        public void RuntimeInit()
        {
            _RuntimeInit();
            foreach (var n in this.GetNext())
                n.RuntimeInit();
            _AfterSequenceRuntimeInit();
        }

        public virtual void ManagedUpdate(float deltaTime)
        {
            
        }

        protected abstract void _RuntimeInit();
        protected virtual void _AfterSequenceRuntimeInit()
        {
        }

        protected void StartNext()
        {
            foreach (var n in this.GetNext())
                n.StartAction();
        }

        public List<AAction> GetNext()
        {
            var toReturn = new List<AAction>();
            var count = this.Next.Count;

            for (var i = 0; i < count; i++)
            {
                var port = GetOutputPort("Next " + i);
                if (port != null && port.IsConnected)
                {
                    for (int j = 0; j < port.ConnectionCount; j++)
                    {
                        var p = port.GetConnection(j);
                        if (p != null)
                        {
                            toReturn.Add((AAction)p.node);
                        }

                    }
                    //toReturn.Add((AAction)port.GetConnection(0).node);
                }

            }
            return toReturn;
        }

        public void OnComplete()
        {
            this.GetSequenceHolder().ManagedUpdate -= this.ManagedUpdate;
            this.StartNext();
        }
        public List<T> GetDynInput<T>(List<T> inputList, string name)
        {
            var count = inputList.Count;
            var DynList = new List<T>();
            for (int i = 0; i < count; i++)
            {
                if (this.GetInputPort(name + " " + i).IsConnected)
                    DynList.Add(this.GetInputValue<T>(name + " " + i));
                else DynList.Add(inputList[i]);
            }
            return DynList;
        }

        public ActionSequence GetActionSequence()
        {
            return (ActionSequence)this.graph;
        }
        public SequenceHolder GetSequenceHolder(){
            return GameObject.FindObjectOfType<SequenceHolder>();
        }

        private static System.Random random = new System.Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
