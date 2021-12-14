using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace ArchToolkit.BuildSystem
{
    [CreateAssetMenu(fileName = "BuildTemplate", menuName = "ArchToolkit/AT+Explore/Build Template")]
    public class SOBuildTemplate : ScriptableObject
    {
        public enum VisualizationType
        {
            VR,
            Mode360,
            AR
        }
        public enum TargetNativePlatform
        {
            Android,
            Ios,
            PC,
            Mac,
            WebGL
        }

        public string Name;

        public string icon;

        public bool VRSupported;
        public bool SinglePassStereo;

        public bool isOculus;
        public bool isQuest;
        public bool isPicoNeo=false;

        public VisualizationType StartWithVisualization;

        public TargetNativePlatform TargetPlatform;

        public List<SODevice> Devices;

        public List<string> dependencies;
        public List<string> usedNameSpaces;

        public List<UnityEngine.Rendering.GraphicsDeviceType> RequiredGraphicAPI;

        public List<string> Symbols;

        public List<GameObject> SceneAvailablePrefabs;

#if UNITY_EDITOR
        public bool CanBuild()
        {
            if (!CheckPlatform()) return false;
            if (!CheckVR()) return false;
            if (!CheckDependencies()) return false;

            return true;
        }
        private bool CheckDependencies()
        {
            foreach (var d in this.usedNameSpaces)
            {
                if (!NamespaceExists(d)) return false;
            }
            return true;
        }
        private bool NamespaceExists(string desiredNamespace)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace == desiredNamespace)
                        return true;
                }
            }
            return false;
        }
        private bool CheckPlatform()
        {
            var target = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
            switch (target)
            {
                case UnityEditor.BuildTarget.Android:
                    return (TargetPlatform == TargetNativePlatform.Android);
                case UnityEditor.BuildTarget.iOS:
                    return (TargetPlatform == TargetNativePlatform.Ios);
                case UnityEditor.BuildTarget.StandaloneWindows:
                    return (TargetPlatform == TargetNativePlatform.PC);
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    return (TargetPlatform == TargetNativePlatform.PC);
                case UnityEditor.BuildTarget.StandaloneOSX:
                    return (TargetPlatform == TargetNativePlatform.Mac);
                case UnityEditor.BuildTarget.WebGL:
                    return (TargetPlatform == TargetNativePlatform.WebGL);
                default:
                    return false;

            }
        }

        public static BuildTargetGroup GetGroupByPlatform(TargetNativePlatform platform)
        {
            switch (platform)
            {
                case TargetNativePlatform.Android:
                    return UnityEditor.BuildTargetGroup.Android;
                case TargetNativePlatform.Ios:
                    return UnityEditor.BuildTargetGroup.iOS;
                case TargetNativePlatform.PC:
                    return UnityEditor.BuildTargetGroup.Standalone;
                default:
                    return UnityEditor.BuildTargetGroup.Standalone;

            }
        }

        private bool CheckVR()
        {
            if (this.VRSupported)
            {
                if (!PlayerSettings.virtualRealitySupported)
                {
                    return false;
                }
            }
            else
            {
                if (PlayerSettings.virtualRealitySupported)
                {
                    return false;
                }
            }

            return true;
        }
#endif


    }

}
