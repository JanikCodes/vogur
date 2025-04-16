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
    [KeyColor(1.0f, 0.0f, 0.831f)]
    public class StringKey : Key<string>
    {
        public override bool Equals(string other)
        {
            return GetValue() == other;
        }
    }
}