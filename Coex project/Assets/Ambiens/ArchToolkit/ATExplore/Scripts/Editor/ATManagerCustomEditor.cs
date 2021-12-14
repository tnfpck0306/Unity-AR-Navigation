using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace ArchToolkit
{
    [CustomEditor(typeof(ArchToolkitManager))]
    public class ATManagerCustomEditor : UnityEditor.Editor
    {
        BoxBoundsHandle handle=new BoxBoundsHandle();
        public override void OnInspectorGUI() 
        {
            base.DrawDefaultInspector();

            if (GUILayout.Button(new GUIContent("Refresh scene Bounds", "Refresh bounds size")))
            {

                var b=ArchToolkitManager.CalculateSceneBounds();

                var m = (ArchToolkitManager)this.target;
                m.sceneBounds = b;
            }
        }

        private void OnSceneGUI()
        {
            ArchToolkitManager man = (ArchToolkitManager)target;

            handle.center = man.sceneBounds.center;
            handle.size = man.sceneBounds.size;

            // draw the handle
            EditorGUI.BeginChangeCheck();
            handle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                // record the target object before setting new values so changes can be undone/redone
                Undo.RecordObject(man, "Change Bounds");

                // copy the handle's updated data back to the target object
                Bounds newBounds = new Bounds();
                newBounds.center = handle.center;
                newBounds.size = handle.size;
                man.sceneBounds = newBounds;
            }

        }

        
    }
}

