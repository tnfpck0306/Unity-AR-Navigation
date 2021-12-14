using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.InputSystem
{

    public class ArchUIDefaultButton : ArchUIInteractable
    {
        public Button button;
        public bool AutoGenerateCollider=true;
        void OnEnable() {
            StartCoroutine(setCollSize());
        }
        IEnumerator setCollSize(){
            yield return new WaitForEndOfFrame();
            var coll=this.GetComponent<BoxCollider>();
            var rectTransform=this.GetComponent<RectTransform>();
            if(AutoGenerateCollider && coll==null && rectTransform!=null){
                if(coll==null)
                    coll=this.gameObject.AddComponent<BoxCollider>();
                coll.size=new Vector3(Mathf.Abs( rectTransform.rect.x*2), Mathf.Abs( rectTransform.rect.y*2),1);               
            }
        }
        private void Reset()
        {
            this.button = this.GetComponent<Button>();
            
        }
        
        public override void OnClick(Transform hitted)
        {

            if (hitted.gameObject != this.transform.gameObject)
                return;

            if (this.button == null)
            {
                this.button = this.GetComponent<Button>();
            }

            if (this.button != null)
                this.button.onClick.Invoke();
        }

        public override void OnExitSensibleObject(RaycastHit hitted)
        {
            
        }

        public override void OnHover(Ray ray, RaycastHit hit)
        {
            
        }

        public override void OnRaycastNull(Ray ray)
        {

        }
    }
}
