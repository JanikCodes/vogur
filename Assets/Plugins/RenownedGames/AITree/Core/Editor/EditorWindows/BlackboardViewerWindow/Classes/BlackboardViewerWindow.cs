/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using RenownedGames.ApexEditor;
using RenownedGames.ExLibEditor;
using RenownedGames.ExLibEditor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RenownedGames.AITreeEditor
{
    public sealed class BlackboardViewerWindow : EditorWindow, IHasCustomMenu
    {
        private static HashSet<BlackboardViewerWindow> Instances;

        public sealed class Styles
        {
            private Texture2D entryTexture;
            private GUIStyle entryStyle;
            private GUIStyle placeholderStyle;
            private GUIStyle lockButtonStyle;

            public Styles()
            {
                entryTexture = CreateTexture(new Color32(64, 64, 64, 255), Color.black);
            }

            /// <summary>
            /// Use it only in GUI calls.
            /// </summary>
            public GUIStyle GetPlaceholderStyle()
            {
                if (placeholderStyle == null)
                {
                    placeholderStyle = new GUIStyle(GUI.skin.label);
                    placeholderStyle.fontStyle = FontStyle.Italic;
                    placeholderStyle.fontSize = 11;
                    placeholderStyle.normal.textColor = Color.gray;
                    placeholderStyle.active.textColor = Color.gray;
                    placeholderStyle.focused.textColor = Color.gray;
                    placeholderStyle.hover.textColor = Color.gray;
                }
                return placeholderStyle;
            }

            /// <summary>
            /// Use it only in GUI calls.
            /// </summary>
            public GUIStyle GetLockButtonStyle()
            {
                if (lockButtonStyle == null)
                {
                    lockButtonStyle = new GUIStyle("IN LockButton");
                }
                return lockButtonStyle;
            }

            /// <summary>
            /// Use it only in GUI calls.
            /// </summary>
            public GUIStyle GetToolbarStyle()
            {
                if (entryStyle == null)
                {
                    entryStyle = new GUIStyle();

                    entryStyle.fontSize = 12;
                    entryStyle.fontStyle = FontStyle.Normal;
                    entryStyle.alignment = TextAnchor.MiddleLeft;
                    entryStyle.border = new RectOffset(2, 2, 2, 2);
                    entryStyle.padding = new RectOffset(10, 0, 0, 0);

                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    entryStyle.normal.textColor = textColor;
                    entryStyle.normal.background = entryTexture;
                    entryStyle.normal.scaledBackgrounds = new Texture2D[1] { entryStyle.normal.background };

                    entryStyle.active.textColor = textColor;
                    entryStyle.active.background = entryStyle.normal.background;
                    entryStyle.active.scaledBackgrounds = new Texture2D[1] { entryStyle.normal.background };

                    entryStyle.focused.textColor = textColor;
                    entryStyle.focused.background = entryStyle.normal.background;
                    entryStyle.focused.scaledBackgrounds = new Texture2D[1] { entryStyle.normal.background };

                    entryStyle.hover.textColor = textColor;
                    entryStyle.hover.background = entryStyle.normal.background;
                    entryStyle.hover.scaledBackgrounds = new Texture2D[1] { entryStyle.normal.background };
                }
                return entryStyle;
            }

            /// <summary>
            /// Create new square texture with border.
            /// </summary>
            public Texture2D CreateTexture(Color mainColor, Color borderColor)
            {
                Texture2D texture = new Texture2D(8, 8);

                Color[] colors = new Color[texture.width * texture.height];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = mainColor;
                }
                texture.SetPixels(colors);

                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, 0, borderColor);
                    texture.SetPixel(x, texture.height - 1, borderColor);
                }

                for (int y = 0; y < texture.height; y++)
                {
                    texture.SetPixel(0, y, borderColor);
                    texture.SetPixel(texture.width - 1, y, borderColor);
                }

                texture.filterMode = FilterMode.Point;
                texture.Apply();

                return texture;
            }
        }

        private bool isLocked;
        private Object target;
        private BehaviourTree behaviourTree;
        private BehaviourRunner runner;
        private Blackboard blackboard;
        private Blackboard lastBlackboard;

        // Stored required properties.
        private string searchText;
        private SearchField searchField;
        private Styles styles;
        private Vector2 scrollPos;
        private List<SerializedField> fields;
        private float viewHeight;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static BlackboardViewerWindow()
        {
            Instances = new HashSet<BlackboardViewerWindow>();
        }

        /// <summary>
        /// Called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            Texture2D icon = EditorResources.Load<Texture2D>("Images/Icons/Window/BlackboardIcon.png");
            titleContent = new GUIContent("Blackboard Viewer", icon);

            styles = new Styles();
            searchText = string.Empty;
            searchField = new SearchField();
            fields = new List<SerializedField>();

            OnSelectionChange();

            EditorApplication.playModeStateChanged += OnPlayModeChanged;

            Instances.Add(this);
            Instances.TrimExcess();
        }

        /// <summary>
        /// Called for rendering and handling GUI events.
        /// </summary>
        private void OnGUI()
        {
            bool hierarchyMode = EditorGUIUtility.hierarchyMode;
            EditorGUIUtility.hierarchyMode = true;
            DrawToolbar();
            DrawContent();
            DrawFooter();
            EditorGUIUtility.hierarchyMode = hierarchyMode;
        }

        /// <summary>
        /// Draw toolbar of window.
        /// </summary>
        private void DrawToolbar()
        {
            Rect rect = new Rect(-1, -1, position.width + 2, 22);
            GUI.Box(rect, GUIContent.none, styles.GetToolbarStyle());

            Rect searchRect = new Rect(rect.xMax - 249, rect.y + 3, 246, 20);
            searchText = searchField.OnToolbarGUI(searchRect, searchText);
        }

        /// <summary>
        /// Draw content of blackboard.
        /// </summary>
        private void DrawContent()
        {
            if(viewHeight == 0)
            {
                viewHeight = fields.Count * 25;
            }

            Rect rect = new Rect(0, 20, position.width, position.height - 20);
            Rect viewRect = new Rect(0, 0, position.width, viewHeight + rect.y);

            if(rect.height < viewRect.height)
            {
                viewRect.width -= 13;
                rect.height -= 20;
                viewRect.height -= 18;
            }

            scrollPos = GUI.BeginScrollView(rect, scrollPos, viewRect);
            {
                Rect entryRect = new Rect(0, 0, viewRect.width, 25);
                GUI.Box(entryRect, GUIContent.none, ApexStyles.BoxEntryEven);

                Rect fieldRect = new Rect(entryRect.x + 4, entryRect.y + 3, entryRect.width - 8, EditorGUIUtility.singleLineHeight);
                EditorGUI.BeginChangeCheck();
                blackboard = (Blackboard)EditorGUI.ObjectField(fieldRect, "Blackboard", blackboard, typeof(Blackboard), true);
                entryRect.y = entryRect.yMax - 1;
                if (EditorGUI.EndChangeCheck())
                {
                    TrackEditor(blackboard);
                }


                if (fields.Count > 0)
                {
                    int index = 0;
                    for (int i = 0; i < fields.Count; i++)
                    {
                        SerializedField field = fields[i];
                        try
                        {
                            field.GetSerializedObject().targetObject.GetType();
                        }
                        catch
                        {
                            fields.RemoveAt(i);
                            i--;
                            continue;
                        }

                        if (string.IsNullOrEmpty(searchText) || field.GetLabel().text.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        {
                            float height = field.GetHeight();
                            if (height > 25)
                            {
                                entryRect.height = height + 7;
                            }
                            else
                            {
                                entryRect.height = 25;
                            }

                            GUI.Box(entryRect, GUIContent.none, index % 2 == 0 ? ApexStyles.BoxEntryOdd : ApexStyles.BoxEntryEven);

                            fieldRect.y = entryRect.y + 3;
                            fieldRect.height = height;
                            field.GetSerializedObject().Update();
                            field.OnGUI(fieldRect);

                            entryRect.y = entryRect.yMax - 1;
                            index++;
                        }
                    }
                }
                viewHeight = entryRect.y;
            }
            GUI.EndScrollView();
        }

        /// <summary>
        /// Draw footer of window.
        /// </summary>
        private void DrawFooter()
        {
            Rect rect = new Rect(-1, position.height - 22, position.width + 2, 22);

            GUI.Box(rect, GUIContent.none, styles.GetToolbarStyle());

            rect.x += 4;
            rect.width -= 4;
            GUIContent content = new GUIContent(GetTargetInfo());
            GUI.Label(rect, content, styles.GetPlaceholderStyle());

            if (blackboard != null && position.width - styles.GetPlaceholderStyle().CalcSize(content).x > 64)
            {
                rect.x = rect.xMax - 64;
                rect.width = 62;
                GUI.Label(rect, "Read-only", styles.GetPlaceholderStyle());
            }
        }

        /// <summary>
        /// Called at 10 frames per second to give the inspector a chance to update.
        /// </summary>
        private void OnInspectorUpdate()
        {
            if(target == null && blackboard != null)
            {
                TrackEditorForce(null);
            }

            if(runner != null)
            {
                blackboard = runner.GetBlackboard();
                if(blackboard == null)
                {
                    blackboard = runner.GetSharedBlackboard();
                }
            }
            else if(behaviourTree != null)
            {
                blackboard = behaviourTree.GetBlackboard();
            }

            if(blackboard != lastBlackboard)
            {
                UpdateFields();
                lastBlackboard = blackboard;
            }

            Repaint();
        }

        /// <summary>
        /// Called when close the window.
        /// </summary>
        private void OnDestroy()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;

            Instances.Remove(this);
            Instances.TrimExcess();
        }

        /// <summary>
        /// Called when the selection changes.
        /// </summary>
        private void OnSelectionChange()
        {
            if (Selection.activeObject != null)
            {
                TrackEditor(Selection.activeObject);
            }
        }

        /// <summary>
        /// Called when editor changed play mode state.
        /// </summary>
        /// <param name="mode">New play mode state.</param>
        private void OnPlayModeChanged(PlayModeStateChange mode)
        {
            styles = new Styles();
            TrackEditorForce(target);
        }

        /// <summary>
        /// Start tracking specified target.
        /// <br>Target can be of Blackboard, BehaviourTree, BehaviourRunner or Gameobject with BehaviourRunner.</br>
        /// </summary>
        /// <param name="target">Tracked target.</param>
        public void TrackEditor(Object target)
        {
            if (!isLocked || (this.target == null && target != null))
            {
                TrackEditorForce(target);
            }
        }

        /// <summary>
        /// Current information in string representation of tracked target.
        /// </summary>
        /// <returns></returns>
        public string GetTargetInfo()
        {
            StringBuilder sb = new StringBuilder();
            if (blackboard != null)
            {
                sb.Append(blackboard.name);

                bool isShared = AssetDatabase.IsNativeAsset(blackboard);
                sb.Append(isShared ? " (Shared)" : " (Associated)");

                if (runner != null)
                {
                    sb.AppendFormat(" of {0} object", runner.name);
                }
                else if (behaviourTree != null)
                {
                    sb.AppendFormat(" of {0} asset", behaviourTree.name);
                }
            }
            else
            {
                sb.Append("Select blackboard...");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Track editor except locking state.
        /// </summary>
        /// <br>Target must be of Blackboard, BehaviourRunner or Gameobject with BehaviourRunner.</br>
        /// <param name="target">Tracked target</param>
        internal void TrackEditorForce(Object target)
        {
            if(target == null)
            {
                this.target = null;
                this.runner = null;
                this.blackboard = null;
            }
            else
            {
                if (target is Blackboard blackboard)
                {
                    this.target = target;

                    this.blackboard = blackboard;
                    this.runner = null;
                    this.behaviourTree = null;
                }
                else if (target is BehaviourTree behaviourTree)
                {
                    this.target = target;
                    this.behaviourTree = behaviourTree;
                    this.blackboard = behaviourTree.GetBlackboard();
                    this.runner = null;
                }
                else if (target is BehaviourRunner runner || (target is GameObject go && go.TryGetComponent<BehaviourRunner>(out runner)))
                {
                    this.target = target;
                    this.runner = runner;
                    blackboard = runner.GetBlackboard();
                    if (blackboard == null)
                    {
                        blackboard = runner.GetSharedBlackboard();
                    }
                    this.blackboard = blackboard;
                    this.behaviourTree = null;
                }
            }

            UpdateFields();
        }

        /// <summary>
        /// Magic method which Unity detects automatically.
        /// </summary>
        /// <param name="position">Position of button.</param>
        private void ShowButton(Rect position)
        {
            isLocked = GUI.Toggle(position, isLocked, GUIContent.none, styles.GetLockButtonStyle());
        }

        /// <summary>
        /// Collect fields of current blackboard.
        /// </summary>
        private void UpdateFields()
        {
            fields.Clear();
            if (blackboard != null)
            {
                foreach (Key key in blackboard.Keys)
                {
                    SerializedObject serialziedObject = new SerializedObject(key);
                    SerializedField field = new SerializedField(serialziedObject, "value");
                    field.SetLabel(new GUIContent(key.name));
                    fields.Add(field);
                }
            }
            fields.TrimExcess();
        }

        #region [IHasCustomMenu Inplementation]
        /// <summary>
        /// Adds custom items to editor window context menu.
        /// </summary>
        /// <param name="menu">Context menu.</param>
        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Ping"), false, () => EditorGUIUtility.PingObject(target));
        }
        #endregion

        #region [Static]
        /// <summary>
        /// Open Blackboard Viewer window.
        /// </summary>
        [MenuItem("Tools/AI Tree/Windows/Blackboard Viewer", false, 25)]
        public static void Open()
        {
            if (HasOpenInstances())
            {
                BlackboardViewerWindow[] windows = GetInstances();
                for (int i = 0; i < windows.Length; i++)
                {
                    BlackboardViewerWindow window = windows[i];
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
        /// Check if has open instances of Blackboard Viewer window.
        /// </summary>
        /// <returns></returns>
        public static bool HasOpenInstances()
        {
            return Instances.Count > 0;
        }

        /// <summary>
        /// Get all open Blackboard Viewer window instances.
        /// </summary>
        /// <returns>System array of open instances.</returns>
        public static BlackboardViewerWindow[] GetInstances()
        {
            return Instances.ToArray();
        }

        /// <summary>
        /// Notify all instance of Blackboard Viewer window, to track specified editor.
        /// </summary>
        /// <param name="target">Tracked target.</param>
        public static void NotifyTrackEditor(Object target)
        {
            if (Instances.Count == 0)
            {
                BlackboardViewerWindow window = CreateWindow();
                window.TrackEditor(target);
                window.MoveToCenter();
                window.Show();
            }
            else
            {
                foreach (BlackboardViewerWindow window in Instances)
                {
                    window.TrackEditor(target);
                }
            }
        }

        /// <summary>
        /// Create new instance of Blackboard Viewer window.
        /// </summary>
        /// <returns></returns>
        internal static BlackboardViewerWindow CreateWindow()
        {
            BlackboardViewerWindow window = CreateInstance<BlackboardViewerWindow>();
            window.minSize = new Vector2(252, 64);
            window.MoveToCenter();
            window.Show();
            return window;
        }
        #endregion
    }
}