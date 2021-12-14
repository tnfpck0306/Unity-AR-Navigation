using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ArchToolkit.MenuSystem{
    public abstract class MenuElementBase 
    {
        public abstract string GetPath();

        public abstract GameObject GetUI(RectTransform parent);
    }
}