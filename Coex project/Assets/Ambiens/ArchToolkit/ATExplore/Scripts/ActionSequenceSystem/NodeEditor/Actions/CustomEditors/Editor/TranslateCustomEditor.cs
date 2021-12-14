using System.Collections.Generic;

using ArchToolkit.AnimationSystem;
using ArchToolkit.Editor.Window;
using UnityEditor;

using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;

namespace ambiens.archtoolkit.atexplore.actionSystem{
    
    [CustomNodeEditor(typeof(TranslateObject))]
    public class TranslateCustomEditor : ATransformCustomEditorBase
    {

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            var translate = this.InitSceneReferences<TranslateObject>();
            if (translate == null) return;

            this.SetPivots(this.references);
            translate.animationAmount = gPivot.amount;

            GUILayout.Space(ArchToolkit.ArchToolkitWindowData.PADDING);

            EditorGUILayout.LabelField("Rotation Pivot", EditorStyles.boldLabel);

            if (GUILayout.Button("Up"))
                AnimationLogics.SnapTranslationToObject(TranslationDirection.up,
                                                        references.gameObjects,
                                                        pivotDir,
                                                        pivotPos,
                                                        SceneView.lastActiveSceneView.camera.transform);
            if (GUILayout.Button("Forward"))
                AnimationLogics.SnapTranslationToObject(TranslationDirection.forward,
                                                        references.gameObjects,
                                                        pivotDir,
                                                        pivotPos,
                                                        SceneView.lastActiveSceneView.camera.transform);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Left"))
                AnimationLogics.SnapTranslationToObject(TranslationDirection.left,
                                                        references.gameObjects,
                                                        pivotDir,
                                                        pivotPos,
                                                        SceneView.lastActiveSceneView.camera.transform);
            if (GUILayout.Button("Right"))
                AnimationLogics.SnapTranslationToObject(TranslationDirection.right,
                                                        references.gameObjects,
                                                        pivotDir,
                                                        pivotPos,
                                                        SceneView.lastActiveSceneView.camera.transform);

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Backward"))
                AnimationLogics.SnapTranslationToObject(TranslationDirection.backward,
                                                        references.gameObjects,
                                                        pivotDir,
                                                        pivotPos,
                                                        SceneView.lastActiveSceneView.camera.transform);
            if (GUILayout.Button("Down"))
                AnimationLogics.SnapTranslationToObject(TranslationDirection.down,
                                                        references.gameObjects,
                                                        pivotDir,
                                                        pivotPos,
                                                        SceneView.lastActiveSceneView.camera.transform);

            GUILayout.Space(ArchToolkit.ArchToolkitWindowData.PADDING);
        }

       


    }
}