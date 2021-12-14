
using ArchToolkit.BuildSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
#if (UNITY_2020_1_OR_NEWER || ATE_AR ) && AT_EXPLORE_INIT
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine.XR.Management;
#endif
using UnityEngine;
using static ArchToolkit.BuildSystem.SOBuildTemplate;

namespace ArchToolkit.Editor.Window
{
    public class BuildTemplateInspectorWindow : ArchInspectorBase
    {

        private bool isCheckingDependencies = false;
        private int CurrentCheckingDependency = 0;
        private AddRequest addRequest;
        private ListRequest listRequest;

        public BuildTemplateInspectorWindow(string name) : base(name)
        {

        }
        public override bool IsInspectorVisible()
        {
            return (ArchBuildWindow.buildType == 0);
        }
        int selGridInt = -1;
        public List<SOBuildTemplate> templates = null;
        public List<GUIContent> templateButtons = null;

        int columns = 4;
        int margin = 4;


        public override void OnGui(Rect pos)
        {
            base.OnGui(pos);

            //CHECK ASYNC OPERATION FOR DEPENDENCIES
            //https://docs.unity3d.com/ScriptReference/PackageManager.Client.html

            if (this.isCheckingDependencies)
            {
                GUILayout.Label("Installing dependecies...");
                return;
            }

            if (!this.inspectorFoldoutOpen)
                return;

            GUILayout.BeginVertical();

            CheckTemplates();

            var w = (pos.width - (columns + 2) * margin) / columns;
            var s = new GUIStyle(GUI.skin.button);
            s.fixedWidth = w;
            s.fixedHeight = w;
            s.normal.background = this.nonSelectedBackground;
            s.focused.background = this.nonSelectedBackground;
            s.active.background = this.nonSelectedBackground;
            s.hover.background = this.nonSelectedBackground;
            s.onNormal.background = this.selectedBackground;


            s.margin = new RectOffset(margin, margin, margin, margin);

            var currTemplate = ArchToolkitManager.Instance.settings.buildTemplate;

            if (selGridInt == -1)
            {
                if (currTemplate == null)
                {
                    selGridInt = 0;
                    ArchToolkitManager.Instance.settings.buildTemplate = this.templates[0];
                    EditorUtility.SetDirty(ArchToolkitManager.Instance.gameObject);
                }
                else
                {
                    selGridInt = this.templates.FindIndex(t => t == currTemplate);
                }
            }
            else
            {
                if (currTemplate == null)
                {
                    ArchToolkitManager.Instance.settings.buildTemplate = this.templates[selGridInt];
                    EditorUtility.SetDirty(ArchToolkitManager.Instance.gameObject);
                }
                else
                {
                    selGridInt = this.templates.FindIndex(t => t == currTemplate);
                }
            }

            selGridInt = GUILayout.SelectionGrid(selGridInt, templateButtons.ToArray(), columns, s);

            ArchToolkitManager.Instance.settings.buildTemplate = this.templates[selGridInt];
            EditorUtility.SetDirty(ArchToolkitManager.Instance.gameObject);

            if (GUILayout.Button("Apply Template"))
            {
                this.ApplyTemplate();
            }
            GUI.enabled = ArchToolkitManager.Instance.settings.buildTemplate.CanBuild();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Build"))
            {
                this.Build();
            }
            /*if (GUILayout.Button("Build & Run"))
            {
                this.BuildRun();
            }*/
            GUILayout.EndHorizontal();
            GUI.enabled = true;

            GUILayout.EndVertical();
        }

        Action OnCompletePackageManagerDependencies = null;

