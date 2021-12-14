using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit.Utils.Texture
{
    public static class TextureUtils
    {
        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        public static Texture2D MakeScreenShotFromMaterial(this Material mat,int resX,int resY)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            go.transform.position = new Vector3(999,9999,999);

            go.GetComponent<MeshRenderer>().materials = new Material[] { mat };

            var goCam = new GameObject("tempCam");

            var cam = goCam.AddComponent<Camera>();

            cam.clearFlags = CameraClearFlags.SolidColor;

            cam.backgroundColor = Color.gray;
            
            goCam.transform.position = new Vector3(999, 9999, 997.35f);
            
            goCam.transform.LookAt(go.transform);
            
            RenderTexture rt = new RenderTexture(resX, resY, 24);
            cam.Render();
            cam.targetTexture = rt;
            cam.Render();

            Texture2D screenShot = new Texture2D(resX, resY, TextureFormat.ARGB32, false);
            cam.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resX, resY), 0, 0);

            cam.targetTexture = null;
            RenderTexture.active = null;

            GameObject.DestroyImmediate(rt);
            GameObject.DestroyImmediate(go);
            GameObject.DestroyImmediate(goCam);

            return screenShot;
        }
    }
}
