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
using RenownedGames.ExLibEditor.Windows;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    [Obsolete("Use BlackboardViewerWindow instead.")]
    public sealed class BlackboardViewer : EditorWindow, IHasCustomMenu
    {
        private static GUIStyle Placeholder;
        private static GUIStyle LockButtonStyle;

        // Stored required properties.
        private List<SerializedField> fields;
        private SearchField searchField;
        private Blackboard selected;
        private Vector2 scrollPos;
        private string searchText;
        private bool isLocked;
        private float contentHeight;

        /// <summary>
        /// Called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            titleContent = new GUIContent("Blackboard Viewer (Obsolete)");

            fields = new List<SerializedField>();
            searchField = new SearchField();
            searchText = string.Empty;

            OnSelectionChange();

            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        /// <summary>
        /// Called for rendering and handling GUI events.
        /// </summary>
        private void OnGUI()
        {
            Rect rect = new Rect(0, 0, position.width, position.height);

            EditorGUI.BeginDisabledGroup(fields.Count == 0);
            Rect searchPosition = new Rect(4, 2, rect.width - 8, EditorGUIUtility.singleLineHeight);
            searchText = searchField.OnGUI(searchPosition, searchText);
            EditorGUI.EndDisabledGroup();

            if (string.IsNullOrEmpty(searchText))
            {
                searchPosition.x += 12;
                searchPosition.y -= 1;
                GUI.Label(searchPosition, "Name", GetPlaceholderStyle());
            }

            const float BOX_HEIGHT = 25;
            Rect boxPosition = new Rect(0, searchPosition.yMax + 1, rect.width, BOX_HEIGHT);
            GUI.Box(boxPosition, GUIContent.none, ApexStyles.BoxEntryEven);

            EditorGUI.BeginDisabledGroup(isLocked);
            Rect fieldPosition = new Rect(4, boxPosition.y + 3, boxPosition.width - 8, EditorGUIUtility.singleLineHeight);
            SelectBlackboard((Blackboard)EditorGUI.ObjectField(fieldPosition, "Blackboard", selected, typeof(Blackboard), false));
            EditorGUI.EndDisabledGroup();

            rect.y = boxPosition.yMax;
            rect.height -= boxPosition.yMax;

            Rect viewRect = new Rect(0, rect.y, rect.width, contentHeight);
            if (rect.height <= contentHeight)
            {
                viewRect.width -= 20;
                boxPosition.width -= 13;
                fieldPosition.width -= 13;
            }

            scrollPos = GUI.BeginScrollView(rect, scrollPos, viewRect);
            {
                if (selected != null && fields.Count > 0)
                {
                    contentHeight = 0;
                    int index = 0;
                    for (int i = 0; i < fields.Count; i++)
                    {
                        SerializedField field = fields[i];

                        if (string.IsNullOrEmpty(searchText) || field.GetLabel().text.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        {
                            float height = field.GetHeight();

                            boxPosition.y = boxPosition.yMax - 1;
                            boxPosition.height = height + 6;
                            GUI.Box(boxPosition, GUIContent.none, index % 2 == 0 ? ApexStyles.BoxEntryOdd : ApexStyles.BoxEntryEven);

                            EditorGUI.BeginChangeCheck();
                            fieldPosition.y = boxPosition.y + 3;
                            fieldPosition.height = height;
                            field.GetSerializedObject().Update();
                            field.OnGUI(fieldPosition);
                            if (EditorGUI.EndChangeCheck())
                            {
                                BreakEditing();
                            }
                            index++;
                        }
                    }
                }
            }
            GUI.EndScrollView();

            contentHeight = boxPosition.yMax - viewRect.y;
        }

        /// <summary>
        /// Called at 10 frames per second to give the inspector a chance to update.
        /// </summary>
        private void OnInspectorUpdate()
        {
            Repaint();
        }

        /// <summary>
        /// Called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }

        /// <summary>
        /// Called when the selection changes.
        /// </summary>
        private void OnSelectionChange()
        {
            if (Selection.activeObject is Blackboard blackboard)
            {
                SelectBlackboard(blackboard);
            }
            else if (Selection.activeGameObject != null && Selection.activeGameObject.TryGetComponent<BlackboardInstance>(out BlackboardInstance instance))
            {
                blackboard = instance.GetBlackboard();
                if (blackboard == null)
                {
                    blackboard = instance.GetSharedBlackboard();
                }

                SelectBlackboard(blackboard);
            }
        }

        /// <summary>
        /// Magic method which Unity detects automatically.
        /// </summary>
        /// <param name="position">Position of button.</param>
        private void ShowButton(Rect position)
        {
            if (LockButtonStyle == null)
            {
                LockButtonStyle = "IN LockButton";
            }
            isLocked = GUI.Toggle(position, isLocked, GUIContent.none, LockButtonStyle);
        }

        /// <summary>
        /// Selects a blackboard.
        /// </summary>
        private void SelectBlackboard(Blackboard blackboard)
        {
            if (blackboard == null)
            {
                selected = null;
                fields.Clear();
                fields.Capacity = 0;
                return;
            }

            if (!Application.isPlaying && !AssetDatabase.IsNativeAsset(blackboard))
            {
                return;
            }

            if (!isLocked || selected == null)
            {
                selected = blackboard;
            }

            fields = new List<SerializedField>();
            foreach (Key key in selected.Keys)
            {
                SerializedObject serialziedObject = new SerializedObject(key);
                SerializedField field = new SerializedField(serialziedObject, "value");
                field.SetLabel(new GUIContent(key.name));
                fields.Add(field);
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
            menu.AddItem(new GUIContent("Lock"), isLocked, () =>
            {
                isLocked = !isLocked;
            });
        }
        #endregion

        #region [Static]
        //[MenuItem("Tools/AI Tree/Blackboard Viewer", false, 20)]
        private static void Open()
        {
            BlackboardViewer window = ScriptableObject.CreateInstance<BlackboardViewer>();
            window.titleContent = new GUIContent("Blackboard Viewer");
            window.Show();
            window.MoveToCenter();
        }

        /// <summary>
        /// Called when editor changed play mode state.
        /// </summary>
        /// <param name="mode">New play mode state.</param>
        private void OnPlayModeChanged(PlayModeStateChange mode)
        {
            if (mode == PlayModeStateChange.ExitingPlayMode && selected != null)
            {
                fields.Clear();
                fields.Capacity = 0;
                selected = null;
            }
        }

        /// <summary>
        /// Break focus from window and show message.
        /// </summary>
        internal static void BreakEditing()
        {
            EditorUtility.DisplayDialog("AI Tree", "You can't change key values through Blackboard Viewer.", "Close");
            GUI.FocusControl(null);
            EditorGUI.FocusTextInControl(null);
            GUIUtility.hotControl = 0;
            GUIUtility.keyboardControl = 0;
        }

        /// <summary>
        /// Get single instance of placeholder style.
        /// </summary>
        internal static GUIStyle GetPlaceholderStyle()
        {
            if (Placeholder == null)
            {
                Placeholder = new GUIStyle(GUI.skin.label);
                Placeholder.fontSize = 11;
                Placeholder.normal.textColor = Color.gray;
                Placeholder.active.textColor = Color.gray;
                Placeholder.focused.textColor = Color.gray;
                Placeholder.hover.textColor = Color.gray;
            }
            return Placeholder;
        }
        #endregion
    }
}