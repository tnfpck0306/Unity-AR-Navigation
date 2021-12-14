using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ArchToolkit.AnimationSystem;
using ArchToolkit.Utils.Texture;
using System.Linq;
using ArchToolkit.Utils;
using ArchToolkit.Utils.String;

namespace ArchToolkit.Editor.Window
{

    [System.Serializable]
    public class ArchMaterialInspector : ArchInspectorBase
    {
        [SerializeField]
        public Object currentEditableObject;

        public Object currentMaterialToChange;

        public static ArchMaterialInspector Instance;

        public System.Action OnObjectSelectorClosed;

        private ArchMaterialAnimation currentAnimation;

        private bool materialSelectedFoldoutOpen = true;

        private bool isMaterialPanelOpened = false;

        private bool isTexturePreviewOpened = false;

        [SerializeField]
        private List<ArchMaterialAnimation> animations = new List<ArchMaterialAnimation>();

        private int materialIndex = 0;

        private List<string> materialsName = new List<string>();

        private Vector2 scroll = Vector2.zero;

        public ArchMaterialInspector(string name) : base(name)
        {
            Instance = this;
        }

        public override void OnUpdate()
        {
            this.CheckInformation();
        }

        public override void OnEnable()
        {
            this.OnSelectionChange(Selection.activeGameObject);

            this.OnObjectSelectorClosed += this.CheckPickerEvent;

            foreach (var item in GameObject.FindObjectsOfType<ArchMaterialAnimation>()) // when the window is opening, refresh every time the animations list, because when window is closed, all datas are deleted
            {
                if (item == null)
                    continue;

                if (!this.animations.Contains(item))
                    this.animations.Add(item);
            }

        }

        public override void OnProjectChange()
        {
            if (this.currentAnimation == null)
                return;

            var currentAnim = this.currentAnimation;

            var currentMaterial = this.currentMaterialToChange;

            var currentEditable = this.currentEditableObject;

            this.animations = GameObject.FindObjectsOfType<ArchMaterialAnimation>().ToList();

            foreach (var animation in this.animations)
            {
                if (animation == null)
                    continue;

                if (this.currentAnimation == null && this.currentAnimation.SwitchInfo == null)
                    continue;

                if (this.currentAnimation.SwitchInfo.Count <= 0)
                    continue;

                this.currentAnimation = animation as ArchMaterialAnimation;
                this.currentEditableObject = animation.gameObject.GetComponent<MeshRenderer>();

                var sWinfo = this.currentAnimation.SwitchInfo.Find(info => info.isDefaultMaterial);

                if (sWinfo == null)
                    continue;

                this.currentMaterialToChange = sWinfo.materialToChange;

                this.CheckInformation();
            }

            this.currentAnimation = currentAnim;
            this.currentMaterialToChange = currentMaterial;
            this.currentEditableObject = currentEditable;
        }

        public override void OnClose()
        {
            this.OnObjectSelectorClosed = null;
        }

        public override void OnGui(Rect pos)
        {
            base.OnGui(pos);

            if (Event.current != null) // Check event, only in the ongui method this can works
            {
                if (Event.current.commandName == "ObjectSelectorClosed" && this.isMaterialPanelOpened)
                {
                    if (this.OnObjectSelectorClosed != null)
                        this.OnObjectSelectorClosed();

                    this.OnObjectSelectorClosed -= this.OnObjectSelectorClosed; // remove it!

                    this.isMaterialPanelOpened = false;
                }

                if (Event.current.commandName == "ObjectSelectorClosed" && this.isTexturePreviewOpened)
                {
                    if (this.OnObjectSelectorClosed != null)
                        this.OnObjectSelectorClosed();

                    this.OnObjectSelectorClosed -= this.OnObjectSelectorClosed; // remove it!

                    this.isTexturePreviewOpened = false;
                }
            }

            if (this.inspectorFoldoutOpen)
                this.CreateBody();

        }

