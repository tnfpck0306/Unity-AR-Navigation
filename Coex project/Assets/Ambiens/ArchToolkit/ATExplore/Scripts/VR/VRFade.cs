using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit.Utils;

namespace ArchToolkit.VR
{

    public class VRFade : SingletonPrefab<VRFade>
    {
        [SerializeField]
        private Material fadeMaterial;

        private void Awake()
        {
            this.fadeMaterial = this.GetComponent<MeshRenderer>().sharedMaterials[0];
        }

        public virtual void StartFade(Vector3 spawnPosition,float time, Color fadeColor)
        {
            this.transform.position = spawnPosition;

            StartCoroutine(this.Fade(time,fadeColor));
        }

        protected virtual IEnumerator Fade(float time, Color fadeColor)
        {
            if (this.fadeMaterial == null)
                yield return null;

            var startColor = this.fadeMaterial.color;

            this.fadeMaterial.color = fadeColor;
            
            yield return new WaitForSecondsRealtime(time);

            GameObject.DestroyImmediate(this.gameObject);
        }
    }
}