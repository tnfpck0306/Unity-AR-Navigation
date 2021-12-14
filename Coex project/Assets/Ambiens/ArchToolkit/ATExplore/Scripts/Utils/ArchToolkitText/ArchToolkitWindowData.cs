#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ArchToolkit.Utils.Texture;

namespace ArchToolkit
{

    public static class ArchToolkitWindowData
    {
        public const float LOGO_HEIGHT = 60f;

        public const float TAB_HEIGHT = 40f;

        public const float BUTTON_HEIGHT = 20f;

        public const float PADDING = 10f;

        public static Color32 ApplyColorButton = new Color32(36,171,180,255);

        public const string WALKABLE_LAYER_KEY = "ArchToolkitWalkableLayer";

        public static float MainAreaAnchor { get { return LOGO_HEIGHT + PADDING; } }

        public static Texture2D GetLogo
        {
            get
            {
                if (logo == null)
                {
                    logo = new Texture2D(1, 1);
                    logo.SetPixel(0, 0, new Color(125, 124, 32, 1));
                    logo.Apply();
                    logo = Resources.Load<Texture2D>(ArchToolkitDataPaths.RESOURCESPLUGINLOGO);
                }

                return logo;
            }
        }
        
        public static Texture2D GetBackground
        {
            get
            {
                if(background == null) 
                    background = TextureUtils.MakeTex(1, 1, Color.gray);

                return background;
            }
        }

        private static Texture2D logo;

        private static Texture2D background;

        public static GUIStyle GetFoldoutStyle(int fontSize)
        {
            var style = new GUIStyle(EditorStyles.foldout);
            
            style.fontSize = fontSize;
            style.fontStyle = FontStyle.Bold;

            return style;
        }

        public static GUIStyle GetStyle(TextAnchor anchor, Texture2D background = null)
        {
            // Create Style for Main logo
            var style = new GUIStyle();
            style.alignment = anchor;
            
            if (background == null)
                style.normal.background = ArchToolkitWindowData.GetBackground;
            else
                style.normal.background = background;

            return style;
        }

        public static bool CreateButtonAdd()
        {
            return GUILayout.Button(Resources.Load<Texture>(ArchToolkitDataPaths.RESOURCESEDITORADDICON), GUILayout.Height(30), GUILayout.Width(30));
        }

        public static bool CreateButtonMinus()
        {
            return GUILayout.Button(Resources.Load<Texture>(ArchToolkitDataPaths.RESOURCESEDITORMINUSICON), GUILayout.Height(30), GUILayout.Width(30));
        }

        public static Texture GetTexture(string path)
        {
            return Resources.Load<Texture>(path);
        }
    }
}
#endif