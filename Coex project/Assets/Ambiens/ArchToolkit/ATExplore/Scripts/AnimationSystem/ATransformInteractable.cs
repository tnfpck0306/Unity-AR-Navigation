using System.Collections.Generic;
using UnityEngine;

namespace ArchToolkit.AnimationSystem
{
    [System.Serializable]
    public abstract class ATransformInteractable : AInteractable
    {

        public Vector3 PivotPosition;
        public Vector3 PivotDirection;
        public float animationDuration = 1;
        public float animationAmount = 90;

        [SerializeField]
        protected bool isAnimating = false;
        [SerializeField]
        protected float unitsPerSecond;
        public float currentSign = 1;
        protected float unitsApplied = 0;
        [HideInInspector]
        public bool TestInEditor = false;
        public Vector3 pivotPosPosition;
        public Vector3 pivotDirPosition;

#if UNITY_EDITOR

        public GizmoRotateAroundDirection PivotDirectionGizmo
        {
            get { return this.pivotDirectionGizmo; }
        }

        public GizmoRotateAroundPivot PivotPositionGizmo
        {
            get { return this.pivotPositionGizmo; }
        }
#endif
        [SerializeField][HideInInspector]
        private GizmoRotateAroundDirection pivotDirectionGizmo;
        [SerializeField][HideInInspector]
        private GizmoRotateAroundPivot pivotPositionGizmo;



        public void SetGizmos(GizmoRotateAroundDirection aroundDirection, GizmoRotateAroundPivot aroundPivot)
        {
#if UNITY_EDITOR
            this.pivotDirectionGizmo = aroundDirection;
            this.pivotPositionGizmo = aroundPivot;
#endif
        }

        public override ArchBasicHandle SetHandle(Vector3 position)
        {
            this.basicHandle = this.TargetList[0].AddComponent<ArchBasicHandle>();

            this.basicHandle.animationToOpen = this;

            this.TargetList[0].tag = ArchToolkitDataPaths.ARCHINTERACTABLETAG;

            return this.basicHandle;
        }

        public override void StartAnimation()
        {
            if (!isAnimating)
            {
                //DISATTIVARE COLLIDER

                this.unitsApplied = 0;
                this.isAnimating = true;
                this.unitsPerSecond = this.currentSign * this.animationAmount / this.animationDuration;
                this.calculateFinalPosition();

                if (this.loopType == LoopType.pingpong)
                    this.currentSign = this.currentSign * -1;
            }
        }
        public abstract void animateFunction(float amount);
        public abstract void calculateAnimationAmount(float amount);
        public override bool AnimationOn()
        {
            return isAnimating;
        }

        protected override void Update()
        {
            /*
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying && !EditorUpdateManager.Instance.IsActionInQueue(this.EditorUpdate))
            {
                EditorUpdateManager.Instance.AddToUpdate(this.EditorUpdate);
            }
#endif*/
            /*Relative position fix? Need to check this!*/
            this.PivotPosition = this.pivotPositionGizmo.position;
            this.PivotDirection = this.pivotPositionGizmo.direction;

            this.pivotDirPosition = this.pivotDirectionGizmo.gameObject.transform.position;
            this.pivotPosPosition = this.pivotPositionGizmo.gameObject.transform.position;

            this.CheckAnimation();
            
        }

        private void CheckAnimation()
        {
            if (isAnimating)
            {
               
                float nextAnimation = this.unitsPerSecond * Time.deltaTime;
                
                this.unitsApplied += this.currentSign * nextAnimation;

                this.animateFunction(nextAnimation);

                if (Mathf.Abs(this.unitsApplied) >= Mathf.Abs(this.animationAmount))
                {
                    this.applyFinalPosition();
                    isAnimating = false;
                    if (this.loop)
                    {
                        this.StartAnimation();
                    }
                    else
                    {
                        this.CompleteAnimation();

                        this.TestInEditor = false;
                    }
                }
            }
        }

        public List<Quaternion> rotations = new List<Quaternion>();
        public List<Vector3> positions = new List<Vector3>();

        public void calculateFinalPosition()
        {
            this.rotations.Clear();
            this.positions.Clear();

            this.animateFunction(this.animationAmount * currentSign);
            foreach (var t in this.TargetList)
            {
                this.rotations.Add(t.transform.rotation);
                this.positions.Add(t.transform.position);
            }
            this.animateFunction(-this.animationAmount * currentSign);
        }

        public void applyFinalPosition()
        {
            for (var i = 0; i < this.TargetList.Count; i++)
            {
                var t = this.TargetList[i].transform;
                t.rotation = this.rotations[i];
                t.position = this.positions[i];
            }
        }

        protected override void EditorUpdate()
        {

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                if(this.pivotDirectionGizmo == null || this.pivotPositionGizmo == null)
                {
                    Debug.LogWarning("Gizmo not found, have you destroyed the pivot? ");

                    GameObject.DestroyImmediate(this.gameObject);

                    return;
                }

                this.PivotPosition = this.pivotPositionGizmo.position;
                this.PivotDirection = this.pivotPositionGizmo.direction;

                this.pivotDirPosition = this.pivotDirectionGizmo.gameObject.transform.position;
                this.pivotPosPosition = this.pivotPositionGizmo.gameObject.transform.position;

                this.calculateAnimationAmount(this.pivotPositionGizmo.amount);

                this.CheckAnimation();

                if (this.TestInEditor)
                {
                    this.StartAnimation();
                }
            }
#endif
        }
    }
}