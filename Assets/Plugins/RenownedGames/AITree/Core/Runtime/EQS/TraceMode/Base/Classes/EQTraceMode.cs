/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    [Serializable]
    public abstract class EQTraceMode
    {
        public abstract IEnumerable<Vector3> TryTracePosition(Vector3 point);
    }
}