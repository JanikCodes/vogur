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
    [NodeContent("Set Vector2 Value", "Tasks/Vector 2/Set Value", IconPath = "Images/Icons/Node/Vector2Icon.png")]
    public class V2SetValueTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private Vector2Key target;

        [SerializeField]
        private Vector2Key source;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if(target == null || source == null)
            {
                return State.Failure;
            }

            target.SetValue(source.GetValue());
            return State.Success;
        }
    }
}