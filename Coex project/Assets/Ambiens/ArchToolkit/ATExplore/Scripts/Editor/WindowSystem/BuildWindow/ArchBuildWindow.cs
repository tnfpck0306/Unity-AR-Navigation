using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit.Utils;
using UnityEditor;
using System;
using System.IO;

namespace ArchToolkit.Editor.Window
{

    public class ArchBuildWindow : ArchWindowBase
    {
        public static int buildType = 0;
        

        public ArchBuildWindow(WindowStatus windowStatus) : base(windowStatus)
        {
            

        }
        string[] options = new string[]
            {
                "Executable", "Asset Bundles"
            };
        protected override void _DrawGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Build Type:");
            buildType = EditorGUILayout.Popup(buildType, options);

            

            EditorGUILayout.EndVertical();

        }
        public override void OnOpen()
        {
            base.OnOpen();
           
        }
        public override void Init()
        {
            this.ButtonCount = 2;
            this.TabLabel = "Build";
        }

        public override void OnClose()
        {
            
        }

        /*public override void DrawInspectors(Rect pos, GUIStyle inspectorStyle)
        {
            GUILayout.BeginArea(new Rect(0, ArchToolkitWindowData.MainAreaAnchor + this.MaxWindowHeight, pos.width, pos.height - 50));

            switch (buildType)
            {
                case 0:
                    this.inspectors.Find(i => i.Name == "Executable").OnGui();
                    break;
                case 1:
                    this.inspectors.Find(i => i.Name == "Asset Bundles").OnGui();
                    break;
            }

            GUILayout.EndArea();
        }*/

        public override void InstantiateInspectors()
        {
            if (!this.inspectors.Exists(i => i.Name == "Asset Bundles"))
                this.inspectors.Add(new ABInspectorWindow("Asset Bundles"));

            if (!this.inspectors.Exists(i => i.Name == "Executable"))
                this.inspectors.Add(new ExeInspectorWindow("Executable"));
            if (!this.inspectors.Exists(i => i.Name == "Build Template"))
                this.inspectors.Add(new BuildTemplateInspectorWindow("Build Template"));
        }
    }
}
