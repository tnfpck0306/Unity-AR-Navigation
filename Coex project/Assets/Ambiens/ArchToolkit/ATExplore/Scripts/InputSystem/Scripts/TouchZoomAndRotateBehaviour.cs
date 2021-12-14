using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit.InputSystem
{
    public class TouchZoomAndRotateBehaviour 
    {
        /// Flag set to true if the user currently makes an rotation gesture, otherwise false
        protected bool rotating = false;
        /// The squared rotation width determining an rotation
        public const float TOUCH_ROTATION_WIDTH = 1; // Always
                                                     /// The threshold in angles which must be exceeded so a touch rotation is recogniced as one
        public const float TOUCH_ROTATION_MINIMUM = 1;
        /// Start vector of the current rotation
        Vector2 startVector = Vector2.zero;
        protected float rotationAmount = 0;
        protected float latestRotationTiming = 0;
        protected float startTouchDistance = 0;
        protected float scaleDistanceMin = 0;
        public float RotationAmount
        {
            get
            {
                return rotationAmount;
            }
        }

        protected float targetScale = 100;
        protected float ScaleStep = 10;
        protected float ScaleMax = 100;
        protected float ScaleMin = 10;

        protected float rotationMax = 2;
        /// Processes input for touch rotation, only the first two touches are used
        public bool ManageTouchGestures()
        {
            if (Input.touchCount == 2)
            {
                if (!rotating)
                {
                    this.startVector = Input.touches[1].position - Input.touches[0].position;
                    this.startTouchDistance = this.startVector.sqrMagnitude;
                    this.scaleDistanceMin = this.startTouchDistance * 0.3f;
                    rotating = this.startVector.sqrMagnitude > TOUCH_ROTATION_WIDTH;
                }
                else
                {
                    Vector2 currVector = Input.touches[1].position - Input.touches[0].position;
                    float angleOffset = Vector2.Angle(this.startVector, currVector);

                    var currentTouchDistance = currVector.sqrMagnitude;

                    //Manage rotation
                    if (angleOffset > TOUCH_ROTATION_MINIMUM)
                    {
                        Vector3 LR = Vector3.Cross(this.startVector, currVector); // z > 0 left rotation, z < 0 right rotation

                        if (LR.z > 0)
                            rotationAmount += angleOffset;
                        else if (LR.z < 0)
                            rotationAmount -= angleOffset;

                        rotationAmount = Mathf.Clamp(rotationAmount, -rotationMax, rotationMax);

                        //GameController.Instance.mainCamera.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
                        this.startVector = currVector;
                    }

                    //Manage Scaling
                    float val = (currentTouchDistance - startTouchDistance);
                    if (val > scaleDistanceMin || val < -scaleDistanceMin)
                    {
                        this.manageScale(currentTouchDistance);
                        this.startTouchDistance = currentTouchDistance;
                        this.scaleDistanceMin = this.startTouchDistance * 0.3f;
                    }

                }
                latestRotationTiming = Time.time;
            }
            else
                rotating = false;

            if (rotating == false)
            {
                if (Time.time - latestRotationTiming > 0.1f)
                {
                    rotationAmount = 0;

                    return false;
                }
                else return true;
            }
            return rotating;
        }

        void manageScale(float currSqrMagnitude)
        {

            if (currSqrMagnitude > startTouchDistance)
            {
                this.targetScale = Mathf.Clamp(this.targetScale - ScaleStep, ScaleMin, ScaleMax);
            }
            else
            {
                this.targetScale = Mathf.Clamp(this.targetScale + ScaleStep, ScaleMin, ScaleMax);
            }


        }

    }

}
