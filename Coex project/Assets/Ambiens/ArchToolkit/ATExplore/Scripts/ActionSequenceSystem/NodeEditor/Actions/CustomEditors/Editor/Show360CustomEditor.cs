using UnityEditor;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;
namespace ambiens.archtoolkit.atexplore.actionSystem
{

    [CustomNodeEditor(typeof(Show360Photo))]
    public class Show360CustomEditor : NodeEditor
    {
        public int width = 600;

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            serializedObject.Update();

            var t360 = target as Show360Photo;

            var holder = GameObject.FindObjectOfType<SequenceHolder>();
            if (holder == null) return;

            var handle = holder.RequestHandle(t360.ID, SequenceHolder.HandleType.SmallCilinder);

            t360.tpTargetPosition = handle.transform.position;

            if (GUILayout.Button("Select Position Handle"))
            {
                Selection.activeGameObject = handle;
                SceneView.FrameLastActiveSceneView();
            }

        }


    }
}