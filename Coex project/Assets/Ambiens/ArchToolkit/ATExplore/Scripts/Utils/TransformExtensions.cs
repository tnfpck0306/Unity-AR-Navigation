using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ambiens.archtoolkit
{
    public static class TransformExtensions
    {
        public static Bounds EncapsulateBounds(this Transform transform, bool checkVisibility = false)
        {
            Bounds bounds = new Bounds();
            bool firstBounds = true;
            var renderers = transform.GetComponentsInChildren<Renderer>();
            if (renderers != null && renderers.Length > 0)
            {
                for (var i = 1; i < renderers.Length; i++)
                {
                    var renderer = renderers[i];
                    if (!checkVisibility || (checkVisibility && renderer.gameObject.activeInHierarchy))
                    {
                        if (firstBounds)
                        {
                            bounds = renderer.bounds;
                            firstBounds = false;
                        }
                        else
                        {
                            bounds.Encapsulate(renderer.bounds);

                        }
                    }
                }
            }
            else
            {
                bounds = new Bounds();
            }
            return bounds;
        }
    }

}
