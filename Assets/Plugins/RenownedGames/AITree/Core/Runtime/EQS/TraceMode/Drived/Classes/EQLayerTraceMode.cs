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

namespace RenownedGames.AITree.EQS
{
    [Serializable]
    [SearchContent("Layer", Image = "Images/Icons/EQS/TraceModes/LayerTMIcon.png")]
    public class EQLayerTraceMode : EQTraceMode
    {
        [SerializeField]
        private bool raycastAll;

        [SerializeField]
        [ShowIf("raycastAll")]
        [MinValue(1)]
        [Indent(1)]
        private int maxQuery = 30;

        [SerializeField]
        [MinValue(0)]
        private float upOffset = 1f;

        [SerializeField]
        [MinValue(0)]
        private float projectDown = 2;

        [SerializeField]
        private LayerMask cullingLayer = ~0;

        // Stored required properties.
        private RaycastHit[] results;

        public override IEnumerable<Vector3> TryTracePosition(Vector3 point)
        {
            if (raycastAll)
            {
                if (results == null || results.Length != maxQuery)
                {
                    results = new RaycastHit[maxQuery];
                }

                int count = Physics.RaycastNonAlloc(point + Vector3.up * upOffset, Physics.gravity.normalized, results, upOffset + projectDown, cullingLayer, QueryTriggerInteraction.Ignore);
                for (int i = 0; i < count; i++)
                {
                    yield return results[i].point;
                }
            }
            else
            {
                if (Physics.Raycast(point + Vector3.up * upOffset, Physics.gravity.normalized, out RaycastHit hitInfo, upOffset + projectDown, cullingLayer, QueryTriggerInteraction.Ignore))
                {
                    yield return hitInfo.point;
                }
            }
        }
    }
}