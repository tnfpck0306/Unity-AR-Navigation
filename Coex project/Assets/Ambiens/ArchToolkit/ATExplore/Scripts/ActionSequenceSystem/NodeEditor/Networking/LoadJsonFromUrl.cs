using ambiens.archtoolkit.atexplore.XNode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Networking/Load Json From Url")]
    public class LoadJsonFromUrl : NetworkingNodeBase
    {

        public enum outputType
        {
            stringlist,
        }
        
        public outputType OutPutType;

        [Serializable]
        public class ListRequestContainer<T>
        {
            [SerializeField]
            public List<T> list;
        }

        [Output]
        public List<string> stringListOutput;

        protected override void _RuntimeInit()
        {
            this.stringListOutput.Clear();
        }

        protected override bool _onComplete()
        {
            string sOutput = this.uwr.downloadHandler.text;
            Debug.Log(sOutput);
            //Json->list
            try
            {
                this.stringListOutput = JsonUtility.FromJson<ListRequestContainer<string>>(sOutput).list;

                return true;
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        protected override void _onStart()
        {
            
        }

        protected override void _onProgress()
        {
            Debug.Log(this.name +" p:" + progress);
        }

        protected override void _onError()
        {
            
        }

        public override object GetValue(NodePort port)
        {

            if (port.fieldName == "stringListOutput") return this.stringListOutput;
            if (port.fieldName == "progress") return this.progress;
            return null;
        }

        protected override bool mustCheckCache()
        {
            return false;
        }
    }
}
