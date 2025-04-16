/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree
{
    public abstract class AuxiliaryNode : Node
    {
        [SerializeField]
        [HideInInspector]
        private Node child;

        // Stored required properties.
        private WrapNode containerNode;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            GetBehaviourTree().Updating += OnFlowUpdate;
        }

        /// <summary>
        /// Updating state of the node in behaviour tree execution order.
        /// </summary>
        /// <returns>State of node after updating.</returns>
        public override State Update()
        {
            return child == null ? State.Failure : base.Update();
        }

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected abstract void OnFlowUpdate();

        /// <summary>
        /// Updating state of the auxiliary node child.
        /// </summary>
        /// <returns>State of child after updating.</returns>
        protected State UpdateChild()
        {
            return child.Update();
        }

        /// <summary>
        /// Traverse resets parameters to specific state.
        /// </summary>
        protected void ResetChild(State state)
        {
            child.Restore(state);
        }

        #region [Getter / Setter]
        /// <summary>
        /// Returns a child node.
        /// </summary>
        public Node GetChild()
        {
            return child;
        }

        /// <summary>
        /// Sets the child node.
        /// </summary>
        /// <param name="child">Node reference.</param>
        internal void SetChild(Node child)
        {
            this.child = child;
        }

        /// <summary>
        /// Returns the wrap node where it is located.
        /// </summary>
        public WrapNode GetContainerNode()
        {
            return containerNode;
        }

        /// <summary>
        /// Set the wrap node where it is located.
        /// </summary>
        /// <param name="container">Container reference in WrapNode reprecentation.</param>
        internal void SetContainerNode(WrapNode container)
        {
            containerNode = container;
        }
        #endregion
    }
}
