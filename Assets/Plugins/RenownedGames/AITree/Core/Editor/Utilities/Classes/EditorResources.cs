/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.IO;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor.Utilities
{
    [System.Obsolete("Use ExlibEditor.EditorResources instead.")]
    public static class EditorResources
    {
        private const string RESOURCE_PATH = "Core/Editor/EditorResources";

        public static Object Load(string path, System.Type type)
        {
            string assetPath = Path.Combine(GetRelativePath(), path);
            return AssetDatabase.LoadAssetAtPath(assetPath, type);
        }

        public static T Load<T>(string path) where T : Object
        {
            string assetPath = Path.Combine(GetRelativePath(), path);
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        public static Object[] LoadAll(string path)
        {
            string assetsPath = Path.Combine(GetRelativePath(), path);
            return AssetDatabase.LoadAllAssetsAtPath(assetsPath);
        }

        public static string GetRelativePath()
        {
            return string.Empty;
        }
    }
}