
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;
namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CustomNodeEditor(typeof(ImageSetFillAmount))]
    public class ImageSetFillAmountEditor : CustomNodeEditorBase
    {

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            serializedObject.Update();

            var sm = this.InitSceneReferences<ImageSetFillAmount>();
            if (sm == null) return;

        }

    }
}
