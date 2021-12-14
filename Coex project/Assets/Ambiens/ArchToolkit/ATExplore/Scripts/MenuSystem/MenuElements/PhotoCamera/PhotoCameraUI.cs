using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotoCameraUI : MonoBehaviour
{
    public Camera cam;
    public RenderTexture text;
    public Transform followelement;

    public static string DestinationPath = "";

    public static Action OnStartTakingPhoto;

    public static Action<string> OnCompleteSavePhoto;
    public static Action<Texture2D, string > SaveOverride;


    private void Start() 
    {
        this.followelement=ArchToolkit.ArchToolkitManager.Instance.visitor.Head.transform;
    }
    private void Update() 
    {
        this.transform.position = Vector3.Lerp(this.transform.position, this.followelement.position + this.followelement.forward * 0.5f, 0.1f);
        this.transform.forward = Vector3.Lerp(this.transform.forward, this.followelement.forward, 0.1f);
    }
    
    public void TakeScreenShot()
    {
        DestinationPath =
#if UNITY_EDITOR
        Application.dataPath.Replace("Assets", "");
#else
            Application.persistentDataPath;
#endif

        if (OnStartTakingPhoto != null) OnStartTakingPhoto();

        string folderPath = DestinationPath + "ATE_Photo";
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        this.RenderAndSaveImg(this.cam, null, folderPath, (string path) =>
        {
            Debug.Log(path);
            if (OnCompleteSavePhoto != null) OnCompleteSavePhoto(path);
            this.Close();
        },(string error)=>
        {
            Debug.Log(error);
            if (OnCompleteSavePhoto != null) OnCompleteSavePhoto(null);
            this.Close();
        });
    }

    public void RenderAndSaveImg(Camera cam, string name, string path, Action<string> OnComplete, Action<string> OnError, int width = -1, int height = -1)
    {
        if (width == -1) width = cam.pixelWidth;
        if (height == -1) height = cam.pixelHeight;
        try
        {
            RenderTexture rt = new RenderTexture(width, height, 24);
            cam.targetTexture = rt;
            Texture2D dest = new Texture2D(width, height, TextureFormat.RGB24, false);
            cam.Render();
            RenderTexture.active = rt;
            dest.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null;
            GameObject.DestroyImmediate(rt);

            if (name == null)
                name = getName(width, height);
            string fLoc = path + "/" + name + ".png";

            if (SaveOverride != null)
            {
                SaveOverride(dest, name+".png");
            }
            else
                System.IO.File.WriteAllBytes(fLoc, dest.EncodeToPNG());

            if (OnComplete != null) OnComplete(fLoc);
        }
        catch(Exception e)
        {
            if (OnError != null) OnError(e.Message);
        }
    }
    private string getName(int width, int height)
    {
        return string.Format("{0}x{1}_{2}",
                          width, height,
                          System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
    public void Close(){
        Destroy(this.gameObject);
    }
}
