using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;

namespace  ambiens.archtoolkit.atexplore.actionSystem{
    
    [CustomNodeEditor(typeof(SwitchSingleMaterial))]
    public class SwitchSingleMaterialCustomEditor : CustomNodeEditorBase
    {

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            serializedObject.Update();
            var sm = this.InitSceneReferences<SwitchSingleMaterial>();
            if (sm == null) return;

        }


    }
}