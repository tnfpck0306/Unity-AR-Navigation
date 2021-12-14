using ArchToolkit.InputSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#endif

namespace ArchToolkit.Editor.Setup
{

    //References:
    //Input Manager asset manipulation: http://plyoung.appspot.com/blog/manipulating-input-manager-in-script.html
    //Tag Manager asset manipulation: http://answers.unity.com/answers/929868/view.html

    public class InputAndTagSetup
#if UNITY_2019_1_OR_NEWER
        : UnityEditor.PackageManager.UI.IPackageManagerExtension
#endif
    {
        protected static string[] Symbols = new string[]
                {
                    "IS_LEGACY_INPUT_INSTALLED",
                    "AT_EXPLORE_INIT"
                };

        public enum AxisType
        {
            KeyOrMouseButton = 0,
            MouseMovement = 1,
            JoystickAxis = 2
        };


        public void Setup()
        {

            AddTag(ArchToolkitDataPaths.ARCHINTERACTABLETAG.ToString());

            //PC Horizontal
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.Horizontal),
                descriptiveName = "Keyboard Horizontal",
                negativeButton = "left",
                positiveButton = "right",
                altNegativeButton = "a",
                altPositiveButton = "d",
                gravity = 3,
                dead = 0.001f,
                sensitivity = 3,
                snap = true,
                invert = false,
                type = AxisType.KeyOrMouseButton,
                axis = 0,
                joyNum = 0
            });
            //XBOX One Horizontal
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.Horizontal),
                descriptiveName = "Joystick Horizontal",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0.19f,
                sensitivity = 1,
                snap = false,
                invert = false,
                type = AxisType.JoystickAxis,
                axis = 0,
                joyNum = 0
            });
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.Vertical),
                descriptiveName = "Keyboard Vertical",
                negativeButton = "down",
                positiveButton = "up",
                altNegativeButton = "s",
                altPositiveButton = "w",
                gravity = 3,
                dead = 0.001f,
                sensitivity = 3,
                snap = true,
                invert = false,
                type = AxisType.KeyOrMouseButton,
                axis = 0,
                joyNum = 0
            });
            //XBOX One Vertical
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.Vertical),
                descriptiveName = "Joystick Vertical",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0.19f,
                sensitivity = 1,
                snap = false,
                invert = true,
                type = AxisType.JoystickAxis,
                axis = 1,
                joyNum = 0
            });
            //VR RIGHT CONTROLLER HORIZONTAL
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.VR_Horizontal),
                descriptiveName = "VR Right Controller Horizontal Axis",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0.19f,
                sensitivity = 1,
                snap = false,
                invert = true,
                type = AxisType.JoystickAxis,
                axis = 4,
                joyNum = 0
            });
            //VR RIGHT CONTROLLER VERTICAL
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.VR_Vertical),
                descriptiveName = "VR Right Controller Vertical Axis",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0.19f,
                sensitivity = 1,
                snap = false,
                invert = true,
                type = AxisType.JoystickAxis,
                axis = 3,
                joyNum = 0
            });
            //VR Left CONTROLLER HORIZONTAL
            /*AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.VR_Horizontal),
                descriptiveName = "VR Left Controller Horizontal Axis",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0.19f,
                sensitivity = 1,
                snap = false,
                invert = true,
                type = AxisType.JoystickAxis,
                axis = 1,
                joyNum = 0
            });
            //VR Left CONTROLLER VERTICAL
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.VR_Vertical),
                descriptiveName = "VR Left Controller Vertical Axis",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0.19f,
                sensitivity = 1,
                snap = false,
                invert = true,
                type = AxisType.JoystickAxis,
                axis = 2,
                joyNum = 0
            });*/
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.TriggerRight),
                descriptiveName = "VR Trigger right",
                negativeButton = "left",
                positiveButton = "right",
                altNegativeButton = "a",
                altPositiveButton = "d",
                gravity = 3,
                dead = 0.001f,
                sensitivity = 3,
                snap = true,
                invert = false,
                type = AxisType.JoystickAxis,
                axis = 9,
                joyNum = 0
            });
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.TriggerLeft),
                descriptiveName = "VR Trigger left",
                negativeButton = "left",
                positiveButton = "right",
                altNegativeButton = "a",
                altPositiveButton = "d",
                gravity = 3,
                dead = 0.001f,
                sensitivity = 3,
                snap = true,
                invert = false,
                type = AxisType.JoystickAxis,
                axis = 8,
                joyNum = 0
            });
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.Fire1),
                descriptiveName = "Joystic button fire 1 or mouse button",
                negativeButton = "",
                positiveButton = "joystick button 0",
                altNegativeButton = "",
                altPositiveButton = "mouse 0",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = AxisType.KeyOrMouseButton,
                axis = 0
            });
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.MouseScrollWeel),
                descriptiveName = "Mouse Scroll wheel",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0f,
                sensitivity = 1f,
                snap = false,
                invert = false,
                type = AxisType.MouseMovement,
                axis = 2,
                joyNum = 0
            });
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.MouseX),
                descriptiveName = "Mouse X",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0,
                sensitivity = 0.1f,
                snap = false,
                invert = false,
                type = AxisType.MouseMovement,
                axis = 0,
                joyNum = 0
            });
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.MouseY),
                descriptiveName = "Mouse Y",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0,
                sensitivity = 0.1f,
                snap = false,
                invert = false,
                type = AxisType.MouseMovement,
                axis = 1,
                joyNum = 0
            });
            //Mouse X Joystick
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.JOY_MouseX),
                descriptiveName = "Mouse X Joystick",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0.19f,
                sensitivity = 1f,
                snap = false,
                invert = false,
                type = AxisType.JoystickAxis,
                axis = 3,
                joyNum = 0
            });
            //Mouse Y Joystick
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.JOY_MouseY),
                descriptiveName = "Mouse Y Joystick",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0.19f,
                sensitivity = 1f,
                snap = false,
                invert = true,
                type = AxisType.JoystickAxis,
                axis = 4,
                joyNum = 0
            });

            //Escape
            AddAxis(new InputAxis()
            {
                name = InputListener.InputMap(InputListener.ATInputNames.Escape),
                descriptiveName = "Escape",
                negativeButton = "",
                positiveButton = "joystick button 1",
                altNegativeButton = "",
                altPositiveButton = "escape",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1f,
                snap = false,
                invert = false,
                type = AxisType.KeyOrMouseButton,
                axis = 4,
                joyNum = 0
            });
