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
    public class FloatQuery : NumberQuery<FloatKey>
    {
        [SerializeField]
        private float value;

        protected override bool CompareNumber(FloatKey key, NumberComparer comparer)
        {
            switch (comparer)
            {
                case NumberComparer.IsEqualTo:
                    return key.GetValue() == value;
                case NumberComparer.IsNotEqualTo:
                    return key.GetValue() != value;
                case NumberComparer.IsLessThen:
                    return key.GetValue() < value;
                case NumberComparer.IsLessThenOrEqualTo:
                    return key.GetValue() <= value;
                case NumberComparer.IsGreaterThen:
                    return key.GetValue() > value;
                case NumberComparer.IsGreaterThenOrEqualTo:
                    return key.GetValue() >= value;
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

            return $"{key.name} is {GetNumberComparer()} {value}";
        }

        #region [Getter / Setter]
        public float GetComparerValue()
        {
            return value;
        }

        public void SetComparerValue(float value)
        {
            this.value = value;
        }
        #endregion
    }
}