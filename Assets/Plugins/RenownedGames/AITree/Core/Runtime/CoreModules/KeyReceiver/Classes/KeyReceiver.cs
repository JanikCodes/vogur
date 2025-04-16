/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree
{
    [System.Serializable]
    public abstract class KeyReceiver
    {
        public abstract void Apply(Key key);

        /// <summary>
        /// Detail description of key receiver.
        /// </summary>
        public virtual string GetDescription(Key key)
        {
            return string.Empty;
        }
    }
}