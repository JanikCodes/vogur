/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree
{
    public abstract class ServiceNode : AuxiliaryNode, IProcessableNode
    {
        // Stored required properties.
        // Temporary solution, after the update, need to move this field in WrapNode.
        private bool processing;

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected override void OnFlowUpdate() { }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected sealed override State OnUpdate()
        {
            OnServiceUpdate();
            return UpdateChild();
        }

        /// <summary>
        /// An internal method that includes the logic of calling the onTick() method.
        /// </summary>
        internal virtual void OnServiceUpdate()
        {
            OnTick();
        }

        /// <summary>
        /// Service tick.
        /// </summary>
        protected abstract void OnTick();

        #region [Getter / Setter]
        public bool IsProcessing()
        {
            return processing;
        }

        internal void IsProcessing(bool value)
        {
            processing = value;
        }
        #endregion
    }
}
