/* DEPRECATED v1.2
using UnityEngine;
using UnityEditor;

namespace ArchToolkit.Editor.Window
{
    public class ArchNavigationWindow : ArchWindowCategory
    {
        private bool gameObjectSelected;

        public ArchNavigationWindow(WindowStatus windowStatus) : base(windowStatus)
        {
            this.ButtonCount = 1;
        }

        public override void DrawGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUI.BeginDisabledGroup(this.gameObjectSelected);
            
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
        }

        public override void OnClose()
        {

        }

        public override void OnOpen()
        {

        }
        
        public override void OnSelectionChange(GameObject gameObject)
        {
            this.gameObjectSelected = Selection.gameObjects.Length <= 0;

            if (MainArchWindow.Instance != null)
                MainArchWindow.Instance.Repaint();
        }

    }
}
*/