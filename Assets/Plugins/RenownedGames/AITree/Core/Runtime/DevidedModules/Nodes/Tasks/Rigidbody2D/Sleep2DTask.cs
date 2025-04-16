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
    [NodeContent("Sleep 2D", "Tasks/Rigidbody 2D/Sleep 2D", IconPath = "Images/Icons/Node/SleepIcon.png")]
    public class Sleep2DTask : TaskNode
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
                    rb.Sleep();
                    return State.Success;
                }
            }

            return State.Failure;
        }
    }
}
