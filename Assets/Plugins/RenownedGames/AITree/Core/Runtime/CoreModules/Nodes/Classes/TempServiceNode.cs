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
    [NodeContent("Temp Service", "Temp Service", IconPath = "Images/Icons/Node/UnknownIcon.png")]
    internal sealed class TempServiceNode : ServiceNode
    {
        [SerializeField]
        [Title("Messages")]
        private string entryMessage = string.Empty;

        [SerializeField]
        private string tickMessage = string.Empty;

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

        protected override void OnTick()
        {
            if (!string.IsNullOrWhiteSpace(tickMessage))
            {
                Debug.Log(tickMessage);
            }
        }

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
            return "Temporary service";
        }
    }
}
