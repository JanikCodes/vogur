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
    [NodeContent("Wait", "Tasks/Common/Wait", IconPath = "Images/Icons/Node/WaitIcon.png")]
    public class WaitTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private FloatKey waitTime;

        [SerializeField]
        private FloatKey randomDeviation;

        // Stored required properties.
        private float startTime;

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();
            startTime = Time.time;

            if (randomDeviation != null)
            {
                float deviation = randomDeviation.GetValue();
                if (deviation > 0)
                {
                    startTime += Random.Range(-deviation, deviation);
                }
            }
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (waitTime == null)
            {
                return State.Failure;
            }

            if (Time.time - startTime > waitTime.GetValue())
            {
                return State.Success;
            }

            return State.Running;
        }

        /// <summary>
        /// Progress of node execution.
        /// </summary>
        /// <returns>If node support progress line, return value of normalized progress. Otherwise null.</returns>
        protected override float? GetProgress()
        {
            return (Time.time - startTime) / waitTime.GetValue();
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = $"Wait: ";

            if (waitTime != null)
            {
                description += waitTime.ToString();

                if (randomDeviation != null)
                {
                    description += $"±{randomDeviation.ToString()}";
                }

                description += "s";
            }
            else
            {
                description += "None";
            }

            return description;
        }
    }
}