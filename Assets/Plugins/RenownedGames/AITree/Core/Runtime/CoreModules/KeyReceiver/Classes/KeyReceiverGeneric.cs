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
    public abstract class KeyReceiver<T> : KeyReceiver where T : Key
    {
        protected abstract void Apply(T key);

        public sealed override void Apply(Key key)
        {
            if (key is T tkey)
            {
                Apply(tkey);
            }
            else
            {
                throw new System.Exception("The specified key type does not match the type of this KeyReceiver type.");
            }
        }
    }
}