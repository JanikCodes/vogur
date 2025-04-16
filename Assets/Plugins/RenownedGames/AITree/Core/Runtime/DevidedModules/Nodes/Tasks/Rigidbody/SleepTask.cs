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
    [NodeContent("Sleep", "Tasks/Rigidbody/Sleep", IconPath = "Images/Icons/Node/SleepIcon.png")]
    public class SleepTask : TaskNode
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
                Rigidbody rb = transform.GetComponent<Rigidbody>();
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
