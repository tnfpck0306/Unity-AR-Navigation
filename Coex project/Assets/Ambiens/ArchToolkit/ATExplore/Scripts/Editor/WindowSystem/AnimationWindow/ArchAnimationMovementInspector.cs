using ArchToolkit.AnimationSystem;
using UnityEngine;
using UnityEditor;
using ArchToolkit.Utils;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ArchToolkit.Editor.Window
{
    public enum RotationDirection { left, right, up, down };
    public enum TranslationDirection { forward, backward, right, left, up, down };


    [Serializable]
    public class ArchAnimationMovementInspector : ArchInspectorBase
    {

        protected GameObject currentGameobjectSelected;

        protected ATransformInteractable currentAnimation;

        protected References currentReference;

        protected bool isTargetListOpen = false;
        protected bool isCallAfterCompleteOpen = false;

        public ArchAnimationMovementInspector(string name) : base(name)
        {

        }

        public override void OnClose()
        {

        }

        public override void OnEnable()
        {
            EditorUtility.SetDirty(EditorUpdateManager.AnimationLogics);
            EditorUtility.SetDirty(EditorUpdateManager.AnimationContainer);
        }

        public override void OnFocus()
        {
            this.OnSelectionChange(Selection.activeGameObject);
        }

        protected T GetAnimationSelected<T>(GameObject checkGameobject) where T : AInteractable
        {
            if (checkGameobject == null)
                return null;

            List<References> references = checkGameobject.GetComponents<References>().ToList();

            References reference = null;

            T currentAnimation = null;

            if (references.Count > 0)
            {
                references.RemoveAll(r => r == null || r.reference == null);
                reference = references.ToList().Find(r => r.reference.GetComponent<T>());
            }

            if (reference != null && reference.reference != null)
                currentAnimation = reference.reference.GetComponent<T>();
            else
                currentAnimation = checkGameobject.GetComponent<T>();

            return currentAnimation;
        }

        public override void OnGui(Rect pos)
        {
            base.OnGui(pos);
        }

        protected void DrawInspectorFields()
        {
            GUILayout.Space(ArchToolkitWindowData.PADDING);

            GUILayout.BeginVertical();

            GUILayout.Label("Fields", new GUIStyle(EditorStyles.boldLabel));

            if (this.currentAnimation == null)
            {
                GUILayout.Label("Select one animation");
                return;
            }

            this.currentAnimation.StartWith = (AInteractable.StartWithType)EditorGUILayout.EnumPopup("Starting mode", this.currentAnimation.StartWith);

            this.currentAnimation.loop = EditorGUILayout.Toggle("Loop", this.currentAnimation.loop);

            this.currentAnimation.loopType = (AInteractable.LoopType)EditorGUILayout.EnumPopup("Loop type", this.currentAnimation.loopType);

            if (this.currentAnimation is RotateAround)
                this.currentAnimation.animationAmount = EditorGUILayout.FloatField("Angle (Degrees)", this.currentAnimation.animationAmount);

            else if (this.currentAnimation is Translate)
                this.currentAnimation.animationAmount = EditorGUILayout.FloatField("Distance (Meters)", this.currentAnimation.animationAmount);

            this.currentAnimation.animationDuration = EditorGUILayout.FloatField("Time (sec)", this.currentAnimation.animationDuration);

            GUILayout.Space(ArchToolkitWindowData.PADDING);

            this.isTargetListOpen = EditorGUILayout.Foldout(this.isTargetListOpen, " Target List ", true);

            if (this.isTargetListOpen)
            {
                if (GUILayout.Button(new GUIContent(ArchToolkitText.ADD_ANIMATION_TARGET, ArchToolkitText.ADD_TARGET_ANIMATION_TOOLTIP)))
                {
                    this.currentAnimation.TargetList.Add(null);
                }

                for (int i = 0; i < this.currentAnimation.TargetList.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    this.currentAnimation.TargetList[i] = EditorGUILayout.ObjectField("Target", this.currentAnimation.TargetList[i], typeof(GameObject), true) as GameObject;

                    if (i >= 1)
                    {
                        if (GUILayout.Button(new GUIContent("X", "Remove Target"), GUILayout.MaxHeight(20), GUILayout.MaxWidth(20), GUILayout.MinHeight(20), GUILayout.MaxWidth(20), GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            this.currentAnimation.TargetList.RemoveAt(i);
                        }
                    }

                    GUILayout.EndHorizontal();
                }
            }

            this.isCallAfterCompleteOpen = EditorGUILayout.Foldout(this.isCallAfterCompleteOpen, "Attached Animation", true);

            if (this.isCallAfterCompleteOpen)
            {
                if (GUILayout.Button(new GUIContent(ArchToolkitText.NEXT_INTERACTION, ArchToolkitText.NEXT_INTERACTION_TOOLTIP)))
                {
                    this.currentAnimation.CallAfterComplete.Add(null);
                }

                if (this.currentAnimation.CallAfterComplete.Count > 0)
                {
                    for (int i = 0; i < this.currentAnimation.CallAfterComplete.Count; i++)
                    {
                        GUILayout.BeginHorizontal();

                        this.currentAnimation.CallAfterComplete[i] = EditorGUILayout.ObjectField("Animation", this.currentAnimation.CallAfterComplete[i], typeof(AInteractable), true) as AInteractable;

                        if (GUILayout.Button(new GUIContent("X", "Remove interaction"), GUILayout.MaxHeight(20), GUILayout.MaxWidth(20), GUILayout.MinHeight(20), GUILayout.MaxWidth(20), GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            this.currentAnimation.CallAfterComplete.RemoveAt(i);
                        }

                        GUILayout.EndHorizontal();
                    }
                }
            }

            GUILayout.EndVertical();
        }

        public override void OnSelectionChange(GameObject gameObject)
        {

        }

        public override void OnUpdate()
        {

        }

        protected void DeleteAnimation()
        {
            GameObject.DestroyImmediate(this.currentReference);

            GameObject.DestroyImmediate(this.currentAnimation.gameObject);

            this.currentReference = null;
            this.currentAnimation = null;
            this.currentGameobjectSelected = null;

            Selection.activeGameObject = null;

        }

        public virtual void AddAnimation<T>() where T : ATransformInteractable
        {
            if (this.currentGameobjectSelected == null)
            {
                EditorUtility.DisplayDialog("Error", ArchToolkitText.SELECT_GAMEOBJECT, "Ok");

                return;
            }


            if (EditorUpdateManager.AnimationLogics == null)
            {
                EditorUpdateManager.AnimationLogics = new GameObject("AVR Logics");

                EditorUpdateManager.AnimationLogics.transform.position = Vector3.zero;
            }

            if (EditorUpdateManager.AnimationContainer == null)
            {
                EditorUpdateManager.AnimationContainer = new GameObject("Animation Container");

                EditorUpdateManager.AnimationContainer.transform.SetParent(EditorUpdateManager.AnimationLogics.transform);
            }

            this.currentReference = this.currentGameobjectSelected.GetComponent<References>();

            if (this.currentReference == null || this.currentReference.reference is T == false)  // if there isn't a reference or the reference is not setted like T
            {
                this.currentReference = this.currentGameobjectSelected.AddComponent<References>();

                var animation = new GameObject(typeof(T).Name + " " + this.currentGameobjectSelected.name);

                this.currentAnimation = animation.gameObject.AddComponent<T>();

                this.currentAnimation.transform.SetParent(EditorUpdateManager.AnimationContainer.transform);

                this.currentAnimation.transform.position = ArchToolkitProgrammingUtils.GetBounds(this.currentGameobjectSelected).center;

                this.currentReference.Init(this.currentAnimation.gameObject);

                this.SetPivots();

                if (this.currentGameobjectSelected.GetComponent<MeshRenderer>())
                {
                    if (!this.currentGameobjectSelected.GetComponent<Collider>())
                        this.currentGameobjectSelected.AddComponent<BoxCollider>();
                }

                if (!this.currentAnimation.TargetList.Contains(this.currentGameobjectSelected))
                    this.currentAnimation.TargetList.Add(this.currentGameobjectSelected);

                if (this.currentAnimation.TargetList.Count == 1) // if this, is the first target
                {
                    if (this.currentAnimation is RotateAround)
                    {
                        SetRotationDirection(RotationDirection.right);
                        this.currentAnimation.animationAmount = 90;
                    }
                    else
                    {
                        this.SetTranslationDirection(TranslationDirection.forward);
                        this.currentAnimation.animationAmount = 1;
                    }

                    SetChildStatic(this.currentGameobjectSelected.transform);
                    // Set ping pong as Default
                    this.currentAnimation.loopType = AInteractable.LoopType.pingpong;

                }

                this.OnSelectionChange(Selection.activeGameObject);
            }

        }


        protected void RecenterPivots()
        {

        }

        protected void SetPivots()
        {
            GizmoRotateAroundDirection gDirection = null;
            GizmoRotateAroundPivot gPivot = null;

            if (this.currentAnimation.gameObject.transform.Find("pivotDir") == null) // create pivot dir
            {
                var pivot = new GameObject("pivotDir");

                pivot.transform.SetParent(this.currentAnimation.transform);

                pivot.transform.position = this.currentGameobjectSelected.transform.position;

                gDirection = pivot.AddComponent<GizmoRotateAroundDirection>();

                this.currentAnimation.pivotDirPosition = gDirection.gameObject.transform.localPosition;
                this.currentAnimation.pivotPosPosition = gDirection.gameObject.transform.localPosition;
            }

            if (this.currentAnimation.gameObject.transform.Find("pivot") == null) // Create pivot 
            {
                var pivot = new GameObject("pivot");

                pivot.transform.SetParent(this.currentAnimation.transform);

                pivot.transform.position = this.currentGameobjectSelected.transform.position;

                gPivot = pivot.AddComponent<GizmoRotateAroundPivot>();

                if (gDirection != null)
                    gPivot.directionTrans = gDirection.transform;

                this.currentAnimation.PivotPosition = gPivot.position;
                this.currentAnimation.PivotDirection = gPivot.direction;
            }

            if (gDirection != null && gPivot != null)
                this.currentAnimation.SetGizmos(gDirection, gPivot);
        }

        protected void SetRotationDirection(RotationDirection rotationDirection)
        {
            if (this.currentAnimation == null)
            {
                EditorUtility.DisplayDialog("Error", "Set animation", "Ok");
                return;
            }

            AnimationLogics.SnapRotationToObject(rotationDirection, this.currentAnimation.TargetList, this.currentAnimation.transform.Find("pivotDir").gameObject, this.currentAnimation.transform.Find("pivot").gameObject);

            if (!this.currentAnimation.isHandleAlreadySetted)
                this.currentAnimation.SetHandle(Vector3.zero);
        }

        protected void SetTranslationDirection(TranslationDirection translationDirection)
        {
            if (this.currentAnimation == null)
            {
                EditorUtility.DisplayDialog("Error", "Set animation", "Ok");
                return;
            }

            AnimationLogics.SnapTranslationToObject(translationDirection, this.currentAnimation.TargetList, this.currentAnimation.transform.Find("pivotDir").gameObject, this.currentAnimation.transform.Find("pivot").gameObject, SceneView.lastActiveSceneView.camera.transform);

            if (!this.currentAnimation.isHandleAlreadySetted)
                this.currentAnimation.SetHandle(Vector3.zero);
        }
    }
}