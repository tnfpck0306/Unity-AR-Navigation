
using ambiens.archtoolkit.atexplore.actionSystem;
using UnityEditor;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;

[CustomNodeEditor(typeof(TelePort))]
public class TelePortCustomEditor : NodeEditor
{
    
    public override void OnBodyGUI()
    {
        base.OnBodyGUI();

        serializedObject.Update();

        var tp = target as TelePort;

        var holder = GameObject.FindObjectOfType<SequenceHolder>();
        if (holder == null) return;

        var handle=holder.RequestHandle(tp.ID, SequenceHolder.HandleType.SmallCube);

        tp.tpTargetPosition = handle.transform.position;

        if (GUILayout.Button("Select Handle"))
        {
            Selection.activeGameObject = handle;
        }

    }


}