        private void ApplyTemplate()
        {
            var bT = ArchToolkitManager.Instance.settings.buildTemplate;

            this.ApplySymbols();

            //PlayerSettings.virtualRealitySupported = bT.VRSupported;
            var group = SOBuildTemplate.GetGroupByPlatform(bT.TargetPlatform);

            var buildTarget = this.SwitchPlatform(bT.TargetPlatform);

            SetVirtualRealitySupportAtStartup(group, bT.VRSupported);

            if (IsVirtualRealitySupported(group))
            {
                PlayerSettings.stereoRenderingPath = (bT.SinglePassStereo) ? StereoRenderingPath.SinglePass : StereoRenderingPath.MultiPass;

                EditorPrefs.SetBool("IsOculusQuest", bT.isQuest);
                EditorPrefs.SetBool("IsPicoNeo", bT.isPicoNeo);

            }

            if (bT.VRSupported)
            {
                SetARSupport(false, group);

                this.SetVRDevices((bT.StartWithVisualization != SOBuildTemplate.VisualizationType.Mode360), bT.isOculus);
                if (bT.StartWithVisualization == SOBuildTemplate.VisualizationType.VR)
                {
                    ArchToolkitManager.Instance.settings.movementTypePerPlatform = MovementTypePerPlatform.VR;
                }
                else
                {
                    ArchToolkitManager.Instance.settings.movementTypePerPlatform = MovementTypePerPlatform.FullScreen360;
                }
            }
            else if (bT.StartWithVisualization == SOBuildTemplate.VisualizationType.AR)
            {
                ArchToolkitManager.Instance.settings.movementTypePerPlatform = MovementTypePerPlatform.AR;
                SetARSupport(true, group);
#if UNITY_ANDROID
                
                
#elif UNITY_IOS
                PlayerSettings.iOS.cameraUsageDescription = "Augmented Reality";
                PlayerSettings.iOS.targetOSVersionString = "11.0";
                PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 1);
#endif

            }
            else
            {
                ArchToolkitManager.Instance.settings.movementTypePerPlatform = MovementTypePerPlatform.FullScreen360;
            }
            this.CurrentCheckingDependency = 0;

            if (bT.RequiredGraphicAPI.Count > 0)
            {
                PlayerSettings.SetGraphicsAPIs(buildTarget, bT.RequiredGraphicAPI.ToArray());
            }

            EditorUtility.SetDirty(ArchToolkitManager.Instance.settings);
            AssetDatabase.SaveAssets();

            this.ApplyDependencies();
            

        }
        private void ApplySymbols()
        {

            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allSymbols = definesString.Split(';').ToList();
            List<string> newSymbols = new List<string>();
            foreach (var s in allSymbols)
            {
                if (s.StartsWith("ATE_"))
                {
                    if (ArchToolkitManager.Instance.settings.buildTemplate.Symbols.Contains(s))
                    {
                        newSymbols.Add(s);
                    }
                    else Debug.Log("Removing ATE Symbol " + s);
                }
                else
                {
                    newSymbols.Add(s);
                }
            }
            foreach (var s in ArchToolkitManager.Instance.settings.buildTemplate.Symbols)
            {
                if (!newSymbols.Contains(s))
                {
                    Debug.Log("Adding Symbol "+s);
                    newSymbols.Add(s);
                }
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", newSymbols.ToArray()));
        }

        public bool AllSymbolsApplied()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allSymbols = definesString.Split(';').ToList();

            foreach (var s in ArchToolkitManager.Instance.settings.buildTemplate.Symbols)
            {
                if (!allSymbols.Contains(s))
                {
                    return false;
                }
            }
            return true;
        }

        private void ApplyDependencies()
        {
            this.isCheckingDependencies = true;

            if (ArchToolkitManager.Instance.settings.buildTemplate.dependencies.Count > CurrentCheckingDependency)
            {
                this.addRequest = Client.Add(ArchToolkitManager.Instance.settings.buildTemplate.dependencies[CurrentCheckingDependency]);
                EditorApplication.update += DependenciesProgress;
            }
            else
            {
                this.isCheckingDependencies = false;
                Debug.Log("Dependencies Applied");
                if (this.OnCompletePackageManagerDependencies != null) this.OnCompletePackageManagerDependencies();

                EditorApplication.update -= DependenciesProgress;
            }
        }

        void DependenciesProgress()
        {
            if (this.addRequest.IsCompleted)
            {
                if (this.addRequest.Status == StatusCode.Success)
                {
                    Debug.Log("Installed: " + this.addRequest.Result.packageId);
                    if (this.addRequest.Result.packageId.Contains("com.unity.xr.legacyinputhelpers"))
                    {
                        Setup.InputAndTagSetup.AddScriptingSymbol();
                    }
                }

                else if (this.addRequest.Status >= StatusCode.Failure)
                    Debug.Log(this.addRequest.Error.message);

                CurrentCheckingDependency++;
                this.ApplyDependencies();
                EditorApplication.update -= DependenciesProgress;
            }
        }

