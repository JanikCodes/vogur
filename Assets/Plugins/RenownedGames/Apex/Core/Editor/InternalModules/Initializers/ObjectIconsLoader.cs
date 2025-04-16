/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using RenownedGames.ExLibEditor;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RenownedGames.ApexEditor
{
    [InitializeOnLoad]
    static class ObjectIconsLoader
    {
        /// <summary>
        /// Static constructor of ObjectIcons loader.
        /// </summary>
        static ObjectIconsLoader()
        {
            EditorApplication.delayCall += LoadObjectIcons;
        }

        /// <summary>
        /// Load and set icon for this object if its has [ObjectIcon] attribute with valid icon path.
        /// </summary>
        private static void LoadObjectIcons()
        {
            string[] guids = AssetDatabase.FindAssets("t:MonoScript");
            TypeCache.TypeCollection types = TypeCache.GetTypesWithAttribute<ObjectIconAttribute>();
            for (int i = 0; i < types.Count; i++)
            {
                Type type = types[i];
                if (type.IsAbstract)
                {
                    continue;
                }

                if (!type.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    Debug.LogWarning($"[ObjectIcon] attribute can be used only with subclasses of MonoBehaviour.");
                    continue;
                }

                ObjectIconAttribute attribute = type.GetCustomAttribute<ObjectIconAttribute>();

                Texture2D icon = EditorResources.Load<Texture2D>(attribute.path);
                if (icon != null)
                {
                    for (int j = 0; j < guids.Length; j++)
                    {
                        string guid = guids[j];
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        string name = System.IO.Path.GetFileNameWithoutExtension(path);
                        if (type.Name == name)
                        {
                            Object monoScript = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                            EditorGUIUtility.SetIconForObject(monoScript, icon);
                            if (attribute.hierarchyWindow)
                            {
                                EditorApplication.hierarchyWindowItemOnGUI = (instanceID, rect) =>
                                {
                                    DrawObjectIconInHierarhcy(rect, instanceID, type, icon);
                                };
                            }
                            break;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Object icon for <i>{ObjectNames.NicifyVariableName(type.Name)}</i> not found, check paths.");
                }
            }
        }

        /// <summary>
        /// Callback to draw object icon in hierarchy window rect.
        /// </summary>
        /// <param name="instanceID">InstanceID of object.</param>
        /// <param name="rect">Rectangle position of object.</param>
        /// <param name="type"></param>
        /// <param name="icon"></param>
        private static void DrawObjectIconInHierarhcy(Rect rect, int instanceID, Type type, Texture2D icon)
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject != null && gameObject.GetComponent(type) != null)
            {
                GUIContent content = new GUIContent(gameObject.name);
                Vector2 size = GUI.skin.label.CalcSize(content);
                if (rect.width < size.x + rect.height + 4)
                {
                    return;
                }

                rect.x = rect.xMax - 4;
                rect.width = rect.height;
                GUI.DrawTexture(rect, icon, ScaleMode.ScaleToFit);
            }
        }

    }
}