        public void SetAnimation(GameObject gameObject)
        {
            if (gameObject == null)
            {
                if (Selection.gameObjects.Length == 0)
                {
                    EditorUtility.DisplayDialog("WARNING", "Select a Gameobject to add interaction", "Ok");
                }

                return;
            }

            if (gameObject.GetComponent<MeshRenderer>() == null)
            {
                EditorUtility.DisplayDialog("WARNING", "Gameobject need the meshrenderer component, to add interaction", "Ok");

                return;

            }
            if (!gameObject.GetComponent<ArchMaterialAnimation>())
            {
                this.currentAnimation = gameObject.AddComponent<ArchMaterialAnimation>();

                this.currentEditableObject = gameObject.GetComponent<MeshRenderer>();

                var go = gameObject;

                this.currentAnimation = go.GetComponent<ArchMaterialAnimation>();

                var rend = (MeshRenderer)this.currentEditableObject;

                this.currentMaterialToChange = rend.sharedMaterials[0]; // as default

                this.currentAnimation.SwitchInfo.Add(new SwitchMaterialInfo(rend, this.currentMaterialToChange as Material, rend.sharedMaterials.ToList().FindIndex(index => index == this.currentMaterialToChange as Material), true));

                this.AddVariant(this.currentMaterialToChange as Material);

                this.animations.Add(this.currentAnimation);

                if (!this.currentAnimation.isHandleAlreadySetted)
                {
                    var handle = this.currentAnimation.SetHandle(this.currentAnimation.gameObject.transform.position);
                    
                    // Spawn handle front of editor camera
                    handle.transform.position = ArchToolkitProgrammingUtils.FrontOfEditorCamera();

                    handle.transform.SetParent(EditorUpdateManager.MaterialSwitchContainer.transform);

                    Selection.activeObject = handle.gameObject;

                    this.currentAnimation.onDestroy += () => { GameObject.DestroyImmediate(handle.gameObject); };
                }
            }

            this.OnSelectionChange(Selection.activeGameObject);
        }

        public override void OnSelectionChange(GameObject gameObject)
        {
            if (EditorApplication.isCompiling)
                return;

            if (gameObject == null)
            {
                this.currentAnimation = null;
                this.currentEditableObject = null;
                this.currentMaterialToChange = null;
                this.isInspectorVisible = false;
                return;
            }

            var selected = gameObject;

            if (selected != null)
            {

                var anim = selected.GetComponent<ArchMaterialAnimation>();

                if (anim != null)
                {
                    this.currentEditableObject = selected.GetComponent<MeshRenderer>();

                    this.currentAnimation = anim;

                }
                else
                {
                    var handle = selected.GetComponent<ArchBasicHandle>();

                    if (handle != null)
                    {
                        if (handle.animationToOpen != null)
                        {
                            this.currentAnimation = handle.animationToOpen as ArchMaterialAnimation;

                            this.currentEditableObject = handle.animationToOpen.gameObject.GetComponent<MeshRenderer>();
                        }
                    }
                    else
                    {
                        var mr = selected.GetComponent<MeshRenderer>();

                        if (mr != null)
                            this.currentEditableObject = mr;
                        else
                            this.currentEditableObject = null;

                        this.currentAnimation = null;

                        this.currentMaterialToChange = null;
                    }
                }

                if (this.currentAnimation != null && this.currentAnimation.SwitchInfo != null && this.currentAnimation.SwitchInfo.Count > 0)
                    this.currentMaterialToChange = this.currentAnimation.SwitchInfo[0].materialToChange;
            }

            if (this.currentAnimation == null)
                this.isInspectorVisible = false;

        }

        public override bool IsInspectorVisible()
        {
            if (!ArchToolkitManager.IsInstanced()) return false;

            if (this.currentAnimation == null)
            {
                this.isInspectorVisible = false;
                return false;
            }
            if (MainArchWindow.Instance.CurrentWindow.GetStatus != WindowStatus.Scene)
            {
                this.isInspectorVisible = true;
                return true;
            }
            this.isInspectorVisible = false;

            return false;
        }

