using System.Collections.Generic;
using UnityEngine;
using ArchToolkit.Navigation;
using UnityEditor;
using ArchToolkit.Character;
using System.Linq;
using UnityEditorInternal;
using UnityEngine.XR;
using ArchToolkit.Settings;
using UnityEditor.SceneManagement;
using ArchToolkit.Editor.Utils;
using System.Reflection;
using System;

namespace ArchToolkit.Editor.Window
{
    internal enum VisualizationType
    {
        VR,
        Mode360
    }

    internal enum TargetNativePlatformSupported
    {
        Android,
        Ios,
        PC,
        Mac,
        WebGL
    }



    public class ArchCharacterInspector : ArchInspectorBase
    {
        private GameObject _selectedGameObject;

        private bool _isVisibleOnlyInSceneTab = true;

        //[SerializeField]
        //private TargetNativePlatformSupported targetNativePlatformSupported;

        //[SerializeField]
        //private VisualizationType visualizationType = VisualizationType.Mode360;

        //[SerializeField]
        //private int visualizationIndexPopup = 0;

        private int tempLayerMask = 0;

        private Dictionary<string, bool> vrMobileApiSupported = new Dictionary<string, bool> { { "cardboard", false }, { "Oculus", false } };

        //private bool tempIsOculus = false;

        public ArchCharacterInspector(string name) : base(name)
        {

            PlatformChangedListener.OnPlatformSwitched += this.PlatformSwitched;

            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);
            //this.tempIsOculus = this.isOculusFirst(group);

            for (int i = 0; i < this.vrMobileApiSupported.Count; i++)
            {
                var sdk = this.vrMobileApiSupported.Keys.ToList()[i];

                if (EditorPrefs.HasKey("Arch" + sdk))
                {
                    this.vrMobileApiSupported[sdk] = EditorPrefs.GetBool("Arch" + sdk);
                }
            }

        }

        protected virtual void PlatformSwitched(BuildTarget buildTarget)
        {
            //this.CheckPlatform(ref this.targetNativePlatformSupported, buildTarget);
        }

        public override bool IsInspectorVisible()
        {
            if (!this._isVisibleOnlyInSceneTab)
            {
                if (_selectedGameObject == null)
                {
                    this.isInspectorVisible = false;
                    return false;
                }
                var selectedPoint = _selectedGameObject.GetComponent<PathPoint>();

                if (selectedPoint != null)
                {
                    if (selectedPoint.ID == 0)
                    {
                        this.isInspectorVisible = true;
                        return true;
                    }
                }

                this.isInspectorVisible = false;
                return false;
            }
            else
            {
                if (MainArchWindow.Instance.CurrentWindow.GetStatus == WindowStatus.Scene)
                {
                    this.isInspectorVisible = true;
                    return true;
                }
            }

            this.isInspectorVisible = false;
            return false;

        }

        public override void OnClose()
        {
            base.OnClose();

            PlatformChangedListener.OnPlatformSwitched -= this.PlatformSwitched;

            //EditorApplication.playModeStateChanged -= this.EditorStateChanged;
        }

