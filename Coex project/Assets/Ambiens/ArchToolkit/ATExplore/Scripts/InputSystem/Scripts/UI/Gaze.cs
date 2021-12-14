using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.InputSystem
{

    public class Gaze : ArchUIInteractable
    {
        private RawImage ___image;
        private RawImage image
        {
            get
            {
                if (___image == null) ___image = this.GetComponent<RawImage>();
                return ___image;
            }
        }

        [SerializeField]
        private Image radialImage;

        private Color startColor;

        public Color animationColor;

        [Range(0.1f,10)]
        public float animationSpeed = 0.1f;

        protected override void Awake()
        {
            
            base.Awake();

        }

        protected override void InitializeUI()
        {
            base.InitializeUI();
            
            //this.image = this.gameObject.GetComponent<RawImage>();

            this.startColor = this.image.color;

            this.startingScale = this.gameObject.transform.localScale;

            if (this.UseTimer() && this.radialImage != null)
                this.radialImage.fillAmount = 0;
        }

        public override void OnHover(Ray ray, RaycastHit hit)
        {
            transform.localScale = Vector3.Lerp(transform.localScale,
                                                 this.hintScale,
                                                 this.animationSpeed);

            if (this.radialImage != null && this.UseTimer())
            {
                this.radialImage.fillAmount = this.inputRaycaster.TimeWatched / this.inputRaycaster.maxTimer;

                if (this.radialImage.fillAmount >= 1)
                    this.radialImage.fillAmount = 0;
            }

            this.image.color = this.animationColor;
        }

        private bool UseTimer()
        {
            if (this.inputRaycaster != null)
                return this.inputRaycaster.UseTimer;

            return false;
        }

        public override void OnExitSensibleObject(RaycastHit hitted)
        {
            this.ResetGaze();
        }

        private void ResetGaze()
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale,
                                               this.startingScale,
                                               this.animationSpeed);

            if (this.radialImage != null)
            {
                this.radialImage.fillAmount = 0;
            }

            this.image.color = this.startColor;
        }

        public override void OnClick(Transform hitted)
        {
           
        }

        public override void OnRaycastNull(Ray ray)
        {
            this.ResetGaze();
        }

        /*public override void OnCheckVRInteract(bool canInteract)
        {
            if (canInteract)
                this.image.color = animationColor;
            else
                this.image.color = Color.red;
        }*/
    }
}
