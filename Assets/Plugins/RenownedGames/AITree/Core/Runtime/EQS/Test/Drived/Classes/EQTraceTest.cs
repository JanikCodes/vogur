/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    [Serializable]
    [SearchContent("Trace", Image = "Images/Icons/EQS/Tests/TraceTestIcon.png")]
    public class EQTraceTest : EQTest
    {
        [Title("Trace")]
        [SerializeField]
        private TargetType traceTo;

        [SerializeField]
        [ShowIf("traceTo", TargetType.Key)]
        private string keyName;

        [SerializeField]
        [Min(0)]
        private float upOffset = 1f;

        [SerializeField]
        private LayerMask cullingLayer = ~0;

        protected override float CalculateWeight(EQItem item)
        {
            Vector3 from = GetPositionByTarget(traceTo, keyName) + Vector3.up * upOffset;
            Vector3 to = GetPositionByTarget(TargetType.Item) + Vector3.up * upOffset;

            return Physics.Linecast(from, to, cullingLayer) ? 1 : 0;
        }
    }
}