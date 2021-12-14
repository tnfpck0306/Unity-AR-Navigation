using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArchToolkit.Editor;
using ArchToolkit.Editor.Utils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CustomEditor(typeof(SequenceHolder))]
    public class SequenceHolderCustomEditor : Editor 
    {

        public static void AddSequence(SequenceHolder seq)
        {
            if (seq.Sequences == null) seq.Sequences = new List<ActionSequence>();
            seq.Sequences.Add(EditorUtils.CreateAsset<ActionSequence>("Action Sequence"));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.LabelField("Action Sequences", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            var seqHolder = target as SequenceHolder;
            if (seqHolder.Sequences == null) seqHolder.Sequences = new List<ActionSequence>();
            if (GUILayout.Button(new GUIContent("+", "Add an handle to the scene")))
            {
                //seqHolder.Sequences.Add(EditorUtils.CreateAsset<ActionSequence>("Action Sequence"));
                AddSequence(seqHolder);
            }
            EditorGUILayout.EndHorizontal();
            var toRemove = new List<ActionSequence>();

            foreach(var seq in seqHolder.Sequences){
                if (seq == null) toRemove.Add(seq);
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    //Vertical
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField(seq.name, EditorStyles.label);
                    if (GUILayout.Button(new GUIContent("Open", "Select Sequence")))
                    {
                        //Selection.activeObject = seq;
                        AssetDatabase.OpenAsset(seq);
                    }
                    EditorGUILayout.EndVertical();
                    //End Vertical
                    if (GUILayout.Button(new GUIContent("X", "Removes Sequence"), GUILayout.Width(30)))
                    {
                        toRemove.Add(seq);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
                
            }

            foreach (var torem in toRemove) {
                seqHolder.Sequences.Remove(torem);
            }
                

            serializedObject.ApplyModifiedProperties();
        }

        
    }

    
}