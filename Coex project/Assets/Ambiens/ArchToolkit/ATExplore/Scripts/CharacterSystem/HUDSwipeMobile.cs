using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ArchToolkit.Character
{

    public class HUDSwipeMobile : HUDJoystickMobile
    {
        public static Vector3 TranslateVector = Vector3.zero;


        public void SetTranslateVectorToForward()
        {
            TranslateVector = Vector3.forward;
        }

        public void SetTranslateVectorToBackward()
        {
            TranslateVector = Vector3.back;
        }

        public void SetTranslateVectorToZero()
        {
            TranslateVector = Vector3.zero;
        }

    }
}