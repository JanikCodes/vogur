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
    public abstract class KeyQuery<T> : KeyQuery where T : Key
    {
        protected abstract bool Result(T key);

        public sealed override bool Result(Key key)
        {
            if (key is T tkey)
            {
                return Result(tkey);
            }

            throw new System.Exception("The specified key type does not match the type of this KeyQuery type.");
        }
    }
}