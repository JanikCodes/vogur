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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RenownedGames.AITree.EQS
{
    [Serializable]
    [SearchContent("NavMesh", Image = "Images/Icons/EQS/TraceModes/NavTMIcon.png")]
    public class EQNavMeshTraceMode : EQTraceMode
    {
        [SerializeField]
        [NavMeshAreaMask]
        private int areaMask = -1;

        [SerializeField]
        [MinValue(.1f)]
        private float distance = 1;

        public override IEnumerable<Vector3> TryTracePosition(Vector3 point)
        {
            if (NavMesh.SamplePosition(point, out NavMeshHit hitInfo, distance, areaMask))
            {
                yield return hitInfo.position;
            }
        }
    }
}