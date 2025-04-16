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
    [KeyColor(0.631f, 1.0f, 0.27f)]
    public class FloatKey : Key<float>
    {
        public override bool Equals(float other)
        {
            return GetValue() == other;
        }
    }
}