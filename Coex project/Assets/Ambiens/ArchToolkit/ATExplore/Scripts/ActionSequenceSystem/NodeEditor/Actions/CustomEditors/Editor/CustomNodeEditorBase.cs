
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;
namespace ambiens.archtoolkit.atexplore.actionSystem{
    public class CustomNodeEditorBase : NodeEditor
    {

        protected SequenceHolder sceneHolder;
        protected GameObjectReferenceHolder references;

        public T InitSceneReferences<T>() where T:AActionNodeBase
        {
            GUILayout.Space(ArchToolkit.ArchToolkitWindowData.PADDING);

            serializedObject.Update();

            var targetNode = target as T;

            this.sceneHolder = GameObject.FindObjectOfType<SequenceHolder>();
            if (this.sceneHolder == null) return null;

            this.references = this.sceneHolder.RequestGameObjectReferences(targetNode.ID);

            EditorGUILayout.LabelField("Target Objects", EditorStyles.boldLabel);

            if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
            {
                var buttonString = (Selection.gameObjects.Length > 1 ? "[ADD] Multiple objects" : "[ADD] " + Selection.gameObjects[0].name);
                if (GUILayout.Button(new GUIContent(buttonString, "Add selected " + (Selection.gameObjects.Length > 1 ? "objects" : "object"))))
                {
                    references.gameObjects.AddRange(Selection.gameObjects);
                    EditorSceneManager.MarkAllScenesDirty();
                }
            }
            else
            {
                EditorGUILayout.LabelField("Select targets from scene", EditorStyles.label);
            }

            var toRemove = new List<GameObject>();

            foreach (var go in references.gameObjects)
            {
                EditorGUILayout.BeginHorizontal();
                //Vertical
                EditorGUILayout.BeginVertical();
                if (go == null)
                {
                    toRemove.Add(go);
                }
                else
                {
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
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            foreach (var go in toRemove)
            {
                references.gameObjects.Remove(go);
                EditorSceneManager.MarkAllScenesDirty();
            }

            return targetNode;
        }

    }

}
