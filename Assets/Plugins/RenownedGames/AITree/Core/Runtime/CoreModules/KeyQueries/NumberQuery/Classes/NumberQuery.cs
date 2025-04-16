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
    public abstract class NumberQuery<T> : KeyQuery<T> where T : Key
    {
        [SerializeField]
        private NumberComparer comparer;

        protected sealed override bool Result(T key)
        {
            return CompareNumber(key, comparer);
        }

        protected abstract bool CompareNumber(T key, NumberComparer comparer);

        #region [Getter / Setter]
        public NumberComparer GetNumberComparer()
        {
            return comparer;
        }

        public void SetNumberComparer(NumberComparer comparer)
        {
            this.comparer = comparer;
        }
        #endregion
    }
}