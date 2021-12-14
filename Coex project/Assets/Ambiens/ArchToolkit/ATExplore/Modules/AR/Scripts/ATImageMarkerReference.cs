using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
#if ATE_AR
using UnityEngine.XR.ARSubsystems;
#endif

namespace ambiens.archtoolkit.atexplore.ar
{
    public class ATImageMarkerReference : MonoBehaviour
    {
#if ATE_AR
        [SerializeField]
        [Tooltip("Reference Image Library")]
        XRReferenceImageLibrary m_ImageLibrary;

        [SerializeField]
        public int imageIndex=0;

        [SerializeField]
        [Tooltip("Reference Runtime Image")]
        MeshRenderer img;

        string matName = "Tracker_";

        public bool UpdateMat = false;

        public void DisableImage()
        {
            this.img.gameObject.SetActive(false);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (UpdateMat)
            {
                UpdateMat = false;

                if (!string.Equals(img.sharedMaterial.name, (matName + imageIndex)))
                {
                    
                    var found = AssetDatabase.FindAssets((matName + imageIndex) + " t:Material");

                    if (found.Length > 0)
                    {
                        img.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(found[0]));
                    }
                    else
                    {
                        var found2 = AssetDatabase.FindAssets("Tracker_NULL");

                        if (found2.Length > 0)
                        {
                            img.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(found2[0]));
                        }
                    }
                }
                this.img.transform.localScale = new Vector3(this.m_ImageLibrary[imageIndex].width, this.m_ImageLibrary[imageIndex].height, 1);
            }
            
        }
#endif
#endif
    }
}

