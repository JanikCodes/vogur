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
    [NodeContent("Move Position 2D", "Tasks/Rigidbody 2D/Move Position 2D", IconPath = "Images/Icons/Node/MoveObjectIcon.png")]
    public class MovePosition2DTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private TransformKey target;

        [SerializeField]
        private Vector2Key position;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (target != null && position != null && target.TryGetTransform(out Transform transform))
            {
                Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.MovePosition(position.GetValue());
                    return State.Success;
                }
            }

            return State.Failure;
        }
    }
}
