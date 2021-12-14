using ArchToolkit.Character;
using UnityEngine;

namespace ArchToolkit.InputSystem {

    public abstract class ArchUIInteractable : MonoBehaviour
    {
        [Tooltip("This scale is used, when gaze is hover on object marked as archinteractable")]
        public Vector3 hintScale = Vector3.one;

        protected Vector3 startingScale;
        
        [SerializeField]
        protected InputRaycaster inputRaycaster;

        protected virtual void Awake()
        {
            this.InitializeUI();

            ArchToolkitManager.Instance.OnVisitorCreated += this.InitializeUI;
        }

        protected virtual void InitializeUI()
        {
            if (ArchToolkitManager.Instance.visitor != null && ArchToolkitManager.Instance.visitor.raycaster != null)
            {
                foreach (var inputRaycaster in GameObject.FindObjectsOfType<InputRaycaster>())
                {
                    inputRaycaster.OnHover += this.OnHover;

                    inputRaycaster.OnExitSensibleObject += this.OnExitSensibleObject;

                    inputRaycaster.OnClick += this.OnClick;

                    inputRaycaster.OnRaycastNull += this.OnRaycastNull;
                }

                if (this.inputRaycaster == null)
                    this.inputRaycaster = ArchToolkitManager.Instance.visitor.raycaster;

            }
        }

        public abstract void OnClick(Transform hitted);

        public abstract void OnHover(Ray ray, RaycastHit hit);

        public abstract void OnRaycastNull(Ray ray);

        public abstract void OnExitSensibleObject(RaycastHit hitted);

        protected virtual void OnDestroy()
        {
            if (!ArchToolkitManager.IsInstanced () )
                return;

            if (ArchToolkitManager.Instance.visitor != null && ArchToolkitManager.Instance.visitor.raycaster != null)
            {
                ArchToolkitManager.Instance.visitor.raycaster.OnHover -= this.OnHover;

                ArchToolkitManager.Instance.visitor.raycaster.OnExitSensibleObject -= this.OnExitSensibleObject;

                ArchToolkitManager.Instance.visitor.raycaster.OnClick -= this.OnClick;
            }

            //ArchToolkitManager.Instance.OnVisitorCreated -= this.InitializeUI;
        }
    }

}