/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using RenownedGames.ExLibEditor;
using RenownedGames.ExLibEditor.Windows;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    public class NodeInspectorWindow : EditorWindow, IHasCustomMenu
    {
        private static HashSet<NodeInspectorWindow> Instances;

        public sealed class Styles
        {
            private GUIStyle toolbarLabelStyle;
            private GUIStyle toolbarStyle;
            private GUIStyle lockButtonStyle;

            /// <summary>
            /// Use it only in GUI calls.
            /// </summary>
            /// <returns></returns>
            public GUIStyle GetToolbarStyle()
            {
                if(toolbarStyle == null)
                {
                    toolbarStyle = new GUIStyle("preToolbar");
                    toolbarStyle.fontSize = 15;
                    toolbarStyle.alignment = TextAnchor.MiddleLeft;
                    toolbarStyle.padding = new RectOffset(10, 0, 10, 10);
                    toolbarStyle.stretchWidth = true;
                    toolbarStyle.stretchHeight = true;
                    toolbarStyle.fixedWidth = 0;
                    toolbarStyle.fixedHeight = 0;
                }
                return toolbarStyle;
            }

            /// <summary>
            /// Use it only in GUI calls.
            /// </summary>
            /// <returns></returns>
            public GUIStyle GetToolbarLabelStyle()
            {
                if (toolbarLabelStyle == null)
                {
                    toolbarLabelStyle = new GUIStyle(GUI.skin.label);
                    toolbarLabelStyle.fontSize = 13;
                    toolbarLabelStyle.fontStyle = FontStyle.Bold;
                    toolbarLabelStyle.alignment = TextAnchor.MiddleLeft;
                    toolbarLabelStyle.padding = new RectOffset(0, 0, 0, 0);
                    toolbarLabelStyle.wordWrap = true;
                    toolbarLabelStyle.stretchWidth = true;
                    toolbarLabelStyle.stretchHeight = true;
                    toolbarLabelStyle.fixedWidth = 0;
                    toolbarLabelStyle.fixedHeight = 0;
                }
                return toolbarLabelStyle;
            }

            /// <summary>
            /// Use it only in GUI calls.
            /// </summary>
            /// <returns></returns>
            public GUIStyle GetLockButtonStyle()
            {
                if (lockButtonStyle == null)
                {
                    lockButtonStyle = new GUIStyle("IN LockButton");
                }
                return lockButtonStyle;
            }
        }

        private bool isLocked;

        private Object target;
        private Editor editor;
        private MonoScript monoScript;

        private Styles styles;
        private Vector2 scrollPos;
        private GUIContent headerContent;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NodeInspectorWindow()
        {
            Instances = new HashSet<NodeInspectorWindow>();
        }

        /// <summary>
        /// Called when open window.
        /// </summary>
        protected virtual void OnEnable()
        {
            Texture2D icon = EditorResources.Load<Texture2D>("Images/Icons/Window/NodeInspectorIcon.png");
            titleContent = EditorGUIUtility.TrTextContentWithIcon("Node Inspector", icon);

            styles = new Styles();

            Undo.undoRedoPerformed += RepaintForce;

            Instances.Add(this);
            Instances.TrimExcess();
        }

        /// <summary>
        /// Called for rendering and handling window GUI.
        /// </summary>
        protected virtual void OnGUI()
        {
            if (target != null && editor != null)
            {
                DrawHeader(headerContent);
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(18);
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Space(4);
                            bool hierarchyMode = EditorGUIUtility.hierarchyMode;
                            EditorGUIUtility.hierarchyMode = true;
                            editor.OnInspectorGUI();
                            EditorGUIUtility.hierarchyMode = hierarchyMode;
                        }
                        GUILayout.EndVertical();
                        GUILayout.Space(4);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
        }

        /// <summary>
        /// Called when close the window.
        /// </summary>
        protected virtual void OnDestroy() 
        {
            Undo.undoRedoPerformed -= RepaintForce;

            Instances.Remove(this);
            Instances.TrimExcess();
        }

        /// <summary>
        /// Called at 10 frames per second to give the inspector a chance to update.
        /// </summary>
        protected virtual void OnInspectorUpdate()
        {
            Repaint();
        }

        /// <summary>
        /// Draw header on node inspector window.
        /// </summary>
        /// <param name="title">Header title.</param>
        /// <param name="icon">Header icon.</param>
        protected virtual void DrawHeader(GUIContent content)
        {
            const float HEIGHT = 52;

            Rect headerRect = GUILayoutUtility.GetRect(0, HEIGHT);
            headerRect.y -= 1;
            headerRect.height += 1;

            GUI.Box(headerRect, GUIContent.none, styles.GetToolbarStyle());

            if(content != null)
            {
                float iconSize = 0;

                headerRect.x = headerRect.xMin + 10;
                if (content.image != null)
                {
                    iconSize = 35;
                    headerRect.y += 10;
                    headerRect.width = iconSize;
                    headerRect.height = iconSize;

                    GUI.DrawTexture(headerRect, content.image, ScaleMode.ScaleToFit);

                    headerRect.x = headerRect.xMax + 7;
                    headerRect.y -= 10;
                }

                headerRect.width = position.width - iconSize;
                headerRect.height = HEIGHT;
                GUI.Label(headerRect, content.text, styles.GetToolbarLabelStyle());
            }
        }

        /// <summary>
        /// Start tracking specified target.
        /// <br>Target must be implmentation of MonoBehaviour or ScriptableObject.</br>
        /// </summary>
        /// <param name="target">Tracked target.</param>
        public void TrackEditor(Object target)
        {
            if (!isLocked || (this.target == null && target != null))
            {
                if (target != null && target is not MonoBehaviour && target is not ScriptableObject)
                {
                    throw new System.Exception("Unsupported type! Tracked type must be MonoBehaviour or ScriptableObject.");
                }

                this.target = target;
                if (target != null)
                {
                    editor = Editor.CreateEditor(target);
                    monoScript = GetMonoScript(target);
                    headerContent = GetHeaderContent(target, monoScript);
                }
                else
                {
                    editor = null;
                    monoScript = null;
                    headerContent = null;
                }
            }
        }

        /// <summary>
        /// Force repaint tracked target editor.
        /// </summary>
        public void RepaintForce()
        {
            if (target != null)
            {
                editor = Editor.CreateEditor(target);
            }
        }

        /// <summary>
        /// Get mono script for target.
        /// </summary>
        private MonoScript GetMonoScript(Object target)
        {
            if (target is MonoBehaviour monoBehaviour)
            {
                return MonoScript.FromMonoBehaviour(monoBehaviour);
            }
            else if (target is ScriptableObject scriptableObject)
            {
                return MonoScript.FromScriptableObject(scriptableObject);
            }
            throw new System.Exception("Unsupported type! Target type must be MonoBehaviour or ScriptableObject.");
        }

        /// <summary>
        /// Get header content for target.
        /// </summary>
        /// <returns></returns>
        private GUIContent GetHeaderContent(Object target, MonoScript monoScript)
        {
            if (target != null && monoScript != null)
            {
                if (target is MonoBehaviour monoBehaviour)
                {
                    return LoadObjectContent(target, monoScript);
                }
                else if (target is ScriptableObject scriptableObject)
                {
                    if (scriptableObject is Node node)
                    {
                        return LoadNodeContent(node, monoScript);
                    }
                    else
                    {
                        return LoadObjectContent(target, monoScript);
                    }
                }
            }
            return GUIContent.none;
        }

        /// <summary>
        /// Load node content for specified target and monoScript.
        /// </summary>
        private GUIContent LoadNodeContent(Node node, MonoScript monoScript)
        {
            NodeTypeCache.NodeCollection nodeInfos = NodeTypeCache.GetNodesInfo();
            for (int i = 0; i < nodeInfos.Count; i++)
            {
                NodeTypeCache.NodeInfo nodeInfo = nodeInfos[i];
                if (nodeInfo.type == node.GetType())
                {
                    GUIContent content = new GUIContent();
                    string name = "Undefined";
                    if(nodeInfo.contentAttribute != null)
                    {
                        if (!string.IsNullOrWhiteSpace(nodeInfo.contentAttribute.name))
                        {
                            name = nodeInfo.contentAttribute.name;
                        }
                        else if (!string.IsNullOrWhiteSpace(nodeInfo.contentAttribute.path))
                        {
                            name = System.IO.Path.GetFileName(nodeInfo.contentAttribute.path);
                        }
                    }

                    content.text = $"{name} ({ObjectNames.NicifyVariableName(monoScript.name)})";
                    content.image = nodeInfo.icon;
                    return content;
                }
            }
            return null;
        }

        /// <summary>
        /// Load object content for specified target and monoScript.
        /// </summary>
        private GUIContent LoadObjectContent(Object target, MonoScript monoScript)
        {
            GUIContent content = new GUIContent();
            content.text = $"{target.name} ({ObjectNames.NicifyVariableName(monoScript.name)})";
            content.image = AssetPreview.GetMiniThumbnail(target);
            return content;
        }

        /// <summary>
        /// Magic method which Unity detects automatically.
        /// </summary>
        /// <param name="position">Position of button.</param>
        private void ShowButton(Rect position)
        {
            isLocked = GUI.Toggle(position, isLocked, GUIContent.none, styles.GetLockButtonStyle());
        }

        #region [IHasCustomMenu Implementation]
        public void AddItemsToMenu(GenericMenu menu)
        {
            if(target != null)
            {
                menu.AddItem(new GUIContent("Ping"), false, () => EditorGUIUtility.PingObject(target));
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Ping"));
            }

            if(monoScript != null)
            {
                menu.AddItem(new GUIContent("Edit Script"), false, () => AssetDatabase.OpenAsset(monoScript));
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Edit Script"));
            }
        }
        #endregion

        #region [Static]
        [MenuItem("Tools/AI Tree/Windows/Node Inspector", false, 21)]
        public static void Open()
        {
            if (HasOpenInstances())
            {
                NodeInspectorWindow[] windows = GetInstances();
                for (int i = 0; i < windows.Length; i++)
                {
                    NodeInspectorWindow window = windows[i];
                    if (!window.isLocked)
                    {
                        window.Focus();
                    }
                }
            }
            else
            {
                CreateWindow();
            }
        }

        /// <summary>
        /// Check if has open instances of Node Inspector window.
        /// </summary>
        /// <returns></returns>
        public static bool HasOpenInstances()
        {
            return Instances.Count > 0;
        }

        /// <summary>
        /// Get all open Node Inspector window instances.
        /// </summary>
        /// <returns>System array of open instances.</returns>
        public static NodeInspectorWindow[] GetInstances()
        {
            return Instances.ToArray();
        }

        /// <summary>
        /// Notify all instance of Node Inspector window, to track specified editor.
        /// </summary>
        /// <param name="target">Tracked target.</param>
        public static void NotifyTrackEditor(Object target)
        {
            if(Instances.Count == 0)
            {
                NodeInspectorWindow window = CreateWindow();
                window.TrackEditor(target);
                window.MoveToCenter();
                window.Show();
            }
            else
            {
                foreach (NodeInspectorWindow window in Instances)
                {
                    window.TrackEditor(target);
                }
            }
        }

        /// <summary>
        /// Create new instance of Node Inspector window.
        /// </summary>
        /// <returns></returns>
        internal static NodeInspectorWindow CreateWindow()
        {
            NodeInspectorWindow window = CreateInstance<NodeInspectorWindow>();
            window.minSize = new Vector2(250, 50);
            window.MoveToCenter();
            window.Show();
            return window;
        }
        #endregion

        #region [Getter / Setter]
        public bool IsLocked()
        {
            return isLocked;
        }

        public void IsLocked(bool value)
        {
            isLocked = value;
        }

        public Object GetTarget()
        {
            return target;
        }

        public Editor GetEditor()
        {
            return editor;
        }

        public Styles GetStyles()
        {
            return styles;
        }

        public GUIContent GetHeaderContent()
        {
            return headerContent;
        }
        #endregion
    }
}