        private void CheckInformation()
        {
            if (this.currentAnimation == null) return;

            this.currentAnimation.variants.RemoveAll(v => v.linkedObject == null);

            this.animations.RemoveAll(anim => anim == null);

            this.currentAnimation.SwitchInfo.RemoveAll(info => info.materialToChange == null || info.meshRenderer == null);

            this.currentAnimation.SwitchInfo = this.currentAnimation.SwitchInfo.Distinct().ToList();

            if (this.currentAnimation.SwitchInfo.Count <= 0)
            {
                if (this.currentMaterialToChange == null)
                    return;

                if (this.currentEditableObject == null)
                    return;

                var mr = this.currentEditableObject as MeshRenderer;
                var index = mr.sharedMaterials.ToList().FindIndex(m => m == this.currentMaterialToChange as Material);

                if (!this.currentAnimation.SwitchInfo.Exists(info => info.materialToChange == this.currentMaterialToChange as Material))
                    this.currentAnimation.SwitchInfo.Add(new SwitchMaterialInfo(mr, this.currentMaterialToChange as Material, index, true));
            }
        }

        private void CreateBody()
        {

            GUILayout.Space(ArchToolkitWindowData.PADDING);

            if (this.currentAnimation == null)
            {

                if (GUILayout.Button(ArchToolkitText.ADD_SWITCH_MATERIAL))
                    this.SetAnimation(Selection.activeGameObject);

                return;

            }

            this.CreateEditableField();

            GUILayout.Space(ArchToolkitWindowData.PADDING);

            if (this.materialSelectedFoldoutOpen = EditorGUILayout.Foldout(this.materialSelectedFoldoutOpen, new GUIContent(ArchToolkitText.MATERIAL_FOLDOUT_NAME, ArchToolkitText.MATERIAL_OPTIONS_TOOLTIP), true))
            {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();

                if (ArchToolkitWindowData.CreateButtonAdd())
                {
                    int controlID = EditorGUIUtility.GetControlID(FocusType.Passive) + 999;

                    EditorGUIUtility.ShowObjectPicker<Material>(this.currentEditableObject, true, "", controlID);

                    this.OnObjectSelectorClosed += this.CheckPickerEvent; // Add it when is open

                    this.isMaterialPanelOpened = true;
                }
                if (ArchToolkitWindowData.CreateButtonMinus())
                {
                    if (this.currentEditableObject != null)
                    {
                        this.currentAnimation.variants.RemoveAt(this.currentAnimation.variants.Count - 1);
                    }

                }

                EditorGUILayout.EndHorizontal();

                //scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Width(MainArchWindow.Instance.position.width - 10), GUILayout.Height(200));

                EditorGUILayout.BeginVertical(GUI.skin.box);

                var deletedVariants = new List<Variant>();

                if (this.currentAnimation != null)
                {
                    foreach (var item in this.currentAnimation.variants)
                    {
                        if (item == null || item.linkedObject == null)
                            continue;

                        var variant = item;

                        GUILayout.BeginHorizontal();
                        
                        if(item.preview == null)
                            item.preview = AssetPreview.GetAssetPreview(variant.linkedObject as Material);

                        if (GUILayout.Button(item.preview, GUILayout.Width(60), GUILayout.Height(60)))
                        {
                            this.isTexturePreviewOpened = true;

                            EditorGUIUtility.ShowObjectPicker<Texture>(item.preview,false,string.Empty,111);

                            this.OnObjectSelectorClosed += () => 
                            {
                                if(this.isTexturePreviewOpened)
                                {
                                    item.preview = (Texture2D)EditorGUIUtility.GetObjectPickerObject();
                                    EditorUtility.SetDirty(this.currentAnimation);
                                }
                            };
                        }
                        
                        EditorGUILayout.BeginVertical();

                        if (GUILayout.Button(ArchToolkitWindowData.GetTexture(ArchToolkitDataPaths.RESOURCESEDITORPICKICON), GUILayout.Width(30), GUILayout.Height(30)))
                        {
                            var mat=variant.linkedObject as Material;
                            item.preview = SaveTexture(mat.GetInstanceID().ToString(), AssetPreview.GetAssetPreview(mat));
                        }
                        if (GUILayout.Button(ArchToolkitWindowData.GetTexture(ArchToolkitDataPaths.RESOURCESEDITORCAMERAICON), GUILayout.Width(30), GUILayout.Height(30)))
                        {
                            CaptureWindowScreenshot( (Texture2D screen)=>{
                                Debug.Log(screen);
                                item.preview=(Texture2D)screen;
                                } );
                            //ScreenCapture.CaptureScreenshot("screen.png",1);
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Name", GUILayout.Width(80));
                        variant.name = GUILayout.TextField(variant.linkedObject.name, 40);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("Description", "Add description for material"), GUILayout.Width(80));
                        variant.description = GUILayout.TextField(variant.description, 90);
                        EditorGUILayout.EndHorizontal();


                        EditorGUILayout.EndVertical();
                        if (GUILayout.Button(new GUIContent("X", "Remove variant"), GUILayout.MaxHeight(20), GUILayout.MaxWidth(20), GUILayout.MinHeight(20), GUILayout.MaxWidth(20), GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            deletedVariants.Add(variant);// aggiunge il gameobject su cui si sta facendo lo switch
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                }

                if (deletedVariants.Count >= 1) // Delete all variants in temp list
                {
                    for (int i = 0; i < deletedVariants.Count; i++)
                    {
                        this.currentAnimation.variants.Remove(deletedVariants[i]);
                    }

                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();

                //EditorGUILayout.EndScrollView();
            }

            GUILayout.BeginHorizontal();
            var color = GUI.backgroundColor;
            GUI.backgroundColor = ArchToolkitWindowData.ApplyColorButton;
            if (GUILayout.Button(new GUIContent(ArchToolkitText.APPLY, ArchToolkitText.APPLY_SWITCH_MATERIAL_TOOLTIP), GUILayout.Width(MainArchWindow.Instance.position.size.x / 3 - 7)))
            {
                if (this.currentEditableObject == null)
                    return;

                if (this.currentAnimation == null)
                    return;

                // TODO: Capire se non far creare la handle
                if (this.currentMaterialToChange == null)
                {
                    EditorUtility.DisplayDialog("Warning", ArchToolkitText.FORGOTMATERIALTOSWITCH, "Ok");
                    return;
                }

                if (this.currentMaterialToChange != null)
                {
                    if (!this.currentAnimation.SwitchInfo[0].meshRenderer.sharedMaterials.ToList().Contains(this.currentMaterialToChange as Material))
                    {
                        EditorUtility.DisplayDialog("Warning", ArchToolkitText.MATERIALSWITCHWRONG, "Ok");
                        return;
                    }
                }

                
            }
            GUI.backgroundColor = color;
            if (this.currentAnimation.SwitchInfo.Count <= 1)
            {
                if (GUILayout.Button(new GUIContent(ArchToolkitText.ITERATE_MATERIAL, ArchToolkitText.FIND_ALL_MATERIALS_TOOLTIP), GUILayout.Width(MainArchWindow.Instance.position.size.x / 3 - 7)))
                {
                    this.SearchMeshRendererWithMaterials();
                }
            }
             
            if(this.currentAnimation.SwitchInfo.Count > 1)
            {
                if (GUILayout.Button(new GUIContent(ArchToolkitText.REVERT_MATERIAL, ArchToolkitText.FIND_ALL_MATERIALS_TOOLTIP), GUILayout.Width(MainArchWindow.Instance.position.size.x / 3 - 7)))
                {
                    this.RevertSwitchInfo();
                }
            }

            if (GUILayout.Button(new GUIContent(ArchToolkitText.REMOVE_SWITCH_INTERACTION, "Delete switch material"), GUILayout.Width(MainArchWindow.Instance.position.size.x / 3 - 7)))
            {
                this.animations.Remove(this.currentAnimation);

                if (this.currentAnimation.GetHandleSetted != null)
                    GameObject.DestroyImmediate(this.currentAnimation.GetHandleSetted.gameObject);

                GameObject.DestroyImmediate(this.currentAnimation);


                this.currentEditableObject = null;
                this.currentAnimation = null;

                Selection.activeGameObject = null;

            }

            GUILayout.EndHorizontal();

        }

        private void RevertSwitchInfo()
        {
            if (this.currentAnimation == null)
                return;

            if (this.currentAnimation.SwitchInfo.Count <= 0)
                return;

            this.currentAnimation.SwitchInfo.RemoveRange(1,this.currentAnimation.SwitchInfo.Count - 1);
        }

        private void SearchMeshRendererWithMaterials()
        {
            if (this.currentAnimation == null)
                return;

            if (this.currentAnimation.SwitchInfo.Count <= 0)
                return;

            var mrs = GameObject.FindObjectsOfType<MeshRenderer>();

            if (mrs != null && mrs.Length > 0)
            {
                foreach (var meshR in mrs)
                {
                    if (meshR == null)
                        continue;

                    var mat = meshR.sharedMaterials.ToList().Find(m => m == this.currentAnimation.SwitchInfo[0].materialToChange);

                    if (mat == null)
                        continue;

                    var index = meshR.sharedMaterials.ToList().FindIndex(m => m == mat);

                    if (!this.currentAnimation.SwitchInfo.Exists(info => info.meshRenderer == meshR))
                        this.currentAnimation.SwitchInfo.Add(new SwitchMaterialInfo(meshR, mat, index));
                }
            }

            EditorUtility.ClearProgressBar();
        }
        public static void CaptureWindowScreenshot(System.Action<Texture2D> onComplete)
        {
            int width=256;
            int height=256;

            GameObject go = new GameObject("fCam");
            Camera cam = go.AddComponent<Camera>();
            cam.farClipPlane = 150;
            var lastSelection = Selection.objects;
            Selection.objects = new GameObject[] {cam.gameObject };
            EditorApplication.delayCall += () =>
            {
                SceneView.lastActiveSceneView.AlignWithView();
                EditorApplication.delayCall += () =>
                {
                    RenderTexture rt = new RenderTexture(width, height, 24);
                    cam.targetTexture = rt;
                    Texture2D dest = new Texture2D(width, height, TextureFormat.RGB24, false);
                    cam.Render();
                    RenderTexture.active = rt;
                    dest.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                    cam.targetTexture = null;
                    RenderTexture.active = null;
                    GameObject.DestroyImmediate(rt);
                    string name=string.Format("switch_{0}x{1}_{2}",
                              width, height,
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
                    string fLoc = Application.dataPath + "/"+name+".png";
                    System.IO.File.WriteAllBytes(fLoc, dest.EncodeToPNG());
                    AssetDatabase.Refresh();
                    EditorApplication.delayCall += () =>
                    {
                        GameObject.DestroyImmediate(go);
                        if(lastSelection!=null)
                            Selection.objects = lastSelection;

                        onComplete(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/"+name+".png"));
                    };
                };
            };
        }

        Texture2D SaveTexture(string name, Texture2D txt){
            string fLoc = Application.dataPath + "/"+name+".png";
            System.IO.File.WriteAllBytes(fLoc, txt.EncodeToPNG());
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/"+name+".png");
        }
        protected void CreateEditableField()
        {
            var previousObject = this.currentEditableObject;
            var previousMaterial = this.currentMaterialToChange;
            this.currentEditableObject = EditorGUILayout.ObjectField("Editable Object", this.currentEditableObject, typeof(MeshRenderer), true);

            var mR = this.currentEditableObject as MeshRenderer;

            if (this.currentEditableObject == null && mR == null)
            {
                // if null or equal do nothing
                return;
            }

            if (previousObject != this.currentEditableObject)
            {
                this.currentAnimation.variants.RemoveAt(this.currentAnimation.variants.FindIndex(v => v.linkedObject as Material == this.currentMaterialToChange));
                this.currentMaterialToChange = null;
            }

            this.materialsName.Clear();

            foreach (var mats in mR.sharedMaterials)
            {
                if (mats == null)
                    continue;

                this.materialsName.Add(mats.name);
            }

            if (materialsName.Count == 0)
            {
                var errorStyle = new GUIStyle(GUI.skin.label);

                errorStyle.normal.textColor = Color.red;
                errorStyle.fontSize = 10;
                errorStyle.wordWrap = true;
                GUILayout.Label("Error! meshrenderer must have at least one material assigned.", errorStyle);
                return;
            }

            if(this.currentAnimation.SwitchInfo.Count == 0)
            {
                this.currentMaterialToChange = mR.sharedMaterials[0];
                this.AddVariant(this.currentMaterialToChange as Material);
                this.currentAnimation.SwitchInfo.Add(new SwitchMaterialInfo(mR, this.currentMaterialToChange as Material,0,true));
            }

            this.materialIndex = EditorGUILayout.Popup("Set material to change", this.currentAnimation.SwitchInfo[0].matIndex, this.materialsName.ToArray());

            this.currentMaterialToChange = mR.sharedMaterials[this.materialIndex] as Material;

            if (previousMaterial != this.currentMaterialToChange)
            {
                if (previousMaterial != null)
                {
                    int index = this.currentAnimation.variants.FindIndex(v => v.linkedObject as Material == previousMaterial);
                    if (index != -1)
                    {
                        this.currentAnimation.variants.RemoveAt(index);
                        previousMaterial = null;
                    }
                }
            }

            var go = ((MeshRenderer)this.currentEditableObject).gameObject;

            if (this.currentAnimation.SwitchInfo.Count > 0 && this.currentAnimation.SwitchInfo[0].materialToChange != this.currentMaterialToChange as Material) // if material to change, is changed
            {
                var rend = (MeshRenderer)this.currentEditableObject;

                this.currentAnimation.SwitchInfo.Clear();

                var index = rend.sharedMaterials.ToList().FindIndex(m => m == this.currentMaterialToChange);

                this.currentAnimation.SwitchInfo.Add(new SwitchMaterialInfo(rend, this.currentMaterialToChange as Material, index, true));

                this.AddVariant(this.currentMaterialToChange as Material);
            }
        }

        protected void CheckPickerEvent()
        {

            if (this.currentEditableObject == null) // if there isn't an editable object, return
                return;

            if (this.currentAnimation == null)
            {
                this.currentAnimation = ((MeshRenderer)this.currentEditableObject).gameObject.GetComponent<ArchMaterialAnimation>();

                if (this.currentAnimation == null)
                {
                    this.currentAnimation = ((MeshRenderer)this.currentEditableObject).gameObject.AddComponent<ArchMaterialAnimation>();
                }
            }

            var obj = EditorGUIUtility.GetObjectPickerObject();

            Material material = obj as Material;

            if (obj == null || material == null)
                return;


            this.AddVariant(material);

            if (!this.animations.Contains(this.currentAnimation))
                this.animations.Add(this.currentAnimation);
        }

        private void AddVariant(Material material)
        {
            if (material == null)
                return;

            var id = ArchStringUtils.GetHashString(material.name);

            if (!this.currentAnimation.variants.Exists(value => value.ID == id))
                this.currentAnimation.variants.Add(new Variant(material, id, material.name, material.MakeScreenShotFromMaterial(256, 256)));

        }

        private void RemoveVariant(Material material)
        {
            if (material == null)
                return;

            var id = ArchStringUtils.GetHashString(material.name);

            var variant = this.currentAnimation.variants.Find(value => value.ID == id);
            if (variant != null)
                this.currentAnimation.variants.Remove(variant);
        }
    }
}