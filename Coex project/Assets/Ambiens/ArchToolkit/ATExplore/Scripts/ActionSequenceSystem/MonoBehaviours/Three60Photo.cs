using ArchToolkit.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{

    public class Three60Photo : SingletonPrefab<Three60Photo>
    {

        public LayerMask sphereMask;
        private LayerMask normalMask;
        public Renderer sphereRenderer;

        public void Initialize()
        {
            ArchToolkit.ArchToolkitManager.Instance.visitor.Teleport(this.transform.position);
            ArchToolkit.ArchToolkitManager.Instance.visitor.LockPosition();
            ArchToolkit.ArchToolkitManager.Instance.visitor.SetVisibleLayer(this.sphereMask);

        }
        public void SetTexture(Texture2D txt)
        {
            sphereRenderer.material.mainTexture = txt;
        }

        public void ClosePhoto()
        {
            ArchToolkit.ArchToolkitManager.Instance.visitor.UnlockPosition();
            ArchToolkit.ArchToolkitManager.Instance.visitor.ResetVisibleLayers();

            Destroy(this.gameObject);
        }
    }

}