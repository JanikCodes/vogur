/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace RenownedGames.AITree
{
    [Serializable]
    public class StringQuery : KeyQuery<StringKey>
    {
        [SerializeField]
        private StringComparer comparer;

        [SerializeField]
        private string value;

        protected override bool Result(StringKey key)
        {
            switch (comparer)
            {
                case StringComparer.IsEqualTo:
                    return key.GetValue() == value;
                case StringComparer.IsNotEqualTo:
                    return key.GetValue() != value;
                case StringComparer.Contains:
                    return key.GetValue().Contains(value);
                case StringComparer.NotContains:
                    return !key.GetValue().Contains(value);
            }

            return false;
        }

        /// <summary>
        /// Detail description of key query.
        /// </summary>
        public override string GetDescription(Key key)
        {
            if (key == null)
            {
                return base.GetDescription(key);
            }

            return $"{key.name} is {comparer} \"{value}\"";
        }

        #region [Getter / Setter]
        public StringComparer GetComparer()
        {
            return comparer;
        }

        public void SetComparer(StringComparer value)
        {
            this.comparer = value;
        }

        public string GetComparerValue()
        {
            return value;
        }

        public void SetComparerValue(string value)
        {
            this.value = value;
        }
        #endregion
    }
}