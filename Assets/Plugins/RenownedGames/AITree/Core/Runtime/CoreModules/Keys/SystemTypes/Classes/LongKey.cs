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
    [KeyColor(0.321f, 1.0f, 0.886f)]
    public class LongKey : Key<long>
    {
        public override bool Equals(long other)
        {
            return GetValue() == other;
        }
    }
}