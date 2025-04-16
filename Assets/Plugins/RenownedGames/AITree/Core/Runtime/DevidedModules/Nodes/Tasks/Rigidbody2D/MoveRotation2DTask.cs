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
    [NodeContent("Move Rotation 2D", "Tasks/Rigidbody 2D/Move Rotation 2D", IconPath = "Images/Icons/Node/MoveObjectIcon.png")]
    public class MoveRotation2DTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private TransformKey target;

        [SerializeField]
        private FloatKey rotation;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (target != null && rotation != null && target.TryGetTransform(out Transform transform))
            {
                Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.MoveRotation(rotation.GetValue());
                    return State.Success;
                }
            }

            return State.Failure;
        }
    }
}
