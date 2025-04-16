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
    [NodeContent("Vector2 Add", "Tasks/Vector 2/Add", IconPath = "Images/Icons/Node/Vector2Icon.png")]
    public class V2AddTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector2Key vectorA;

        [SerializeField]
        private Vector2Key vectorB;

        [SerializeField]
        [NonLocal]
        private Vector2Key storedResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if(vectorA == null || vectorB == null || storedResult == null)
            {
                return State.Failure;
            }

            Vector2 result = vectorA.GetValue() + vectorB.GetValue();
            storedResult.SetValue(result);

            return State.Success;
        }
    }
}