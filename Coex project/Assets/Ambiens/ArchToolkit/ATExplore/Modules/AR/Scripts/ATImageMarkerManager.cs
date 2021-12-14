using ArchToolkit.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ATE_AR
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#endif
namespace ambiens.archtoolkit.atexplore.ar
{


    public class ATImageMarkerManager : MonoBehaviour
    {
#if ATE_AR
        [SerializeField]
        [Tooltip("Image manager on the AR Session Origin")]
        ARTrackedImageManager m_ImageManager;

        public ARAnchorManager m_AnchorManager;
        /// <summary>
        /// Get the <c>ARTrackedImageManager</c>
        /// </summary>
        public ARTrackedImageManager ImageManager
        {
            get => m_ImageManager;
            set => m_ImageManager = value;
        }

        [SerializeField]
        [Tooltip("Reference Image Library")]
        XRReferenceImageLibrary m_ImageLibrary;

        /// <summary>
        /// Get the <c>XRReferenceImageLibrary</c>
        /// </summary>
        public XRReferenceImageLibrary ImageLibrary
        {
            get => m_ImageLibrary;
            set => m_ImageLibrary = value;
        }
        
        ATImageMarkerReference[] VirtualReference;
        Pose[] poses;
        ARAnchor[] anchors;
        public ArchARCharacter arChar;
        public ARSessionOrigin sOrigin;

        public ATMarkerlessObjectManager m_markerlessManager;

        void OnEnable()
        {
            var virtualImageReferences = GameObject.FindObjectsOfType<ATImageMarkerReference>();
            if (virtualImageReferences.Length>0)
            {
                VirtualReference = new ATImageMarkerReference[this.ImageLibrary.count];
                poses = new Pose[this.ImageLibrary.count];
                anchors = new ARAnchor[this.ImageLibrary.count];
                arChar = this.GetComponentInParent<ArchARCharacter>();

                foreach (var virtualImage in virtualImageReferences)
                {
                    VirtualReference[virtualImage.imageIndex] = virtualImage;
                    //virtualImage.DisableImage();
                }

                m_ImageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;
            }
            else
            {
                m_markerlessManager.enabled = true;
                this.enabled = false;
                this.m_ImageManager.enabled = false;
            }
            
        }

        void OnDisable()
        {
            m_ImageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;
        }

        int GetIndexOfTrackerByImage(ARTrackedImage img)
        {
            int index = 0;
            foreach (var i in this.ImageLibrary)
            {
                if (i.guid == img.referenceImage.guid)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
        {
            if (isAligning) return;

            // added, spawn prefab
            foreach (ARTrackedImage image in obj.added)
            {
                if (image.trackingState == TrackingState.Tracking)
                {
                    int index = GetIndexOfTrackerByImage(image);
                    //Debug.Log("new IMG " + index + " " + image.trackingState);
                    if (index != -1)
                    {
                        if (VirtualReference[index] != null)
                            this.ManageAlignment(index, image);
                    }
                }
            }

            // updated, set prefab position and rotation
            foreach (ARTrackedImage image in obj.updated)
            {
                if (image.trackingState == TrackingState.Tracking)
                {
                    int index = GetIndexOfTrackerByImage(image);
                    
                    if (index != -1)
                    {
                        if (VirtualReference[index] != null)
                            this.ManageAlignment(index, image); 
                    }
                }
            }

            // removed, destroy spawned instance
            foreach (ARTrackedImage image in obj.removed)
            {
                //int index = GetIndexOfTrackerByImage(image);
            }
        }

        bool isAligning = false;
        void ManageAlignment(int index, ARTrackedImage image)
        {
            isAligning = true;
            
            Pose p = poses[index];
            ARAnchor a = anchors[index];
            if (a == null)
            {
                p = new Pose(image.transform.position, GetFixedRotation( image.transform.localRotation));
                poses[index] = p;
                a = this.m_AnchorManager.AddAnchor(p);
                anchors[index] = a;
            }
            else
            {
                p.position = image.transform.position;
                p.rotation = GetFixedRotation(image.transform.localRotation);
            }

            if (!this.arChar.root.gameObject.activeSelf)
                this.arChar.root.gameObject.SetActive(true);
            
            isAligning = false;
        }
        private Quaternion GetFixedRotation(Quaternion original)
        {
            var euler = original.eulerAngles;
            euler.x = 0;
            euler.z = 0;

            return Quaternion.Euler(euler);
        }
        private void Update()
        {
            for(int index=0; index<anchors.Length; index++)
            {
                if (anchors[index] != null)
                {
                    
                    this.sOrigin.MakeContentAppearAt(this.VirtualReference[index].transform, 
                        anchors[index].transform.position,
                        anchors[index].transform.localRotation);

                    this.arChar.positionOffset = this.VirtualReference[index].transform.position - anchors[index].transform.position;

                }
            }
        }
#endif
    }
}
