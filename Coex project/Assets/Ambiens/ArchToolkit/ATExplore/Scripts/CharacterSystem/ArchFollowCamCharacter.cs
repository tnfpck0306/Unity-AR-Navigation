using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ArchToolkit.InputSystem;
using UnityEngine.XR;

namespace ArchToolkit.Character
{


	public class ArchFollowCamCharacter : ArchCharacterBase
    {
        
        [Header("Input")]
        public InputRaycaster raycaster;


        [Space]
        [Space]

        public Transform cameraToFollow;


        protected virtual void Awake()
        {
            var cam = GameObject.FindObjectOfType<Camera>();
            this.cameraToFollow = cam.transform;
            this.raycaster.currentCamera = cam;
            ArchToolkitManager.Instance.settings.OnExit += this.OnExit;

        }


        protected virtual void OnExit()
        {
           // this.SetCursor(CursorLockMode.None, true);
        }


        protected override void Update()
        {
            if (this.cameraToFollow == null)
            {
                //Debug.LogError("Head is null,maybe you forgot to link it, in the inspector");
                return;
            }
            this.transform.position = this.cameraToFollow.position;
            this.transform.rotation = this.cameraToFollow.rotation;

        }

    }
}