#if !AT_EXPLORE_INIT
            AddScriptingSymbol();
#endif
#if UNITY_2019_1_OR_NEWER && !IS_LEGACY_INPUT_INSTALLED

            if (!EditorPrefs.HasKey("XR_LEGACY_INSTALLED"))
            {
                EditorPrefs.SetBool("XR_LEGACY_INSTALLED", true);
                EditorUtility.DisplayDialog("Warning!", ArchToolkitText.XR_LEGACY_NOT_INSTALLED, "Ok");

                string pdfPath = string.Format("{0}{1}", Application.dataPath, "/Plugins/Ambiens/ArchToolkit/ATExplore/QuickStart.pdf");

                if (System.IO.File.Exists(pdfPath))
                    Application.OpenURL(pdfPath);
                else
                {
                    pdfPath = string.Format("{0}{1}", Application.dataPath, "/Ambiens/ArchToolkit/ATExplore/QuickStart.pdf");

                    if (System.IO.File.Exists(pdfPath))
                        Application.OpenURL(pdfPath);
                }
            }

            if (!EditorPrefs.GetBool("XR_LEGACY_INSTALLED"))
            {
                EditorUtility.DisplayDialog("Warning!", ArchToolkitText.XR_LEGACY_NOT_INSTALLED, "Ok");
            }
#elif !UNITY_2019_1_OR_NEWER
            this.AddScriptingSymbol();
