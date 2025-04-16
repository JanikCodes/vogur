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
    [NodeContent("Debug Draw Line", "Tasks/Debug/Draw Line", IconPath = "Images/Icons/Node/DebugIcon.png")]
    public class DebugDrawLineTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key from;

        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key to;

        [Title("Node")]
        [SerializeField]
        private Color color = Color.white;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (from == null || to == null)
            {
                return State.Failure;
            }

            from.TryGetPosition(Space.World, out Vector3 start);
            to.TryGetPosition(Space.World, out Vector3 end);

            Debug.DrawLine(start, end, color);

            return State.Success;
        }
    }
}