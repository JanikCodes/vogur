/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Get Atan2 From Vector2", "Tasks/Math/Trigonometry/Get Atan2 From Vector2", IconPath = "Images/Icons/Node/TrigonometryIcon.png")]
    public class GetAtan2FromV2Task : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private Vector2Key vector2;

        [SerializeField]
        [NonLocal]
        private FloatKey storeResult;

        [SerializeField]
        private bool radToDeg;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (vector2 == null || storeResult == null)
            {
                return State.Failure;
            }

            Vector2 vector = vector2.GetValue();
            float result = Mathf.Atan2(vector.y, vector.x);
            storeResult.SetValue(radToDeg ? Mathf.Rad2Deg * result : result);
            return State.Success;
        }
    }
}