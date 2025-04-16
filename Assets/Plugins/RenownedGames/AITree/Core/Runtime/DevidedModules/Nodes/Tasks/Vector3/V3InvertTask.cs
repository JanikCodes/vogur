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
    [NodeContent("Vector3 Invert", "Tasks/Vector 3/Invert", IconPath = "Images/Icons/Node/Vector3Icon.png")]
    public class V3InvertTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector3Key vector3;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (vector3 == null)
            {
                return State.Failure;
            }

            vector3.SetValue(-vector3.GetValue());
            return State.Success;
        }
    }
}