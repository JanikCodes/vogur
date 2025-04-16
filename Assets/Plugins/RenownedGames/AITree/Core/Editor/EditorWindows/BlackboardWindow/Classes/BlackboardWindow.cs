/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using RenownedGames.AITreeEditor.UIElements;
using RenownedGames.ExLibEditor;
using RenownedGames.ExLibEditor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    public sealed class BlackboardWindow : EditorWindow, IHasCustomMenu
    {
        private static HashSet<BlackboardWindow> Instances;

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
        static BlackboardWindow()
        {
            Instances = new HashSet<BlackboardWindow>();
        }

        // Stored required properties.
        private bool isLocked;
        private int lastRunnerID;
        private string searchText;
        private Styles styles;
        private Blackboard blackboard;

        // Stored required elements.
        private ToolbarButton toolbarNewKey;
        private Label blackboardName;
        private TextField searchInput;
        private VisualElement searchPlaceholder;
        private ListElement inheritedKeyList;
        private ListElement keyList;
        private Foldout inheritKeysFoldout;

        private Event keyboardEvent;

        private Key selectedKey;
        private VisualElement selectedElement;

        /// <summary>
        /// Called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            Instances.Add(this);
            Instances.TrimExcess();

            Texture2D icon = EditorResources.Load<Texture2D>("Images/Icons/Window/BlackboardIcon.png");
            titleContent = new GUIContent("Blackboard", icon);

            styles = new Styles();

            LoadVisualElements();
            RefreshKeys();

            if (blackboard != null)
            {
                TrackEditor(blackboard);
            }
            else
            {
                OnSelectionChange();
            }

            EditorApplication.update -= CheckDeleteCallback;
            EditorApplication.update += CheckDeleteCallback;

            Undo.undoRedoPerformed -= OnUndoRedo;
            Undo.undoRedoPerformed += OnUndoRedo;

            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
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
            keyboardEvent = Event.current;

            searchPlaceholder.style.display = string.IsNullOrEmpty(searchInput.text) ? DisplayStyle.Flex : DisplayStyle.None;
            inheritedKeyList.style.display = blackboard != null ? DisplayStyle.Flex : DisplayStyle.None;
            keyList.style.display = blackboard != null ? DisplayStyle.Flex : DisplayStyle.None;

            if (searchText != searchInput.text)
            {
                searchText = searchInput.text;
                RefreshKeys();
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            EditorApplication.update -= CheckDeleteCallback;
        }

        /// <summary>
        /// Start tracking specified blackboard.
        /// </summary>
        /// <param name="target">Blackboard reference.</param>
        public void TrackEditor(Blackboard target)
        {
            if (HasUnloadedVisualElements())
            {
                LoadVisualElements();
            }

            if (!isLocked || (blackboard == null && target != null))
            {
                if (target != null)
                {
                    blackboard = target;
                    blackboardName.text = ObjectNames.NicifyVariableName(blackboard.name);
                    blackboard.InitializeSelfKey();
                    RefreshKeys();
                }
                else
                {
                    const string DEFAULT_NAME = "BLACKBOARD";
                    blackboardName.text = DEFAULT_NAME;
                    inheritKeysFoldout.style.display = DisplayStyle.None;
                    keyList.style.display = DisplayStyle.None;
                    inheritedKeyList.ClearItems();
                    keyList.ClearItems();
                    Repaint();
                }
            }
        }

        /// <summary>
        /// Refresh all key instance.
        /// </summary>
        public void RefreshKeys()
        {
            if(blackboard != null)
            {
                ClearNullKeys();
                InitializeInheritKeys();
                InitializeKeys();
                Repaint();
            }
        }

        /// <summary>
        /// Show new key creation search window.
        /// </summary>
        public void ShowKeyCreationWindow()
        {
            if (blackboard == null)
            {
                if (EditorUtility.DisplayDialog("AI Tree", "Blackboard is not selected, select Blackboard or create a new one.", "Create", "Cancel"))
                {
                    Texture2D icon = EditorResources.LoadExact<Texture2D>("RenownedGames/AITree", "Images/Icons/ScriptableObject/BlackboardIcon.png");
                    ProjectWindowUtility.CreateScriptableObject<Blackboard>("NewBlackboard", icon);
                }
                return;
            }

            ExSearchWindow searchWindow = ExSearchWindow.Create("Nodes");

            foreach (Type type in TypeCache.GetTypesDerivedFrom<Key>())
            {
                if (type.IsAbstract || type.IsGenericType || type == typeof(SelfKey)) continue;

                string name = type.Name;

                const string KEY_SUFFIX = "Key";
                if (name.EndsWith(KEY_SUFFIX, StringComparison.OrdinalIgnoreCase))
                {
                    int index = name.LastIndexOf(KEY_SUFFIX);
                    name = name.Remove(index, KEY_SUFFIX.Length);
                }

                if (type.GetCustomAttribute<ObsoleteAttribute>() != null)
                {
                    name = $"Deprecated/{name}";
                }

                searchWindow.AddEntry(new GUIContent(name), () =>
                {
                    BlackboardUtility.AddKey(blackboard, type);

                    foreach (BlackboardWindow window in Instances)
                    {
                        if (window.blackboard == blackboard)
                        {
                            window.RefreshKeys();
                        }
                    }
                });
            }

            Rect buttonRect = toolbarNewKey.contentRect;
            buttonRect.x += 88;
            buttonRect.y += 2 + buttonRect.height;
            searchWindow.Open(buttonRect);
        }

        /// <summary>
        /// Initialize inherit blackboard keys.
        /// </summary>
        private void InitializeInheritKeys()
        {
            inheritKeysFoldout.style.display = DisplayStyle.None;
            inheritedKeyList.ClearItems();

            if (blackboard.GetParent() != null)
            {
                inheritKeysFoldout.style.display = DisplayStyle.Flex;

                if (blackboard.GetParent().GetKeys().Count > 0)
                {
                    inheritedKeyList.style.display = DisplayStyle.Flex;

                    foreach (Key key in blackboard.GetParent().GetKeys())
                    {
                        if (key.name.ToLower().Contains(searchInput.text.ToLower()))
                        {
                            string name = key.name;
                            if (key.GetType().GetCustomAttribute<ObsoleteAttribute>() != null)
                            {
                                name += " (Deprecated)";
                            }

                            ListItem item = new ListItem(name, key.GetCategory(), key.GetDescription(), key.GetColor(), 
                                
                            (element) =>
                            {
                                OnSelectKey(key, element);
                                BlackboardDetailsWindow.NotifyTrackEditor(key, blackboard);
#pragma warning disable CS0618 // Key Details window is obsolete
                                if (KeyDetailsWindow.HasOpenInstances())
                                {
                                    KeyDetailsWindow.NotifyTrackEditor(key, blackboard);
                                }
#pragma warning restore CS0618 // Key Details window is obsolete
                            }, 
                            
                            (element) =>
                            {
                                if (key is not SelfKey)
                                {
                                    if (BlackboardUtility.DeleteKey(blackboard, key))
                                    {
                                        selectedKey = null;
                                        selectedElement = null;

                                        RefreshKeys();
                                    }
                                }
                                else
                                {
                                    EditorUtility.DisplayDialog("AI Tree", "Self is a system key and cannot be deleted.", "Ok");
                                }
                            });

                            inheritedKeyList.AddItem(item);
                        }
                    }
                }
            }

            inheritedKeyList.Initialize();
        }

        /// <summary>
        /// Initialize blackboard keys.
        /// </summary>
        private void InitializeKeys()
        {
            keyList.style.display = DisplayStyle.None;
            keyList.ClearItems();

            if (blackboard.GetKeys().Count > 0)
            {
                keyList.style.display = DisplayStyle.Flex;

                foreach (Key key in blackboard.GetKeys())
                {
                    if (key.name.ToLower().Contains(searchInput.text.ToLower()))
                    {
                        string name = key.name;
                        if (key.GetType().GetCustomAttribute<ObsoleteAttribute>() != null)
                        {
                            name += " (Deprecated)";
                        }

                        ListItem item = new ListItem(name, key.GetCategory(), key.GetDescription(), key.GetColor(), (element) =>
                        {
                            OnSelectKey(key, element);
                            BlackboardDetailsWindow.NotifyTrackEditor(key, blackboard);
#pragma warning disable CS0618 // Key Details window is obsolete
                            if (KeyDetailsWindow.HasOpenInstances())
                            {
                                KeyDetailsWindow.NotifyTrackEditor(key, blackboard);
                            }
#pragma warning restore CS0618 // Key Details window is obsolete
                        },
                        (element) =>
                        {
                            if (key is not SelfKey)
                            {
                                if (BlackboardUtility.DeleteKey(blackboard, key))
                                {
                                    selectedKey = null;
                                    selectedElement = null;

                                    RefreshKeys();
                                }
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("AI Tree", "Self is a system key and cannot be deleted.", "Ok");
                            }
                        });

                        keyList.AddItem(item);
                    }
                }
            }

            keyList.Initialize();
        }

        /// <summary>
        /// Clear null blackboard asset keys.
        /// </summary>
        private void ClearNullKeys()
        {
            bool hasNull = false;

            SerializedObject serializedObject = new SerializedObject(blackboard);
            SerializedProperty keys = serializedObject.FindProperty("keys");
            for (int i = 0; i < keys.arraySize; i++)
            {
                SerializedProperty key = keys.GetArrayElementAtIndex(i);
                if(key.objectReferenceValue == null)
                {
                    keys.DeleteArrayElementAtIndex(i);
                    i--;
                    hasNull = true;
                }
            }

            if (hasNull)
            {
                serializedObject.ApplyModifiedProperties();
                EditorApplication.delayCall += () =>
                {
                    EditorUtility.DisplayDialog("AI Tree Blackboard", 
                        "Some types of keys that were contained in this blackboard were removed from the project or renamed.\n\n" +
                        "Undefined keys were automatically deleted, but references to their objects remained in the blackboard asset file.\n\n" +
                        "Expand the blackboard asset (the arrow to the right of the asset file) and manually delete the extra key assets.",
                        "Ok");
                };
            }
        }

        /// <summary>
        /// Called when you click on the list item.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="visualElement">VisualElement.</param>
        private void OnSelectKey(Key key, VisualElement visualElement)
        {
            if (selectedElement != null)
            {
                selectedElement.RemoveFromClassList("selected");
            }
            selectedElement = visualElement;
            selectedElement.AddToClassList("selected");

            selectedKey = key;
        }

        /// <summary>
        /// Called whenever the selection has changed.
        /// </summary>
        private void OnSelectionChange()
        {
            if (Selection.activeObject is Blackboard activeBlackboard)
            {
                lastRunnerID = -1;
                TrackEditor(activeBlackboard);
            } 
            else if (Selection.activeObject is BehaviourTree behaviourTree)
            {
                lastRunnerID = -1;
                TrackEditor(behaviourTree.GetBlackboard());
            } 
            else if (Selection.activeGameObject != null && Selection.activeGameObject.TryGetComponent<BehaviourRunner>(out BehaviourRunner runner))
            {
                lastRunnerID = runner.GetInstanceID();
                Blackboard blackboard = EditorApplication.isPlaying ? runner.GetBlackboard() : runner.GetSharedBlackboard();
                TrackEditor(blackboard);
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

            VisualTreeAsset visualTree = settings.GetBlackboardUXML();
            visualTree.CloneTree(rootVisualElement);
            rootVisualElement.styleSheets.Add(settings.GetBlackboardUSS());

            toolbarNewKey = rootVisualElement.Q<ToolbarButton>("toolbar-new-key");
            blackboardName = rootVisualElement.Q<Label>("blackboard-name");
            searchInput = rootVisualElement.Q<TextField>("search-input");
            searchPlaceholder = rootVisualElement.Q<VisualElement>("search-placeholder");
            inheritedKeyList = rootVisualElement.Q<ListElement>("inherited-keys-list");
            keyList = rootVisualElement.Q<ListElement>("keys-list");
            inheritKeysFoldout = rootVisualElement.Q<Foldout>("inherited-keys-foldout");

            toolbarNewKey.clicked -= ShowKeyCreationWindow;
            toolbarNewKey.clicked += ShowKeyCreationWindow;
        }

        /// <summary>
        /// Check if window has new or unloaded visual elements.
        /// </summary>
        /// <returns>True if has new or unloaded visual elements, otherwise false.</returns>
        private bool HasUnloadedVisualElements()
        {
            return toolbarNewKey == null
                || blackboardName == null
                || searchInput == null
                || searchPlaceholder == null
                || inheritedKeyList == null
                || keyList == null
                || inheritKeysFoldout == null;
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

        #region [Callbacks]
        /// <summary>
        /// Called when undo/redo.
        /// </summary>
        private void OnUndoRedo()
        {
            RefreshKeys();
            if (blackboard != null)
            {
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(blackboard));
            }
        }

        /// <summary>
        /// Check delete callback.
        /// </summary>
        private void CheckDeleteCallback()
        {
            if (keyboardEvent == null)
            {
                return;
            }

            if (focusedWindow == null)
            {
                return;
            }

            Type focusedWindowType = focusedWindow.GetType();
#pragma warning disable CS0618 // Key Details window is obsolete
            if (focusedWindowType == typeof(BlackboardWindow) || focusedWindowType == typeof(BlackboardWindow) || focusedWindowType == typeof(KeyDetailsWindow))
#pragma warning restore CS0618 // Key Details window is obsolete
            {
                if ((keyboardEvent.keyCode == KeyCode.Delete || (keyboardEvent.control && keyboardEvent.keyCode == KeyCode.Backspace)) && selectedKey != null)
                {
                    if (selectedKey is not SelfKey)
                    {
                        if (BlackboardUtility.DeleteKey(blackboard, selectedKey))
                        {
                            selectedKey = null;
                            selectedElement = null;

                            foreach (BlackboardWindow window in Instances)
                            {
                                if(window.blackboard == blackboard)
                                {
                                    window.RefreshKeys();
                                }
                            }
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("AI Tree", "Self is a system key and cannot be deleted.", "Ok");
                    }
                }
            }
        }

        /// <summary>
        /// Called when play mode state change.
        /// </summary>
        /// <param name="state">Enumeration specifying a change in the Editor's play mode state. See Also: PauseState, EditorApplication.playModeStateChanged, EditorApplication.isPlaying.</param>
        private void OnPlayModeChanged(PlayModeStateChange state)
        {
            BehaviourRunner runner = EditorUtility.InstanceIDToObject(lastRunnerID) as BehaviourRunner;
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                if (runner != null)
                {
                    TrackEditor(runner.GetBlackboard());
                }
            }
            else if(state == PlayModeStateChange.EnteredEditMode)
            {
                if (runner != null)
                {
                    TrackEditor(runner.GetSharedBlackboard());
                }
            }
        }
        #endregion

        #region [Static Methods]
        [MenuItem("Tools/AI Tree/Windows/Blackboard", false, 22)]
        public static void Open()
        {
            if (HasOpenInstances())
            {
                BlackboardWindow[] windows = GetInstances();
                for (int i = 0; i < windows.Length; i++)
                {
                    BlackboardWindow window = windows[i];
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
        /// Check if has open instances of Blackboard window.
        /// </summary>
        /// <returns></returns>
        public static bool HasOpenInstances()
        {
            return Instances.Count > 0;
        }

        /// <summary>
        /// Get all open Blackboard window instances.
        /// </summary>
        /// <returns>System array of open instances.</returns>
        public static BlackboardWindow[] GetInstances()
        {
            return Instances.ToArray();
        }

        /// <summary>
        /// Notify all instance of Blackboard window, to track specified key editor.
        /// </summary>
        /// <param name="blackboard">Blackboard reference.</param>
        public static void NotifyTrackEditor(Blackboard blackboard)
        {
            if (Instances.Count == 0)
            {
                BlackboardWindow window = CreateWindow();
                window.TrackEditor(blackboard);
                window.MoveToCenter();
                window.Show();
            }
            else
            {
                foreach (BlackboardWindow window in Instances)
                {
                    window.TrackEditor(blackboard);
                }
            }
        }

        /// <summary>
        /// Create new instance of Blackboard window.
        /// </summary>
        internal static BlackboardWindow CreateWindow()
        {
            BlackboardWindow window = CreateInstance<BlackboardWindow>();
            window.minSize = new Vector2(290, 205);
            window.minSize = new Vector2(250, 90);
            window.Show();
            window.OnSelectionChange();
            return window;
        }

        [OnOpenAsset]
        private static bool OnOpenAsset(int instanceId, int line)
        {
            UnityEngine.Object asset = EditorUtility.InstanceIDToObject(instanceId);
            if (asset is Blackboard)
            {
                Open();
                return true;
            }
            return false;
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

        public Blackboard GetBlackboard()
        {
            return blackboard;
        }

        public string GetSearchText()
        {
            return searchText;
        }

        public void SetSearchText(string value)
        {
            searchText = value;
        }
        #endregion
    }
}