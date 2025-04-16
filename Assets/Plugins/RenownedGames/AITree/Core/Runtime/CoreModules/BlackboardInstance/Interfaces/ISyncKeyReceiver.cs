/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;

namespace RenownedGames.AITree
{
    public interface ISyncKeyReceiver
    {
        void OnSynchronization(List<Key> keys);
    }
}