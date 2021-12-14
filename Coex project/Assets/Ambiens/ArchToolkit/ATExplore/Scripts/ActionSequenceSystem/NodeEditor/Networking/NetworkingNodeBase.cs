using ambiens.archtoolkit.atexplore.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    public abstract class NetworkingNodeBase : AAction
    {
        [Input]
        public string OnlineUrl;

        [Output]
        public AAction OnLoadComplete;
        [Output]
        public AAction OnLoadStart;
        [Output]
        public AAction OnLoadError;
        [Output]
        public AAction OnLoadProgress;

        [Output]
        public float progress;

        protected UnityWebRequest uwr;
        protected UnityWebRequestAsyncOperation operation;
        protected string m_LocalCacheUrl;



        protected override bool _StartAction()
        {
            this.StartLoading();
            return false;
        }

        private void StartLoading()
        {
            Debug.Log(this.name);

            var p = this.GetInputPort("OnlineUrl");
            if (p.IsConnected)
            {
                this.OnlineUrl = this.GetInputValue<string>("OnlineUrl");
            }
            Debug.Log("Downloading " + this.OnlineUrl);

            if (!mustCheckCache())
            {
                uwr = UnityWebRequest.Get(OnlineUrl);
                //uwr.downloadHandler = new DownloadHandlerFile(localUrl);
                this.operation = uwr.SendWebRequest();
                
                this.OnStartCallback();
            }
            else
            {
                this.OnStartCallback();
                FileAssetCache.FetchCachedAssetFromUrl(
                    OnlineUrl,
                    (UnityWebRequestAsyncOperation o, UnityWebRequest req) =>
                    {
                        this.uwr = req;
                        this.operation = o;
                    },
                    (string LocalUrl) =>
                    {
                        Debug.Log("Complete! =>" + LocalUrl);
                        m_LocalCacheUrl = LocalUrl;
                        this.OnCompleteCallback();
                        
                    },
                    (string error) =>
                    {

                    }
                    );
            }
        }

        public override void ManagedUpdate(float deltaTime)
        {
            if (this.uwr == null || this.operation == null ) return;

            if (mustCheckCache())
            {
                this.progress = this.operation.progress;
                this.OnProgressCallback();
            }
            else
            {
                //Debug.Log("ManagedUpdate");
                if (this.operation.isDone)
                {
                    if (this.uwr.error == null)
                        this.OnCompleteCallback();
                    else
                        this.OnErrorCallback();
                }
                else
                {
                    this.progress = this.operation.progress;
                    this.OnProgressCallback();
                }
            }
            
        }
        protected abstract bool mustCheckCache();
        protected abstract bool _onComplete();
        protected abstract void _onStart();
        protected abstract void _onProgress();
        protected abstract void _onError();

        private void OnProgressCallback()
        {
            this._onProgress();
            this.CallCallback("OnLoadProgress");
        }

        private void OnCompleteCallback()
        {
            Debug.Log("ON COMPLETE request");

            if (this._onComplete())
            {
                this.CallCallback("OnLoadComplete");
                this.uwr = null;
                this.operation = null;
            }
            else
            {
                this.OnErrorCallback();
            }

        }
        private void OnStartCallback()
        {
            Debug.Log("Start request");
            this._onStart();
            this.CallCallback("OnLoadStart");
        }
        private void OnErrorCallback()
        {
            Debug.Log("ERROR");

            this._onError();
            this.CallCallback("OnLoadError");
            this.uwr = null;
            this.operation = null;
        }

       
    }
}