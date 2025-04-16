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
    [NodeContent("Add Force At Position 2D", "Tasks/Rigidbody 2D/Add Force At Position 2D", IconPath = "Images/Icons/Node/ForceIcon.png")]
    public class AddForceAtPosition2DTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private TransformKey target;

        [SerializeField]
        private Vector2Key force;

        [SerializeField]
        private Vector2Key position;

        [SerializeField]
        private ForceMode2D mode = ForceMode2D.Force;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (force == null || position == null)
            {
                return State.Failure;
            }

            if (target != null && target.TryGetTransform(out Transform transform))
            {
                Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForceAtPosition(force.GetValue(), position.GetValue(), mode);
                    return State.Success;
                }
            }

            return State.Failure;
        }
    }
}
