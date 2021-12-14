using ArchToolkit.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit
{
    public abstract class ArchToolkitManagerSceneOverride : MonoBehaviour
    {
        public abstract void LoadVisitor(Vector3 pos, Quaternion quat, Action<GameObject> onComplete);
    }
}