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
    [NodeContent("Get Tan", "Tasks/Math/Trigonometry/Get Tan", IconPath = "Images/Icons/Node/TrigonometryIcon.png")]
    public class GetTanTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private FloatKey value;

        [SerializeField]
        private bool degToRag;

        [SerializeField]
        [NonLocal]
        private FloatKey storeResult;

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

            float result = Mathf.Tan(degToRag ? Mathf.Deg2Rad * value.GetValue() : value.GetValue());
            storeResult.SetValue(result);
            return State.Success;
        }
    }
}