using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Show 360 Photo")]
    public class Show360Photo : AAction
    {
        [HideInInspector]
        public Vector3 tpTargetPosition;
        [Input]
        public Texture2D texture;
        [SerializeField]
        public float yRotation;
        [SerializeField]
        public bool invertYTiling;
        protected override void _RuntimeInit()
        {

        }

        protected override bool _StartAction()
        {

            Three60Photo.Instance.transform.position = this.tpTargetPosition;

            var txt = this.GetInputValue<Texture2D>("texture");
            if (txt == null) txt = texture;
            
            Three60Photo.Instance.SetTexture(txt);
            Three60Photo.Instance.Initialize();

            Three60Photo.Instance.transform.Rotate(Vector3.up, yRotation);
            if(invertYTiling)
                Three60Photo.Instance.transform.localScale=new Vector3(-1,1,1);
            return true;
        }

    }
}
