using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ArchToolkit.Editor.Window
{
    [System.Serializable]
    public class ArchInspectorBase : IArchInspector
    {
        protected bool inspectorFoldoutOpen = true;

        protected GUIStyle foldoutStyle;

        public ArchInspectorBase(string name)
        {
            //MainArchWindow.Instance.AddInspector(this);

            this.Name = name;
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                this._name = value;
            }
        }

        public bool InspectorVisible
        {
            get
            {
                return this.isInspectorVisible;
            }

        }

        protected bool isInspectorVisible = false;

        private string _name;

        public virtual void OnClose()
        {
            
        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnGui(Rect pos)
        {
            if (this.foldoutStyle == null)
                this.foldoutStyle = ArchToolkitWindowData.GetFoldoutStyle(15);

            //style.normal.background = background;
            this.inspectorFoldoutOpen = EditorGUILayout.Foldout(this.inspectorFoldoutOpen, this.Name, true, this.foldoutStyle);

            GUILayout.Space(ArchToolkitWindowData.PADDING);

        }

        public virtual void OnSelectionChange(GameObject gameObject)
        {
            
        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnFocus()
        {
            
        }

        public virtual bool IsInspectorVisible()
        {
            return false;
        }

        public virtual void OnProjectChange()
        {
            
        }

        public static void SetChildStatic(Transform child)
        {
            child.gameObject.isStatic = false;

            for (int i = 0; i < child.childCount; i++)
            {
                if (child.GetChild(i) == null)
                    continue;

                child.GetChild(i).gameObject.isStatic = false;

                SetChildStatic(child.GetChild(i));
            }
        }
    }
}