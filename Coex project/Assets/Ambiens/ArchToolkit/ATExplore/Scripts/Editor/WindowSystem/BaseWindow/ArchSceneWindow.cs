using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit.Utils;
using UnityEditor;

namespace ArchToolkit.Editor.Window
{

    public class ArchSceneWindow : ArchWindowBase
    {
        [SerializeField]
        private ArchToolkitManager toolkitManager;

        public ArchSceneWindow(WindowStatus status) : base(status)
        {

        }

        protected override void _DrawGUI()
        {
            if (this.toolkitManager == null)
                this.toolkitManager = GameObject.FindObjectOfType<ArchToolkitManager>();

            EditorGUILayout.BeginVertical();

            EditorGUI.BeginDisabledGroup(this.toolkitManager != null && this.toolkitManager.managerContainer.pathManager.PathPoints.Count > 0);

            if (GUILayout.Button(new GUIContent(ArchToolkitText.TOM_BUTTON,ArchToolkitText.TOM_BUTTON_TOOLTIP), GUILayout.Height(ArchToolkitWindowData.BUTTON_HEIGHT)))
            {
                if (!ArchToolkitManager.IsInstanced())
                {
                    ArchToolkitManager.Factory();
                }

                if (ArchToolkitManager.IsInstanced())
                {

                    if (ArchToolkitManager.Instance.managerContainer.pathManager.PathPoints.Count > 0)
                    {
                        EditorUtility.DisplayDialog("Warning", "Starting point already exist", "Ok, I understand");
                        return;
                    }

                    ArchToolkitManager.Instance.managerContainer.pathManager.SetStartingPoint();

                    this.toolkitManager = ArchToolkitManager.Instance;
                }
            }

            EditorGUI.EndDisabledGroup();

            if (ArchToolkitManager.IsInstanced())
            {
                foreach( GameObject go in ArchToolkitManager.Instance.settings.buildTemplate.SceneAvailablePrefabs)
                {
                    if (GUILayout.Button(new GUIContent("ADD "+ go.name, "Add "+go.name+" GameObject"), GUILayout.Height(ArchToolkitWindowData.BUTTON_HEIGHT)))
                    {
                        var instance=InstatiateInScene(go);
                        UnityEditor.Selection.activeGameObject = instance;
                    }
                }
            }

            EditorGUILayout.EndVertical();

        }

        public override void Init()
        {
            
            this.ButtonCount = 1+ ArchToolkitManager.Instance.settings.buildTemplate.SceneAvailablePrefabs.Count;
            this.TabLabel = "Scene";
        }

        public override void OnClose()
        {
            
        }

        public override void OnOpen()
        {
            base.OnOpen();
            this.toolkitManager = GameObject.FindObjectOfType<ArchToolkitManager>();
        }

        
        public override void InstantiateInspectors()
        {

           
            if (!this.inspectors.Exists(i => i.Name == "Scene Options"))
                this.inspectors.Add(new ArchCharacterInspector("Scene Options"));

            //TODO-> Add Action Sequence inspector!

        }
    }
}
