using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ambiens.archtoolkit.atexplore.XNodeEditor;

namespace ambiens.archtoolkit.atexplore.actionSystem{
    [CustomNodeEditor(typeof(TriggerOnObjectPointerEnter))]
    public class TriggerOnObjectPointerEnterCustomEditor : NodeEditor
    {

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            serializedObject.Update();

            var trigger = target as TriggerOnObjectPointerEnter;    

            var holder = GameObject.FindObjectOfType<SequenceHolder>();
            if (holder == null) return;

            var references = holder.RequestGameObjectReferences(trigger.ID);

            EditorGUILayout.LabelField("Target Objects", EditorStyles.boldLabel);

            if(Selection.gameObjects!=null && Selection.gameObjects.Length>0){
                var buttonString = (Selection.gameObjects.Length > 1 ? "[ADD] Multiple objects" : "[ADD] " + Selection.gameObjects[0].name);
                if (GUILayout.Button(new GUIContent(buttonString, "Add selected "+(Selection.gameObjects.Length > 1?"objects":"object"))))
                {
                    references.gameObjects.AddRange(Selection.gameObjects);
                    EditorSceneManager.MarkAllScenesDirty();
                }
            }
            else{
                EditorGUILayout.LabelField("Select targets from scene", EditorStyles.label);
            }

            var toRemove = new List<GameObject>();

            foreach(var go in references.gameObjects){
                EditorGUILayout.BeginHorizontal();
                //Vertical
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField(go.name, EditorStyles.label);
                if (GUILayout.Button(new GUIContent("Select", "Select target")))
                {
                    Selection.activeGameObject = go;
                }
                EditorGUILayout.EndVertical();
                //End Vertical
                if (GUILayout.Button(new GUIContent("X", "Removes target"), GUILayout.Width(30)))
                {
                    toRemove.Add(go);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            foreach (var go in toRemove) {
                references.gameObjects.Remove(go);
                EditorSceneManager.MarkAllScenesDirty();
            }
                

        }
    }
}

