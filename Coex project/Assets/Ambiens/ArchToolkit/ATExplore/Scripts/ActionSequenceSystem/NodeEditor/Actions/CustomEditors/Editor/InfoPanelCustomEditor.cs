using UnityEditor;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;
using System.Collections.Generic;

namespace ambiens.archtoolkit.atexplore.actionSystem
{

    [CustomNodeEditor(typeof(ShowInfoPanel))]
    public class InfoPanelCustomEditorEditor : NodeEditor
    {
        public override int GetWidth()
        {
            return 400;
        }
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            serializedObject.Update();

            var i = (ShowInfoPanel)this.target;

            i.data = this.ShowDictionaryStringStringEditor("Title", "Description", i.data);

            serializedObject.ApplyModifiedProperties();

        }
        
        public List<ArchToolkit.InputSystem.InfoDialog.KeyVal> ShowDictionaryStringStringEditor(
            string keyLabel, 
            string valueLabel, 
            List<ArchToolkit.InputSystem.InfoDialog.KeyVal> dict)
        {
            List<ArchToolkit.InputSystem.InfoDialog.KeyVal> todelete = new List<ArchToolkit.InputSystem.InfoDialog.KeyVal>();

            
            if (GUILayout.Button("ADD", GUILayout.Width(60)))
            {
                dict.Add(new ArchToolkit.InputSystem.InfoDialog.KeyVal("New Key", "New Value"));
            }
            

            for (int i=0; i<dict.Count; i++)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.BeginVertical();
                
                dict[i].Key = EditorGUILayout.TextField(keyLabel, dict[i].Key);
                dict[i].Val = EditorGUILayout.TextField(valueLabel, dict[i].Val);
                
                EditorGUILayout.EndVertical();

                if (GUILayout.Button(new GUIContent("X", "Remove element"), GUILayout.MaxHeight(20), GUILayout.MaxWidth(20), GUILayout.MinHeight(20), GUILayout.MaxWidth(20), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    todelete.Add(dict[i]);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            foreach (var k in todelete) dict.Remove(k);

            return dict;
        }
    }
}