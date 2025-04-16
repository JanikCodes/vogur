/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree
{
    [KeyColor(1.0f, 0.6f, 0.11f)]
    public class Vector2Key : Key<Vector2>
    {
        public override bool Equals(Vector2 other)
        {
            return GetValue() == other;
        }
    }
}