#endif
        }

        public static void AddScriptingSymbol()
        {

            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            allDefines.AddRange(Symbols.Except(allDefines));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
        }
        public static void AddScriptingSymbol(string symbol)
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            if (!allDefines.Contains(symbol))
            {
                allDefines.Add(symbol);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
            }
        }
        public static void RemoveScriptingSymbol()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            List<string> allDefines = definesString.Split(';').ToList();

            allDefines.Remove(Symbols[0]);

            if (allDefines.Count > 1)
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
            else
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(string.Empty, allDefines.ToArray()));
        }
        public static void RemoveScriptingSymbol(string symbol)
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            List<string> allDefines = definesString.Split(';').ToList();

            allDefines.Remove(symbol);

            if (allDefines.Count > 1)
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
            else
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(string.Empty, allDefines.ToArray()));
        }

        public class InputAxis
        {
            public string name;
            public string descriptiveName;
            public string descriptiveNegativeName;
            public string negativeButton;
            public string positiveButton;
            public string altNegativeButton;
            public string altPositiveButton;

            public float gravity;
            public float dead;
            public float sensitivity;

            public bool snap = false;
            public bool invert = false;

            public AxisType type;

            public int axis;
            public int joyNum;
        }

        private static bool AxisDefined(InputAxis input)
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

            serializedObject.ApplyModifiedProperties();

            for (int i = 0; i < axesProperty.arraySize; i++)
            {
                SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(i);
                if (GetChildProperty(axisProperty, "m_Name").stringValue == input.name &&
                    GetChildProperty(axisProperty, "descriptiveName").stringValue == input.descriptiveName) return true;
            }
            return false;

        }

        private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
        {
            SerializedProperty child = parent.Copy();
            child.Next(true);
            do
            {
                if (child.name == name) return child;
            }
            while (child.Next(false));
            return null;
        }

        private static void AddAxis(InputAxis axis)
        {
            if (AxisDefined(axis)) return;

            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

            axesProperty.arraySize++;
            serializedObject.ApplyModifiedProperties();

            SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

            GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
            GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
            GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
            GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
            GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
            GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
            GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
            GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
            GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
            GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
            GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
            GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
            GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
            GetChildProperty(axisProperty, "axis").intValue = axis.axis;
            GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

            serializedObject.ApplyModifiedProperties();
        }

        public static void AddTag(string tag)
        {
            UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if ((asset != null) && (asset.Length > 0))
            {
                SerializedObject so = new SerializedObject(asset[0]);
                SerializedProperty tags = so.FindProperty("tags");

                for (int i = 0; i < tags.arraySize; ++i)
                {
                    if (tags.GetArrayElementAtIndex(i).stringValue == tag)
                    {
                        return;     // Tag already present, nothing to do.
                    }
                }

                tags.InsertArrayElementAtIndex(0);
                tags.GetArrayElementAtIndex(0).stringValue = tag;
                so.ApplyModifiedProperties();
                so.Update();
            }
        }
#if UNITY_2019_1_OR_NEWER
        public VisualElement CreateExtensionUI()
        {
            return null;
        }

        public void OnPackageSelectionChange(UnityEditor.PackageManager.PackageInfo packageInfo)
        {

        }

        public void OnPackageAddedOrUpdated(UnityEditor.PackageManager.PackageInfo packageInfo)
        {
            
            Debug.Log(packageInfo.name);
            if (packageInfo.name == "com.unity.xr.legacyinputhelpers")
            {
                // Add new define symbol
                AddScriptingSymbol();
            }
        }

        public void OnPackageRemoved(UnityEditor.PackageManager.PackageInfo packageInfo)
        {
            if (packageInfo.name == "com.unity.xr.legacyinputhelpers")
            {
                EditorUtility.DisplayDialog("Warning!", "You are removing an essential package for archtoolkit", "Ok");

                RemoveScriptingSymbol();
            }
        }
#endif
    }
}
