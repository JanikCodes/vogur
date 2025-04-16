/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.ApexEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    sealed class AITreeProvider : SettingsProvider
    {
        private AITreeSettings settings;
        private AEditor editor;

        /// <summary>
        /// AITreeProvider constructor.
        /// </summary>
        /// <param name="path">Path used to place the SettingsProvider in the tree view of the Settings window. The path should be unique among all other settings paths and should use "/" as its separator.</param>
        /// <param name="scopes">Scope of the SettingsProvider. The Scope determines whether the SettingsProvider appears in the Preferences window (SettingsScope.User) or the Settings window (SettingsScope.Project).</param>
        /// <param name="keywords">List of keywords to compare against what the user is searching for. When the user enters values in the search box on the Settings window, SettingsProvider.HasSearchInterest tries to match those keywords to this list.</param>
        public AITreeProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords) { }

        /// <summary>
        /// Use this function to implement a handler for when the user clicks on the Settings in the Settings window. You can fetch a settings Asset or set up UIElements UI from this function.
        /// </summary>
        /// <param name="searchContext">Search context in the search box on the Settings window.</param>
        /// <param name="rootElement">Root of the UIElements tree. If you add to this root, the SettingsProvider uses UIElements instead of calling SettingsProvider.OnGUI to build the UI. If you do not add to this VisualElement, then you must use the IMGUI to build the UI.</param>
        public override void OnActivate(string searchContext, UnityEngine.UIElements.VisualElement rootElement)
        {
            settings = AITreeSettings.instance;
            settings.advanced = false;
            settings.hideFlags &= ~HideFlags.NotEditable;
            editor = (AITreeSettingsEditor)Editor.CreateEditor(settings, typeof(AITreeSettingsEditor));
        }

        /// <summary>
        /// Use this function to implement a handler for when the user clicks
        /// on another setting or when the Settings window closes.
        /// </summary>
        public override void OnDeactivate()
        {
            if (settings != null)
            {
                settings.hideFlags |= HideFlags.NotEditable;
            }
        }

        /// <summary>
        /// Use this function to override drawing the title for the SettingsProvider using IMGUI. This allows you to add custom UI (such as a toolbar button) next to the title. 
        /// AssetSettingsProvider uses this mechanism to display the "add to preset" and the "help" buttons.
        /// </summary>
        public override void OnTitleBarGUI()
        {
            Rect position = GUILayoutUtility.GetRect(0, 0);
            position.x -= 19.0f;
            position.y += 6.0f;
            position.width = 20;
            position.height = 20;

            Rect popupPosition = new Rect(position.x, position.y, position.width, position.height);
            if (GUI.Button(popupPosition, EditorGUIUtility.IconContent("_Popup"), "IconButton"))
            {
                GenericMenu popupMenu = new GenericMenu();
                popupMenu.AddItem(new GUIContent("Reset", "Reset setting to default."), false, settings.Reset);
                popupMenu.AddItem(new GUIContent("Advanced", "Reset setting to default."), settings.advanced, () => settings.advanced = !settings.advanced);

                Rect dropdownPosition = new Rect(popupPosition.x - 88, popupPosition.y, popupPosition.width, popupPosition.height);
                popupMenu.DropDown(dropdownPosition);
            }
        }


        /// <summary>
        /// Use this function to draw the UI based on IMGUI. This assumes you haven't added any children to the rootElement passed to the OnActivate function.
        /// </summary>
        /// <param name="searchContext">Search context for the Settings window. Used to show or hide relevant properties.</param>
        public override void OnGUI(string searchContext)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(13);
            GUILayout.BeginVertical();
            GUILayout.Space(9);
            EditorGUIUtility.labelWidth = 248;
            if (settings != null)
            {
                editor.OnInspectorGUI();
            }
            GUILayout.EndVertical();
            GUILayout.Space(3);
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Register AITreeProvider in Project Settings window.
        /// </summary>
        /// <returns>New AITreeProvider instance.</returns>
        [SettingsProvider]
        public static SettingsProvider Register()
        {
            return new AITreeProvider("Preferences/Renowned Games/AI Tree", SettingsScope.User, Keywords);
        }

        /// <summary>
        /// AI Tree setting provider keywords.
        /// <br>
        /// When the user enters values in the search box on the Settings window, 
        /// SettingsProvider.HasSearchInterest tries to match those keywords to this list.
        /// </br>
        /// </summary>
        public static IEnumerable<string> Keywords
        {
            get
            {
                yield return "Renowned Games";
                yield return "AI";
                yield return "Behaviour";
                yield return "BT";
                yield return "Tree";
            }
        }

    }
}