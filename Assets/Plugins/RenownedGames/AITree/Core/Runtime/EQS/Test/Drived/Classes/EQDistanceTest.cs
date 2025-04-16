/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    [SearchContent("Distance", Image = "Images/Icons/EQS/Tests/DistTestIcon.png")]
    public class EQDistanceTest : EQTest
    {
        [Header("Distance")]
        [SerializeField]
        private TargetType distanceTo;

        [SerializeField]
        [ShowIf("distanceTo", TargetType.Key)]
        private string keyName;

        protected override float CalculateWeight(EQItem item)
        {
            return Vector3.Distance(GetPositionByTarget(TargetType.Item), GetPositionByTarget(distanceTo, keyName));
        }
    }
}