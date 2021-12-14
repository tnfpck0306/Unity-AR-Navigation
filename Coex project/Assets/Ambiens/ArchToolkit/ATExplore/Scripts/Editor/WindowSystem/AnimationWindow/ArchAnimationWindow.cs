using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit.AnimationSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ArchToolkit.Editor.Window
{
    public class ArchAnimationWindow : ArchWindowBase
    {
        public ArchAnimationWindow(WindowStatus status) : base(status)
        {
            
        }
        public Texture2D rotateTexture;
        public Texture2D traslateTexture;
        public Texture2D switchMatTexture;
        public Texture2D draggableTexture;
        public Texture2D draggableTextureADD;
        public Texture2D actionSequenceTexture;

        public static int width = 40;
        public static int height = 40;


        protected override void _DrawGUI()
        {
            EditorGUILayout.BeginHorizontal();
            this.InitTextures();

            if (GUILayout.Button(
                new GUIContent(rotateTexture, ArchToolkitText.ROTATION_TOOLTIP),
                GUILayout.Height(width),
                GUILayout.Width(height)
                )
            )
            {
                ArchDoorInspector.Instance.AddAnimation<RotateAround>();
            }

            if (GUILayout.Button(
                new GUIContent(traslateTexture, ArchToolkitText.TRANSLATION_TOOLTIP),
                GUILayout.Height(width),
                GUILayout.Width(height)
                )
                )
            {
                ArchDrawerInspector.Instance.AddAnimation<Translate>();
            }

            if(GUILayout.Button(
                new GUIContent(switchMatTexture, ArchToolkitText.ADD_SWITCH_MATERIAL),
                GUILayout.Height(width),
                GUILayout.Width(height)
                )
                )
            {
                ArchMaterialInspector.Instance.SetAnimation(Selection.activeGameObject);
            }

            if (Selection.activeGameObject != null)
            {
                var d = Selection.activeGameObject.GetComponent<DraggableObjectInteractable>();
                if (Selection.activeGameObject != null && d == null)
                {
                    if (GUILayout.Button(
                        new GUIContent(draggableTexture, ArchToolkitText.DRAGGABLE_TOOLTIP),
                        GUILayout.Height(width),
                        GUILayout.Width(height)
                        )
                        )
                    {
                        DraggableInteractionInspector.AddDraggable(Selection.activeGameObject);
                    }
                }
                else if (d != null)
                {
                    if (GUILayout.Button(
                        new GUIContent(draggableTexture, ArchToolkitText.REMOVE_DRAGGABLE),
                        GUILayout.Height(width),
                        GUILayout.Width(height)
                        )
                        )
                    {
                        GameObject.DestroyImmediate(d);
                    }
                }
            }
            /*
            EditorGUI.BeginDisabledGroup(!this.IsMobile());

            if (GUILayout.Button(new GUIContent(ArchToolkitText.ADD_TELEPORT_POINT,ArchToolkitText.VR_MOBILE_TELEPORT_POINT_TOOLTIP), GUILayout.Height(ArchToolkitWindowData.BUTTON_HEIGHT)))
            {
                ArchToolkitManager.Instance.managerContainer.pathManager.AddPoint();
            }

            EditorGUI.EndDisabledGroup();
            */
            //Sequence Holder
            var seqHolder=GameObject.FindObjectOfType<ambiens.archtoolkit.atexplore.actionSystem.SequenceHolder>();
            if (seqHolder == null)
            {
                if (GUILayout.Button(
                        new GUIContent(this.actionSequenceTexture, ArchToolkitText.ADD_ACTION_SEQUENCE_MATERIAL),
                        GUILayout.Height(width),
                        GUILayout.Width(height)
                        )
                    )
                {
                    var go = new GameObject("[Sequence Holder]");
                    var sh=go.AddComponent<ambiens.archtoolkit.atexplore.actionSystem.SequenceHolder>();
                }
            }
            else{
                if (GUILayout.Button(
                    new GUIContent(this.actionSequenceTexture, ArchToolkitText.OPEN_SEQUENCE),
                    GUILayout.Height(width),
                    GUILayout.Width(height)
                    )
                    )
                {
                    if (seqHolder.Sequences==null || seqHolder.Sequences.Count == 0) 
                    {
                        ambiens.archtoolkit.atexplore.actionSystem.SequenceHolderCustomEditor.AddSequence(seqHolder);
                    }
                    AssetDatabase.OpenAsset(seqHolder.Sequences[0]);
                    //Selection.activeGameObject=seqHolder.gameObject;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public void InitTextures()
        {
            if(this.rotateTexture==null)
                this.rotateTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ambiens/ArchToolkit/ATExplore/Icons/rotate.png", typeof(Texture2D));
            if (this.traslateTexture == null)
                this.traslateTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ambiens/ArchToolkit/ATExplore/Icons/traslate.png", typeof(Texture2D));
            if (this.switchMatTexture == null)
                this.switchMatTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ambiens/ArchToolkit/ATExplore/Icons/materials.png", typeof(Texture2D));
            if (this.draggableTexture == null)
                this.draggableTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ambiens/ArchToolkit/ATExplore/Icons/drag.png", typeof(Texture2D));
            if (this.draggableTextureADD == null)
                this.draggableTextureADD = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ambiens/ArchToolkit/ATExplore/Icons/drag.png", typeof(Texture2D));

            if (this.actionSequenceTexture == null)
                this.actionSequenceTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ambiens/ArchToolkit/ATExplore/Icons/action_sequence.png", typeof(Texture2D));
        }

        public override void Init()
        {
            this.ButtonCount = 2;
            this.TabLabel = "Interactions";
        }

        private bool IsMobile()
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                return true;

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
                return true;

            return false;
        }

        public override void InstantiateInspectors()
        {
            
            if (!this.inspectors.Exists(i => i.Name == "Switch material inspector"))
                this.inspectors.Add(new ArchMaterialInspector("Switch material inspector"));

            if (!this.inspectors.Exists(i => i.Name == "Rotate Animation inspector"))
                this.inspectors.Add(new ArchDoorInspector("Rotate Animation inspector"));

            if (!this.inspectors.Exists(i => i.Name == "Translate Animation inspector"))
                this.inspectors.Add(new ArchDrawerInspector("Translate Animation inspector"));

            //if (!this.archInspectors.Exists(i => i.Name == "Scene Options"))
            //    new ArchCharacterInspector("Scene Options");

            //TODO-> Add Action Sequence inspector!

        }
    }
}
