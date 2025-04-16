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
    [HideMonoScript]
    [NodeContent("One Time Execution", "One Time Execution", IconPath = "Images/Icons/Node/OneIcon.png")]
    public class OneTimeExecutionDecorator : DecoratorNode
    {
        private enum CompletedStateType
        {
            OwnLastState,
            Custom
        }

        [SerializeField]
        private CompletedStateType completedStateType;

        [SerializeField]
        [ShowIf("completedStateType", CompletedStateType.Custom)]
        private State stateOnCompleted = State.Success;

        // Stored required properties.
        private State completedState;
        private bool completed;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (completed)
            {
                if (completedStateType == CompletedStateType.OwnLastState)
                {
                    return completedState;
                }
                else
                {
                    return stateOnCompleted;
                }
            }

            return UpdateChild();
        }

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected override void OnFlowUpdate() { }

        /// <summary>
        /// Called when behaviour tree exit from node.
        /// </summary>
        protected override void OnExit()
        {
            base.OnExit();
            completed = true;
            completedState = GetState();
        }

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            return $"One Time Execution: {(completed ? "Completed" : "Waiting")}";
        }
        #endregion
    }
}