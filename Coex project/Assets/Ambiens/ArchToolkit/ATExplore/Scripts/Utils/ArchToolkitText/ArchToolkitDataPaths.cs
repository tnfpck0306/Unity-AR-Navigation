using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit
{

    public static class ArchToolkitDataPaths
    {
        public const string ARCHINTERACTABLETAG = "ArchInteractable";

        public const string DEFAULT_TAG = "Untagged";

        public static string LAYERPATH = Application.dataPath + "/ArchToolkit/Resources/Text/Layer.json";

#if !UNITY_EDITOR
        public const string RESOURCESLAYERPATH = "Text/Layer";
#else 
        public const string RESOURCESLAYERPATH = "Text/Layer.json";
#endif

        public const string RESOURCESMATERIALHANDLE = "Handles/MaterialHandle";

#if UNITY_EDITOR
        public const string RESOURCESEDITORADDICON = "UI/Add";
        public const string RESOURCESEDITORMINUSICON = "UI/Minus";
        public const string RESOURCESEDITORPICKICON = "UI/picker";
        public const string RESOURCESEDITORCAMERAICON = "UI/camera";
#endif
        public const string ARCH_SETTINGS = "Settings/Settings";

        public const string AVR_LOGICS_NAME = "AVR Logics";

        public const string MATERIAL_CONTAINER = "Material Container";

        public const string ANIMATION_CONTAINER = "Animation Container";

        public const string RESOURCEPATH_ROTATION_ARROW = "Handles/RotArrow";

        public const string RESOURCESPATH_TRANSLATE_ARROW = "Handles/TranslateArrow";

        public const string RESOURCES_HUD_PATH = "UI/ArchHUD";

        public const string RESOURCES_EVENTSYSTEM = "UI/EventSystem";

        /*v1.3 deprecation ->todelete
        public const string RESOURCES_VIVE_CONTROLLER = "VR/HTC Vive/Controller";

        public const string RESOURCES_OCULUS_CONTROLLER_LEFT = "VR/Oculus/LeftController/Controller";

        public const string RESOURCES_WMX_CONTROLLER_RIGHT = "VR/WindowsMixedReality/RightController/Controller";

        public const string RESOURCES_WMX_CONTROLLER_LEFT = "VR/WindowsMixedReality/LeftController/Controller";

        public const string RESOURCES_OCULUS_CONTROLLER_RIGHT = "VR/Oculus/RightController/Controller";

        public const string RESOURCES_OCULUS_QUEST_S_CONTROLLER_RIGHT = "VR/QuestRiftS/RightController/Controller";
        public const string RESOURCES_OCULUS_QUEST_S_CONTROLLER_LEFT = "VR/QuestRiftS/RightController/Controller";

        public const string RESOURCES_COSMOS_CONTROLLER_RIGHT = "VR/Cosmos/vive_cosmos_controller_right_v1";
        public const string RESOURCES_COSMOS_CONTROLLER_LEFT = "VR/Cosmos/vive_cosmos_controller_left_v1";

        public const string RESOURCES_OCULUS_GO_CONTROLLER = "VR/OculusGo/Controller";
        */
        public const string RESOURCES_MOBILE_HUD_JOYSTICK_PATH = "UI/ArchHUDMobileJoystick";

        public const string RESOURCES_MOBILE_HUD_SWIPE_PATH = "UI/ArchHUDMobileSwipe";

        public const string RESOURCES_HUD_VR_MOBILE_PATH = "UI/ArchHUDVRMobile";

        public const string RESOURCES_CHARACTER_PREFAB = "Visitor";

        public const string RESOURCES_CHARACTER_VR_PREFAB = "VisitorVR";

        public const string RESOURCES_CHARACTER_AR_PREFAB = "VisitorAR";

        public const string RESOURCES_CHARACTER_FOLLOWCAM_PREFAB = "VisitorFollowCam";
        
        public const string RESOURCES_TELEPORT_POINT = "Handles/TeleportPoint";

        public const string RESOURCES_TOM_PATH = "Tom";

        public const string RESOURCESPLUGINLOGO = "UI/ATExploreLogo512x128";

    }
}
