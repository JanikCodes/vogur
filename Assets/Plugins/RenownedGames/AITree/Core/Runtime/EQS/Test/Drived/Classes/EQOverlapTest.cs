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
    [SearchContent("Overlap", Image = "Images/Icons/EQS/Tests/OverlapTestIcon.png")]
    public class EQOverlapTest : EQTest
    {
        [Title("Overlap")]
        [SerializeField]
        private Shape shape = Shape.Sphere;

        [SerializeField]
        private float upOffset = 1f;

        [SerializeField]
        [ShowIf("shape", Shape.Sphere)]
        private float radius = .5f;

        [SerializeField]
        [MinValue(1)]
        private int maxQuery = 30;

        [SerializeField]
        [ShowIf("shape", Shape.Box)]
        private Vector3 halfExtents = Vector3.one;

        [SerializeField]
        private LayerMask cullingLayer = ~0;

        // Stored required properties.
        private Collider[] colliders;

        protected override float CalculateWeight(EQItem item)
        {
            if(colliders == null || colliders.Length != maxQuery)
            {
                colliders = new Collider[maxQuery];
            }

            Vector3 center = GetPositionByTarget(TargetType.Item) + Vector3.up * upOffset;

            switch (shape)
            {
                case Shape.Sphere:
                    return Physics.OverlapSphereNonAlloc(center, radius, colliders, cullingLayer) == 0 ? 0 : 1;
                case Shape.Box:
                    return Physics.OverlapBoxNonAlloc(center, halfExtents, colliders, Quaternion.identity, cullingLayer) == 0 ? 0 : 1;
            }

            return 0f;
        }
    }
}