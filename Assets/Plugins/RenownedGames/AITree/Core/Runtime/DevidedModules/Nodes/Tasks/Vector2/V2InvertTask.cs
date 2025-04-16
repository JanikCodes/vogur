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
    [NodeContent("Vector2 Invert", "Tasks/Vector 2/Invert", IconPath = "Images/Icons/Node/Vector2Icon.png")]
    public class V2InvertTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector2Key vector2;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if(vector2 == null)
            {
                return State.Failure;
            }

            vector2.SetValue(-vector2.GetValue());
            return State.Success;
        }
    }
}