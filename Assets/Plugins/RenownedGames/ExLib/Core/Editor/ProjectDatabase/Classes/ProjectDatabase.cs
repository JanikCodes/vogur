/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.ExLibEditor
{
    public static class ProjectDatabase
    {
        /// <summary>
        /// Load all assets type of T.
        /// </summary>
        /// <typeparam name="T">Type of UnityEngine.Object</typeparam>
        public static T[] LoadAll<T>() where T : Object
        {
            return LoadAll<T>("*");
        }

        /// <summary>
        /// Load all assets type of T.
        /// </summary>
        /// <typeparam name="T">Type of UnityEngine.Object</typeparam>
        /// <param name="searchPattern">
        /// The search string to match against the names of files in path. 
        /// This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, 
        /// but it doesn't support regular expressions.
        /// </param>
        /// <returns>Array of all loaded asset type of (T)</returns>
        public static T[] LoadAll<T>(string searchPattern) where T : Object
        {
            List<T> assets = new List<T>();
            string[] paths = Directory.GetFiles(Application.dataPath, searchPattern, SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                T asset = AssetDatabase.LoadAssetAtPath<T>(GetRelativePath(path));
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets.ToArray();
        }

        /// <summary>
        /// Go through all assets type of T in project.
        /// </summary>
        /// <typeparam name="T">Type of UnityEngine.Object</typeparam>
        /// <param name="searchPattern">
        /// The search string to match against the names of files in path. 
        /// This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, 
        /// but it doesn't support regular expressions.
        /// </param>
        /// <returns></returns>
        public static IEnumerable<T> EnumerateAssets<T>(string searchPattern) where T : Object
        {
            IEnumerable<string> paths = Directory.EnumerateFiles(Application.dataPath, searchPattern, SearchOption.AllDirectories);
            foreach (string path in paths)
            {
                string relativePath = GetRelativePath(path);
                T asset = AssetDatabase.LoadAssetAtPath<T>(relativePath);
                if (asset != null)
                {
                    yield return asset;
                }
            }
        }

        /// <summary>
        /// Go through all assets type of (T).
        /// </summary>
        public static IEnumerable<T> EnumerateAssets<T>() where T : Object
        {
            return EnumerateAssets<T>("*");
        }

        /// <summary>
        /// Get relative path to project /Assets folder.
        /// </summary>
        /// <param name="path">Absolute path to asset.</param>
        public static string GetRelativePath(string path)
        {
            return path.Remove(0, path.IndexOf("Assets"));
        }
    }
}