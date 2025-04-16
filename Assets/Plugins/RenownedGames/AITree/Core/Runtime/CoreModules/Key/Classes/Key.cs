/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace RenownedGames.AITree
{
    public abstract class Key : ScriptableObject, IKeySynced, IKeyLocal, IKeyType, IEntityDescription
    {
        [SerializeField]
        private string category = string.Empty;

        [SerializeField]
        private string description = string.Empty;

        [SerializeField]
        [HideInInspector]
        private bool sync = false;

        [SerializeField]
        [HideInInspector]
        private bool isLocal = false;

        /// <summary>
        /// System object reference value.
        /// </summary>
        public abstract object GetValueObject();

        #region [ToString Implementation]
        /// <summary>
        /// Returns the name of the key.
        /// </summary>
        /// <returns>The name returned by ToString.</returns>
        public override string ToString()
        {
            if (this.IsLocal())
            {
                object value = GetValueObject();
                return value != null ? value.ToString() : "Null";
            }
            else
            {
                return name;
            }
        }
        #endregion

        #region [IKeySynced Implementation]
        /// <summary>
        /// This is used to determine if the Key will be synchronized across all instances of the Blackboard.
        /// </summary>
        public bool IsSync()
        {
            return sync;
        }
        #endregion

        #region [IKeyLocal Implementation]
        public bool IsLocal()
        {
            return isLocal;
        }
        #endregion

        #region [IKeyType Implementation]
        /// <summary>
        /// System value type of key.
        /// </summary>
        public abstract Type GetValueType();
        #endregion

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of key.
        /// </summary>
        public string GetDescription()
        {
            return description;
        }
        #endregion

        #region [Event Callback Functions]
        /// <summary>
        /// Called when the key value has been changed.
        /// </summary>
        public abstract event Action ValueChanged;
        #endregion

        #region [Internal Methods]
        /// <summary>
        /// Change key local state.
        /// </summary>
        internal void IsLocal(bool value)
        {
            isLocal = value;
        }
        #endregion

        #region [Getter / Setter]
        public string GetCategory()
        {
            return category;
        }

        public void SetCategory(string category)
        {
            this.category = category;
        }

        public void SetDescription(string description)
        {
            this.description = description;
        }

        public void IsSync(bool sync)
        {
            this.sync = sync;
        }
        #endregion
    }
}