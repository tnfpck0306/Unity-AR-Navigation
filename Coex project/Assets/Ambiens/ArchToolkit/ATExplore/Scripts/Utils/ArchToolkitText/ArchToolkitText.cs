using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit
{

    public class ArchToolkitText
    {
        public const string PLUGIN_VERSION = "V 1.4.6";

        public const string FORGOTMATERIALTOSWITCH = "You forgot to add the material to change";

        public const string MATERIALSWITCHWRONG = "Your material to change,not exist in the materials list of the object";

        public const string SELECT_GAMEOBJECT = "You forgot to select a gameobject";

        public const string ROTATION_PREFIX = "Rot";

        public const string TRANSLATION_PREFIX = "Tr";

        public const string APPLY = "Apply";

        public const string ADD_ROTATION = "Rotate";

        public const string ADD_COLLIDER_TOOLTIP = "Add a Collider to this object to make it blockable";

        public const string ROTATION_TOOLTIP = "Make swing doors open";

        public const string TRANSLATION_TOOLTIP = "Make sliding doors and drawers open";

        public const string SWITCH_MATERIAL_TOOLTIP = "Give the user the chance to have different material options";
        
        public const string DRAGGABLE_TOOLTIP = "Make the element movable by the user";

        public const string VR_MOBILE_TELEPORT_POINT_TOOLTIP = "Give the user the chance to set different teleport point (ONLY MOBILE VR)";

        public const string ADD_TARGET_ANIMATION_TOOLTIP = "Add Object to Interaction";

        public const string NEXT_INTERACTION_TOOLTIP = "This interaction will be automatically activated as soon as the previous one has been completed";

        public const string FIND_ALL_MATERIALS_TOOLTIP = "Apply the interaction to all the objects having this material assigned";

        public const string REMOVE_INTERACTION_TOOLTIP = "Remove interaction from the selected object";

        public const string APPLY_SWITCH_MATERIAL_TOOLTIP = "Apply the interaction and then put the brush icon where you want it to be showned up in the scene";

        public const string ADD_TRANSLATION = "Translate";

        public const string REMOVE_ANIMATION = "Remove Animation";

        public const string ADD_SWITCH_MATERIAL = "Switch material";
        
        public const string ADD_DRAGGABLE = "Add draggable";
        public const string REMOVE_DRAGGABLE = "Remove draggable";


        public const string ADD_ACTION_SEQUENCE_MATERIAL = "Add Action Sequence";


        public const string ADD_TELEPORT_POINT = "Add teleport point";

        public const string ADD_SEQUENCE_HOLDER = "Add Action Sequence";
        public const string OPEN_SEQUENCE = "Open sequence";


        public const string ADD_ANIMATION_TARGET = "+";

        public const string NEXT_INTERACTION = "Next interaction";

        public const string WALKABLE_LAYERS_TOOLTIP = "Attention: this will be affected on the movement";

        public const string MATERIAL_FOLDOUT_NAME = "Materials Options";

        public const string MATERIAL_OPTIONS_TOOLTIP = "Add here all the materials' object you need to switch ";

        public const string REMOVE_SWITCH_INTERACTION = "Remove interaction";

        public const string WELCOME_TEXT = "Hello! Thank you for buying Archtoolkit Beta: It will help you in the creation of interactive " +
                                           "architectural environments without coding. Lots of new features coming soon " +
                                           "Here(link) you can find our Tutorial Playlist." + 
                                           "If you need any help: info @ambiensvr.com";

        public const string XR_LEGACY_NOT_INSTALLED = "You need to install the XR Legacy Input Helpers when using Unity 2019. Go to Window -> Package Manager -> XR Legacy Input Helpers and click on Install. For more information read QuickStart.pdf";

        public const string TOM_BUTTON = "Create starting point (TOM)";

        public const string TOM_BUTTON_TOOLTIP = "Tom defines starting point and direction of the camera";

        public const string ITERATE_MATERIAL = "Find all materials";

        public const string REVERT_MATERIAL = "Revert";

    }
}
