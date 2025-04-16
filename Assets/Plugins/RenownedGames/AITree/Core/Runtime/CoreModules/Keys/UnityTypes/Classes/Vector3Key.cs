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
    [KeyColor(1.0f, 0.792f, 0.137f)]
    public class Vector3Key : Key<Vector3>
    {
        public override bool Equals(Vector3 other)
        {
            return GetValue() == other;
        }
    }
}