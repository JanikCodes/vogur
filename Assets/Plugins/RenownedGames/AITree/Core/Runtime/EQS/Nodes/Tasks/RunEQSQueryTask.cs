/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    [NodeContent("Run EQS Query", "Tasks/EQS/Run EQS Query", IconPath = "Images/Icons/EQS/Nodes/Tasks/RunEQSQueryIcon.png")]
    public class RunEQSQueryTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private Vector3Key key;

        [Title("EQS")]
        [SerializeField]
        private EnvironmentQuery environmentQuery;

        [SerializeField]
        [Slider(0f, 1f)]
        private float sampling = 0;

        // Stored required properties.
        private static RunEQSQueryTask debuger;

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();
            debuger = this;
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (environmentQuery != null)
            {
                environmentQuery.SetQuerier(GetOwner().transform);
                environmentQuery.SetBlackboard(GetOwner().GetBlackboard());

                EQItem item = environmentQuery.GetRandomlyItemBySampling(sampling);
                if (item != null)
                {
                    key.SetValue(item.GetPosition());
                    return State.Success;
                }
            }

            return State.Failure;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = "Run EQS Query: ";

            description += environmentQuery != null ? environmentQuery.name : "None";

            return description;
        }

        /// <summary>
        /// Implement OnDrawGizmosSelected to draw a gizmo if the object is selected.
        /// </summary>
        public override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            if (debuger == this && environmentQuery != null)
            {
#if UNITY_EDITOR
                environmentQuery.SetQuerier(GetOwner().transform);
                environmentQuery.SetBlackboard(GetOwner().GetBlackboard());
                environmentQuery.Visualize(true, false);
#endif
            }
        }
    }
}