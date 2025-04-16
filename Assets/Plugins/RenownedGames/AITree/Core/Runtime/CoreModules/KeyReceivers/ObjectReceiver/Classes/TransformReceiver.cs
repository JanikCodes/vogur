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
    public class TransformReceiver : KeyReceiver<TransformKey>
    {
        [SerializeField]
        private Transform value;

        protected override void Apply(TransformKey key)
        {
            key.SetValue(value);
        }

        /// <summary>
        /// Detail description of key receiver.
        /// </summary>
        public override string GetDescription(Key key)
        {
            if (key == null)
            {
                return base.GetDescription(key);
            }

            return $"Set {value} to {key.name}";
        }

        #region [Getter / Setter]
        public Transform GetValue()
        {
            return value;
        }

        public void SetValue(Transform value)
        {
            this.value = value;
        }
        #endregion
    }
}