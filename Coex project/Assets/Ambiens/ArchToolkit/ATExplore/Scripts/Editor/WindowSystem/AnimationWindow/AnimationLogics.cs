using ArchToolkit.AnimationSystem;
using UnityEngine;
using UnityEditor;
using ArchToolkit.Utils;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ArchToolkit.Editor.Window
{
    public class AnimationLogics
    {
        
        public static void SnapTranslationToObject(TranslationDirection direction,
                                               List<GameObject> TargetList,
                                               GameObject pivotDir,
                                               GameObject pivotPos,
                                               Transform relativeTo = null)
        {
            if (pivotDir == null || pivotPos == null)
            {
                Debug.LogWarning("Pivot null");
                return;
            }
            if (SceneView.lastActiveSceneView.camera == null)
            {
                Debug.LogWarning("Scene view camera null");
                return;
            }

            float startLenght = 0.5f;

            var pivotDirection = Vector3.zero;
            var pivotPosition = Vector3.zero;



            var bounds = ArchToolkitProgrammingUtils.GetBounds(TargetList);


            var parent = pivotPos.transform.parent.gameObject;

            if (parent != null)
            {
                parent.transform.position = bounds.center;
            }

            if (direction == TranslationDirection.forward || direction == TranslationDirection.backward)
            {
                pivotDirection = Vector3.forward;

                float distanceFromForward = (relativeTo.position - pivotPos.transform.position).sqrMagnitude;
                pivotPosition = Vector3.zero;
                float distanceFromCenter = (relativeTo.position - pivotPos.transform.position).sqrMagnitude;

                if (distanceFromForward >= distanceFromCenter)
                {
                    if (direction == TranslationDirection.forward)
                        direction = TranslationDirection.backward;
                    else
                        direction = TranslationDirection.forward;
                }
            }

            switch (direction)
            {
                case TranslationDirection.forward:
                    pivotDirection = Vector3.forward * startLenght;
                    break;

                case TranslationDirection.backward:

                    pivotDirection = Vector3.back * startLenght;

                    break;

                case TranslationDirection.right:

                    pivotDirection = Vector3.right * startLenght;


                    break;

                case TranslationDirection.left:

                    pivotDirection = Vector3.left * startLenght;

                    break;

                case TranslationDirection.up:

                    pivotDirection = Vector3.up * startLenght;


                    break;

                case TranslationDirection.down:

                    pivotDirection = Vector3.down * startLenght;

                    break;

                default:
                    break;
            }


            pivotDir.transform.localPosition = pivotDirection;
            pivotPos.transform.localPosition = pivotPosition;

        }

        public static void SnapRotationToObject(RotationDirection direction,
                                            List<GameObject> TargetList,
                                            GameObject pivotDir,
                                            GameObject pivotPos)
        {
            if (pivotDir == null || pivotPos == null)
            {
                Debug.LogWarning("Pivot null");
                return;
            }
            if (SceneView.lastActiveSceneView.camera == null)
            {
                Debug.LogWarning("Scene view camera null");
                return;
            }

            var bounds = ArchToolkitProgrammingUtils.GetBounds(TargetList);


            var parent = pivotPos.transform.parent.gameObject;

            if (parent != null)
            {
                parent.transform.position = bounds.center;
            }


            if (bounds.size.x > bounds.size.z) //la x è ok
            {
                float minxLeftRight = 0;
                if (direction == RotationDirection.left)
                {
                    if (SceneView.lastActiveSceneView.camera.WorldToScreenPoint(bounds.min).x < SceneView.lastActiveSceneView.camera.WorldToScreenPoint(bounds.max).x)
                        minxLeftRight = bounds.min.x;
                    else
                        minxLeftRight = bounds.max.x;
                }
                else if (direction == RotationDirection.right)
                {
                    if (SceneView.lastActiveSceneView.camera.WorldToScreenPoint(bounds.min).x < SceneView.lastActiveSceneView.camera.WorldToScreenPoint(bounds.max).x)
                        minxLeftRight = bounds.max.x;
                    else
                        minxLeftRight = bounds.min.x;
                }

                //Debug.Log("1");
                switch (direction)
                {
                    case RotationDirection.left:
                        pivotDir.transform.position = new Vector3(minxLeftRight, bounds.max.y, bounds.max.z);
                        pivotPos.transform.position = new Vector3(minxLeftRight, bounds.min.y, bounds.max.z);
                        break;
                    case RotationDirection.right:
                        pivotDir.transform.position = new Vector3(minxLeftRight, bounds.max.y, bounds.max.z);
                        pivotPos.transform.position = new Vector3(minxLeftRight, bounds.min.y, bounds.max.z);
                        break;
                    case RotationDirection.up:
                        pivotDir.transform.position = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
                        pivotPos.transform.position = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
                        break;
                    case RotationDirection.down:
                        pivotDir.transform.position = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
                        pivotPos.transform.position = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
                        break;
                    default:
                        break;
                }

                //TargetList[0].transform.position = TargetList[0].transform.TransformDirection(TargetList[0].transform.position);
            }

            else if (bounds.size.z > bounds.size.x)
            {
                //Debug.Log("2");

                float minzLeftRight = 0;
                if (direction == RotationDirection.right)
                {
                    if (SceneView.lastActiveSceneView.camera.WorldToScreenPoint(bounds.min).x > SceneView.lastActiveSceneView.camera.WorldToScreenPoint(bounds.max).x)
                        minzLeftRight = bounds.min.z;
                    else minzLeftRight = bounds.max.z;
                }
                else if (direction == RotationDirection.left)
                {
                    if (SceneView.lastActiveSceneView.camera.WorldToScreenPoint(bounds.min).x > SceneView.lastActiveSceneView.camera.WorldToScreenPoint(bounds.max).x)
                        minzLeftRight = bounds.max.z;
                    else minzLeftRight = bounds.min.z;
                }

                switch (direction)
                {
                    case RotationDirection.left:
                        pivotDir.transform.position = new Vector3(bounds.max.x, bounds.max.y, minzLeftRight);
                        pivotPos.transform.position = new Vector3(bounds.max.x, bounds.min.y, minzLeftRight);
                        break;

                    case RotationDirection.right:
                        pivotDir.transform.position = new Vector3(bounds.max.x, bounds.max.y, minzLeftRight);
                        pivotPos.transform.position = new Vector3(bounds.max.x, bounds.min.y, minzLeftRight);
                        break;

                    case RotationDirection.up:
                        pivotDir.transform.position = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
                        pivotPos.transform.position = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
                        break;

                    case RotationDirection.down:
                        pivotDir.transform.position = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
                        pivotPos.transform.position = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
                        break;

                    default:

                        break;
                }
            }

            else if (bounds.size.z == bounds.size.x)
            {
                pivotDir.transform.position = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
                pivotPos.transform.position = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            }

        }
    }
}