        private BuildTarget SwitchPlatform(TargetNativePlatform t)
        {
            BuildTargetGroup tGroup = BuildTargetGroup.Android;
            BuildTarget target = BuildTarget.Android;

            switch (t)
            {
                case TargetNativePlatform.WebGL:
                    tGroup = BuildTargetGroup.WebGL;
                    target = BuildTarget.WebGL;

                    break;
                case TargetNativePlatform.Android:
                    tGroup = BuildTargetGroup.Android;
                    target = BuildTarget.Android;
                    break;
                case TargetNativePlatform.Ios:
                    tGroup = BuildTargetGroup.iOS;
                    target = BuildTarget.iOS;

                    break;
                case TargetNativePlatform.PC:
                    tGroup = BuildTargetGroup.Standalone;
                    target = BuildTarget.StandaloneWindows64;

                    break;
#if UNITY_2017
                case TargetNativePlatform.Mac:
                    tGroup = BuildTargetGroup.Standalone;
                    target = BuildTarget.StandaloneOSXUniversal;
                    break;
#else
                case TargetNativePlatform.Mac:
                    tGroup = BuildTargetGroup.Standalone;
                    target = BuildTarget.StandaloneOSX;
                    break;
#endif
                default:
                    break;
            }

            EditorUserBuildSettings.SwitchActiveBuildTarget(tGroup, target);



            return target;
        }

        private void SetVRDevices(bool StartAsVR, bool isOculusBuild = false)
        {
#if UNITY_STANDALONE
            this.SetVRDevicesForPlatform(StartAsVR, isOculusBuild, BuildTargetGroup.Standalone);
#elif UNITY_ANDROID

            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;

            this.SetVRDevicesForPlatform(StartAsVR, isOculusBuild, BuildTargetGroup.Android);
#elif UNITY_IOS
            this.SetVRDevicesForPlatform(StartAsVR, isOculusBuild, BuildTargetGroup.iOS);
#endif

            if (!StartAsVR)
            {
                if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager != null)
                    GameObject.DestroyImmediate(ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.gameObject);
            }
            else
            {
#if AVR_OCULUS_QUEST_HAND_TRACKING
                CheckOrInitOculusQuestHandTracking();
#endif
            }

        }

        public void SetVirtualRealitySupportAtStartup(BuildTargetGroup group, bool enabled)
        {
#if UNITY_2020_1_OR_NEWER  && AT_EXPLORE_INIT
            XRGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings != null)
            {
                XRGeneralSettings settings = buildTargetSettings.SettingsForBuildTarget(group);
                settings.InitManagerOnStart = enabled;
            }
            else
            {
                Debug.LogWarning("ATE: BuildTargetSettings == null");
            }
#elif !UNITY_2020_1_OR_NEWER
            PlayerSettings.virtualRealitySupported = enabled;
#endif
        }

        public bool IsVirtualRealitySupported(BuildTargetGroup group)
        {
#if UNITY_2020_1_OR_NEWER && AT_EXPLORE_INIT
            XRGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings != null)
            {
                XRGeneralSettings settings = buildTargetSettings.SettingsForBuildTarget(group);
                return settings.InitManagerOnStart;
            }
            else
            {
                Debug.LogWarning("ATE: BuildTargetSettings == null");
                return false;
            }
#elif !UNITY_2020_1_OR_NEWER
            return PlayerSettings.virtualRealitySupported;
#else
            return false;
#endif
        }

        void SetARSupport(bool enabled, BuildTargetGroup group)
        {
#if ATE_AR
            XRGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out buildTargetSettings);
            XRGeneralSettings settings = buildTargetSettings.SettingsForBuildTarget(group);
            settings.InitManagerOnStart = enabled;

            if (enabled)
            {
                if (group == BuildTargetGroup.Android)
                    XRPackageMetadataStore.AssignLoader(settings.Manager, "UnityEngine.XR.ARCore.ARCoreLoader", group);
                else if (group == BuildTargetGroup.iOS)
                    XRPackageMetadataStore.AssignLoader(settings.Manager, "UnityEngine.XR.ARKit.ARKitLoader", group);
            }
            else
            {
                if (group == BuildTargetGroup.Android)
                    XRPackageMetadataStore.RemoveLoader(settings.Manager, "UnityEngine.XR.ARCore.ARCoreLoader", group);
                else if (group == BuildTargetGroup.iOS)
                    XRPackageMetadataStore.RemoveLoader(settings.Manager, "UnityEngine.XR.ARKit.ARKitLoader", group);
            }
