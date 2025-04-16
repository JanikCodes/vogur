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
    [NodeContent("Set Vector3 Value", "Tasks/Vector 3/Set Value", IconPath = "Images/Icons/Node/Vector3Icon.png")]
    public class V3SetValueTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector3Key sourceVector;

        [SerializeField]
        [NonLocal]
        private Vector3Key storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if(sourceVector == null || storeResult == null)
            {
                return State.Failure;
            }

            storeResult.SetValue(sourceVector.GetValue());
            return State.Success;
        }
    }
}