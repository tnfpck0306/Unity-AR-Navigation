using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit.AnimationSystem;
using ArchToolkit.Editor.Window;
namespace ambiens.archtoolkit.atexplore.actionSystem
{

    public class ATransformCustomEditorBase : CustomNodeEditorBase
    {
        protected GizmoRotateAroundDirection gDirection;
        protected GizmoRotateAroundPivot gPivot;

        protected GameObject pivotDir;
        protected GameObject pivotPos;

        protected void SetPivots(GameObjectReferenceHolder references)
        {
            this.pivotDir = references.GetGameObject("pivotDir");
            this.pivotPos = references.GetGameObject("pivot");

            this.gDirection = this.pivotDir.GetComponent<GizmoRotateAroundDirection>();
            if (gDirection == null)
            {
                gDirection = this.pivotDir.AddComponent<GizmoRotateAroundDirection>();
                gDirection.type = GizmoRotateAroundDirection.rotDirType.translation;
            }

            gPivot = this.pivotPos.GetComponent<GizmoRotateAroundPivot>();
            if (gPivot == null)
            {
                gPivot = this.pivotPos.AddComponent<GizmoRotateAroundPivot>();
                gDirection.type = GizmoRotateAroundDirection.rotDirType.rotation;

            }
            gPivot.directionTrans = gDirection.transform;

        }
    }
}