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
    [KeyColor(0.627f, 0.705f, 1.0f)]
    public class QuaternionKey : Key<Quaternion>
    {
        public override bool Equals(Quaternion other)
        {
            return GetValue() == other;
        }
    }
}