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
    [NodeContent("Time Limit", "Time Limit", IconPath = "Images/Icons/Node/WaitIcon.png")]
    public class TimeLimitDecorator : DecoratorNode
    {
        [Title("Decorator")]
        [SerializeField]
        private FloatKey timeLimit;

        // Stored required properties.
        private float startTime;

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();
            startTime = Time.time;
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (timeLimit == null)
            {
                return State.Failure;
            }

            State state = UpdateChild();
            if (state == State.Running && Time.time - startTime >= timeLimit.GetValue())
            {
                ResetChild(State.Failure);
                return State.Failure;
            }
            return state;
        }

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected override void OnFlowUpdate() { }

        /// <summary>
        /// Progress of node execution.
        /// </summary>
        /// <returns>If node support progress line, return value of normalized progress. Otherwise null.</returns>
        protected override float? GetProgress()
        {
            return (Time.time - startTime) / timeLimit.GetValue();
        }

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = base.GetDescription();

            if (!string.IsNullOrEmpty(description)) description += "\n";

            description += $"Time Limit: Failed after ";

            if (timeLimit != null)
            {
                description += timeLimit.ToString() + "s";
            }
            else
            {
                description += "None";
            }

            return description;
        }
        #endregion
    }
}