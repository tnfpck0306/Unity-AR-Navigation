using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Open Scene")]
    public class OpenScene : AAction
    {

        public string sceneName;
        protected override void _RuntimeInit()
        {

        }

        protected override bool _StartAction()
        {

            SceneManager.LoadScene(this.sceneName);
            return true;
        }

    }
}
