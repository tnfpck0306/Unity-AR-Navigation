using ambiens.archtoolkit.atexplore.XNode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Open Scene from Asset Bundle")]
    public class OpenSceneAB : AAction
    {
        [Input]
        public string LocalUrl;

        public bool LoadAsync;
        public LoadSceneMode loadSceneMode = LoadSceneMode.Single;
        private AsyncOperation loadOperation;
        private AssetBundleCreateRequest abRequest;

        [Output]
        public AAction OnLoadProgress;

        [Output]
        public float progress;
        
        protected override void _RuntimeInit()
        {
            if (this.abRequest != null)
            {
                this.abRequest.assetBundle.Unload(true);
                this.abRequest = null;
            }
            
            this.loadOperation = null;

            AssetBundle.UnloadAllAssetBundles(false);
        }

        protected override bool _StartAction()
        {
            AssetBundle.UnloadAllAssetBundles(false);

            var p = this.GetInputPort("LocalUrl");
            if (p.IsConnected)
            {
                this.LocalUrl = this.GetInputValue<string>("LocalUrl");
            }
            
            this.abRequest = AssetBundle.LoadFromFileAsync(this.LocalUrl);
            abRequest.completed += OnCompleteLoadAB;

            return false;
        }

        void OnCompleteLoadAB(AsyncOperation op)
        {
            abRequest.completed -=OnCompleteLoadAB;
            try
            {
                var assetBundleScenes = abRequest.assetBundle.GetAllScenePaths();
                if (LoadAsync)
                {
                    this.loadOperation = SceneManager.LoadSceneAsync(assetBundleScenes[0], this.loadSceneMode);
                }
                else
                {
                    SceneManager.LoadScene(assetBundleScenes[0], this.loadSceneMode);
                    abRequest.assetBundle.Unload(true);
                }


            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                this.OnComplete();
            }
        }

        public override void ManagedUpdate(float deltaTime)
        {
            if (this.abRequest != null)
            {
                progress = this.abRequest.progress;
                this.CallCallback("OnLoadProgress");
            }
            if (this.loadOperation != null)
            {
                progress = this.loadOperation.progress;
                this.CallCallback("OnLoadProgress");
            }
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "progress") return this.progress;
            return null;
        }
    }
}
