/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    [NodeContent("Run EQS", "Run EQS", IconPath = "Images/Icons/EQS/Nodes/Tasks/RunEQSQueryIcon.png")]
    public class RunEQSService : IntervalServiceNode
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

        /// <summary>
        /// Service tick.
        /// </summary>
        protected override void OnTick()
        {
            if (environmentQuery != null)
            {
                environmentQuery.SetQuerier(GetOwner().transform);
                environmentQuery.SetBlackboard(GetOwner().GetBlackboard());

                EQItem item = environmentQuery.GetRandomlyItemBySampling(sampling);
                if (item != null)
                {
                    key.SetValue(item.GetPosition());
                }
            }
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = base.GetDescription();
            description += "\nRun EQS: ";
            description += environmentQuery != null ? environmentQuery.name : "None";
            return description;
        }

        public override void OnDrawGizmosNodeSelected()
        {
            base.OnDrawGizmosNodeSelected();

            if (environmentQuery != null)
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