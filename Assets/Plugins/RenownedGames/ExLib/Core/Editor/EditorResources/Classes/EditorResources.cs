/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RenownedGames.ExLibEditor
{
    [InitializeOnLoad]
    public static class EditorResources
    {
        private static string[] Paths;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static EditorResources()
        {
            RequsetUpdatePaths();
        }

        /// <summary>
        /// Returns the first asset object of type type at given path assetPath.
        /// </summary>
        /// <param name="assetPath">Relative path of the asset to load.</param>
        /// <param name="type">Data type of the asset.</param>
        /// <returns>The asset matching the parameters.</returns>
        public static Object Load(string assetPath, Type type)
        {
            for (int i = 0; i < Paths.Length; i++)
            {
                string path = Paths[i];
                path = Path.Combine(path, assetPath);
                if (Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Object asset = AssetDatabase.LoadAssetAtPath(path, type);
                    if (asset != null)
                    {
                        return asset;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the asset object of type type at given path assetPath.
        /// </summary>
        /// <param name="directory">Exact directory at path of editor resources folder.</param>
        /// <param name="assetPath">Relative path of the asset to load.</param>
        /// <param name="type">Data type of the asset.</param>
        /// <returns>The asset matching the parameters.</returns>
        public static Object LoadExact(string directory, string assetPath, Type type)
        {
            string path = GetExactPath(directory, Paths);
            if (!string.IsNullOrEmpty(path))
            {
                return AssetDatabase.LoadAssetAtPath(Path.Combine(path, assetPath), type);
            }
            return null;
        }

        /// <summary>
        /// Returns the first asset object of type type at given path assetPath.
        /// </summary>
        /// <param name="assetPath">Relative path of the asset to load.</param>
        /// <returns>The asset matching the parameters.</returns>
        public static T Load<T>(string assetPath) where T : Object
        {
            for (int i = 0; i < Paths.Length; i++)
            {
                string path = Paths[i];
                path = Path.Combine(path, assetPath);
                if (Directory.Exists(Path.GetDirectoryName(path)))
                {
                    T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (asset != null)
                    {
                        return asset;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the asset object of type type at given path assetPath.
        /// </summary>
        /// <param name="directory">Exact directory at path of editor resources folder.</param>
        /// <param name="assetPath">Relative path of the asset to load.</param>
        /// <returns>The asset matching the parameters.</returns>
        public static T LoadExact<T>(string directory, string assetPath) where T : Object
        {
            string path = GetExactPath(directory, Paths);
            if (!string.IsNullOrEmpty(path))
            {
                return AssetDatabase.LoadAssetAtPath<T>(Path.Combine(path, assetPath));
            }
            return null;
        }

        /// <summary>
        /// Returns the array of the assets at given path.
        /// </summary>
        /// <param name="assetsPath">Relative path of the assets to load.</param>
        /// <returns>The asset matching the parameters.</returns>
        public static T[] LoadAll<T>(string assetsPath, SearchOption searchOption) where T : Object
        {
            List<T> assets = new List<T>();
            for (int i = 0; i < Paths.Length; i++)
            {
                string path = Paths[i];
                path = Path.Combine(path, assetsPath);
                if (Directory.Exists(path))
                {
                    string[] filePaths = Directory.GetFiles(path, "*", searchOption);
                    for (int j = 0; j < filePaths.Length; j++)
                    {
                        T asset = AssetDatabase.LoadAssetAtPath<T>(filePaths[j]);
                        if (asset != null)
                        {
                            assets.Add(asset);
                        }
                    }
                }
            }

            if (assets.Count > 0)
            {
                return assets.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Returns the array of the assets at given path.
        /// </summary>
        /// <param name="directory">Exact directory at path of editor resources folder.</param>
        /// <param name="assetsPath">Relative path of the assets to load.</param>
        /// <returns>The asset matching the parameters.</returns>
        public static T[] LoadAllExact<T>(string directory, string assetsPath, SearchOption searchOption) where T : Object
        {
            string path = GetExactPath(directory, Paths);
            if (!string.IsNullOrEmpty(path))
            {
                path = Path.Combine(path, assetsPath);
                if (Directory.Exists(path))
                {
                    List<T> assets = new List<T>();
                    string[] filePaths = Directory.GetFiles(path, "*", searchOption);
                    for (int j = 0; j < filePaths.Length; j++)
                    {
                        T asset = AssetDatabase.LoadAssetAtPath<T>(filePaths[j]);
                        if (asset != null)
                        {
                            assets.Add(asset);
                        }
                    }

                    if (assets.Count > 0)
                    {
                        return assets.ToArray();
                    }

                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Get all stored paths until editor resources, relative project.
        /// <br>Note: This method does not take into account folders created after previous compilation. See: <see cref="GetAllPaths" /></br>
        /// </summary>
        public static string[] GetAllPathsNonAlloc()
        {
            return Paths;
        }

        /// <summary>
        /// Get all paths until editor resources, relative project.
        /// <br>Note: This method starts a new recursive search for the project. See: <see cref="GetAllPathsNonAlloc" /></br>
        /// </summary>
        public static string[] GetAllPaths()
        {
            string[] paths = Directory.GetDirectories(Application.dataPath, "EditorResources", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = ProjectDatabase.GetRelativePath(paths[i]);
            }
            return paths;
        }

        /// <summary>
        /// Find the editor resource folder of a exact directory. 
        /// </summary>
        /// <param name="directory">Clarification by directory.</param>
        /// <param name="paths">All editor resource paths.</param>
        /// <returns>Project relative path to editor resource folder.</returns>
        public static string GetExactPath(string directory, string[] paths)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new IOException("Project directory name cannot be null or empty!");
            }

            if(paths == null || paths.Length == 0)
            {
                return string.Empty;
            }

            directory = directory.Replace('/', '\\');

            int bestIndex = int.MaxValue;
            int lastIndex = int.MaxValue;
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];

                if (path.Contains(directory))
                {
                    int index = path.IndexOf(directory);
                    if (index < lastIndex)
                    {
                        lastIndex = index;
                        bestIndex = i;
                    }
                }
            }

            if(bestIndex < paths.Length)
            {
                return paths[bestIndex];
            }

            return string.Empty;
        }

        /// <summary>
        /// Request to update stored paths.
        /// </summary>
        public static void RequsetUpdatePaths()
        {
            Paths = GetAllPaths();
        }
    }
}