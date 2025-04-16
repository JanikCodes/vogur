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
    [NodeContent("Set Bool", "Tasks/Animator/Set Bool", IconPath = "Images/Icons/Node/AnimSetBoolIcon.png")]
    public class SetBoolTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private string parameterName = "name";

        [SerializeField]
        private BoolKey value;

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
            if (animator == null || value == null)
            {
                return State.Failure;
            }

            animator.SetBool(parameterHash, value.GetValue());
            return State.Success;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = $"Set Bool: {parameterName} set ";

            if (value != null)
            {
                description += value.ToString();
            }
            else
            {
                description += "None";
            }

            return description;
        }
    }
}