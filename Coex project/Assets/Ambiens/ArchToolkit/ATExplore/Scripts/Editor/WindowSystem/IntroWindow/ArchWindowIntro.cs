using UnityEngine;
using UnityEditor;

namespace ArchToolkit.Editor.Window
{
    [InitializeOnLoad]
    public class ArchWindowIntro : MainArchWindowBase
    {
        public static ArchWindowIntro Instance;
    
        private Vector2 scroll;

        private SocialReferements[] socialClasses = new SocialReferements[]
        {
            //new SocialReferements ("UI/Documentation", "", "Click here to open documentation","Documentation"), // Docs 
            //new SocialReferements ("UI/Facebook", "", "Send us an email if you need support","Support"), // Support 
            //new SocialReferements ("UI/YouTube", "https://www.youtube.com/channel/UCkqMEDTMuARl75aCK5T5ryA", "Tutorial playlist","Tutorials"), // Youtube
            new SocialReferements ("UI/Facebook", "https://www.facebook.com/groups/152547958672484/", "Facebook page","Facebook official page"), // Facebook
            new SocialReferements ("UI/Instagram", "https://www.instagram.com/ambiensvr/", "Instagram page","Instagram official page"), // Instagram 
            new SocialReferements ("UI/ATIcon", "https://www.archtoolkit.com", "Archtoolkit site, for stay up to date","Archtoolkit site"), // Archtoolkit site
            new SocialReferements ("UI/Almbiens", "https://www.ambiensvr.com","Our company site","AmbiensVR site") // Ambiens site
        };

        static ArchWindowIntro()
        {
            Init();
        }

        //[MenuItem("Window/Ambiens/ClearKey")]
        private static void ClearKey()
        {
            if (EditorPrefs.HasKey("ATKey"))
            {
                EditorPrefs.DeleteKey("ATKey");
            }
        }

        [MenuItem("Tools/Ambiens/Info")]
        private static void Open()
        {
            var window = EditorWindow.GetWindow<ArchWindowIntro>("Info");

            window.maxSize = new Vector2(450, 600);

            window.minSize = new Vector2(350, 550);

            //window.InitializeSocialReferements();

            Instance = window;
        }

        public static void Init()
        {
            if (EditorPrefs.HasKey("ATKey"))
            {
                if (!EditorPrefs.GetBool("ATKey"))
                {
                    EditorPrefs.SetBool("ATKey", true);
                    Open();
                }
            }
            else
            {
                EditorPrefs.SetBool("ATKey", true);
                Open();
            }
        }

        

        private void InitializeSocialReferements()
        {
            foreach (var item in this.socialClasses)
            {
                item.SetTextureImage();
            }
        }

        private void Update()
        {
            foreach (var item in this.socialClasses)
            {
                if (item == null)
                    continue;

                if(item.textureImage == null)
                {
                    item.SetTextureImage();
                }
            }
        }

        private void OnEnable()
        {
            this.InitializeSocialReferements();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, this.position.width, ArchToolkitWindowData.LOGO_HEIGHT + ArchToolkitWindowData.PADDING),GUI.skin.textArea);

            GUILayout.BeginHorizontal();

            this.ApplyLogo();

          //  if (GUILayout.Button("X",GUILayout.Width(20),GUILayout.Height(20)))
          //      this.Close();

            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(0, ArchToolkitWindowData.MainAreaAnchor, this.position.width, this.position.height));

            var welcomeTextStyle = new GUIStyle();

            welcomeTextStyle.wordWrap = true;

            welcomeTextStyle.normal.textColor = Color.black;

            GUILayout.BeginArea(new Rect(0, ArchToolkitWindowData.PADDING, this.position.width,120), GUI.skin.textArea);

            EditorGUILayout.LabelField(ArchToolkitText.WELCOME_TEXT,welcomeTextStyle);
            
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(0, 120 + ArchToolkitWindowData.PADDING, this.position.width,this.position.height));

            var scrollbarStyle = new GUIStyle(GUI.skin.horizontalScrollbar);

            scrollbarStyle.fixedHeight = scrollbarStyle.fixedWidth = 0;

            scroll = EditorGUILayout.BeginScrollView(scroll, scrollbarStyle, GUI.skin.verticalScrollbar, GUILayout.Width(this.position.width),GUILayout.Height(370));

            GUILayout.BeginVertical();

            var customButton = new GUIStyle(GUI.skin.button);

            customButton.imagePosition = ImagePosition.ImageOnly;
            customButton.alignment = TextAnchor.MiddleCenter;
            customButton.normal.background = null;

            var customLabel = new GUIStyle(GUI.skin.label);
            customLabel.fontSize = 10;
            customLabel.wordWrap = true;
            customLabel.alignment = TextAnchor.LowerLeft;

            var titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.fontSize = 12;
            
            foreach (var social in this.socialClasses)
            {
                GUILayout.BeginHorizontal();

                if (social == null)
                    continue;

                if (GUILayout.Button(social.textureImage,customButton,GUILayout.Width(50),GUILayout.Height(50)))
                    Help.BrowseURL(social.siteUrl);

                GUILayout.BeginVertical();

                GUILayout.Space(10);

                GUILayout.Label(social.title,titleStyle);

                GUILayout.Label(social.description,customLabel);

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndVertical();

            EditorGUILayout.EndScrollView();

            GUILayout.EndArea();

            GUILayout.EndArea();

        }
    }
}
