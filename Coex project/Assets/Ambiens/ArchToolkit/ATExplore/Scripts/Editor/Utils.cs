using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ArchToolkit.Editor.Utils
{
    public static class EditorUtils
    {
        /// <summary>
            //	This makes it easy to create, name and place unique new ScriptableObject asset files.
            /// </summary>
            public static T CreateAsset<T> (string name="", string path="") where T : ScriptableObject
            {
                T asset = ScriptableObject.CreateInstance<T> ();
                if(string.IsNullOrEmpty(path))
                    path = AssetDatabase.GetAssetPath (Selection.activeObject);
                if (path == "") 
                {
                    path = "Assets";
                } 
                else if (Path.GetExtension (path) != "") 
                {
                    path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
                }
                var n=(string.IsNullOrEmpty(name)?typeof(T).ToString():name);
                string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/"+ n + ".asset");

                AssetDatabase.CreateAsset (asset, assetPathAndName);
        
                AssetDatabase.SaveAssets ();
                    AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow ();
                Selection.activeObject = asset;
                return asset;
            }
    }
}