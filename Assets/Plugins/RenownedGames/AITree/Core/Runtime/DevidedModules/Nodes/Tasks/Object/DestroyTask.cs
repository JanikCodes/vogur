/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [Obsolete("Use Destroy Object task instead")]
    [NodeContent("Destroy", "Tasks/Object/Destroy", IconPath = "Images/Icons/Node/DestroyIcon.png", Hide = true)]
    public class DestroyTask : TaskNode
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
            if (target != null)
            {
                Transform transform = target.GetValue();
                if (transform != null)
                {
                    Destroy(transform.gameObject);
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
            string description = $"Destroy: ";

            if (target != null)
            {
                description += target.ToString();
            }
            else
            {
                description += "None";
            }

            return description;
        }
    }
}