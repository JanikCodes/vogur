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
using UnityEngine.AI;

namespace RenownedGames.AITree.EQS
{
    [Serializable]
    [SearchContent("HasPath", Image = "Images/Icons/EQS/Tests/HasPathTestIcon.png")]
    public class EQHasPathTest : EQTest
    {
        [Title("Has path")]
        [SerializeField]
        private TargetType from = TargetType.Querier;

        [SerializeField]
        private TargetType to = TargetType.Item;

        protected override float CalculateWeight(EQItem item)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(GetPositionByTarget(from), GetPositionByTarget(to), NavMesh.AllAreas, path))
            {
                return path.status == NavMeshPathStatus.PathComplete ? 1 : 0;
            }
            return 0;
        }
    }
}