/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2024 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree
{
    [NodeContent("Temp Decorator", "Temp Decorator", IconPath = "Images/Icons/Node/UnknownIcon.png")]
    internal sealed class TempDecoratorNode : DecoratorNode
    {
        [SerializeField]
        [Title("Node")]
        private State returnState = State.Success;

        [SerializeField]
        [Title("Messages")]
        private string entryMessage = string.Empty;

        [SerializeField]
        private string updateMessage = string.Empty;

        [SerializeField]
        private string exitMessage = string.Empty;

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            if (!string.IsNullOrWhiteSpace(entryMessage))
            {
                Debug.Log(entryMessage);
            }
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        protected override State OnUpdate()
        {
            if (!string.IsNullOrWhiteSpace(updateMessage))
            {
                Debug.Log(updateMessage);
            }
            return returnState;
        }

        protected override void OnFlowUpdate() { }

        /// <summary>
        /// Called when behaviour tree exit from node.
        /// </summary>
        protected override void OnExit()
        {
            if (!string.IsNullOrWhiteSpace(exitMessage))
            {
                Debug.Log(exitMessage);
            }
        }

        /// <summary>
        /// Detail description of task.
        /// </summary>
        public override string GetDescription()
        {
            return "Temporary decorator";
        }
    }
}
