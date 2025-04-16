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
    [NodeContent("Get Atan", "Tasks/Math/Trigonometry/Get Atan", IconPath = "Images/Icons/Node/TrigonometryIcon.png")]
    public class GetAtanTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private FloatKey value;

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
            if (value == null || storeResult == null)
            {
                return State.Failure;
            }

            float result = Mathf.Atan(value.GetValue());
            storeResult.SetValue(radToDeg ? Mathf.Rad2Deg * result : result);
            return State.Success;
        }
    }
}