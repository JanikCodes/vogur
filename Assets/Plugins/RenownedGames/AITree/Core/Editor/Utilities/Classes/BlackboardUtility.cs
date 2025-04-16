/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using System;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    public static class BlackboardUtility
    {
        /// <summary>
        /// Adds a pre-created key to the blackboard.
        /// </summary>
        /// <param name="blackboard">Blackboard.</param>
        /// <param name="key">Created key.</param>
        /// <param name="withoutUndo">If true, the addition will not be recorded in the Undo history.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool AddKey(Blackboard blackboard, Key key)
        {
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(key, blackboard);
            }

            blackboard.AddKey(key);
            AssetDatabase.SaveAssets();
            return true;
        }

        /// <summary>
        /// Creates a key and adds it to the blackboard.
        /// </summary>
        /// <param name="blackboard">Blackboard.</param>
        /// <param name="keyType">Key type.</param>
        /// <param name="withoutUndo">If true, the addition will not be recorded in the Undo history.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool AddKey(Blackboard blackboard, Type keyType)
        {
            Key key = ScriptableObject.CreateInstance(keyType) as Key;
            if (key != null)
            {
                int count = 0;
                string name = keyType.Name;
                while (blackboard.Contains(name))
                {
                    name = $"{keyType.Name} {++count}";
                }

                key.name = name;
                return AddKey(blackboard, key);
            }
            return false;
        }

        /// <summary>
        /// Removes the key from the blackboard and destroys it.
        /// </summary>
        /// <param name="blackboard">Blackboard.</param>
        /// <param name="key">Key.</param>
        /// <param name="withoutUndo">If true, the deletion will not be recorded in the Undo history.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool DeleteKey(Blackboard blackboard, Key key)
        {
            if (!blackboard.GetKeys().Contains(key))
            {
                return false;
            }

            Undo.RecordObject(blackboard, "DELETE_KEY_FROM_BLACKBOARD");
            blackboard.DeleteKey(key);
            Undo.DestroyObjectImmediate(key);
            AssetDatabase.SaveAssets();

            return true;
        }

        /// <summary>
        /// Get all blackboard in project.
        /// </summary>
        /// <returns>Array of blackboards.</returns>
        public static Blackboard[] GetAllBlackboards()
        {
            const string FILTER = "t:Blackboard";
            string[] guids = AssetDatabase.FindAssets(FILTER);
            Blackboard[] blackboards = new Blackboard[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];
                string path = AssetDatabase.GUIDToAssetPath(guid);
                blackboards[i] = AssetDatabase.LoadAssetAtPath<Blackboard>(path);
            }
            return blackboards;
        }
    }
}