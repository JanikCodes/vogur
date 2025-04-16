/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree
{
    [KeyColor(0f, .666f, .96f)]
    public class TransformKey : Key<Transform>
    {
        public override bool Equals(Transform other)
        {
            return GetValue() == other;
        }
    }
}