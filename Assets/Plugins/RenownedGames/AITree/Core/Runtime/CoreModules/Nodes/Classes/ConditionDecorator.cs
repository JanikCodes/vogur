/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree
{
    public abstract class ConditionDecorator : DecoratorNode
    {
        [Title("Condition")]
        [SerializeField]
        private bool inverseCondition;

        [SerializeField]
        private bool waitUntilTrue;

        // Stored required properties.
        private bool running;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected sealed override State OnUpdate()
        {
            bool result = CalculateResult();

            if (inverseCondition)
            {
                result = !result; 
            }

            if (result || running)
            {
                running = true;
                return UpdateChild();
            }

            if (waitUntilTrue)
            {
                return State.Running;
            }

            return State.Failure;
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
            running = false;
        }

        /// <summary>
        /// Calculates the result of the condition.
        /// </summary>
        protected abstract bool CalculateResult();

        #region [Getter / Setter]
        public bool InverseCondition()
        {
            return inverseCondition;
        }

        public void InverseCondition(bool value)
        {
            inverseCondition = value;
        }
        #endregion
    }
}