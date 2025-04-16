/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.AITree
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class NodeContentAttribute : Attribute
    {
        public readonly string name;
        public readonly string path;

        /// <summary>
        /// Path of node in search window, last path is name of node.
        /// </summary>
        public NodeContentAttribute(string path)
        {
            this.path = path;
            name = string.Empty;
            Hide = false;
            IconPath = string.Empty;
        }

        /// <summary>
        /// Path of node in search window, with custom node name.
        /// </summary>
        public NodeContentAttribute(string name, string path) : this(path)
        {
            this.name = name;
        }

        #region [Optional Parameters]
        /// <summary>
        /// Hide node in search window.
        /// </summary>
        public bool Hide { get; set; }

        /// <summary>
        /// Icon path of node.
        /// <br>The path relative to the EditorResources folder.</br>
        /// </summary>
        public string IconPath;
        #endregion
    }
}