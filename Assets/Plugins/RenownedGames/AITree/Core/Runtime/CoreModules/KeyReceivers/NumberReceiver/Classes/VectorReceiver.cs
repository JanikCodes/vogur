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
    public class VectorReceiver : KeyReceiver<Vector3Key>
    {
        [SerializeField]
        private Vector3 value;

        protected override void Apply(Vector3Key key)
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
        public Vector3 GetValue()
        {
            return value;
        }

        public void SetValue(Vector3 value)
        {
            this.value = value;
        }
        #endregion
    }
}