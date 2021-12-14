using ArchToolkit.Character;

namespace ArchToolkit.AnimationSystem
{
    public class ArchTeleportAnimation : AInteractable
    {
        private ArchVRCharacter visitor;


        public override bool AnimationOn()
        {
            return false;
        }

        public override void StartAnimation()
        {
            if (this.visitor == null)
            {
                this.visitor = ArchToolkitManager.Instance.visitor as ArchVRCharacter;
            }

            if (visitor != null)
                visitor.TeleportWithFade();
        }

        protected override void EditorUpdate()
        {
            
        }
    }
}