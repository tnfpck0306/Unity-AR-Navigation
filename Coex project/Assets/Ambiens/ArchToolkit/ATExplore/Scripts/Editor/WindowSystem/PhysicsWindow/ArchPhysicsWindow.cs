using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit.Utils;
using UnityEditor;

namespace ArchToolkit.Editor.Window
{
    /* DEPRECATED IN v1.2
    public class ArchPhysicsWindow : ArchWindowCategory
    {
        private GameObject selected;

        public ArchPhysicsWindow(WindowStatus windowStatus) : base(windowStatus)
        {
            this.ButtonCount = 1;
        }

        public override void DrawGUI()
        {
            EditorGUILayout.BeginVertical();


            EditorGUI.BeginDisabledGroup(this.AlreadyHasCollider());

            this.ButtonCount = 1;

            if (GUILayout.Button(new GUIContent("Add Block Property", ArchToolkitText.ADD_COLLIDER_TOOLTIP), GUILayout.Height(ArchToolkitWindowData.BUTTON_HEIGHT)))
            {
                if (this.selected != null && !this.AlreadyHasCollider())
                    this.selected.AddComponent<MeshCollider>();
            }

            EditorGUI.EndDisabledGroup();

            if (this.AlreadyHasCollider())
            {
                this.ButtonCount += 2;
                EditorGUILayout.HelpBox("The selected object already has a Collider Component",MessageType.Warning);
            }

            if(this.selected == null)
            {
                this.ButtonCount += 2;
                EditorGUILayout.HelpBox("You need to select at least one object", MessageType.Warning);
            }
            
            EditorGUILayout.EndVertical();

        }
        
        private bool AlreadyHasCollider()
        {
            if (this.selected == null)
                return false;

            return this.selected.GetComponent<MeshFilter>() == null ||
                                       this.selected.GetComponent<MeshFilter>().sharedMesh == null ||
                                       this.selected.GetComponent<Collider>();
        }

        public override void OnClose()
        {
            
        }

        public override void OnOpen()
        {
            
        }

        public override void OnSelectionChange(GameObject gameObject)
        {
            this.selected = gameObject;
            
        }
    }
    */
}
