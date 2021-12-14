using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{

    [CreateNodeMenuAttribute("Variables/Switch Between Platforms")]

    public class SwitchCasePlatform : AAction
    {

        [Output]
        public AAction OnAndroid;
        [Output]
        public AAction OnIOS;
        [Output]
        public AAction OnStandaloneDesktop;
        [Output]
        public AAction OnWebGL;

        protected override void _RuntimeInit()
        {
           
        }

        protected override bool _StartAction()
        {
#if UNITY_ANDROID
            this.CallCallback("OnAndroid");
#elif UNITY_IOS
            this.CallCallback("OnIOS");
#elif UNITY_STANDALONE
            this.CallCallback("OnStandaloneDesktop");
#elif UNITY_WEBGL
            this.CallCallback("OnWebGL");
#endif

            return true;
        }
    }

}