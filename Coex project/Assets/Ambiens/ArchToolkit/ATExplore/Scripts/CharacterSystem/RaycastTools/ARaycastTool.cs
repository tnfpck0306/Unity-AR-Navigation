using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit.Character
{
    public abstract class ARaycastTool
    {
        protected InputRaycaster raycaster;
        protected ArchCharacter character;
        public bool initialized = false;
        public ARaycastTool()
        {
            this.Init();
        }
        protected virtual void initialize()
        {
            if (!initialized)
            {
                initialized = true;

                this.raycaster = GameObject.FindObjectOfType<InputRaycaster>();
                this.character = ArchToolkitManager.Instance.visitor;

                raycaster.OnHover += this.OnHover;
                raycaster.OnRayCastHit += this.OnRaycastHit;
                raycaster.OnExitSensibleObject += this.OnExitSensibleObject;
                raycaster.OnClick += this.OnClick;
                raycaster.OnRaycastNull += this.OnRaycastNull;
                raycaster.OnHoverFloor += this.OnHoverFloor;
                raycaster.OnTimerClickOnFloor += this.OnTimerClickOnFloor;
            }
           

        }
        public virtual void Disable()
        {
            initialized = false;
            raycaster.OnHover -= this.OnHover;
            raycaster.OnRayCastHit -= this.OnRaycastHit;
            raycaster.OnExitSensibleObject -= this.OnExitSensibleObject;
            raycaster.OnClick -= this.OnClick;
            raycaster.OnRaycastNull -= this.OnRaycastNull;
            raycaster.OnHoverFloor -= this.OnHoverFloor;
            raycaster.OnTimerClickOnFloor -= this.OnTimerClickOnFloor;
        }
        protected abstract void OnTimerClickOnFloor();

        public void Init()
        {
            this.initialize();
        }
        public abstract bool Update(float time);

        protected abstract void OnRaycastHit(Ray ray, RaycastHit hit);
        protected abstract void OnRaycastNull(Ray ray);
        protected abstract void OnClick(Transform obj);
        protected abstract void OnExitSensibleObject(RaycastHit obj);
        protected abstract void OnHoverFloor(RaycastHit hit);
        protected abstract void OnHover(Ray ray, RaycastHit hit);
    }
}

