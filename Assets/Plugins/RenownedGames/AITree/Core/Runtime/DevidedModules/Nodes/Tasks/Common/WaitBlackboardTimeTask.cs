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
    [NodeContent("Wait Blackboard Time", "Tasks/Common/Wait Blackboard Time", IconPath = "Images/Icons/Node/WaitIcon.png")]
    public class WaitBlackboardTimeTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        [KeyTypes(typeof(int), typeof(float))]
        private Key key;

        // Stored required properties.
        private float startTime;
        private float waitTime;

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();
            startTime = Time.time;

            waitTime = 0;
            if (key != null)
            {
                if (key.TryGetInt(out int intValue))
                {
                    waitTime = intValue;
                }
                else if(key.TryGetFloat(out float floatValue))
                {
                    waitTime = floatValue;
                }

            }
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (Time.time - startTime > waitTime)
            {
                return State.Success;
            }

            return State.Running;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            return $"Wait Blackboard Time: {key?.name ?? "None"}";
        }
    }
}