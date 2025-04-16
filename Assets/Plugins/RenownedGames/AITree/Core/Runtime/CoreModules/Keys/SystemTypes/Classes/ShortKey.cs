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
    [KeyColor(0.0f, 0.69f, 0.486f)]
    public class ShortKey : Key<short>
    {
        public override bool Equals(short other)
        {
            return GetValue() == other;
        }
    }
}