/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RenownedGames.AITree
{
    [CreateAssetMenu(fileName = "Blackboard", menuName = "Renowned Games/AI Tree/Blackboard")]
    public sealed class Blackboard : ScriptableObject, IBlackboard, IBlackboardKeys
    {
        [SerializeReference]
        [HideInInspector]
        private List<Key> keys = new List<Key>();

        [SerializeField]
        [HideInInspector]
        private Blackboard parent;

        /// <summary>
        /// Try get key by name with specified type, only in this blackboard.
        /// </summary>
        /// <param name="name">Name of key.</param>
        /// <param name="value">Output reference of key.</param>
        /// <returns>True if key with same name and type found. Otherwise false.</returns>
        public bool TryGetKey(string name, out Key value)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                Key key = keys[i];
                if (key.name == name)
                {
                    value = key;
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Try get key by name with specified type, only in this blackboard.
        /// </summary>
        /// <typeparam name="T">Type of Key.</typeparam>
        /// <param name="name">Name of key.</param>
        /// <param name="value">Output reference of key.</param>
        /// <returns>True if key with same name and type found. Otherwise false.</returns>
        public bool TryGetKey<T>(string name, out T value) where T : Key
        {
            System.Type keyType = typeof(T);
            if(TryGetKey(name, out Key key) && key.GetType() == keyType)
            {
                value = (T)key;
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Try find key by name with specified type, in this blackboard and including all parents.
        /// </summary>
        /// <param name="name">Name of key.</param>
        /// <param name="value">Output reference of key.</param>
        /// <returns>True if key with same name and type found. Otherwise false.</returns>
        public bool TryFindKey(string name, out Key value)
        {
            foreach (Key key in Keys)
            {
                if (key.name == name)
                {
                    value = key;
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Try find key by name with specified type, in this blackboard and including all parents.
        /// </summary>
        /// <typeparam name="T">Type of Key.</typeparam>
        /// <param name="name">Name of key.</param>
        /// <param name="value">Output reference of key.</param>
        /// <returns>True if key with same name and type found. Otherwise false.</returns>
        public bool TryFindKey<T>(string name, out T value) where T : Key
        {
            System.Type keyType = typeof(T);
            if(TryFindKey(name, out Key key) && key.GetType() == keyType)
            {
                value = (T)key;
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Get list of all keys in this blackboard and parents.
        /// </summary>
        /// <returns>List of Key.</returns>
        public List<Key> GetAllKeys()
        {
            List<Key> allKeys = new List<Key>(keys);
            if (parent != null)
            {
                allKeys.AddRange(parent.GetAllKeys());
            }
            return allKeys;
        }

        /// <summary>
        /// Check if blackboard contains key with same name.
        /// </summary>
        /// <param name="name">Name of searching key.</param>
        /// <param name="includeParents">Search key including in parent blackboards.</param>
        public bool Contains(string name, bool includeParents = true)
        {
            if (includeParents)
            {
                foreach (Key key in Keys)
                {
                    if (key.name == name)
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    if(keys[i].name == name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Clone this blackboard.
        /// </summary>
        /// <returns>New blackboard instance.</returns>
        public Blackboard Clone()
        {
            Blackboard blackboard = CreateInstance<Blackboard>();
            blackboard.name = name;
            blackboard.keys = new List<Key>();

            for (int i = 0; i < keys.Count; i++)
            {
                Key key = keys[i];
                if (!key.IsSync())
                {
                    Key cloneKey = CreateInstance(key.GetType()) as Key;
                    cloneKey.name = key.name;
                    blackboard.keys.Add(cloneKey);
                }
                else
                {
                    blackboard.keys.Add(key);
                }
            }

            if (parent != null)
            {
                blackboard.parent = parent.Clone();
            }

            return blackboard;
        }

        /// <summary>
        /// Check this blackboard nested of other.
        /// </summary>
        /// <param name="blackboard">Blackboard to compare.</param>
        /// <returns>True if nested. Otherwise false.</returns>
        public bool IsNested(Blackboard blackboard)
        {
            if (this == blackboard)
            {
                return true;
            }

            if (parent != null)
            {
                return parent.IsNested(blackboard);
            }

            return false;
        }

        #region [IBlackboard Implementation]
        /// <summary>
        /// Add new key to blackboard.
        /// </summary>
        /// <param name="key">Key reference.</param>
        public void AddKey(Key key)
        {
            keys.Add(key);
        }

        /// <summary>
        /// Remove key from blackboard.
        /// </summary>
        /// <param name="key">Key reference.</param>
        public void DeleteKey(Key key)
        {
            keys.Remove(key);
        }
        #endregion

        #region [IBlackboardKeys Implementation]
        /// <summary>
        /// Iterate through all keys in blackboard.
        /// </summary>
        public IEnumerable<Key> Keys
        {
            get
            {
                if (parent != null)
                {
                    foreach (Key key in parent.Keys)
                    {
                        yield return key;
                    }
                }

                for (int i = 0; i < keys.Count; i++)
                {
                    yield return keys[i];
                }
            }
        }
        #endregion

        #region [Indexer]
        /// <summary>
        /// Get key by name in this blackboard.
        /// </summary>
        /// <param name="name">Name of key.</param>
        /// <returns>Key reference.</returns>
        public Key this[string name]
        {
            get
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    Key key = keys[i];
                    if (key.name == name)
                    {
                        return key;
                    }
                }
                throw new KeyNotFoundException($"Key with [{name}] not found in {this.name} blackboard.");
            }
        }
        #endregion

        #region [Editor]
#if UNITY_EDITOR
        private void Reset()
        {
            EditorApplication.update -= DelayedInitializeSelfKey;
            EditorApplication.update += DelayedInitializeSelfKey;
        }

        private void DelayedInitializeSelfKey()
        {
            if (AssetDatabase.IsNativeAsset(this))
            {
                EditorApplication.update -= DelayedInitializeSelfKey;
                InitializeSelfKey();
            }
        }

        internal void InitializeSelfKey()
        {
            if (parent == null)
            {
                if (!keys.Any(key => key is SelfKey))
                {
                    SelfKey selfKey = ScriptableObject.CreateInstance<SelfKey>();
                    selfKey.name = "Self";

                    if (!Application.isPlaying)
                    {
                        AssetDatabase.AddObjectToAsset(selfKey, this);
                    }

                    AddKey(selfKey);
                    AssetDatabase.SaveAssets();
                }
            }
            else
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i] is SelfKey selfKey)
                    {
                        DeleteKey(selfKey);
                        DestroyImmediate(selfKey, true);
                        AssetDatabase.SaveAssets();
                        break;
                    }
                }
            }
        }
#endif
        #endregion

        #region [Getter / Setter]
        public List<Key> GetKeys()
        {
            return keys;
        }

        public void SetKeys(List<Key> keys)
        {
            this.keys = keys;
        }

        public Blackboard GetParent()
        {
            return parent;
        }

        public void SetParent(Blackboard parent)
        {
            this.parent = parent;
#if UNITY_EDITOR
            InitializeSelfKey();
#endif
        }
        #endregion
    }
}