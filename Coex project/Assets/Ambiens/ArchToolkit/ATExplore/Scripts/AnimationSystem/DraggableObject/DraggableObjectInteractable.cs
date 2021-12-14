using ArchToolkit.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ArchToolkit.AnimationSystem
{
    [System.Serializable]
    public class DraggableObjectInteractable : AInteractable
    {

        public bool ControlsOn=false;
        public override bool AnimationOn()
        {
            return ControlsOn;
        }

        public override void StartAnimation()
        {
            if (!ControlsOn)
            {
                ControlsOn = true;
                this.GetComponent<BoxCollider>().enabled = false;
                ArchToolkitManager.Instance.visitor.SetRaycastTool(new MoveObjectRaycastTool(this.transform));
                ArchToolkitManager.Instance.visitor.OnChangeRaycastTool -= this.onChangeRayCastTool;
                ArchToolkitManager.Instance.visitor.OnChangeRaycastTool += this.onChangeRayCastTool;
                this.ApplyShader();
            }
            else
            {
                ControlsOn = false;
                ArchToolkitManager.Instance.visitor.OnChangeRaycastTool -= this.onChangeRayCastTool;
                ArchToolkitManager.Instance.visitor.SetRaycastTool(null);
                this.GetComponent<BoxCollider>().enabled = true;
                this.RemoveShader();
            }

        }

        private void RemoveShader()
        {
            var mat = Resources.Load<Material>("Outline");

            foreach (var rend in this.GetComponentsInChildren<MeshRenderer>())
            {
                var mats = new List<Material>(rend.sharedMaterials);
                Material toremove = null;
                foreach (var m in mats)
                {
                    if (m.shader == mat.shader)
                    {
                        toremove = m;
                    }
                }
                if (toremove!=null)
                {
                    mats.Remove(toremove);
                    rend.sharedMaterials = mats.ToArray();

                }
            }
        }

        public void ApplyShader()
        {

            var mat = Resources.Load<Material>("Outline");

            foreach(var rend in this.GetComponentsInChildren<MeshRenderer>())
            {
                var mats = new List<Material>(rend.sharedMaterials);
                var found = false;
                foreach(var m in mats)
                {
                    if (m.shader==mat.shader) found = true;
                }
                if (!found)
                {
                    var toAdd = new Material(mat);
                    if (mats.Count > 0)
                    {
                        toAdd.mainTexture = mats[0].mainTexture;
                        toAdd.SetTextureOffset("_MainTex", mats[0].mainTextureOffset);
                        toAdd.SetTextureScale("_MainTex", mats[0].mainTextureScale);

                        toAdd.SetColor("_Color", mats[0].color);
                    }
                    
                   
                    mats.Add(toAdd);
                    rend.sharedMaterials = mats.ToArray();

                }
            }

        }

        private void onChangeRayCastTool(ARaycastTool tool)
        {
            
            if (tool == null)
            {
                ArchToolkitManager.Instance.visitor.OnChangeRaycastTool -= this.onChangeRayCastTool;
                this.RemoveShader();
                StartCoroutine(enableCollider());
            }
            
        }
        IEnumerator enableCollider()
        {
            yield return new WaitForEndOfFrame();
            this.transform.GetComponent<BoxCollider>().enabled = true;
            ControlsOn = false;
            this.RemoveShader();
        }
        public override ArchBasicHandle SetHandle(Vector3 position)
        {
            this.basicHandle = this.TargetList[0].AddComponent<ArchBasicHandle>();

            this.basicHandle.animationToOpen = this;

            this.TargetList[0].tag = ArchToolkitDataPaths.ARCHINTERACTABLETAG;

            return this.basicHandle;
        }

        protected override void EditorUpdate()
        {
            
        }
    }
}