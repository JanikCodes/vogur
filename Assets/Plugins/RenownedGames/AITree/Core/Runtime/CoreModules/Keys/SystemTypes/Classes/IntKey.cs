/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree
{
    [KeyColor(0.121f, 0.89f, 0.686f)]
    public class IntKey : Key<int>
    {
        public override bool Equals(int other)
        {
            return GetValue() == other;
        }
    }
}