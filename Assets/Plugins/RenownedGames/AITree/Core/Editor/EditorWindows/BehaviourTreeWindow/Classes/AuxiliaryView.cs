/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = RenownedGames.AITree.Node;

namespace RenownedGames.AITreeEditor
{
    internal abstract class AuxiliaryView : NodeView
    {
        private bool canMove;
        private Vector2 offset;
        private Vector2? startPos;
        private NodeView parentView = null;

        /// <summary>
        /// Auxiliary view constructor.
        /// </summary>
        /// <param name="node">Node reference.</param>
        /// <param name="uxmlFile">UXML file path.</param>
        public AuxiliaryView(BehaviourTreeGraph graph, Node node, string uxmlFile) : base(graph, node, uxmlFile)
        {
            style.position = Position.Relative;
        }

        /// <summary>
        /// Called once when loading node view to update capabilities.
        /// </summary>
        protected override void UpdateCapabilities()
        {
            base.UpdateCapabilities();
            capabilities &= ~Capabilities.Snappable;
            capabilities &= ~Capabilities.Groupable;
        }

        /// <summary>
        /// Set node position.
        /// </summary>
        /// <param name="newPos">New position.</param>
        public override void SetPosition(Rect newPos)
        {
            if (!startPos.HasValue)
            {
                startPos = newPos.position;
            }

            if (parentView == null)
            {
                parentView = BehaviourTreeGraph.PQ<WrapView>(this);
                if (parentView == null)
                {
                    return;
                }

                canMove = !BehaviourTreeGraph.PQ<BehaviourTreeGraph>(parentView).selection.Contains(parentView);
                offset = startPos.Value;

                parentView.BringToFront();
            }

            newPos.position -= offset;

            if (canMove)
            {
                base.SetPosition(newPos);
            }
            
            style.position = Position.Relative;
        }

        /// <summary>
        /// Update presenter position.
        /// </summary>
        public override void UpdatePresenterPosition()
        {
            startPos = null;
            parentView = null;
            base.SetPosition(Rect.zero);
            style.position = Position.Relative;
        }

        /// <summary>
        /// Add menu items to the node contextual menu.
        /// </summary>
        /// <param name="evt">The event holding the menu to populate.</param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) { }

        /// <summary>
        /// Collect elements.
        /// </summary>
        /// <param name="collectedElementSet">HashSet collection of graph elements.</param>
        /// <param name="conditionFunc">Func callback of condition.</param>
        public override void CollectElements(HashSet<GraphElement> collectedElementSet, Func<GraphElement, bool> conditionFunc) { }
    }
}