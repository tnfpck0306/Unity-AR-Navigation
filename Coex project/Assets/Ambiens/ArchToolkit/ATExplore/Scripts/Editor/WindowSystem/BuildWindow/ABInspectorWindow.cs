using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ArchToolkit.Editor.Window
{
    public class ABInspectorWindow : ArchInspectorBase
    {
        private Vector2 scenesPos, platformPos;
        private AssetBundlesMenuItems assetBundle;
        public ABInspectorWindow(string name) : base(name)
        {

        }
        public override bool IsInspectorVisible()
        {
            return (ArchBuildWindow.buildType==1);
        }

        public override void OnGui(Rect pos)
        {
            base.OnGui(pos);

            if (!this.inspectorFoldoutOpen)
                return;

            if (assetBundle == null)
            {
                this.assetBundle = new AssetBundlesMenuItems();
                this.assetBundle.InitializeBundle();
            }
            /*
            this.scenesPos = EditorGUILayout.BeginScrollView(this.scenesPos,
                   false,
                   true,
                   GUILayout.Height(400),
                   GUILayout.ExpandWidth(false));
                   */
            GUILayout.Label("Platforms", new GUIStyle(EditorStyles.boldLabel));

            GUILayout.BeginVertical();

            foreach (var item in AssetBundlesMenuItems.boolList)
            {
                item.Value = GUILayout.Toggle(item.Value, "  " + item.Name, GUILayout.Height(20));
            }

            GUILayout.EndVertical();

            GUILayout.Label("Scenes", new GUIStyle(EditorStyles.boldLabel));

            foreach (var item in AssetBundlesMenuItems.sceneList)
            {
                item.Value = GUILayout.Toggle(item.Value, "  " + item.Name, GUILayout.Height(20));
            }

            if (GUILayout.Button("Generate AssetBundles", GUILayout.Height(35)))
            {
                try
                {
                    assetBundle.StartBuildAssetBundle(BuildAssetBundleOptions.None);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e.Message);
                }
            }

        }

    }
}
