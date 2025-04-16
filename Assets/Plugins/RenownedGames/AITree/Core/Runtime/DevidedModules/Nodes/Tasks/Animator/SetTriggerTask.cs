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
    [RequireComponent(typeof(Animator))]
    [NodeContent("Set Trigger", "Tasks/Animator/Set Trigger", IconPath = "Images/Icons/Node/AnimSetTriggerIcon.png")]
    public class SetTriggerTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private string parameterName = "name";

        // Stored required components.
        private Animator animator;

        // Stored required properties.
        private int parameterHash;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            animator = GetOwner().GetComponent<Animator>();
            parameterHash = Animator.StringToHash(parameterName);
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (animator == null)
            {
                return State.Failure;
            }

            animator.SetTrigger(parameterHash);
            return State.Success;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = $"Set Trigger: {parameterName}";

            return description;
        }
    }
}