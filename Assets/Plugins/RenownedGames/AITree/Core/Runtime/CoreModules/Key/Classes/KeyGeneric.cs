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
    public abstract class Key<T> : Key, IEquatable<T>, IKeyValue<T>
    {
        [SerializeField]
        private T value;

        /// <summary>
        /// System object reference value.
        /// </summary>
        public override object GetValueObject()
        {
            return value;
        }

        #region [IKeyValue<T> Implementation]
        /// <summary>
        /// Get key value.
        /// </summary>
        public T GetValue()
        {
            return value;
        }
        #endregion

        #region [IKeyType Implementation]
        /// <summary>
        /// System value type of key.
        /// </summary>
        public override Type GetValueType()
        {
            return typeof(T);
        }
        #endregion

        #region [IEquatable<T> Implementation]
        public virtual bool Equals(T other)
        {
            return value.Equals(other);
        }
        #endregion

        #region [Events]
        /// <summary>
        /// Called when the key value has been changed.
        /// </summary>
        public override event Action ValueChanged;
        #endregion

        #region [Getter / Setter]
        /// <summary>
        /// Set key value.
        /// </summary>
        public void SetValue(T value)
        {
            bool changed = value == null && this.value != null || value != null && (this.value == null || !Equals(value));
            if (changed)
            {
                this.value = value;
                ValueChanged?.Invoke();
            }
        }
        #endregion
    }
}
