/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using RenownedGames.ExLibEditor;
using RenownedGames.ExLibEditor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    [Obsolete("Use Blackboard Details window instead.")]
    public sealed class KeyDetailsWindow : EditorWindow, IHasCustomMenu
    {
        private static HashSet<KeyDetailsWindow> Instances;

        public sealed class Styles
        {
            private GUIStyle lockButtonStyle;

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

        /// <summary>
        /// Static constructor.
        /// </summary>
        static KeyDetailsWindow()
        {
            Instances = new HashSet<KeyDetailsWindow>();
        }

        private bool isLocked;

        private Key key;
        private Blackboard blackboard;
        private SerializedObject serializedKey;

        // Stored required properties.
        private Styles styles;
        private Label typeField;
        private IMGUIContainer preContainer;
        private IMGUIContainer postContainer;
        private VisualElement typeColor;
        private IMGUIContainer parentContainer;
        private ScrollView keyFoldoutScrollView;

        /// <summary>
        /// Called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            Instances.Add(this);
            Instances.TrimExcess();

            Texture2D icon = EditorResources.Load<Texture2D>("Images/Icons/Window/KeyDetailsIcon.png");
            titleContent = new GUIContent("Key Details (Obsolete)", icon);

            styles = new Styles();

            LoadVisualElements();

            if (key != null)
            {
                serializedKey = new SerializedObject(key);
            }
        }

        /// <summary>
        /// Called when the window gets keyboard focus.
        /// </summary>
        private void OnFocus()
        {
            if (HasUnloadedVisualElements())
            {
                LoadVisualElements();
            }
        }

        /// <summary>
        /// Called for rendering and handling GUI events.
        /// </summary>
        private void OnGUI()
        {
            keyFoldoutScrollView.style.display = key != null ? DisplayStyle.Flex : DisplayStyle.None;
            if (key != null)
            {
                typeColor.style.backgroundColor = new StyleColor(key.GetColor());
                typeField.text = key.GetValueType().Name;

                bool hierarchyMode = EditorGUIUtility.hierarchyMode;
                EditorGUIUtility.hierarchyMode = true;
                typeColor.style.left = EditorGUIUtility.labelWidth;

                const float SPACE = 21;
                typeField.style.left = EditorGUIUtility.labelWidth + SPACE;
                EditorGUIUtility.hierarchyMode = hierarchyMode;
            }
            Repaint();
        }

        /// <summary>
        /// Called when close the window.
        /// </summary>
        private void OnDestroy()
        {
            Instances.Remove(this);
            Instances.TrimExcess();
        }

        /// <summary>
        /// Called when on pre-container IMGUI rendering.
        /// </summary>
        private void OnPreGUI()
        {
            if (key != null && serializedKey != null)
            {
                serializedKey.Update();

                bool repaint = false;
                bool hierarchyMode = EditorGUIUtility.hierarchyMode;

                EditorGUIUtility.hierarchyMode = true;
                EditorGUI.BeginChangeCheck();
                EditorGUI.BeginDisabledGroup(key is SelfKey);
                SerializedProperty name = serializedKey.FindProperty("m_Name");
                Rect nameRect = EditorGUILayout.GetControlRect(true);
                string newName = EditorGUI.DelayedTextField(nameRect, "Name", name.stringValue);
                EditorGUI.EndDisabledGroup();
                if (EditorGUI.EndChangeCheck())
                {
                    newName = newName.Trim();
                    if (newName != "Self")
                    {
                        if (blackboard.Contains(newName))
                        {
                            EditorUtility.DisplayDialog("AI Tree", "A key with this name already exists.", "Ok");
                        }
                        else
                        {
                            name.stringValue = newName;
                            repaint = true;
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("AI Tree", "Self key name is reserved by the system.", "Ok");
                    }
                }

                EditorGUI.BeginChangeCheck();
                SerializedProperty category = serializedKey.FindProperty("category");
                Rect categoryRect = EditorGUILayout.GetControlRect(true);
                category.stringValue = EditorGUI.DelayedTextField(categoryRect, "Category", category.stringValue);
                if (EditorGUI.EndChangeCheck())
                {
                    repaint = true;
                }

                SerializedProperty description = serializedKey.FindProperty("description");
                Rect textAreaRect = EditorGUILayout.GetControlRect(true, 50);
                textAreaRect = EditorGUI.PrefixLabel(textAreaRect, new GUIContent("Description"));

                GUIStyle style = new GUIStyle(GUI.skin.textField);
                style.wordWrap = true;
                description.stringValue = EditorGUI.TextArea(textAreaRect, description.stringValue, style);

                if (serializedKey.hasModifiedProperties)
                {
                    serializedKey.ApplyModifiedProperties();
                }

                if (repaint && HasOpenInstances<BlackboardWindow>())
                {
                    GetWindow<BlackboardWindow>().RefreshKeys();
                }
                EditorGUIUtility.hierarchyMode = hierarchyMode;
            }
        }

        /// <summary>
        /// Called when on post-container IMGUI rendering.
        /// </summary>
        private void OnPostGUI()
        {
            if (key != null && serializedKey != null)
            {
                bool hierarchyMode = EditorGUIUtility.hierarchyMode;
                EditorGUIUtility.hierarchyMode = true;
                EditorGUI.BeginDisabledGroup(key is SelfKey);
                EditorGUI.BeginChangeCheck();
                SerializedProperty sync = serializedKey.FindProperty("sync");
                Rect syncRect = EditorGUILayout.GetControlRect(true);
                sync.boolValue = EditorGUI.Toggle(syncRect, "Sync", sync.boolValue);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedKey.ApplyModifiedProperties();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUIUtility.hierarchyMode = hierarchyMode;
            }
        }

        /// <summary>
        /// Called when on parent container IMGUI rendering.
        /// </summary>
        private void OnParentGUI()
        {
            if (blackboard != null)
            {
                bool hierarchyMode = EditorGUIUtility.hierarchyMode;
                EditorGUIUtility.hierarchyMode = true;
                Blackboard parent = (Blackboard)EditorGUILayout.ObjectField(new GUIContent("Parent"), blackboard.GetParent(), typeof(Blackboard), false);
                EditorGUIUtility.hierarchyMode = hierarchyMode;

                if (parent != blackboard.GetParent() && (parent == null || !parent.IsNested(blackboard)))
                {
                    blackboard.SetParent(parent);
                    GetWindow<BlackboardWindow>().TrackEditor(blackboard);
                }
            }
        }

        /// <summary>
        /// Track editor of specified key reference.
        /// </summary>
        /// <param name="key">Key reference.</param>
        /// <param name="blackboard">Blackboard reference.</param>
        public void TrackEditor(Key key, Blackboard blackboard)
        {
            if (HasUnloadedVisualElements())
            {
                LoadVisualElements();
            }

            if (!isLocked || (this.key == null && key != null))
            {
                this.key = key;
                this.blackboard = blackboard;

                if(key != null)
                {
                    serializedKey = new SerializedObject(key);
                }
                else
                {
                    serializedKey = null;
                }

                Repaint();
            }
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
        /// Load all required Visual Elements.
        /// </summary>
        private void LoadVisualElements()
        {
            AITreeSettings settings = AITreeSettings.instance;

            rootVisualElement.Clear();

            VisualTreeAsset visualTree = settings.GetKeyDetailsUXML();
            visualTree.CloneTree(rootVisualElement);

            rootVisualElement.styleSheets.Add(settings.GetKeyDetailsUSS());

            preContainer = rootVisualElement.Q<IMGUIContainer>("pre-container");
            postContainer = rootVisualElement.Q<IMGUIContainer>("post-container");
            typeField = rootVisualElement.Q<Label>("type-field");
            typeColor = rootVisualElement.Q<VisualElement>("type-color");
            parentContainer = rootVisualElement.Q<IMGUIContainer>("parent-container");
            keyFoldoutScrollView = rootVisualElement.Q<ScrollView>("key-foldout-scrollview");

            keyFoldoutScrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

            preContainer.onGUIHandler = OnPreGUI;
            postContainer.onGUIHandler = OnPostGUI;
            parentContainer.onGUIHandler = OnParentGUI;
        }

        /// <summary>
        /// Check if window has new or unloaded visual elements.
        /// </summary>
        /// <returns>True if has new or unloaded visual elements, otherwise false.</returns>
        private bool HasUnloadedVisualElements()
        {
            return preContainer == null
                || postContainer == null
                || typeField == null
                || typeColor == null
                || parentContainer == null
                || keyFoldoutScrollView == null;
        }

        #region [IHasCustomMenu Implementation]
        /// <summary>
        /// Adds your custom menu items to an Editor Window.
        /// </summary>
        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Locked"), isLocked, () => isLocked = !isLocked);
        }
        #endregion

        #region [Static Methods]
        //[MenuItem("Tools/AI Tree/Windows/Key Details", false, 23)]
        public static void Open()
        {
            if (HasOpenInstances())
            {
                KeyDetailsWindow[] windows = GetInstances();
                for (int i = 0; i < windows.Length; i++)
                {
                    KeyDetailsWindow window = windows[i];
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
        /// Check if has open instances of Key Details window.
        /// </summary>
        /// <returns></returns>
        public static bool HasOpenInstances()
        {
            return Instances.Count > 0;
        }

        /// <summary>
        /// Get all open Key Details window instances.
        /// </summary>
        /// <returns>System array of open instances.</returns>
        public static KeyDetailsWindow[] GetInstances()
        {
            return Instances.ToArray();
        }

        /// <summary>
        /// Notify all instance of Key Details window, to track specified key editor.
        /// </summary>
        /// <param name="key">Key reference.</param>
        /// <param name="blackboard">Blackboard reference.</param>
        public static void NotifyTrackEditor(Key key, Blackboard blackboard)
        {
            if (Instances.Count == 0)
            {
                KeyDetailsWindow window = CreateWindow();
                window.TrackEditor(key, blackboard);
                window.MoveToCenter();
                window.Show();
            }
            else
            {
                foreach (KeyDetailsWindow window in Instances)
                {
                    window.TrackEditor(key, blackboard);
                }
            }
        }

        /// <summary>
        /// Create new instance of Key Details window.
        /// </summary>
        internal static KeyDetailsWindow CreateWindow()
        {
            KeyDetailsWindow window = CreateInstance<KeyDetailsWindow>();
            window.minSize = new Vector2(290, 205);
            window.MoveToCenter();
            window.Show();
            return window;
        }
        #endregion

        #region [Getter / Setter]
        public Key GetKey()
        {
            return key;
        }

        public Blackboard GetBlackboard()
        {
            return blackboard;
        }
        #endregion
    }
}