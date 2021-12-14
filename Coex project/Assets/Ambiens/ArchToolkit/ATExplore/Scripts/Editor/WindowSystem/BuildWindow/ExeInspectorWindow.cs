
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace ArchToolkit.Editor.Window
{
    public class ExeInspectorWindow : ArchInspectorBase
    {

        [SerializeField]
        public List<EditorBuildSettingsScene> scenes;
        public ExeInspectorWindow(string name) : base(name)
        {

        }
        public override bool IsInspectorVisible()
        {
            return (ArchBuildWindow.buildType == 0);
        }

        public override void OnGui(Rect pos)
        {
            base.OnGui(pos);

            if (!this.inspectorFoldoutOpen)
                return;

            GUILayout.BeginVertical();
            //Company Name
            PlayerSettings.companyName = EditorGUILayout.TextField("Company Name", PlayerSettings.companyName);
            //App Name
            PlayerSettings.productName = EditorGUILayout.TextField("Product Name", PlayerSettings.productName);
            //Version
            PlayerSettings.bundleVersion = EditorGUILayout.TextField("Version", PlayerSettings.bundleVersion);
            //Scenes
            /*GUILayout.Label("Scenes", new GUIStyle(EditorStyles.boldLabel));
            this.scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

            foreach (var item in EditorBuildSettings.scenes)
            {
                var n = Path.GetFileNameWithoutExtension(item.path);
                item.enabled = GUILayout.Toggle(item.enabled,
                    new GUIContent("  " + n, item.path),
                    GUILayout.Height(20));
            }*/
            //List of Build Settings


            GUILayout.EndVertical();
        }
    }
}