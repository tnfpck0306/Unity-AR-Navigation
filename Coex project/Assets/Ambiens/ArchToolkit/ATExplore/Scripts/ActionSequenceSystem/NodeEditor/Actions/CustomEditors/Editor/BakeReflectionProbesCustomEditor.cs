using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;
namespace ambiens.archtoolkit.atexplore.actionSystem{
    [CustomNodeEditor(typeof(BakeReflectionProbes))]
    public class BakeReflectionProbesCustomEditor : CustomNodeEditorBase
    {

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            serializedObject.Update();

            var sm = this.InitSceneReferences<BakeReflectionProbes>();
            if (sm == null) return;

        }


    }
}
