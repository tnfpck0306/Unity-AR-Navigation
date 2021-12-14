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

    [CustomNodeEditor(typeof(ShowMultiChoicePanel))]
    public class ShowMultiChoicePanelCustomEditor : NodeEditor
    {
        public override int GetWidth()
        {
           return 400;
        }
        int currentPickerWindow=-1;
        int currentVariantEditingIndex;


        public override void OnBodyGUI()

        {
            //base.OnBodyGUI();

            // Unity specifically requires this to save/update any serial object.
            // serializedObject.Update(); must go at the start of an inspector gui, and
            // serializedObject.ApplyModifiedProperties(); goes at the end.
            serializedObject.Update();
            string[] excludes = { "m_Script", "graph", "position", "ports", "attributes" };

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

            var multi = target as ShowMultiChoicePanel;

            AAssetCollection collection = multi.GetInputValue<AAssetCollection>("collection");
            bool hasCollection = ( collection != null );

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();

            if(!hasCollection){
                if (GUILayout.Button("ADD", GUILayout.Width(60)))
                {
                    var portName = ShowMultiChoicePanel.VARIANT_PORT_NAME + "_" + AActionNodeBase.RandomString(3);
                    var variant = new Variant(null, portName, "New Variant", null, "Variant Description");
                    multi.variants.Add(variant);
                }
            }
            else{
                multi.variants = this.RefreshVariantsFromCollection(multi.variants, collection.assetList);
                NodePort outputPort=null;
                foreach (ambiens.archtoolkit.atexplore.XNode.NodePort dynamicPort in target.DynamicPorts)
                {
                    if (dynamicPort.fieldName == ShowMultiChoicePanel.OUTPUT_PORT_NAME)
                    {
                        outputPort = dynamicPort;
                    }
                }
                if(outputPort==null){
                    this.target.AddDynamicOutput(typeof(Object),
                       XNode.Node.ConnectionType.Multiple,
                         XNode.Node.TypeConstraint.Inherited, ShowMultiChoicePanel.OUTPUT_PORT_NAME);
                }
                NodeEditorGUILayout.PortField(outputPort);
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginVertical(GUI.skin.box);

            var deletedVariants = new List<Variant>();

            for (int i = 0; i < multi.variants.Count; i++)
            {

                var variant =  multi.variants[i];

                NodePort currentPort = null;
                NodePort itemRefPort = null;
                foreach (ambiens.archtoolkit.atexplore.XNode.NodePort dynamicPort in target.DynamicPorts)
                {
                    if(dynamicPort.fieldName== variant.ID)
                    {
                        currentPort = dynamicPort;
                    }
                }

                GUILayout.BeginHorizontal();


                if (GUILayout.Button(variant.preview, GUILayout.Width(60), GUILayout.Height(60)))
                {
                    if (currentPickerWindow != -1)
                    {
                        Debug.LogWarning("Please close the picker first ");
                    }
                    else
                    {
                        currentPickerWindow = EditorGUIUtility.GetControlID(FocusType.Passive) + 100;
                        EditorGUIUtility.ShowObjectPicker<Texture>(variant.preview, false, string.Empty, currentPickerWindow);
                        this.currentVariantEditingIndex = i;
                    }
                }

                Object effectGO = null;
                if (Event.current.commandName == "ObjectSelectorUpdated" && 
                    EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
                {
                    effectGO = EditorGUIUtility.GetObjectPickerObject();
                    currentPickerWindow = -1;
                    multi.variants[this.currentVariantEditingIndex].preview = (Texture2D)effectGO;
                }
                else if(Event.current.commandName == "ObjectSelectorClosed" &&
                    EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
                {
                    Debug.Log("Closed");
                    currentPickerWindow = -1;
                }

                EditorGUILayout.BeginVertical();

                /*if (GUILayout.Button(ArchToolkitWindowData.GetTexture(ArchToolkitDataPaths.RESOURCESEDITORPICKICON), GUILayout.Width(30), GUILayout.Height(30)))
                {
                    var mat = variant.linkedObject as Material;
                    item.preview = SaveTexture(mat.GetInstanceID().ToString(), AssetPreview.GetAssetPreview(mat));
                }*/
                if (GUILayout.Button(ArchToolkitWindowData.GetTexture(ArchToolkitDataPaths.RESOURCESEDITORCAMERAICON), GUILayout.Width(30), GUILayout.Height(30)))
                {
                    ArchToolkit.Editor.Window.ArchMaterialInspector.CaptureWindowScreenshot((Texture2D screen) => {
                        Debug.Log(screen);
                        variant.preview = (Texture2D)screen;
                    });
                    //ScreenCapture.CaptureScreenshot("screen.png",1);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Name", GUILayout.Width(80));
                if (!hasCollection)
                {
                    variant.name = GUILayout.TextField(variant.name, 40);
                }
                else
                {
                    EditorGUILayout.LabelField(variant.name,EditorStyles.boldLabel);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Description", "Add description "), GUILayout.Width(80));
                if (!hasCollection)
                {
                    variant.description = GUILayout.TextField(variant.description, 90);
                }
                else
                {
                    EditorGUILayout.LabelField(variant.description, EditorStyles.boldLabel);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Connected Node", "Add description for material"), GUILayout.Width(80));
                if(currentPort==null)
                {
                    this.target.AddDynamicOutput(typeof(AActionNodeBase),
                        XNode.Node.ConnectionType.Multiple,
                         XNode.Node.TypeConstraint.Inherited, variant.ID);
                }

                EditorGUILayout.BeginVertical();
                NodeEditorGUILayout.PortField(currentPort);
                EditorGUILayout.EndVertical();
                //variant.linkedObject = 
                // NodeEditorGUILayout.PortField(multi.GetPort("variant_out_0"));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                if (!hasCollection)
                {
                    if (GUILayout.Button(new GUIContent("X", "Remove variant"), GUILayout.MaxHeight(20), GUILayout.MaxWidth(20), GUILayout.MinHeight(20), GUILayout.MaxWidth(20), GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        deletedVariants.Add(variant);// aggiunge il gameobject su cui si sta facendo lo switch
                        multi.RemoveDynamicPort(currentPort);
                        if(itemRefPort!=null)
                            multi.RemoveDynamicPort(itemRefPort);

                    }
                }
                EditorGUILayout.EndHorizontal();
               
            }

            if (deletedVariants.Count >= 1) // Delete all variants in temp list
            {
                for (int i = 0; i < deletedVariants.Count; i++)
                {
                    multi.variants.Remove(deletedVariants[i]);
                }

            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

        }

        List<Variant> RefreshVariantsFromCollection(List<Variant> current, List<AAssetCollection.AssetReference> assetFromCollection ){

            List<Variant> toret = new List<Variant>();
            //First we recreate the list from all references
            foreach(var assetRef in assetFromCollection){
                toret.Add(new Variant(assetRef.Asset,
                                    assetRef.ID,
                                    assetRef.Title,
                                    null,
                                    assetRef.Description));
            }
            for (int i = 0; i < toret.Count; i++){
                
                var found = current.Find(v => v.ID == toret[i].ID);
                if(found!=null)
                {
                    if(found.preview == null && toret[i].linkedObject!=null){
                        found.preview = AssetPreview.GetAssetPreview(toret[i].linkedObject as Material);
                    }
                    toret[i].preview = found.preview;
                }
                else{
                    toret[i].preview = AssetPreview.GetAssetPreview(toret[i].linkedObject as Material);
                }
            }
            return toret;
        }

    }


}