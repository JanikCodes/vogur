/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RenownedGames.AITreeEditor
{
    public static class KeyEditorUtility
    {
        /// <summary>
        /// Get specified color of key in representation of RGBA colors in 32 bit format. 
        /// </summary>
        /// <param name="key">Key reference.</param>
        /// <returns>Representation of RGBA colors in 32 bit format.</returns>
        public static Color32 GetColor(this Key key)
        {
            KeyColorAttribute attribute = key.GetType().GetCustomAttribute<KeyColorAttribute>();
            if(attribute != null)
            {
                return new Color(attribute.r, attribute.g, attribute.b, attribute.a);
            }

            using (SHA256Managed sha = new SHA256Managed())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(key.GetValueType().Name);
                byte[] hashBytes = sha.ComputeHash(textBytes);
                return new Color32(hashBytes[1], hashBytes[2], hashBytes[3], 255);
            }
        }

        /// <summary>
        /// Make a displayable name for a key value type. 
        /// </summary>
        /// <param name="name">Value type of key..</param>
        /// <returns>Display name of key value type.</returns>
        public static string NicifyValueType(Type valueType)
        {
            const string SUFFIX = "Key";
            string name = valueType.Name;
            if (name.EndsWith(SUFFIX, StringComparison.OrdinalIgnoreCase))
            {
                int index = name.LastIndexOf(SUFFIX);
                name = name.Remove(index, SUFFIX.Length);
            }
            return name;
        }

        /// <summary>
        /// Get value type of specified key type.
        /// </summary>
        /// <param name="keyType">Type of key.</param>
        /// <returns></returns>
        public static Type GetValueType(Type keyType)
        {
            Key keyImplInstance = ScriptableObject.CreateInstance(keyType) as Key;
            Type valueType = keyImplInstance.GetValueType();
            Object.DestroyImmediate(keyImplInstance);
            return valueType;
        }
    }
}
