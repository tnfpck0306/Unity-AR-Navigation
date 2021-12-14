using ArchToolkit.AnimationSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.InputSystem
{
    public class ArchUIButton : ArchUIInteractable
    {
        public Action<Variant> clickCallback;

        public RawImage Thumbnail;

        public Color SelectedColor;

        private Color startingColor;

        public bool isSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {

                this._isSelected = value;

                if (this._isSelected) this.Butt_Selected();
                else if (!this._isSelected) this.Butt_Unselected();

            }
        }
        
        public Variant selItem;

        private bool _isSelected;

        protected override void Awake()
        {
            base.Awake();

            this.startingScale = this.transform.localScale;
        }

        public void InitButton(Variant Object, Action<Variant> click)
        {
            var spr = this.transform.GetChild(0).GetComponent<RawImage>();

            if (this.Thumbnail == null)
                this.Thumbnail = spr;

            this.selItem = Object;

            this.clickCallback = click;

            if (Object.preview != null)
            {
                this.Thumbnail.texture = Object.preview;
            }
            if(this.Thumbnail != null)
            {
                this.startingColor = this.Thumbnail.color;
            }
        }

        public override void OnClick(Transform hitted)
        {
            if (hitted.gameObject != this.transform.gameObject)
                return;

            if (this.clickCallback != null)
            {
                this.clickCallback(this.selItem);

                this.isSelected = true;
            }
        }
        
        protected virtual void Butt_Unselected()
        {
            if (this.Thumbnail != null)
            {
                this.Thumbnail.color = this.startingColor;
            }
        }
        
        protected virtual void Butt_Selected()
        {
            if (this.Thumbnail != null)
            {
                this.Thumbnail.color = this.SelectedColor;
            }
        }

        public override void OnHover(Ray ray, RaycastHit hit)
        {
            if (hit.transform.gameObject != this.transform.gameObject)
                return;

            transform.localScale = this.startingScale + new Vector3(0.15f, 0.15f, 0.15f);
        }

        public override void OnExitSensibleObject(RaycastHit hitted)
        {
            if (hitted.transform.gameObject != this.transform.gameObject)
                return;

            //ImageSelectedDialog.Instance.description.text = hitted.gameObject.name;
           
            this.transform.localScale = this.startingScale;
            
        }

        public override void OnRaycastNull(Ray ray)
        {

        }

        
    }
}