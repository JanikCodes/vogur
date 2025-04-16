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
    [NodeContent("Cross Fade In Fixed Time", "Tasks/Animator/Cross Fade In Fixed Time", IconPath = "Images/Icons/Node/CrossFadeTimeIcon.png")]
    public class CrossFadeInFixedTimeTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private string animation = "name";

        [SerializeField]
        [MinValue(0)]
        private float fixedTransitionDuration;

        [SerializeField]
        [MinValue(-1)]
        private int layer = -1;

        [SerializeField]
        [MinValue(0)]
        private float fixedTimeOffset = 0f;

        [SerializeField]
        [MinValue(0)]
        private float normalizedTransitionTime = 0f;

        // Stored required components.
        private Animator animator;

        // Stored required properties.
        private int animationHash;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            animator = GetOwner().GetComponent<Animator>();
            animationHash = Animator.StringToHash(animation);
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

            animator.CrossFadeInFixedTime(animationHash, fixedTransitionDuration, layer, fixedTimeOffset, normalizedTransitionTime);
            return State.Success;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = $"Cross Fade In Fixed Time: {animation}";

            if (fixedTransitionDuration > 0)
            {
                description += $"\nFixed Transition Duration: {fixedTransitionDuration}";
            }

            if (layer > -1)
            {
                description += $"\nLayer: {layer}";
            }

            if (fixedTimeOffset > 0)
            {
                description += $"\nFixed Time Offset: {fixedTimeOffset}";
            }

            if (normalizedTransitionTime > 0)
            {
                description += $"\nNormalized Transition Time : {normalizedTransitionTime }";
            }

            return description;
        }
    }
}