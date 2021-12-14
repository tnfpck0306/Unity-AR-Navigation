using ambiens.archtoolkit.atexplore.XNode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Networking/Download or get Cached File")]
    public class DownloadGetCachedFile : NetworkingNodeBase
    {
        
        [Output]
        public string LocalCacheUrl;

        protected override bool _onComplete()
        {
            Debug.Log("Complete download file " + m_LocalCacheUrl);
            this.LocalCacheUrl = m_LocalCacheUrl;
            return true;
        }

        protected override void _onStart()
        {

        }

        protected override void _onProgress()
        {
            //Debug.Log(this.name +" p:" + progress);
        }

        protected override void _onError()
        {

        }

        protected override bool mustCheckCache()
        {
            return true;
        }

        public override object GetValue(NodePort port)
        {

            if (port.fieldName == "LocalCacheUrl") return this.LocalCacheUrl;
            if (port.fieldName == "progress") return this.progress;
            return null;
        }

        protected override void _RuntimeInit()
        {
            this.LocalCacheUrl = "";
        }
    }
}
