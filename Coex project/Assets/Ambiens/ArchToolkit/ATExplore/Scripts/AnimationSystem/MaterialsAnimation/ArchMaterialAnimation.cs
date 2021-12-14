using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit.InputSystem;
using System;
using System.Linq;

namespace ArchToolkit.AnimationSystem
{

    [Serializable]
    public class ArchMaterialAnimation : AInteractable
    {
        [SerializeField]
        public List< SwitchMaterialInfo> SwitchInfo = new List<SwitchMaterialInfo>();

        [SerializeField]
        public List<Variant> variants = new List<Variant>();

        protected void OnChange(Variant variant)
        {
            if (variant == null || variant.linkedObject == null)
                return;

            List<Material> materials = new List<Material>();

            var matVariant = variant.linkedObject as Material;

            for (int rendererCount = 0; rendererCount < this.SwitchInfo.Count; rendererCount++)
            {
                var currentRenderer = this.SwitchInfo[rendererCount];

                if (currentRenderer.meshRenderer == null)
                    continue;

                foreach (var mat in currentRenderer.meshRenderer.sharedMaterials)
                {
                    materials.Add(mat);
                }

                materials[currentRenderer.matIndex] = matVariant;
                
                currentRenderer.meshRenderer.sharedMaterials = materials.ToArray();

                currentRenderer.materialToChange = matVariant;

                materials.Clear();
            }
        }

       
        [ExecuteInEditMode]
        protected void OnDestroy()
        {
            this.variants.Clear();

            this.variants = null;

            if (this.basicHandle != null)
                GameObject.DestroyImmediate(this.basicHandle.gameObject);
        }

        public override ArchBasicHandle SetHandle(Vector3 position)
        {
            var handle = new GameObject("ArchAnimHandle_" + this.gameObject.name);

            handle.transform.localScale = new Vector3(0.1f,0.1f,0.1f);

            this.basicHandle = handle.AddComponent<ArchBasicHandle>();

            var spRend = handle.gameObject.AddComponent<SpriteRenderer>();

            if (spRend != null)
            {
                spRend.sprite = Resources.Load<Sprite>("Handles/icon_product");
            }
            //Deprecated 20200420
            //var handleGizmo = handle.gameObject.AddComponent<ArchGizmos.ArchMaterialHandleGizmo>();
            //handleGizmo.GizmoinGame = handle.gameObject;

            //handle.transform.position = position;

            this.basicHandle.animationToOpen = this;

            var sphereCollider = handle.AddComponent<SphereCollider>();

            sphereCollider.radius = 1;

            sphereCollider.isTrigger = true;

            handle.tag = ArchToolkitDataPaths.ARCHINTERACTABLETAG;

            return this.basicHandle;
        }

        public override void StartAnimation()
        {
            ImageSelectedDialog.Instance.InitMenu(0, this.variants, this.OnChange, null,
                this.basicHandle.transform
                /*new Vector3(0.015f, 0.015f, 0.015f)*/);
        }

        public override bool AnimationOn()
        {
            return false;
        }
        
        protected override void EditorUpdate()
        {
            
        }
    }
}
