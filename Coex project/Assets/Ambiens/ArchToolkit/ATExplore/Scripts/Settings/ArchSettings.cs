using ArchToolkit.Character;
using UnityEngine;
using System;
using ArchToolkit.BuildSystem;

namespace ArchToolkit.Settings
{
    [CreateAssetMenu(fileName = "Settings",menuName = "ArchToolkit/AT+Explore/Create settings")]
    public class ArchSettings : ScriptableObject
    {
        public bool StartAutomatically = true;

        public MovementType movementType = MovementType.FlyCam; // Default is Flycam

        public MobileMovementType mobileMovementType = MobileMovementType.Swipe;

        public LockMovementTo lockMovementTo = LockMovementTo.None; // Default is None

        public LayerMask walkableLayers = 1;

        public float RunSpeed = 4.5f;

        public float RunFaster = 15;

        public float movementSpeed = 2;

        public float clumbSpeed = 4;

        public float MouseRotationSpeed = 120;
        public float TouchRotationSpeed = 20;
        public bool UseRightClickToRotate = false;

        public float maxGazeTimer = 1;
        
        public MovementTypePerPlatform movementTypePerPlatform = MovementTypePerPlatform.FullScreen360;
        
        public Action OnExit=null;

        public Action ExitCallbackOverride=null;

        public bool UIShowTouchControls = true;
        public bool UIShowControlsGuide = true;

        public SOBuildTemplate buildTemplate;

        public bool EnableVRSmoothMovement = false;
        public float VRFadeTime = 0.3f;
    }
}