        public override void OnEnable()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);
            //this.tempIsOculus = this.isOculusFirst(group);
        }

        public override void OnFocus()
        {

            //this.CheckPlatform(ref this.targetNativePlatformSupported);
            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);
            //this.tempIsOculus = this.isOculusFirst(group);

            //this.visualizationType = (VisualizationType)EditorPrefs.GetInt("VisualizationType");
        }

        public override void OnGui(Rect pos)
        {
            base.OnGui(pos);

            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);

            if (!this.inspectorFoldoutOpen)
                return;

            if (EditorApplication.isPlaying || EditorApplication.isPaused)
                return;

            if (!ArchToolkitManager.IsInstanced())
            {
                GUILayout.Label("Arch toolkit manager is not instanced");

                ArchToolkitManager.Factory();
            }
            else
            {
                if (ArchToolkitManager.Instance.settings == null)
                {
                    EditorGUILayout.LabelField("Please, you need to create a new Settings");
                    return;
                }

                EditorGUILayout.LabelField("Movement Options", EditorStyles.boldLabel);

                GUILayout.Space(4);

                bool prevGlobalSettings = ArchToolkitManager.Instance.useGlobalSettings;
                ArchToolkitManager.Instance.useGlobalSettings = EditorGUILayout.Toggle("Use Global Settings:", ArchToolkitManager.Instance.useGlobalSettings);

                if (prevGlobalSettings != ArchToolkitManager.Instance.useGlobalSettings)
                {
                    if (ArchToolkitManager.Instance.useGlobalSettings)
                    {
                        ArchToolkitManager.Instance.ForcedSceneSettings = null;
                        ArchToolkitManager.Instance.ForceSettingsRefresh();
                    }
                    else
                    {
                        var currentScene = EditorSceneManager.GetActiveScene();

                        var p = currentScene.path.Replace(currentScene.name + ".unity", "");
                        var n = currentScene.name + "_settings";
                        var found = AssetDatabase.FindAssets(n, new[] { p.Remove(p.Length - 1) });
                        if (found.Count() > 0)
                        {
                            ArchToolkitManager.Instance.ForcedSceneSettings = AssetDatabase.LoadAssetAtPath<ArchSettings>(AssetDatabase.GUIDToAssetPath(found[0]));
                        }
                        else
                        {
                            var s = EditorUtils.CreateAsset<ArchSettings>(n, p);
                            ArchToolkitManager.Instance.ForcedSceneSettings = s;
                        }
                    }
                    EditorUtility.SetDirty(ArchToolkitManager.Instance.gameObject);
                }


                ArchToolkitManager.Instance.settings.movementSpeed = EditorGUILayout.FloatField("Movement Speed:", ArchToolkitManager.Instance.settings.movementSpeed);
                ArchToolkitManager.Instance.settings.clumbSpeed = EditorGUILayout.FloatField("Climb Speed:", ArchToolkitManager.Instance.settings.clumbSpeed);
                ArchToolkitManager.Instance.settings.MouseRotationSpeed = EditorGUILayout.FloatField("Mouse Rotation Speed:", ArchToolkitManager.Instance.settings.MouseRotationSpeed);
                ArchToolkitManager.Instance.settings.TouchRotationSpeed = EditorGUILayout.FloatField("Touch Rotation Speed:", ArchToolkitManager.Instance.settings.TouchRotationSpeed);

                ArchToolkitManager.Instance.settings.UseRightClickToRotate = EditorGUILayout.Toggle("Use Right Click to Rotate:", ArchToolkitManager.Instance.settings.UseRightClickToRotate);

                ArchToolkitManager.Instance.settings.RunFaster = EditorGUILayout.FloatField("Run Faster:", ArchToolkitManager.Instance.settings.RunFaster);
                ArchToolkitManager.Instance.settings.RunSpeed = EditorGUILayout.FloatField("Run Speed:", ArchToolkitManager.Instance.settings.RunSpeed);

                this.tempLayerMask = EditorGUILayout.MaskField(new GUIContent("Walkable layers: ", ArchToolkitText.WALKABLE_LAYERS_TOOLTIP), InternalEditorUtility.LayerMaskToConcatenatedLayersMask(ArchToolkitManager.Instance.settings.walkableLayers.value),
                                                                                                                      InternalEditorUtility.layers);

                ArchToolkitManager.Instance.settings.walkableLayers = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(this.tempLayerMask);
                this.tempLayerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(this.tempLayerMask);
                ArchToolkitManager.Instance.settings.walkableLayers = this.tempLayerMask;

                GUILayout.Space(2);

                ArchToolkitManager.Instance.settings.lockMovementTo = (LockMovementTo)EditorGUILayout.EnumPopup("Lock Movement:", ArchToolkitManager.Instance.settings.lockMovementTo);

                if (ArchToolkitManager.Instance.settings.lockMovementTo == LockMovementTo.None)
                {
                    ArchToolkitManager.Instance.settings.movementType = (MovementType)EditorGUILayout.EnumPopup("Movement Type:", ArchToolkitManager.Instance.settings.movementType);
                }
                else
                {
                    if (ArchToolkitManager.Instance.settings.lockMovementTo == LockMovementTo.Classic)
                    {
                        ArchToolkitManager.Instance.settings.movementType = MovementType.Classic;
                    }
                    else if (ArchToolkitManager.Instance.settings.lockMovementTo == LockMovementTo.FlyCam)
                    {
                        ArchToolkitManager.Instance.settings.movementType = MovementType.FlyCam;
                    }
                }
                GUILayout.Space(10);
                EditorGUILayout.LabelField("VR Options", EditorStyles.boldLabel);
                GUILayout.Space(4);

                ArchToolkitManager.Instance.settings.EnableVRSmoothMovement = EditorGUILayout.Toggle("(Beta) VR Smooth Movement:", ArchToolkitManager.Instance.settings.EnableVRSmoothMovement);
                ArchToolkitManager.Instance.settings.VRFadeTime = EditorGUILayout.FloatField("(Beta) VR TP fade time:", ArchToolkitManager.Instance.settings.VRFadeTime);


                GUILayout.Space(10);
                EditorGUILayout.LabelField("UI Options", EditorStyles.boldLabel);
                GUILayout.Space(4);

                ArchToolkitManager.Instance.settings.UIShowTouchControls = EditorGUILayout.Toggle("Show Touch Controls:", ArchToolkitManager.Instance.settings.UIShowTouchControls);
                ArchToolkitManager.Instance.settings.UIShowControlsGuide = EditorGUILayout.Toggle("Show Keys:", ArchToolkitManager.Instance.settings.UIShowControlsGuide);

                GUILayout.Space(10);

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(ArchToolkitManager.Instance.settings);
                    AssetDatabase.SaveAssets();
                }


            }
        }
        /*
        private void SetVRDevices(bool StartAsVR, bool isOculusBuild = false)
        {

            this.SetVRDevicesForPlatform(StartAsVR, isOculusBuild, BuildTargetGroup.Standalone);
            this.SetVRDevicesForPlatform(StartAsVR, isOculusBuild, BuildTargetGroup.Android);
            this.SetVRDevicesForPlatform(StartAsVR, isOculusBuild, BuildTargetGroup.iOS);

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

        }*/
        /*
        bool isOculusFirst(BuildTargetGroup group)
        {

            var currentTargetDevices = PlayerSettings.GetVirtualRealitySDKs(group);
            if (currentTargetDevices.Count() == 0)
            {
                return false;
            }
            if (currentTargetDevices[0] == "Oculus")
            {
                return true;
            }
            return false;
        }*/
        /*void SetVRDevicesForPlatform(bool StartAsVR, bool isOculusBuild, BuildTargetGroup group)
        {
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
                        currentTargetDevices = new string[] { "OpenVR", "Oculus" };
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
        }*/


        public override void OnSelectionChange(GameObject gameObject)
        {
            this._selectedGameObject = gameObject;

            //this.CheckPlatform(ref this.targetNativePlatformSupported);
        }

        public override void OnUpdate()
        {

        }

        /*private bool IsOnMobile()
        {
            return (this.targetNativePlatformSupported == TargetNativePlatformSupported.Android ||
                    this.targetNativePlatformSupported == TargetNativePlatformSupported.Ios);
        }*/
        public bool HasType(string typeName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == typeName)
                        return true;
                }
            }

            return false;
        }
        public bool NamespaceExists(string desiredNamespace)
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

#if AVR_OCULUS_QUEST_HAND_TRACKING
        public static void CheckOrInitOculusQuestHandTracking()
        {
            var hands = GameObject.FindObjectsOfType<OVRHand>();
            if (hands.Length == 0)
            {
                var handPrefab = AssetDatabase.FindAssets("AVR_OVRHandPrefab");
                if (handPrefab.Length > 0)
                {
                    var handP = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(handPrefab[0]));
                    var go=GameObject.Instantiate(handP);
                }
            }
        }
#endif
    }
}
