using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ArchToolkit.Utils.Texture;

namespace ArchToolkit.Editor.Window
{
    [System.Serializable]
    public class MainArchWindowBase : EditorWindow
    {
        protected Rect header;
        
        private void OnEnable()
        {
            EditorUtility.SetDirty(this);

           
        }

        protected void ApplyLogo()
        {
            this.header.x = -1;
            this.header.y = 5;

            this.header.width = this.position.width;
            this.header.height = ArchToolkitWindowData.LOGO_HEIGHT;

            // Apply main logo
            GUI.DrawTexture(this.header, ArchToolkitWindowData.GetLogo, ScaleMode.ScaleToFit);

            var versioningStyle = new GUIStyle(GUI.skin.label);

            versioningStyle.fontSize = 8;
            GUILayout.Label(ArchToolkitText.PLUGIN_VERSION,versioningStyle);
        }
    }
}