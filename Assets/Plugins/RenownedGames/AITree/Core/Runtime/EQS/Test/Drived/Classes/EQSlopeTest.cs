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
    [SearchContent("Slope", Image = "Images/Icons/EQS/Tests/SlopeTestIcon.png")]
    public class EQSlopeTest : EQTest
    {
        [Title("Slope")]
        [SerializeField]
        private float upOffset = .5f;

        protected override float CalculateWeight(EQItem item)
        {
            float angle = 0f;

            if (Physics.Raycast(item.GetPosition() + Vector3.up * upOffset, Vector3.down, out RaycastHit hitInfo))
            {
                angle = Vector3.Angle(Vector3.up, hitInfo.normal);
            }

            return angle;
        }
    }
}