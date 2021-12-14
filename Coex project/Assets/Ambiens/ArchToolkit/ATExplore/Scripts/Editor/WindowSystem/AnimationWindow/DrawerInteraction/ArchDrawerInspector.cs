using ArchToolkit.AnimationSystem;
using UnityEngine;
using UnityEditor;

namespace ArchToolkit.Editor.Window
{

    public class ArchDrawerInspector : ArchAnimationMovementInspector
    {
        public static ArchDrawerInspector Instance;

        public ArchDrawerInspector(string name) : base(name)
        {
            Instance = this;
        }

        public override void OnClose()
        {
            base.OnClose();
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override bool IsInspectorVisible()
        {
            if (!ArchToolkitManager.IsInstanced()) return false;

            if (MainArchWindow.Instance.CurrentWindow.GetStatus != WindowStatus.Scene)
            {
                if (this.currentGameobjectSelected != null)
                {
                    if (GetAnimationSelected<Translate>(this.currentGameobjectSelected) != null)
                    {
                        this.isInspectorVisible = true;
                        return true;
                    }
                }
            }

            this.isInspectorVisible = false;

            return false;
        }
        
        public override void OnGui(Rect pos)
        {
            base.OnGui(pos);

            if (this.inspectorFoldoutOpen)
            {

                GUILayout.Space(ArchToolkitWindowData.PADDING);

                if (this.currentAnimation == null || this.currentAnimation is Translate == false) // if there isn't an animation, add it!
                {
                    if (GUILayout.Button("Add Animation"))
                    {
                        this.AddAnimation<Translate>();
                    }
                }
                else
                {

                    if (GUILayout.Button("Up"))
                        this.SetTranslationDirection(TranslationDirection.up);

                    if (GUILayout.Button("Forward"))
                        this.SetTranslationDirection(TranslationDirection.forward);


                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("Left"))
                        this.SetTranslationDirection(TranslationDirection.left);

                    if (GUILayout.Button("Right"))
                        this.SetTranslationDirection(TranslationDirection.right);

                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Backward"))
                        this.SetTranslationDirection(TranslationDirection.backward);

                    if (GUILayout.Button("Down"))
                        this.SetTranslationDirection(TranslationDirection.down);

                    GUILayout.Space(ArchToolkitWindowData.PADDING);

                    this.DrawInspectorFields();

                    GUILayout.Space(ArchToolkitWindowData.PADDING);

                    GUILayout.BeginHorizontal();

                    // if (GUILayout.Button("Test Animation"))
                    // {
                    //     this.currentAnimation.TestInEditor = true;
                    // }
                    //
                    // if (GUILayout.Button("Stop Test Animation"))
                    // {
                    //     this.currentAnimation.TestInEditor = false;
                    // }

                    //if(GUILayout.Button("Recenter animation"))
                    //{
                    //    this.animationLogics._snapTranslationToObject(this.animationLogics.translationDirection,
                    //                                                  this.currentAnimation,
                    //                                                  this.currentAnimation.PivotDirectionGizmo.gameObject,
                    //                                                  this.currentAnimation.PivotPositionGizmo.gameObject,
                    //                                                  SceneView.lastActiveSceneView.camera.transform);
                    //}


                    if (GUILayout.Button(new GUIContent(ArchToolkitText.REMOVE_ANIMATION, ArchToolkitText.REMOVE_INTERACTION_TOOLTIP)))
                    {
                        this.DeleteAnimation();
                    }



                    GUILayout.EndHorizontal();
                }
            }
        }

        public override void OnSelectionChange(GameObject gameObject)
        {
            if (gameObject == null)
            {
                // if null, return!
                this.currentGameobjectSelected = null;
                this.currentAnimation = null;
                this.currentReference = null;
                this.isInspectorVisible = false;
                return;
            }
            
            this.currentReference = null;
            this.currentAnimation = null;

            this.currentGameobjectSelected = gameObject;

            this.currentAnimation = this.GetAnimationSelected<Translate>(this.currentGameobjectSelected);
            
        }

        
        public override void OnUpdate()
        {
            base.OnUpdate();

            if(this.currentAnimation != null)
            {
               //this.currentAnimation.animationAmount = Vector3.Distance();
            }
        }

    }
}