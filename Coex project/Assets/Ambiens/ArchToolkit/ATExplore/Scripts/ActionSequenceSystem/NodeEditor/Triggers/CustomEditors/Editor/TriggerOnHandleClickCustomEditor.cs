using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ambiens.archtoolkit.atexplore.XNodeEditor;

namespace ambiens.archtoolkit.atexplore.actionSystem{
    [CustomNodeEditor(typeof(TriggerOnHandleClick))]
    public class TriggerOnHandleClickCustomEditor : NodeEditor
    {
        private int handleIndex;
        private List<Sprite> spriteList = null;
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            serializedObject.Update();

            var trigger = target as TriggerOnHandleClick;

            var holder = GameObject.FindObjectOfType<SequenceHolder>();
            if (holder == null) return;

            var references = holder.RequestGameObjectReferences(trigger.ID);

            EditorGUILayout.LabelField("Handles", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if(spriteList==null)spriteList= GetAssetList<Sprite>("Ambiens/ArchToolkit/ATExplore/Resources/Handles/HandleTypes");

            var handlesNames = new List<string>();
            foreach (var h in spriteList) handlesNames.Add(h.name);
            
            this.handleIndex=EditorGUILayout.Popup("Handle", this.handleIndex, handlesNames.ToArray());

            if (GUILayout.Button(new GUIContent("+", "Add an handle to the scene")))
            {
                var go=GameObject.Instantiate(Resources.Load<GameObject>("Handles/GenericHandle"));
                go.transform.parent = references.transform;
                go.transform.position= ArchToolkit.Utils.ArchToolkitProgrammingUtils.FrontOfEditorCamera();
                go.GetComponent<SpriteRenderer>().sprite = spriteList[handleIndex];

                references.gameObjects.Add(go);
                EditorSceneManager.MarkAllScenesDirty();
            }
            EditorGUILayout.EndHorizontal();
            var toRemove = new List<GameObject>();

            foreach(var go in references.gameObjects){
                if (go == null)
                {
                    toRemove.Add(go);
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    //Vertical
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField(go.name, EditorStyles.label);
                    if (GUILayout.Button(new GUIContent("Select", "Select handle")))
                    {
                        Selection.activeGameObject = go;
                        SceneView.FrameLastActiveSceneView();
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
                
            }

            foreach (var go in toRemove) {
                references.gameObjects.Remove(go);
                GameObject.DestroyImmediate(go);
                EditorSceneManager.MarkAllScenesDirty();
            }
                

        }

        public static List<T> GetAssetList<T>(string path) where T : class
        {
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

            return fileEntries.Select(fileName =>
            {
                string assetPath = fileName.Substring(fileName.IndexOf("Assets"));
                assetPath = Path.ChangeExtension(assetPath, null);
                return UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
            })
                .OfType<T>()
                .ToList();
        }
    }
}

