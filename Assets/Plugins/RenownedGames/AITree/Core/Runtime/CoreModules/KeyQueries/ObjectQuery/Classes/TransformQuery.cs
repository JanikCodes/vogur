/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree
{
    public class TransformQuery : KeyQuery<TransformKey>
    {
        [SerializeField]
        private ObjectComparer comparer;

        protected override bool Result(TransformKey key)
        {
            switch (comparer)
            {
                case ObjectComparer.IsSet:
                    return key.GetValue() != null;
                case ObjectComparer.IsNotSet:
                    return key.GetValue() == null;
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

            return $"{key.name} is {comparer}";
        }

        #region [Getter / Setter]
        public ObjectComparer GetComparer()
        {
            return comparer;
        }

        public void SetComparer(ObjectComparer value)
        {
            this.comparer = value;
        }
        #endregion
    }
}