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
    [NodeContent("Wake Up 2D", "Tasks/Rigidbody 2D/Wake Up 2D", IconPath = "Images/Icons/Node/WakeUpIcon.png")]
    public class WakeUp2DTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private TransformKey target;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (target != null && target.TryGetTransform(out Transform transform))
            {
                Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.WakeUp();
                    return State.Success;
                }
            }

            return State.Failure;
        }
    }
}
