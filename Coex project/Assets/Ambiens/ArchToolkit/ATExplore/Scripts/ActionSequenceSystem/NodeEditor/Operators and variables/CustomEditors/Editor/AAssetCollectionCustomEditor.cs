using ambiens.archtoolkit.atexplore.XNode;
using ArchToolkit;
using ArchToolkit.AnimationSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;
namespace ambiens.archtoolkit.atexplore.actionSystem
{

    public abstract class AAssetCollectionCustomEditor : NodeEditor
    {
        public override int GetWidth()
        {
            return 400;
        }
        //int currentPickerWindow = -1;
        Variant currentVariantEditing;

        protected abstract void DrawAssetPicker(AAssetCollection.AssetReference variant );
        protected abstract List<AAssetCollection.AssetReference> GetAssetList();
        protected abstract void SetAssetList(List<AAssetCollection.AssetReference> list);
        protected abstract AAssetCollection GetTarget();
        protected abstract string GetConnectionName();

        public override void OnBodyGUI()
        {
            //base.OnBodyGUI();

            // Unity specifically requires this to save/update any serial object.
            // serializedObject.Update(); must go at the start of an inspector gui, and
            // serializedObject.ApplyModifiedProperties(); goes at the end.
            serializedObject.Update();
            string[] excludes = { "m_Script", "graph", "position", "ports" };

            // Iterate through serialized properties and draw them like the Inspector (But with ports)
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            EditorGUIUtility.labelWidth = 84;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (excludes.Contains(iterator.name)) continue;
                NodeEditorGUILayout.PropertyField(iterator, true);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();


            var assetList = GetAssetList();
            var aCollection = this.GetTarget();
            //Gestire Variants

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("ADD", GUILayout.Width(60)))
            {
                var portName = this.GetConnectionName() + "_" + AActionNodeBase.RandomString(3);
                var variant = new AAssetCollection.AssetReference() { Title = "", Description = "", ID = portName };
                assetList.Add(variant);
                aCollection.AddDynamicOutput(typeof(Material),
                    XNode.Node.ConnectionType.Multiple,
                    XNode.Node.TypeConstraint.Inherited, portName);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(GUI.skin.box);

            var deletedVariants = new List<AAssetCollection.AssetReference>();

            foreach (var item in assetList)
            {

                var variant = item;

                NodePort currentPort = null;
                foreach (ambiens.archtoolkit.atexplore.XNode.NodePort dynamicPort in aCollection.DynamicPorts)
                {
                    if (dynamicPort.fieldName == item.ID)
                    {
                        currentPort = dynamicPort;
                    }

                }

                GUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Title", GUILayout.Width(80));
                variant.Title = GUILayout.TextField(variant.Title, 40);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Description", "Add description "), GUILayout.Width(80));
                variant.Description = GUILayout.TextField(variant.Description, 90);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Asset", "Select Asset"), GUILayout.Width(80));
                this.DrawAssetPicker(variant);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Connected Node", "Add description for material"), GUILayout.Width(80));
                NodeEditorGUILayout.PortField(currentPort);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                if (GUILayout.Button(new GUIContent("X", "Remove variant"), GUILayout.MaxHeight(20), GUILayout.MaxWidth(20), GUILayout.MinHeight(20), GUILayout.MaxWidth(20), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    deletedVariants.Add(variant);// aggiunge il gameobject su cui si sta facendo lo switch
                    try
                    {
                        aCollection.RemoveDynamicPort(currentPort);
                    }
                    catch(System.Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10);
            }

            if (deletedVariants.Count >= 1) // Delete all variants in temp list
            {
                for (int i = 0; i < deletedVariants.Count; i++)
                {
                    assetList.Remove(deletedVariants[i]);
                }

            }
            this.SetAssetList(assetList);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();


        }

    }


}
