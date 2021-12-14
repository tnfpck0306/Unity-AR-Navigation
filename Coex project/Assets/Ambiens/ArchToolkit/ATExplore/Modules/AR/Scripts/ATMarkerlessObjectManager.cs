using ArchToolkit.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ATE_AR
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#endif
namespace ambiens.archtoolkit.atexplore.ar
{

    public class ATMarkerlessObjectManager : MonoBehaviour
    {
#if ATE_AR

        public ARRaycastManager m_RaycastManager;

        public ARAnchorManager m_AnchorManager;

        public ArchARCharacter arChar;
        public ARSessionOrigin sOrigin;

        private float targetScale = 1;

        public float ScaleStep = .1f;
        public float ScaleMax = 1;
        public float ScaleMin = 1;

        public bool ScaleEnabled = false;

        void OnEnable()
        {
            this.targetScale = this.sOrigin.transform.localScale.y;
        }

        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                var mousePosition = Input.mousePosition;
                touchPosition = new Vector2(mousePosition.x, mousePosition.y);
                return true;
            }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

            touchPosition = default;
            return false;
        }

        bool Scaling = false;
        bool Rotating = false;


        float EPSILON = 0.1f;

        void Update()
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
            {
                this.Rotating = false;
                this.Scaling = false;
                return;
            }

            if (Input.touchCount == 2)
            {
                if (TouchRotation())
                {
                    sOrigin.MakeContentAppearAt(this.arChar.root, Quaternion.AngleAxis(this.rotationAmount, Vector3.up));
                }
                else if (ScaleEnabled)
                {
                    if (TouchScale())
                    {
                        if (System.Math.Abs(this.targetScale - this.sOrigin.transform.localScale.y) > EPSILON)
                        {
                            this.sOrigin.transform.localScale = Vector3.one * Mathf.Lerp(this.sOrigin.transform.localScale.y, this.targetScale, 0.1f);
                        }
                    }
                }
                    
            }
            else if ( 
                !(this.Scaling && this.Rotating) 
                && Input.touchCount==1 
                && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Ended))
            {
                if(m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    var hitPose = s_Hits[0].pose;

                    if (!this.arChar.root.gameObject.activeSelf)
                    {
                        this.arChar.root.gameObject.SetActive(true);

                    }

                    sOrigin.MakeContentAppearAt(this.arChar.root, hitPose.position);
                }
            }
            

        }
        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        /// The squared rotation width determining an rotation
        public const float TOUCH_ROTATION_WIDTH = 0.1f; // Always
                                                        /// The threshold in angles which must be exceeded so a touch rotation is recogniced as one
        public const float TOUCH_ROTATION_MINIMUM = 1;
        /// Start vector of the current rotation
        Vector2 startVector = Vector2.zero;
        float rotationAmount;
        private float startTouchDistance = 0;
        private float scaleDistanceMin = 0;
        private float currentTouchDistance = 0;

        /// Processes input for touch rotation, only the first two touches are used
        private bool TouchRotation()
        {
            if (Input.touchCount == 2)
            {
                if (!this.Rotating)
                {
                    startVector = Input.touches[1].position - Input.touches[0].position;
                    this.startTouchDistance = this.startVector.sqrMagnitude;
                    this.scaleDistanceMin = this.startTouchDistance * 0.3f;

                    this.Rotating = startVector.sqrMagnitude > TOUCH_ROTATION_WIDTH;
                }
                else
                {
                    Vector2 currVector = Input.touches[1].position - Input.touches[0].position;
                    float angleOffset = Vector2.Angle(startVector, currVector);
                    this.currentTouchDistance = currVector.sqrMagnitude;

                    if (angleOffset > TOUCH_ROTATION_MINIMUM)
                    {
                        Vector3 LR = Vector3.Cross(startVector, currVector); // z > 0 left rotation, z < 0 right rotation

                        if (LR.z > 0)
                            this.rotationAmount -= angleOffset;
                        else if (LR.z < 0)
                            this.rotationAmount += angleOffset;

                        startVector = currVector;

                        return true;
                    }

                }
            }
            //else
            //    this.Rotating = false;

            return false;
        }
        private bool TouchScale()
        {
            if (Input.touchCount == 2)
            {
                //Manage Scaling
                float val = (currentTouchDistance - startTouchDistance);
                if (val > scaleDistanceMin || val < -scaleDistanceMin)
                {
                    this.manageScale(currentTouchDistance);
                    this.startTouchDistance = currentTouchDistance;
                    //this.scaleDistanceMin = this.startTouchDistance * 0.3f;
                    this.Scaling = true;
                    return true;
                }
            }
            //else
            //    this.Scaling = false;

            return false;
        }


        void manageScale(float currSqrMagnitude)
        {

            if (currSqrMagnitude < startTouchDistance)
            {
                this.targetScale = Mathf.Clamp(this.targetScale + ScaleStep, ScaleMin, ScaleMax);
            }
            else
            {
                this.targetScale = Mathf.Clamp(this.targetScale - ScaleStep, ScaleMin, ScaleMax);
            }
        }


#endif
    }
}