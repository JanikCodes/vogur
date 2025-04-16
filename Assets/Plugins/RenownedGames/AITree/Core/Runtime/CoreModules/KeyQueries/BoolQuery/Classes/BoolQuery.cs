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
    public class BoolQuery : KeyQuery<BoolKey>
    {
        [SerializeField]
        protected bool comparer;

        protected override bool Result(BoolKey key)
        {
            return key.GetValue() == comparer;
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

            return $"{key.name} is {comparer}";
        }

        #region [Getter / Setter]
        public bool GetComparer()
        {
            return comparer;
        }

        public void SetComparer(bool value)
        {
            this.comparer = value;
        }
        #endregion
    }
}