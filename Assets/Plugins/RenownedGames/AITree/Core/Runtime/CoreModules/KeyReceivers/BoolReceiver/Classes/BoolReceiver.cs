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
    public class BoolReceiver : KeyReceiver<BoolKey>
    {
        [SerializeField]
        private bool value;

        protected override void Apply(BoolKey key)
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
        public bool GetValue()
        {
            return value;
        }

        public void SetValue(bool value)
        {
            this.value = value;
        }
        #endregion
    }
}