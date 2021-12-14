using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

#endif

namespace ArchToolkit.Editor.Window
{
    public abstract class ArchWindowBase 
    {
        protected WindowStatus windowStatus;

        public string TabLabel;

        public List<ArchInspectorBase> inspectors=new List<ArchInspectorBase>();

        public float MaxWindowHeight 
        {
            get
            {
                // 100 is default 
                // 50 is the height between tabs and button
                return ArchToolkitWindowData.TAB_HEIGHT +
                        ArchToolkitWindowData.PADDING +
                        ((ArchToolkitWindowData.BUTTON_HEIGHT + GUI.skin.button.padding.bottom) * _buttonCount) +
                            ArchToolkitWindowData.PADDING;
            }
           
        }

        public int ButtonCount
        {
            get
            {
                return this._buttonCount;
            }
            set
            {
                this._buttonCount = value;
            }
        }

        protected float _windowHeight;

        protected int _buttonCount;

        public WindowStatus GetStatus
        {
            get
            {
                return windowStatus;
            }
        }
        public int TabIndex { get; private set; }
        
        public ArchWindowBase(WindowStatus windowStatus)
        {
            this.windowStatus = windowStatus;

            this.Init();
            this.InstantiateInspectors();

            if (MainArchWindow.Instance != null)
                this.TabIndex=MainArchWindow.Instance.AddWindow(this);
        }

        public abstract void Init();
        public abstract void InstantiateInspectors();

        public void DrawGUI()
        {
            
            this._DrawGUI();

        }
        private Vector2 scroll;
        public virtual void DrawInspectors(Rect pos, GUIStyle inspectorStyle)
        {
            GUILayout.BeginArea(new Rect(0, ArchToolkitWindowData.MainAreaAnchor + this.MaxWindowHeight, pos.width, pos.height - 50));

            GUILayout.BeginVertical();

            if (this.inspectors.Count > 0)
            {
                var scrollbarStyle = new GUIStyle(GUI.skin.horizontalScrollbar);

                scrollbarStyle.fixedHeight = scrollbarStyle.fixedWidth = 0;

                scroll = EditorGUILayout.BeginScrollView(scroll, scrollbarStyle, GUI.skin.verticalScrollbar, GUILayout.Width(pos.width), GUILayout.Height(pos.height - (ArchToolkitWindowData.MainAreaAnchor + this.MaxWindowHeight)));

                foreach (var inspector in this.inspectors)
                {
                    if (inspector.IsInspectorVisible())
                    {
                        GUILayout.BeginVertical(inspectorStyle);

                        GUILayout.Space(ArchToolkitWindowData.PADDING);

                        inspector.OnGui(pos);

                        GUILayout.EndVertical();
                    }
                }

                EditorGUILayout.EndScrollView();
            }

            GUILayout.EndVertical();

            GUILayout.EndArea();
        }

        protected abstract void _DrawGUI();

        public virtual void OnClose()
        {
            
        }

        public virtual void OnOpen()
        {
            
        }

        public virtual GameObject InstatiateInScene(GameObject prefab)
        {
            var instance= PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            Camera sceneCam = SceneView.lastActiveSceneView.camera;
            if (sceneCam != null)
            {
                Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 5));
                instance.transform.position = spawnPos;
                instance.transform.rotation = Quaternion.Euler(0, sceneCam.transform.rotation.eulerAngles.y, 0);
            }
            else
                instance.transform.position = Vector3.one;

            return instance;
        }

        public virtual void OnSelectionChange(GameObject gameObject)
        {
            if (this.inspectors.Count > 0)
            {
                foreach (var inspector in this.inspectors)
                {
                    inspector.OnSelectionChange(Selection.activeGameObject);
                }
            }
        }
        
    }
}
