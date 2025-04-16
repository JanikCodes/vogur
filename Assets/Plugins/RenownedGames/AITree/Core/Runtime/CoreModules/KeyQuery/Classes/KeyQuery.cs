/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree
{
    [System.Serializable]
    public abstract class KeyQuery
    {
        public abstract bool Result(Key key);

        /// <summary>
        /// Detail description of key query.
        /// </summary>
        public virtual string GetDescription(Key key)
        {
            return string.Empty;
        }
    }
}