using UnityEngine;
using ArchToolkit.AnimationSystem;

namespace ArchToolkit.Editor.Window
{

    [System.Serializable]
    public class ArchDoorInspector : ArchAnimationMovementInspector
    {
        public static ArchDoorInspector Instance;

        public ArchDoorInspector(string name) : base(name)
        {
            Instance = this;
        }

        public override void OnClose()
        {

        }

        public override void OnEnable()
        {

        }

        public override bool IsInspectorVisible()
        {
            if (!ArchToolkitManager.IsInstanced()) return false;

            if (MainArchWindow.Instance.CurrentWindow.GetStatus != WindowStatus.Scene)
            {
                if (this.currentGameobjectSelected != null)
                {
                    if (GetAnimationSelected<RotateAround>(this.currentGameobjectSelected) != null)
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

                if (this.currentAnimation == null || this.currentAnimation is RotateAround == false) // if there isn't an animation, add it!
                {
                    if (GUILayout.Button(new GUIContent( ArchToolkitText.ADD_ROTATION,ArchToolkitText.ROTATION_TOOLTIP)))
                    {
                        this.AddAnimation<RotateAround>();
                    }
                }
                else
                {

                    if(GUILayout.Button("Up"))
                    {
                        this.SetRotationDirection(RotationDirection.up);
                    }

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("Left"))
                        this.SetRotationDirection(RotationDirection.left);

                    if (GUILayout.Button("Right"))
                        this.SetRotationDirection(RotationDirection.right);

                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Down"))
                        this.SetRotationDirection(RotationDirection.down);

                    GUILayout.Space(ArchToolkitWindowData.PADDING);

                    this.DrawInspectorFields();

                    GUILayout.Space(ArchToolkitWindowData.PADDING);

                    GUILayout.BeginHorizontal();

                    //  if(GUILayout.Button("Test Animation"))
                    //  {
                    //      this.currentAnimation.TestInEditor = true;
                    //  }
                    //
                    //  if (GUILayout.Button("Stop Test Animation"))
                    //  {
                    //      this.currentAnimation.TestInEditor = false;
                    //  }

                    //if (GUILayout.Button("Recenter Animation"))
                    //    this.animationLogics._snapRotationToObject(this.animationLogics.rotationDirection,
                    //                                               this.currentAnimation,
                    //                                               this.currentAnimation.PivotDirectionGizmo.gameObject,
                    //                                               this.currentAnimation.PivotPositionGizmo.gameObject);

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

            this.currentAnimation = this.GetAnimationSelected<RotateAround>(this.currentGameobjectSelected);

        }

        public override void OnUpdate()
        {

        }
    }
}
