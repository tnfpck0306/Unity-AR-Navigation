using System;
using UnityEngine;

namespace ArchToolkit.AnimationSystem
{
    [Serializable]
    public class SwitchMaterialInfo
    {
        public Material materialToChange;

        public MeshRenderer meshRenderer;

        public int matIndex;

        public bool isDefaultMaterial;

        public SwitchMaterialInfo(MeshRenderer meshRenderer, Material materialToChange, int matIndex, bool isDefaultMaterial = false)
        {
            this.meshRenderer = meshRenderer;
            this.matIndex = matIndex;
            this.materialToChange = materialToChange;
            this.isDefaultMaterial = isDefaultMaterial;
        }
    }
}