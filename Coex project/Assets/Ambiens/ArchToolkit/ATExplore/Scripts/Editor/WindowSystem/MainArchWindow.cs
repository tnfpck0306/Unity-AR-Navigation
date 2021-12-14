using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace ArchToolkit.Editor.Window
{
    [System.Serializable]
    public enum WindowStatus
    {
        Interaction = 0,
        Scene = 1,
        Build = 2,
        Navigation = 3
    }

    public class PlatformChangedListener : IActiveBuildTargetChanged
    {
        public int callbackOrder { get { return 0; } }

        public static Action<BuildTarget> OnPlatformSwitched;

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            Debug.Log("Platform changed Succesfully to: " + newTarget);
            if (OnPlatformSwitched != null)
                OnPlatformSwitched(newTarget);
        }
    }

    [System.Serializable]
    [InitializeOnLoad]
    public class MainArchWindow : MainArchWindowBase
    {
        public static MainArchWindow Instance;

        public int selectedInspector = 0;

        public Action OnOpen;

        public ArchWindowBase CurrentWindow
        {
            get { return this.currentWindow; }
        }

        public int CurrentCheckingDependency { get; private set; }

        [SerializeField]
        private List<ArchWindowBase> archWindows = new List<ArchWindowBase>();

        [SerializeField]
        private List<IArchInspector> archInspectors = new List<IArchInspector>();

        private List<string> tabs = new List<string>();

        private ArchWindowBase currentWindow;

        private int selectedWindow = 0;

        private Vector2 scroll;

        private GUIStyle mainAreaStyle;

        private GUIStyle inspectorStyle;

        private Rect inspectorRectArea;

        [SerializeField]
        private EditorUpdateManager updateManager;

        private GameObject _currentSelected;
        [SerializeField]
        private ArchToolkitManager atManager;

        private bool isCheckingDependencies = false;
        private AddRequest addRequest;
        public List<string> dependencies = new List<string>()
        {
#if !UNITY_2020_1_OR_NEWER
            "com.unity.xr.legacyinputhelpers",
#endif
            "com.unity.xr.management",
            "com.unity.textmeshpro"
        };
#if !AT_EXPLORE_INIT
        [MenuItem("Tools/Ambiens/ArchToolkit/AT+Explore SETUP")]
        public static void Setup()
        {
            var window = EditorWindow.GetWindow<MainArchWindow>("AT+Explore");
        }
#else

        [MenuItem("Tools/Ambiens/ArchToolkit/AT+Explore")]
        public static void Init()
        {
            var window = EditorWindow.GetWindow<MainArchWindow>("AT+Explore");

            Instance = window;

            Instance.GenerateWindows();
            //TMPro.TMP_PackageUtilities.ImportProjectResourcesMenu();
            //Instance.GenerateInspector();

            if (Instance.atManager == null)
                Instance.atManager = ArchToolkitManager.Factory();

            Instance.mainAreaStyle = ArchToolkitWindowData.GetStyle(TextAnchor.UpperCenter);

            if (Instance.currentWindow == null)
                Instance.currentWindow = Instance.archWindows[0];   

            EditorUtility.SetDirty(Instance);
        }
#endif
        public int AddWindow(ArchWindowBase window)
        {
            if (this.archWindows.Exists(w => w.GetStatus == window.GetStatus))
                return -1;

            this.archWindows.Add(window);
            this.tabs.Add(window.TabLabel);
            return this.tabs.Count-1;
        }

        public bool AddInspector(IArchInspector inspector)
        {
            if (this.archInspectors.Exists(i => i.Name == inspector.Name))
                return false;

            this.archInspectors.Add(inspector);
            return true;
        }
        

        
        private void OnFocus()
        {
            if (this.archInspectors.Count > 0)
            {
                foreach (var inspector in this.archInspectors)
                {
                    inspector.OnFocus();
                }
            }
        }

        private void GenerateWindows()
        {
            this.tabs.Clear();

            new ArchAnimationWindow(WindowStatus.Interaction);

            new ArchSceneWindow(WindowStatus.Scene);

            new ArchBuildWindow(WindowStatus.Build);
        }

        private void OnProjectChange()
        {
            //this.Repaint();

            foreach (var inspector in this.archInspectors)
            {
                if (inspector == null)
                    continue;

                inspector.OnProjectChange();
            }

            this.OnSelectionChange();
        }

        private void Update()
        {
#if AT_EXPLORE_INIT
            if (EditorApplication.isPlaying || EditorApplication.isPaused)
                return;


            if (EditorApplication.isCompiling)
            {
                this._currentSelected = null;
                return;
            }

            // Check that windows is null
            if (Instance == null) // this fix the bug when app compile, the window change to null
            {
                Init();

                return;
            }

            if (this.atManager == null)
            {
                this.atManager = ArchToolkitManager.Factory();
            }

            if (this.archInspectors.Count > 0)
            {
                foreach (var inspector in this.archInspectors)
                {
                    inspector.OnUpdate();
                }
            }

            if (this._currentSelected != Selection.activeGameObject || this._currentSelected == null) // if it changed while window was not on focus
            {
                this.OnSelectionChange();
            }
#endif
        }

        private void OnGUI()
        {

#if !AT_EXPLORE_INIT
            if(!this.isCheckingDependencies)
            {
                ApplyDependencies();
            }
            else{
                
            }
            GUILayout.Label("Installing dependencies... please wait");

#else
            if (Instance == null)
                return;


            if (this.inspectorStyle == null)
                this.inspectorStyle = new GUIStyle(GUI.skin.box);

            var window = this.currentWindow;

            //if (this.archInspectors == null || this.archInspectors.Count <= 0)
            //    return;

            if (this.currentWindow == null)
            {
                this.selectedWindow = 0;
                this.currentWindow = this.archWindows[this.selectedWindow];
                this.currentWindow.OnOpen();
            }
            else
                this.selectedWindow = this.currentWindow.TabIndex;

            this.ApplyLogo();

            this.inspectorRectArea = new Rect(0, ArchToolkitWindowData.MainAreaAnchor, this.position.width, this.currentWindow.MaxWindowHeight);

            // Create the main area
            GUILayout.BeginArea( this.inspectorRectArea,this.mainAreaStyle);

            this.currentWindow = this.archWindows[
                GUILayout.Toolbar(this.selectedWindow, 
                    this.tabs.ToArray(), 
                    GUILayout.Height(ArchToolkitWindowData.TAB_HEIGHT))
                    ];
            //ON CHANGE CURRENT WINDOW
            if (window != this.currentWindow || window == null)
            {
                if (this.currentWindow != null)
                {
                    this.currentWindow.OnOpen();
                }

                if (window != null)
                {
                    window.OnClose();
                }
            }

            GUILayout.Space(ArchToolkitWindowData.PADDING);

            if (this.currentWindow != null)
                this.currentWindow.DrawGUI();
            else
            {
               
                this.currentWindow = this.archWindows[0];

                this.currentWindow.DrawGUI();
            }

            // end the main area
            GUILayout.EndArea();

            this.currentWindow.DrawInspectors(this.position, this.inspectorStyle);
#endif
        }
#if !AT_EXPLORE_INIT
        private void ApplyDependencies()
        {
            this.isCheckingDependencies = true;

            if (ArchToolkitManager.Instance.settings.buildTemplate.dependencies.Count > CurrentCheckingDependency)
            {
                this.addRequest = Client.Add(this.dependencies[CurrentCheckingDependency]);
                EditorApplication.update += DependenciesProgress;
            }
            else
            {
                this.isCheckingDependencies = false;
                EditorApplication.update -= DependenciesProgress;
                Debug.ClearDeveloperConsole();
            }
        }

        void DependenciesProgress()
        {
            if (this.addRequest.IsCompleted)
            {
                if (this.addRequest.Status == StatusCode.Success)
                {
                    Debug.Log("Installed: " + this.addRequest.Result.packageId);
                }

                else if (this.addRequest.Status >= StatusCode.Failure)
                    Debug.Log(this.addRequest.Error.message);

                CurrentCheckingDependency++;
                this.ApplyDependencies();
                EditorApplication.update -= DependenciesProgress;
            }
        }
#endif

        private void OnEnable()
        {
            if (!ArchToolkitManager.IsInstanced())
            {
                ArchToolkitManager.Factory();
            }


#if UNITY_2019_1_OR_NEWER
            UnityEditor.PackageManager.UI.PackageManagerExtensions.RegisterExtension(new Setup.InputAndTagSetup());
#endif
            new Setup.InputAndTagSetup().Setup();

            if (this.currentWindow != null)
                this.currentWindow.OnOpen();

            if (this.archInspectors.Count > 0)
            {
                foreach (var inspector in this.archInspectors)
                {
                    inspector.OnEnable();
                }
            }

            if (this.updateManager == null)
            {
                if (!EditorUpdateManager.IsInstanced()) // if is not instanced , create a new EditorUpdatemanager
                    this.updateManager = new EditorUpdateManager();
                else
                    this.updateManager = EditorUpdateManager.Instance;
            }
            else
            {
                this.updateManager.RefreshAllActions();
            }

            if (this.OnOpen != null)
                this.OnOpen();

            this.Repaint();

            Selection.selectionChanged += this.Repaint;

            EditorUtility.SetDirty(this);
        }

        private void OnSelectionChange()
        {
            this._currentSelected = Selection.activeGameObject;

            if (this.currentWindow != null)
            {
                this.currentWindow.OnSelectionChange(Selection.activeGameObject);
            }
            /*
            if (this.archInspectors.Count > 0)
            {
                foreach (var inspector in this.archInspectors)
                {
                    inspector.OnSelectionChange(Selection.activeGameObject);
                }
            }
            */
            this.Repaint();

        }

        private void OnDestroy()
        {

            if (this.archInspectors.Count > 0)
            {
                foreach (var inspector in this.archInspectors)
                {
                    inspector.OnClose();
                }
            }

            this.archInspectors.Clear();

            this.archWindows.Clear();

            this.OnOpen = null;

            Selection.selectionChanged -= this.Repaint;

            Instance = null;
        }
    }
}