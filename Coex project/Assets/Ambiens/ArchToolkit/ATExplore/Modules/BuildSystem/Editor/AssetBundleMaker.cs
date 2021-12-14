using System.Linq;
using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace ArchToolkit
{
    public enum AssetBundleState { none, inRunning, Finished };
    public class AssetBundlesMenuItems
    {

        public PlatformProprierty android = new PlatformProprierty();
        public PlatformProprierty ios = new PlatformProprierty();
        // public PlatformProprierty webGL = new PlatformProprierty();
        public PlatformProprierty pc = new PlatformProprierty();

        static public List<PlatformProprierty> boolList = new List<PlatformProprierty>();
        static public List<SceneProprierty> sceneList = new List<SceneProprierty>();
        private static List<BuildTarget> buildTargets = new List<BuildTarget>();
        private static string path;

        public void InitializeBundle()
        {

            boolList.Clear();
            buildTargets.Clear();
            sceneList.Clear();

            android.Name = "Android";
            //webGL.Name = "WebGL";
            ios.Name = "iOS";
            pc.Name = "Windows & Mac";

            android.target = BuildTarget.Android;
            android.bGroup = BuildTargetGroup.Android;
            ios.bGroup = BuildTargetGroup.iOS;
            ios.target = BuildTarget.iOS;
            //webGL.target = BuildTarget.WebGL;
#if UNITY_EDITOR_OSX
            pc.target = BuildTarget.StandaloneOSX;
#elif UNITY_EDITOR_WIN
            pc.target = BuildTarget.StandaloneWindows;
            pc.bGroup = BuildTargetGroup.Standalone;
#endif
            android.Value = PlayerPrefs.GetInt("value" + android.Name) == 1 ? true : false;
            ios.Value = PlayerPrefs.GetInt("value" + ios.Name) == 1 ? true : false;
            pc.Value = PlayerPrefs.GetInt("value" + pc.Name) == 1 ? true : false;
            //webGL.Value = PlayerPrefs.GetInt("value" + webGL.Name) == 1 ? true : false; // se vero true else false

            if (AssetBundlesMenuItems.boolList.Count == 0)
            {
                AssetBundlesMenuItems.boolList.Add(android);
                //AssetBundlesMenuItems.boolList.Add(webGL);
                AssetBundlesMenuItems.boolList.Add(ios);
                AssetBundlesMenuItems.boolList.Add(pc);
            }

            this.GetSceneList();


        }

        public void GetSceneList()
        {
            sceneList.Clear();

            foreach (var scene in AssetDatabase.FindAssets("t:scene", null))
            {
                string path = AssetDatabase.GUIDToAssetPath(scene);
                var sceneName = Path.GetFileNameWithoutExtension(path).ToLower().Replace(" ", "_");

                //AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(scene)).SetAssetBundleNameAndVariant(Path.GetFileName(r).Replace(".unity", ""), "hd.avrb");
                SceneProprierty sceneProprierty = new SceneProprierty();
                sceneProprierty.key = scene;
                sceneProprierty.path = Path.GetFullPath(path);
                sceneProprierty.Name = sceneName;
                sceneProprierty.Value = PlayerPrefs.GetInt("scene" + sceneProprierty.Name) == 1 ? true : false;
                sceneList.Add(sceneProprierty);
            }
        }

        private void GetBuildableScenePath(SceneProprierty scene)
        {

            if (scene.Value)
            {

                string fileName = scene.Name;


                if (Regex.IsMatch(fileName, @"\s"))
                {
                    fileName = fileName.Replace(" ", "_");
                }

                AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(scene.key)).SetAssetBundleNameAndVariant(fileName, "hd.avrb");

            }
            else if (!scene.Value)
            {
                AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(scene.key)).SetAssetBundleNameAndVariant("none".Replace(".unity", ""), "None");
            }
        }

        private static void DeleteDirectory(string target_dir)
        {
            try
            {
                string[] files = Directory.GetFiles(target_dir);
                string[] dirs = Directory.GetDirectories(target_dir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(target_dir, false);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning(e.Message);
            }

        }

        public void StartBuildAssetBundle(BuildAssetBundleOptions type)
        {
            try
            {
                path = Application.dataPath + "/AssetBundles";
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning(e.Message);

            }


            var found = false;

            for (var i = 0; i < boolList.Count; i++)
            {
                if (boolList[i].Value)
                {
                    string app = ((BuildTarget)i).ToString();
                    buildTargets.Add(boolList[i].target);
                    //Debug.Log("Build Target: " + boolList[i].target);
                    found = true;
                }
            }
            if (found) // se trova degli elementi dentro target list
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

                foreach (var item in sceneList)
                {
                    GetBuildableScenePath(item);
                }

                BuildAssetBundles(type);
            }
        }

        private static void BuildAssetBundles(BuildAssetBundleOptions type)
        {

            foreach (var element in boolList)
            {
                PlayerPrefs.SetInt("value" + element.Name, element.Value ? 1 : 0);
            }
            foreach (var item in sceneList)
            {
                PlayerPrefs.SetInt("scene" + item.Name, item.Value ? 1 : 0);
            }
            PlayerPrefs.Save();

            foreach (var build in boolList)
            {
                if (build.Value)
                    BuildAssetBundles(build.target, type);
            }


            EndAssetBundle();

        }

        private static void EndAssetBundle()
        {
            DeleteFiles();


            if (Application.platform == RuntimePlatform.OSXEditor)
                Process.Start("/System/Library/CoreServices/Finder.app", path);
            else if (Application.platform == RuntimePlatform.WindowsEditor)
                Process.Start("explorer.exe", path.Replace("/", "\\"));

        }


        private static void DeleteFiles()// elimina i file che non hanno estensione avrb
        {
            try
            {
                path = Application.dataPath.Replace(@"/Assets", "") + @"/AssetBundles".ToString();

                List<string> listDir = new List<string>();

                foreach (var dir in Directory.GetDirectories(path))
                {

                    DirectoryInfo d = new DirectoryInfo(dir);

                    foreach (var file in Directory.GetFiles(dir).Where(item => !item.EndsWith("avrb", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        File.Delete(file);
                    }
                }

            }

            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);

            }

        }
        public const string AssetBundlesOutputPath = "AssetBundles";
        public static void BuildAssetBundles(BuildTarget target, BuildAssetBundleOptions type = BuildAssetBundleOptions.None)
        {
            // Choose the output path according to the build target.
            string outputPath = Path.Combine(AssetBundlesOutputPath, target.ToString());
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            try
            {
                BuildPipeline.BuildAssetBundles(outputPath, type, target);

            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.Log( e.Message);
            }

        }
    }

    public class PlatformProprierty
    {
        public string Name { get; set; }

        public bool Value { get; set; }

        public BuildTarget target { get; set; }

        public BuildTargetGroup bGroup { get; set; }
    }

    public class SceneProprierty
    {
        public string Name { get; set; }

        public bool Value { get; set; }

        public string path;

        public string key; // key for find scene in assetdatabase 
    }
}