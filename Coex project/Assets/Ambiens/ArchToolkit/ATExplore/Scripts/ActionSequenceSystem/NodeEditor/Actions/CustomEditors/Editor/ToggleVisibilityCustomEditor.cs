
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;
namespace ambiens.archtoolkit.atexplore.actionSystem{
    [CustomNodeEditor(typeof(ToggleVisibility))]
    public class ToggleVisibilityCustomEditor : CustomNodeEditorBase
    {

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            serializedObject.Update();

            var sm = this.InitSceneReferences<ToggleVisibility>();
            if (sm == null) return;

        }


    }
}
