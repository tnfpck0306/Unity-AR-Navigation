using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Switch Material")]
    public class SwitchSingleMaterial : AAction
    {

        [Input]
        public Material material;

        public bool ChangeAllMaterials = true;
        public int ChangeMaterialIndex = 0;

        protected override void _RuntimeInit()
        {
            if (!this.InitSceneReferences())
            {
                return;
            }

        }

        protected override bool _StartAction()
        {
            
            var mat = GetInputValue<Material>("material");
            if (mat == null) mat = material;

            List<Material> auxMaterials = new List<Material>();

            foreach (var go in this.SceneReferences)
            {
                if(go!=null){
                    var rend = go.GetComponent<MeshRenderer>();
                    if (rend != null)
                    {
                        foreach (var m in rend.materials)
                            auxMaterials.Add(m);

                        if (ChangeAllMaterials)
                        {
                            for (int i = 0; i < auxMaterials.Count; i++) auxMaterials[i] = mat;
                        }
                        else
                        {
                            int realIndex = Mathf.Clamp(this.ChangeMaterialIndex, 0, auxMaterials.Count - 1);
                            auxMaterials[realIndex] = mat;
                        }

                        rend.sharedMaterials = auxMaterials.ToArray();
                    }
                    else
                    {
                        Debug.LogWarning("SWITCH MATERIAL Warning: " + go.name + " does not contain a mesh renderer! ");
                    }
                    auxMaterials.Clear();
                }

            }

            return true;
        }


    }
}
