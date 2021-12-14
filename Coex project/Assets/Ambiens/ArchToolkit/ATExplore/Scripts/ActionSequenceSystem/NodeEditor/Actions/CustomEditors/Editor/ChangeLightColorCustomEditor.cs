
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;
namespace ambiens.archtoolkit.atexplore.actionSystem{
    [CustomNodeEditor(typeof(ChangeLightColor))]
    public class ChangeLightColorCustomEditor : CustomNodeEditorBase
    {

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            serializedObject.Update();

            var sm = this.InitSceneReferences<ChangeLightColor>();
            if (sm == null) return;

        }


    }
}
