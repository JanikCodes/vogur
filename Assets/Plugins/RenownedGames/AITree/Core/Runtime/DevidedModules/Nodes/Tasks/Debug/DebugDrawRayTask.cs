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
    [NodeContent("Debug Draw Ray", "Tasks/Debug/Draw Ray", IconPath = "Images/Icons/Node/DebugIcon.png")]
    public class DebugDrawRayTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key from;

        [SerializeField]
        private Vector3Key direction;

        [Title("Node")]
        [SerializeField]
        private Color color = Color.white;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (from == null || direction == null)
            {
                return State.Failure;
            }

            from.TryGetPosition(Space.World, out Vector3 start);
            Vector3 dir = direction.GetValue();

            Debug.DrawRay(start, dir, color);

            return State.Success;
        }
    }
}