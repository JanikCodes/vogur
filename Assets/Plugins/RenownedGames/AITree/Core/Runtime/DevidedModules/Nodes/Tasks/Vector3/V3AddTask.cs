/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev, Vladimir Deryabin
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Vector3 Add", "Tasks/Vector 3/Add", IconPath = "Images/Icons/Node/Vector3Icon.png")]
    public class V3AddTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector3Key vectorA;

        [SerializeField]
        private Vector3Key vectorB;

        [SerializeField]
        [NonLocal]
        private Vector3Key storedResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (vectorA == null || vectorB == null || storedResult == null)
            {
                return State.Failure;
            }

            Vector3 result = vectorA.GetValue() + vectorB.GetValue();
            storedResult.SetValue(result);

            return State.Success;
        }
    }
}