using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.actionSystem;
using ArchToolkit.AnimationSystem;
using ArchToolkit.Editor.Window;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;

[CustomNodeEditor(typeof(RotateAroundPivot))]
public class RotateAroundPivotCustomEditor : ATransformCustomEditorBase
{

    public override void OnBodyGUI()
    {
        base.OnBodyGUI();
        GUILayout.Space(ArchToolkit.ArchToolkitWindowData.PADDING);

        serializedObject.Update();

        var rotate = this.InitSceneReferences<RotateAroundPivot>();
        if (rotate == null) return;

        this.SetPivots(this.references);
        gDirection.animationAmount = rotate.animationAmount;

        GUILayout.Space(ArchToolkit.ArchToolkitWindowData.PADDING);

        EditorGUILayout.LabelField("Rotation Pivot", EditorStyles.boldLabel);

        if (GUILayout.Button("Up"))
            AnimationLogics.SnapRotationToObject(RotationDirection.up,
                                                 references.gameObjects,
                                                 pivotDir,
                                                 pivotPos);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Left"))
            AnimationLogics.SnapRotationToObject(RotationDirection.left,
                                                 references.gameObjects,
                                                 pivotDir,
                                                 pivotPos);

        if (GUILayout.Button("Right"))
            AnimationLogics.SnapRotationToObject(RotationDirection.right,
                                                 references.gameObjects,
                                                 pivotDir,
                                                 pivotPos);

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Down"))
            AnimationLogics.SnapRotationToObject(RotationDirection.down,
                                                 references.gameObjects,
                                                 pivotDir,
                                                 pivotPos);

        GUILayout.Space(ArchToolkit.ArchToolkitWindowData.PADDING);
    }


    

}