#endif
        }
        void SetVRDevicesForPlatform(bool StartAsVR, bool isOculusBuild, BuildTargetGroup group)
        {

#if UNITY_2020_1_OR_NEWER && AT_EXPLORE_INIT

            XRGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out buildTargetSettings);

            XRGeneralSettings settings = buildTargetSettings.SettingsForBuildTarget(group);

            if (!StartAsVR)
            {
                settings.InitManagerOnStart = false;

            }
            else
            {
                settings.InitManagerOnStart = true;

                if (group == BuildTargetGroup.Standalone)
                {
                    if (isOculusBuild)
                    {
                        XRPackageMetadataStore.AssignLoader(settings.Manager, "Unity.XR.Oculus.OculusLoader", BuildTargetGroup.Standalone);
                    }
                    else
                    {

                    }
                }
                else if (group == BuildTargetGroup.Android)
                {
                    if (isOculusBuild)
                    {
                        XRPackageMetadataStore.AssignLoader(settings.Manager, "Unity.XR.Oculus.OculusLoader", BuildTargetGroup.Android);
                    }
                    else
                    {

                    }
                }
                else if (group == BuildTargetGroup.iOS)
                {

                }
            }
#elif !UNITY_2020_1_OR_NEWER

            var currentTargetDevices = PlayerSettings.GetVirtualRealitySDKs(group);

            if (!StartAsVR)
            {
                if (currentTargetDevices.Count() == 0)
                {
                    currentTargetDevices = new string[] { "None" };
                }
                else
                {
                    if (currentTargetDevices[0] != "None")
                    {
                        var aux = new string[currentTargetDevices.Count() + 1];
                        aux[0] = "None";
                        for (int i = 0; i < currentTargetDevices.Count(); i++) aux[i + 1] = currentTargetDevices[i];
                        currentTargetDevices = aux;
                    }
                }
            }
            else
            {

                if (group == BuildTargetGroup.Standalone)
                {
                    if (isOculusBuild)
                        currentTargetDevices = new string[] { "Oculus", "OpenVR" };
                    else
                        currentTargetDevices = new string[] { "OpenVR" };
                }
                else if (group == BuildTargetGroup.Android)
                {
                    if (isOculusBuild)
                        currentTargetDevices = new string[] { "Oculus" };
                    else
                        currentTargetDevices = new string[] { "cardboard" };
                }
                else if (group == BuildTargetGroup.iOS)
                    currentTargetDevices = new string[] { "cardboard" };

            }

            PlayerSettings.SetVirtualRealitySDKs(group, currentTargetDevices);
#endif
        }

        private void Build()
        {
            BuildPlayerWindow.ShowBuildPlayerWindow();
        }
        private void BuildRun()
        {
            BuildPlayerWindow.ShowBuildPlayerWindow();
        }
        private string iconsRoot = "Assets/Ambiens/ArchToolkit/ATExplore/Icons/Templates/";
        private Texture2D selectedBackground = null;
        private Texture2D nonSelectedBackground = null;

        private void CheckTemplates()
        {
            if (this.selectedBackground == null)
            {
                this.selectedBackground = AssetDatabase.LoadAssetAtPath<Texture2D>(this.iconsRoot + "2x2_selected.png");
            }
            if (this.nonSelectedBackground == null)
            {
                this.nonSelectedBackground = AssetDatabase.LoadAssetAtPath<Texture2D>(this.iconsRoot + "2x2_white.png");
            }

            if (this.templates == null || this.templates.Count == 0)
            {
                this.templates = new List<SOBuildTemplate>();
                this.templateButtons = new List<GUIContent>();
                var tmpID = AssetDatabase.FindAssets("t:SOBuildTemplate", null);

                foreach (var tID in tmpID)
                {
                    var t = AssetDatabase.LoadAssetAtPath<SOBuildTemplate>(AssetDatabase.GUIDToAssetPath(tID));
                    if (t != null)
                    {
                        this.templates.Add(t);
                        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(this.iconsRoot + t.icon + ".png");
                        this.templateButtons.Add(new GUIContent(texture, t.Name));
                    }
                }
            }
        }

    }
}