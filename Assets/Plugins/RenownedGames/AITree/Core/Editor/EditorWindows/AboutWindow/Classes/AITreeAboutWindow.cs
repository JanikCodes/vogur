/* ==================================================================
  ----------------------------------------------------------------
  Project   :   AI Tree
  Publisher :   Renowned Games
  Developer :   Tamerlan Shakirov
  ----------------------------------------------------------------
  Copyright 2022-2023 Renowned Games All rights reserved.
  ================================================================== */

using RenownedGames.ExLibEditor;
using RenownedGames.ExLibEditor.Windows;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    public sealed class AITreeAboutWindow : AboutWindow
    {
        /// <summary>
        /// Implement this method to add project name.
        /// </summary>
        protected override void InitializeProjectName(out string projectName)
        {
            projectName = "AI Tree";
        }

        /// <summary>
        /// Implement this method to add version label.
        /// </summary>
        protected override void InitializeVersion(out string version)
        {
            version = "Version: 1.9.1";
        }

        /// <summary>
        /// Implement this method to add all the people involved in the development.
        /// </summary>
        protected override void InitializeDevelopers(out Developer[] developers)
        {
            developers = new Developer[4]
            {
                new Developer("Publisher: ", "Renowned Games"),
                new Developer("Creative Director: ", "Tamerlan Shakirov"),
                new Developer("Lead Programmer: ", "Tamerlan Shakirov"),
                new Developer("Programmers: ", "Zinnur Davleev\nVladimir Deryabin\nIlnur Mukhametkhanov"),
            };
        }

        /// <summary>
        /// Implement this method to add logotype.
        /// </summary>
        public override void InitializeLogotype(out Texture2D texture, out float width, out float height)
        {
            texture = EditorResources.Load<Texture2D>("Images/Logotype/AITree_420x280.png");
            width = 186;
            height = 124;
        }

        /// <summary>
        /// Implement this method to add copyright.
        /// </summary>
        protected override void InitializeCopyright(out string copyright)
        {
            copyright = "Copyright 2024 Renowned Games All rights reserved.";
        }

        /// <summary>
        /// Implement this method to add publisher link button.
        /// </summary>
        protected override void InitializePublisherLink(out string url)
        {
            url = "https://assetstore.unity.com/publishers/26774";
        }

        [MenuItem("Tools/AI Tree/About", false, 0)]
        public static void Open()
        {
            Open<AITreeAboutWindow>(new GUIContent("About"), new Vector2(480, 200));
        }
    }
}