
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;
namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CustomNodeEditor(typeof(PlayPauseVideoPlayer))]
    public class PlayPauseVideoPlayerCustomEditor : CustomNodeEditorBase
    {

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            serializedObject.Update();

            var sm = this.InitSceneReferences<PlayPauseVideoPlayer>();
            if (sm == null) return;

        }


    }
}
