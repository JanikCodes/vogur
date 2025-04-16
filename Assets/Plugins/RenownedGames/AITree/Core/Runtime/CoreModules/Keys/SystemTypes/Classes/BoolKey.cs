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
    [KeyColor(0.584f, 0.0f, 0.0f)]
    public class BoolKey : Key<bool>
    {
        public override bool Equals(bool other)
        {
            return GetValue() == other;
        }
    }
}