using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ArchToolkit.Character
{
    public abstract class CustomArchCharacter : ArchVRCharacter
    {
        public abstract void Init(MovementTypePerPlatform movementType);
    }
}